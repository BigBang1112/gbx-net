using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

internal static class PackExtractCommand
{
    private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

    private static StringComparer PathComparer => OperatingSystem.IsLinux() ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        string? outputPath = null;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "pack");
                    return;
                case "-o" or "--output":
                    outputPath = outputPath is null ? args.NextValue("--output") : throw new ArgumentException("Output can only be specified once.");
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected("pack extract", arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("pack extract");
        }

        await using var file = await FileObject.OpenArchiveAsync(inputPath, context.CancellationToken);

        var directoryPath = Path.GetFullPath(outputPath ?? GetDefaultDirectoryName(file));
        var unstored = 0;

        IReadOnlyList<ExtractEntry> entries;

        switch (file.Value)
        {
            case ZipArchive zip:
                entries = [.. zip.Select(file.EntryPattern).Select(entry => new ExtractEntry(entry.FullName, (destination, cancellationToken) => CopyZipEntryAsync(entry, destination, cancellationToken)))];
                break;
            case Pak pak:
                var pakEntries = pak.Select(file.EntryPattern).ToArray();
                unstored = pakEntries.Count(entry => entry.IsUnstored);
                entries = [.. pakEntries.Where(entry => !entry.IsUnstored).Select(entry => new ExtractEntry(entry.Path, (destination, cancellationToken) => CopyPakEntryAsync(pak, entry, destination, cancellationToken)))];
                break;
            default:
                throw new InvalidOperationException("Unexpected archive type.");
        }

        if (file.EntryPattern is not null && entries.Count + unstored == 0)
        {
            throw new FileNotFoundException($"No files matching '{file.EntryPattern}' were found in '{file.Name}'.");
        }

        if (File.Exists(directoryPath))
        {
            throw new IOException($"Cannot extract to '{directoryPath}', because it is a file. Choose a folder with -o.");
        }

        // Every destination is checked before anything is written, so a rejected extraction leaves no files behind.
        var destinations = GetDestinations(directoryPath, entries);

        Directory.CreateDirectory(directoryPath);

        for (var i = 0; i < entries.Count; i++)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var destinationPath = destinations[i];
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

            await using (var destination = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await entries[i].CopyAsync(destination, context.CancellationToken);
            }

            await context.Console.WriteLineAsync($"{CliConsole.Written}Written:{CliConsole.Reset} {CliConsole.Escape(destinationPath)}");
        }

        await context.Console.WriteLineAsync(CliConsole.Escape(unstored == 0
            ? $"Extracted {entries.Count} files to {directoryPath}."
            : $"Extracted {entries.Count} files to {directoryPath}; {unstored} entries without stored data skipped."));
    }

    /// <summary>
    /// A folder in the working directory, named after the title ID of a title pack. Other archives, such as ZIP files
    /// and PAK files before version 12, use their name without the whole extension, so <c>Model.Pack.Gbx</c> becomes <c>Model</c>.
    /// </summary>
    private static string GetDefaultDirectoryName(FileObject file)
    {
        if (file.Value is Pak6 { TitleId: { } titleId } && !string.IsNullOrWhiteSpace(titleId)
            && titleId.IndexOfAny(invalidFileNameChars) < 0 && titleId is not ("." or ".."))
        {
            return titleId;
        }

        // A leading dot is part of the name, not an extension.
        var extension = file.Name.IndexOf('.', 1);
        return extension > 0 ? file.Name[..extension] : file.Name;
    }

    /// <summary>
    /// Resolves the destination of every entry under <paramref name="directoryPath"/>.
    /// </summary>
    /// <exception cref="InvalidDataException">An entry path is unsafe, or two entries would be written to the same file.</exception>
    /// <exception cref="IOException">A destination already exists.</exception>
    private static string[] GetDestinations(string directoryPath, IReadOnlyList<ExtractEntry> entries)
    {
        var root = Path.TrimEndingDirectorySeparator(directoryPath) + Path.DirectorySeparatorChar;
        var destinations = new string[entries.Count];
        var files = new Dictionary<string, string>(PathComparer);
        var folders = new HashSet<string>(PathComparer);

        for (var i = 0; i < entries.Count; i++)
        {
            var entryPath = entries[i].Path;
            var destination = GetDestination(root, entryPath);

            if (!files.TryAdd(destination, entryPath))
            {
                throw new InvalidDataException($"Entries '{files[destination]}' and '{entryPath}' would both be extracted to '{destination}'.");
            }

            if (File.Exists(destination) || Directory.Exists(destination))
            {
                throw new IOException($"'{destination}' already exists.");
            }

            for (var folder = Path.GetDirectoryName(destination); folder is not null && folder.Length >= root.Length && folders.Add(folder); folder = Path.GetDirectoryName(folder))
            {
                if (File.Exists(folder))
                {
                    throw new IOException($"'{folder}' already exists as a file.");
                }
            }

            destinations[i] = destination;
        }

        if (files.Keys.FirstOrDefault(folders.Contains) is { } conflict)
        {
            throw new InvalidDataException($"Entry '{files[conflict]}' would be extracted to '{conflict}', which other entries need as a folder.");
        }

        return destinations;
    }

    /// <summary>
    /// Rejects rooted paths, <c>..</c> parts and invalid filename characters, and checks that the result stays under <paramref name="root"/>.
    /// </summary>
    private static string GetDestination(string root, string entryPath)
    {
        var parts = PathPattern.Normalize(entryPath).Split('/');

        if (Path.IsPathRooted(entryPath) || parts.Any(part => part is "" or "." or ".." || part.IndexOfAny(invalidFileNameChars) >= 0))
        {
            throw Unsafe(entryPath);
        }

        var destination = Path.GetFullPath(Path.Join(root, Path.Join(parts)));

        return destination.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? destination : throw Unsafe(entryPath);
    }

    private static InvalidDataException Unsafe(string entryPath) => new($"Entry '{entryPath}' is not a safe relative file path.");

    private static async Task CopyZipEntryAsync(ZipArchiveEntry entry, Stream destination, CancellationToken cancellationToken)
    {
        await using var source = entry.Open();
        await source.CopyToAsync(destination, cancellationToken);
    }

    /// <summary>
    /// Reads exactly the entry's data, without the padding after it. A Gbx is saved again through GBX.NET,
    /// because the stored data can depend on how the game reads it. Anything else is written as stored.
    /// </summary>
    private static async Task CopyPakEntryAsync(Pak pak, PakEntry entry, Stream destination, CancellationToken cancellationToken)
    {
        var data = new byte[entry.File.UncompressedSize];

        await using (var source = await pak.OpenFileAsync(entry.File, cancellationToken))
        {
            await source.ReadExactlyAsync(data, cancellationToken);
        }

        if (FileSignature.Detect(data) == FileKind.Gbx && await TrySaveGbxAsync(pak, entry, cancellationToken) is { } saved)
        {
            await destination.WriteAsync(saved, cancellationToken);
            return;
        }

        await destination.WriteAsync(data, cancellationToken);
    }

    /// <returns>The saved Gbx, or null if it could not be fully parsed.</returns>
    private static async Task<byte[]?> TrySaveGbxAsync(Pak pak, PakEntry entry, CancellationToken cancellationToken)
    {
        try
        {
            var gbx = await pak.OpenGbxFileAsync(entry.File, cancellationToken: cancellationToken);

            if (gbx.Node is null || gbx.Body.Exception is not null)
            {
                return null;
            }

            using var memory = new MemoryStream();
            gbx.Save(memory);
            return memory.ToArray();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return null;
        }
    }

    private sealed record ExtractEntry(string Path, Func<Stream, CancellationToken, Task> CopyAsync);
}
