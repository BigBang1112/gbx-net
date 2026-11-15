using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

internal static class PackCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            switch (arg)
            {
                case "verify":
                    return await VerifyCommand.RunAsync(argsEnumerator, output, cancellationToken);
                case "extract":
                    await ExtractAsync(argsEnumerator, output, cancellationToken);
                    break;
                case "list":
                    await ListFilesAsync(argsEnumerator, output, cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Unexpected argument: {arg}");
            }
        }

        return 0;
    }

    private static async Task ExtractAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        string? inputPath = null;
        string? outputPath = null;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg is "-o" or "--output")
            {
                if (outputPath is not null)
                {
                    throw new ArgumentException("Output can only be specified once.");
                }

                if (!argsEnumerator.MoveNext() || string.IsNullOrWhiteSpace(argsEnumerator.Current))
                {
                    throw new ArgumentException($"Missing file path after {arg}.");
                }

                outputPath = argsEnumerator.Current;
            }
            else if (inputPath is null)
            {
                inputPath = arg;
            }
            else
            {
                throw new ArgumentException($"Unexpected argument: {arg}.");
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing input file for pack extract.");
        }

        var (archivePath, entryPattern) = FileObject.Split(inputPath);
        var directoryPath = Path.GetFullPath(outputPath ?? archivePath);

        try
        {
            await using var zip = await ZipFile.OpenReadAsync(archivePath, cancellationToken);

            await WriteEntriesAsync(directoryPath,
                zip.Select(entryPattern),
                entry => entry.FullName,
                entry => entry.Open(),
                reparse: false,
                output,
                cancellationToken);

            return;
        }
        catch (InvalidDataException)
        {
            // Not a valid ZIP file
        }

        await using var file = await FileObject.OpenAsync(archivePath, fullGbx: false, cancellationToken);

        if (file.Value is not Pak pak)
        {
            throw new InvalidDataException("The pack extract command requires a PAK (NadeoPak) or ZIP archive.");
        }

        if (pak.IsHeaderPrivate && pak.Files.Count == 0)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        await WriteEntriesAsync(
            directoryPath,
            pak.Select(entryPattern),
            entry => entry.Path,
            entry => pak.OpenFile(entry.File, out _),
            reparse: true,
            output,
            cancellationToken);
    }

    private static async Task ListFilesAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        string? inputPath = null;
        bool? resolved = null;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg is "--resolved" or "--unresolved")
            {
                var requested = arg == "--resolved";

                if (resolved is not null && resolved != requested)
                {
                    throw new ArgumentException("The --resolved and --unresolved options cannot be combined.");
                }

                resolved = requested;
            }
            else if (inputPath is null)
            {
                inputPath = arg;
            }
            else
            {
                throw new ArgumentException($"Unexpected argument: {arg}.");
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing input file for pack list.");
        }

        var (archivePath, entryPattern) = FileObject.Split(inputPath);

        await using var file = await FileObject.OpenAsync(archivePath, fullGbx: false, cancellationToken);

        IEnumerable<(string Path, bool IsResolved)> entries;

        switch (file.Value)
        {
            case ZipArchive zip:
                // ZIP entries are always named, so none of them are unresolved hashes.
                entries = zip.Select(entryPattern).Select(entry => (entry.FullName.Replace('/', Path.DirectorySeparatorChar), true));
                break;
            case Pak pak:
                if (pak.IsHeaderPrivate && pak.Files.Count == 0)
                {
                    throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
                }

                entries = pak.Select(entryPattern).Select(entry => (entry.Path, entry.IsResolved));
                break;
            default:
                throw new InvalidDataException("The pack list command requires a PAK (NadeoPak) or ZIP archive.");
        }

        var matched = false;

        foreach (var (path, isResolved) in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            matched = true;

            if (resolved is null || isResolved == resolved.Value)
            {
                await output.WriteLineAsync(path);
            }
        }

        if (entryPattern is not null && !matched)
        {
            throw new FileNotFoundException($"No files matching '{entryPattern}' were found in the archive.");
        }
    }

    private static async Task WriteEntriesAsync<T>(
        string directoryPath,
        IEnumerable<T> entries,
        Func<T, string> getPath,
        Func<T, Stream> open,
        bool reparse,
        TextWriter output,
        CancellationToken cancellationToken)
    {
        var count = 0;

        foreach (var entry in entries)
        {
            var extractPath = Path.Combine(directoryPath, getPath(entry));
            await using var stream = open(entry);

            Directory.CreateDirectory(Path.GetDirectoryName(extractPath)!);

            await using var destination = new FileStream(extractPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            if (reparse)
            {
                await CopyGbxFromPakAsync(stream, destination, cancellationToken);
            }
            else
            {
                await stream.CopyToAsync(destination, cancellationToken);
            }

            await output.WriteLineAsync($"Written: {extractPath}");

            count++;
        }

        await output.WriteLineAsync($"Extracted {count} files to {directoryPath}.");
    }

    private static async Task CopyGbxFromPakAsync(Stream stream, Stream destination, CancellationToken cancellationToken)
    {
        try
        {
            var gbx = await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);

            if (gbx.Node is null)
            {
                await stream.CopyToAsync(destination, cancellationToken);
                return;
            }

            if (gbx.Body.Exception is not null)
            {
                await stream.CopyToAsync(destination, cancellationToken);
                return;
            }

            gbx.Save(destination);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await stream.CopyToAsync(destination, cancellationToken);
        }
    }
}
