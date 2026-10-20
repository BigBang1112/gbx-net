using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

internal static class PakCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        if (!argsEnumerator.MoveNext())
        {
            throw new ArgumentException("Missing PAK subcommand. Use gbx pak files <file>, gbx pak verify <file>, or gbx pak extract <file>.");
        }

        var command = argsEnumerator.Current;

        if (Cli.TryHandleGlobalOption(command, output, "pak"))
        {
            return 0;
        }

        if (command is not ("files" or "verify" or "extract"))
        {
            throw new ArgumentException($"Unknown PAK subcommand: {command}. Use gbx pak files <file>, gbx pak verify <file>, or gbx pak extract <file>.");
        }

        var arguments = PakArguments.Parse(argsEnumerator, command, output);

        if (arguments is null)
        {
            return 0;
        }

        var (archivePath, entryPattern) = ParsedFile.ResolveInputPath(arguments.InputPath);

        if (File.Exists(archivePath) && await ZipFileIndex.IsZipAsync(archivePath))
        {
            return await RunZipAsync(archivePath, entryPattern, command, arguments, output);
        }

        await using var file = await ParsedFile.OpenAsync(archivePath, fullGbx: false);

        if (file.Value is not Pak pak)
        {
            throw new InvalidDataException($"The pak {command} command requires a PAK (NadeoPak) archive.");
        }

        if (!file.HasPakFileIndex)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        var entries = new PakFileIndex(pak).Select(entryPattern);

        if (entryPattern is not null && entries.Length == 0)
        {
            throw new FileNotFoundException($"No files matching '{entryPattern}' were found in the PAK.");
        }

        if (command == "verify")
        {
            return await VerifyAsync(pak, entries, output);
        }

        if (command == "extract")
        {
            return await ExtractAsync(pak, entries, arguments.OutputPath ?? archivePath + ".extracted", output);
        }

        foreach (var entry in entries.Where(entry => arguments.Resolved is null || entry.IsResolved == arguments.Resolved.Value))
        {
            await output.WriteLineAsync(entry.Path);
        }

        return 0;
    }

    private static async Task<int> RunZipAsync(string archivePath, string? entryPattern, string command, PakArguments arguments, TextWriter output)
    {
        using var zip = new ZipFileIndex(archivePath);
        var entries = zip.Select(entryPattern);

        if (entryPattern is not null && entries.Length == 0)
        {
            throw new FileNotFoundException($"No files matching '{entryPattern}' were found in the ZIP.");
        }

        if (command == "verify")
        {
            return await VerifyZipAsync(entries, output);
        }

        if (command == "extract")
        {
            return await ExtractZipAsync(entries, arguments.OutputPath ?? archivePath + ".extracted", output);
        }

        // ZIP entries are always named, so none of them are unresolved hashes.
        if (arguments.Resolved == false)
        {
            return 0;
        }

        foreach (var entry in entries)
        {
            await output.WriteLineAsync(entry.Path);
        }

        return 0;
    }

    private static async Task<int> VerifyZipAsync(ZipFileIndex.Entry[] entries, TextWriter output)
    {
        var parsed = 0;
        var failed = 0;
        var skipped = 0;

        foreach (var entry in entries)
        {
            try
            {
                if (!entry.Path.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase) && !await HasGbxSignatureAsync(entry.File))
                {
                    skipped++;
                    continue;
                }

                // ZIP entry streams cannot seek, so the Gbx is buffered before parsing.
                using var data = new MemoryStream();
                await using (var source = entry.File.Open())
                {
                    await source.CopyToAsync(data);
                }

                if (data.Length < 3)
                {
                    throw new InvalidDataException("The file is too short to contain a Gbx signature.");
                }

                data.Position = 0;
                var gbx = await Gbx.ParseAsync(data);

                if (gbx.Node is null)
                {
                    throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported; its body was not parsed.");
                }

                parsed++;
            }
            catch (Exception exception)
            {
                failed++;
                await output.WriteLineAsync($"{entry.Path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
            }
        }

        await output.WriteLineAsync($"Parsed {parsed} Gbx files; {failed} failed; {skipped} non-Gbx files skipped.");
        return failed == 0 ? 0 : 1;
    }

    private static async Task<bool> HasGbxSignatureAsync(ZipArchiveEntry file)
    {
        if (file.Length < 3)
        {
            return false;
        }

        await using var stream = file.Open();
        var signature = new byte[3];
        await stream.ReadExactlyAsync(signature);
        return signature.AsSpan().SequenceEqual("GBX"u8);
    }

    private static async Task<int> ExtractZipAsync(ZipFileIndex.Entry[] entries, string outputPath, TextWriter output)
    {
        var directoryPath = Path.GetFullPath(outputPath);
        var paths = GetExtractionPaths(directoryPath, entries.Select(entry => entry.Path), "ZIP");
        Directory.CreateDirectory(directoryPath);

        for (var i = 0; i < entries.Length; i++)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(paths[i])!);
            await using var source = entries[i].File.Open();
            await using var destination = new FileStream(paths[i], FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await source.CopyToAsync(destination);
            await output.WriteLineAsync($"Written: {paths[i]}");
        }

        await output.WriteLineAsync($"Extracted {entries.Length} files to {directoryPath}.");
        return 0;
    }

    private static string[] GetExtractionPaths(string directoryPath, IEnumerable<string> entryPaths, string archiveKind)
    {
        var paths = entryPaths.Select(entryPath => GetExtractionPath(directoryPath, entryPath, archiveKind)).ToArray();
        var destinations = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        foreach (var path in paths)
        {
            if (!destinations.Add(path))
            {
                throw new InvalidDataException($"Multiple {archiveKind} entries resolve to the extraction path '{path}'.");
            }

            if (File.Exists(path) || Directory.Exists(path))
            {
                throw new IOException($"The extraction path '{path}' already exists.");
            }
        }

        return paths;
    }

    private static async Task<int> ExtractAsync(Pak pak, PakFileIndex.Entry[] entries, string outputPath, TextWriter output)
    {
        var directoryPath = Path.GetFullPath(outputPath);
        var paths = GetExtractionPaths(directoryPath, entries.Select(entry => entry.Path), "PAK");
        var files = entries.Select((entry, i) => (entry.File, Path: paths[i])).ToArray();

        Directory.CreateDirectory(directoryPath);
        var buffer = new byte[81920];

        foreach (var (file, path) in files)
        {
            if (file.UncompressedSize < 0)
            {
                throw new InvalidDataException($"The file '{file.Name}' has a negative uncompressed size.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using var source = pak.OpenFile(file, out _);
            await using var destination = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            // PAK entry streams can include padding or subsequent files. Copy only the entry's declared size.
            var remaining = file.UncompressedSize;

            while (remaining > 0)
            {
                var count = Math.Min(remaining, buffer.Length);
                await source.ReadExactlyAsync(buffer.AsMemory(0, count));
                await destination.WriteAsync(buffer.AsMemory(0, count));
                remaining -= count;
            }

            await output.WriteLineAsync($"Written: {path}");
        }

        await output.WriteLineAsync($"Extracted {files.Length} files to {directoryPath}.");
        return 0;
    }

    private static string GetExtractionPath(string directoryPath, string entryPath, string archiveKind)
    {
        var relativePath = entryPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        if (Path.IsPathRooted(relativePath)
            || relativePath.Split(Path.DirectorySeparatorChar).Any(part => part == ".." || part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
        {
            throw new InvalidDataException($"The {archiveKind} entry '{entryPath}' is not a safe relative file path.");
        }

        var path = Path.GetFullPath(Path.Combine(directoryPath, relativePath));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        var prefix = Path.EndsInDirectorySeparator(directoryPath) ? directoryPath : directoryPath + Path.DirectorySeparatorChar;

        if (!path.StartsWith(prefix, comparison))
        {
            throw new InvalidDataException($"The {archiveKind} entry '{entryPath}' is outside the extraction directory.");
        }

        return path;
    }

    private static async Task<int> VerifyAsync(Pak pak, PakFileIndex.Entry[] entries, TextWriter output)
    {
        var parsed = 0;
        var failed = 0;
        var skipped = 0;

        foreach (var entry in entries)
        {
            var file = entry.File;

            try
            {
                if (!await IsGbxAsync(pak, file, entry.Path))
                {
                    skipped++;
                    continue;
                }

                if (file.UncompressedSize < 3)
                {
                    throw new InvalidDataException("The file is too short to contain a Gbx signature.");
                }

                var gbx = await pak.OpenGbxFileAsync(file);

                if (gbx.Node is null)
                {
                    throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported; its body was not parsed.");
                }

                parsed++;
            }
            catch (Exception exception)
            {
                failed++;
                await output.WriteLineAsync($"{entry.Path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
            }
        }

        await output.WriteLineAsync($"Parsed {parsed} Gbx files; {failed} failed; {skipped} non-Gbx files skipped.");
        return failed == 0 ? 0 : 1;
    }

    private static async Task<bool> IsGbxAsync(Pak pak, PakFile file, string path)
    {
        if (path.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (file.UncompressedSize < 3)
        {
            return false;
        }

        using var stream = pak.OpenFile(file, out _);
        var signature = new byte[3];
        await stream.ReadExactlyAsync(signature);
        return signature.AsSpan().SequenceEqual("GBX"u8);
    }
}
