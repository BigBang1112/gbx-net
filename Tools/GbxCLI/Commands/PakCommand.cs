using GBX.NET.PAK;
using System.IO.Enumeration;

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

        if (Cli.TryHandleGlobalOption(command, output))
        {
            return 0;
        }

        if (command is not ("files" or "verify" or "extract"))
        {
            throw new ArgumentException($"Unknown PAK subcommand: {command}. Use gbx pak files <file>, gbx pak verify <file>, or gbx pak extract <file>.");
        }

        var arguments = FileArguments.Parse(argsEnumerator, $"pak {command}", output, requiresOutput: false, allowsOutput: command == "extract");

        if (arguments is null)
        {
            return 0;
        }

        var (archivePath, entryPattern) = command == "extract"
            ? ParsedFile.ResolveInputPath(arguments.InputPath)
            : (arguments.InputPath, null);
        await using var file = await ParsedFile.OpenAsync(archivePath, fullGbx: false);

        if (file.Value is not Pak pak)
        {
            throw new InvalidDataException($"The pak {command} command requires a PAK (NadeoPak) archive.");
        }

        if (!file.HasPakFileIndex)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        if (command == "verify")
        {
            return await VerifyAsync(pak, output);
        }

        if (command == "extract")
        {
            return await ExtractAsync(pak, entryPattern, arguments.OutputPath ?? archivePath + ".extracted", output);
        }

        foreach (var path in pak.Files.Keys.Order(StringComparer.Ordinal))
        {
            await output.WriteLineAsync(path);
        }

        return 0;
    }

    private static async Task<int> ExtractAsync(Pak pak, string? entryPattern, string outputPath, TextWriter output)
    {
        var pattern = entryPattern?.Replace('\\', '/');
        var directoryPath = Path.GetFullPath(outputPath);
        var files = pak.Files.OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Where(pair => pattern is null || FileSystemName.MatchesSimpleExpression(pattern, pair.Key.Replace('\\', '/'), ignoreCase: true))
            .Select(pair => (File: pair.Value, Path: GetExtractionPath(directoryPath, pair.Key)))
            .ToArray();

        if (pattern is not null && files.Length == 0)
        {
            throw new FileNotFoundException($"No files matching '{entryPattern}' were found in the PAK.");
        }

        foreach (var (_, path) in files)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                throw new IOException($"The extraction path '{path}' already exists.");
            }
        }

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

    private static string GetExtractionPath(string directoryPath, string entryPath)
    {
        var relativePath = entryPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        if (Path.IsPathRooted(relativePath)
            || relativePath.Split(Path.DirectorySeparatorChar).Any(part => part == ".." || part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
        {
            throw new InvalidDataException($"The PAK entry '{entryPath}' is not a safe relative file path.");
        }

        var path = Path.GetFullPath(Path.Combine(directoryPath, relativePath));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        var prefix = Path.EndsInDirectorySeparator(directoryPath) ? directoryPath : directoryPath + Path.DirectorySeparatorChar;

        if (!path.StartsWith(prefix, comparison))
        {
            throw new InvalidDataException($"The PAK entry '{entryPath}' is outside the extraction directory.");
        }

        return path;
    }

    private static async Task<int> VerifyAsync(Pak pak, TextWriter output)
    {
        var parsed = 0;
        var failed = 0;
        var skipped = 0;

        foreach (var (path, file) in pak.Files.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            try
            {
                if (!await IsGbxAsync(pak, file))
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
                await output.WriteLineAsync($"{path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
            }
        }

        await output.WriteLineAsync($"Parsed {parsed} Gbx files; {failed} failed; {skipped} non-Gbx files skipped.");
        return failed == 0 ? 0 : 1;
    }

    private static async Task<bool> IsGbxAsync(Pak pak, PakFile file)
    {
        if (file.Name.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase))
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
