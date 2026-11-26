using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI;

/// <summary>
/// Opens a <see cref="Gbx"/>, <see cref="Pak"/> or <see cref="ZipArchive"/> from a path. The path can point to the file
/// itself (<c>My/Map.Gbx</c>, <c>My/Pak.pak</c>) or continue inside an archive (<c>My/Pak.pak/My/Map.Gbx</c>,
/// <c>skin.zip/Model.Pack.Gbx/good.gbx</c>). The file type is detected from its signature, not its extension.
/// </summary>
internal sealed class FileObject : IAsyncDisposable
{
    private enum OpenMode
    {
        /// <summary>Parse a Gbx with its body.</summary>
        Full,

        /// <summary>Stop at the innermost archive and keep the rest of the path as an entry pattern.</summary>
        Archive
    }

    private readonly List<object> owned;

    /// <summary>
    /// A <see cref="Gbx"/>, <see cref="Pak"/> or <see cref="ZipArchive"/>. For paths inside an archive, this is the opened entry.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// The path as it was passed to open the file.
    /// </summary>
    public string InputPath { get; }

    /// <summary>
    /// Name of the opened file or archive entry, without folders.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Size of the opened file or archive entry (uncompressed) in bytes.
    /// </summary>
    public long FileSize { get; }

    /// <summary>
    /// Whether a key decrypted the PAK file index. Only meaningful when <see cref="Value"/> is a <see cref="Pak"/>.
    /// </summary>
    public bool HasKey { get; }

    /// <summary>
    /// Path or pattern of the entries inside the archive, left over when it was opened with <see cref="OpenArchiveAsync"/>.
    /// </summary>
    public string? EntryPattern { get; }

    /// <summary>
    /// Whether <see cref="Value"/> was opened from inside an archive, so it cannot be written back to <see cref="InputPath"/>.
    /// </summary>
    public bool IsArchiveEntry { get; }

    private FileObject(string inputPath, bool isArchiveEntry, Opened opened, List<object> owned)
    {
        InputPath = inputPath;
        IsArchiveEntry = isArchiveEntry;
        Value = opened.Value;
        Name = Path.GetFileName(PathPattern.Normalize(opened.Name).TrimEnd('/').Replace('/', Path.DirectorySeparatorChar));
        FileSize = opened.FileSize;
        HasKey = opened.HasKey;
        EntryPattern = opened.EntryPattern;
        this.owned = owned;
    }

    /// <summary>
    /// Opens a Gbx with its body parsed, or a PAK or ZIP archive. A path inside an archive selects one entry.
    /// </summary>
    public static Task<FileObject> OpenAsync(string path, CancellationToken cancellationToken = default)
        => OpenAsync(path, OpenMode.Full, cancellationToken);

    /// <summary>
    /// Opens the innermost PAK or ZIP archive on the path. What is left of the path, if anything, is <see cref="EntryPattern"/>.
    /// A ZIP entry is only opened as a nested archive when the path continues after it, such as <c>skin.zip/Inner.zip/*</c>.
    /// </summary>
    public static Task<FileObject> OpenArchiveAsync(string path, CancellationToken cancellationToken = default)
        => OpenAsync(path, OpenMode.Archive, cancellationToken);

    private static async Task<FileObject> OpenAsync(string path, OpenMode mode, CancellationToken cancellationToken)
    {
        var (filePath, entryPath) = Split(path);
        var owned = new List<object>();

        try
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            owned.Add(stream);

            var opened = await ParseAsync(stream, filePath, entryPath, mode, owned, cancellationToken);

            // A parsed Gbx doesn't read from its file anymore, so the file is closed and can be replaced, such as by eval --write.
            if (entryPath is null && opened.Value is Gbx)
            {
                await DisposeAllAsync(owned);
            }

            return new FileObject(path, entryPath is not null, opened, owned);
        }
        catch
        {
            await DisposeAllAsync(owned);
            throw;
        }
    }

    /// <summary>
    /// Splits the path into the file on disk and the path inside of it, using the longest leading part that is an existing file.
    /// </summary>
    /// <exception cref="FileNotFoundException">No leading part of the path is an existing file.</exception>
    public static (string FilePath, string? EntryPath) Split(string path) => TrySplit(path, out var filePath, out var entryPath)
        ? (filePath, entryPath)
        : throw new FileNotFoundException($"File '{path}' was not found.", path);

    /// <inheritdoc cref="Split"/>
    /// <returns>False if no leading part of the path is an existing file.</returns>
    public static bool TrySplit(string path, out string filePath, out string? entryPath)
    {
        filePath = path;
        entryPath = null;

        if (File.Exists(path))
        {
            return true;
        }

        for (var i = path.Length - 1; i > 0; i--)
        {
            if (path[i] is '/' or '\\' && File.Exists(path[..i]))
            {
                filePath = path[..i];
                entryPath = i + 1 < path.Length ? path[(i + 1)..] : null;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Opens a ZIP entry of an archive that this object owns. It is parsed like a file, so it can be a nested archive.
    /// </summary>
    public async Task<object> OpenEntryAsync(ZipArchiveEntry entry, CancellationToken cancellationToken = default)
    {
        var memory = await BufferAsync(entry, cancellationToken);
        owned.Add(memory);

        return (await ParseAsync(memory, entry.FullName, entryPath: null, OpenMode.Full, owned, cancellationToken)).Value;
    }

    /// <summary>
    /// Opens a PAK entry of an archive that this object owns as a fully parsed Gbx. PAK entries are not opened as nested archives.
    /// </summary>
    public static async Task<Gbx> OpenEntryAsync(Pak pak, PakEntry entry, CancellationToken cancellationToken = default)
    {
        if (entry.IsUnstored)
        {
            throw new InvalidDataException($"PAK entry '{entry.Path}' has no stored data.");
        }

        if (!await pak.CheckFileIsGbxAsync(entry.File, cancellationToken))
        {
            throw new InvalidDataException($"PAK entry '{entry.Path}' is not a Gbx file.");
        }

        return await pak.OpenGbxFileAsync(entry.File, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Copies a ZIP entry into memory, because Gbx and PAK parsing need a seekable stream.
    /// </summary>
    public static async Task<MemoryStream> BufferAsync(ZipArchiveEntry entry, CancellationToken cancellationToken = default)
    {
        var memory = new MemoryStream(capacity: (int)Math.Min(entry.Length, Array.MaxLength));

        await using (var entryStream = entry.Open())
        {
            await entryStream.CopyToAsync(memory, cancellationToken);
        }

        memory.Position = 0;
        return memory;
    }

    /// <summary>
    /// Gets the opened Gbx to change.
    /// </summary>
    /// <exception cref="InvalidOperationException">The opened file is not a Gbx.</exception>
    public Gbx GetGbx() => Value as Gbx
        ?? throw new InvalidOperationException($"Only a Gbx can be changed and written, not a {ValueFormatter.GetTypeName(Value.GetType())}.");

    /// <summary>
    /// Gets the opened Gbx to write. A Gbx from an archive needs <paramref name="outputPath"/>, because archives are not written.
    /// </summary>
    /// <param name="outputPath">The file to write instead of <see cref="InputPath"/>, or null.</param>
    /// <exception cref="InvalidOperationException">The opened file is not a Gbx, or it is an archive entry without <paramref name="outputPath"/>.</exception>
    public Gbx GetWritableGbx(string? outputPath)
    {
        var gbx = GetGbx();

        return outputPath is null && IsArchiveEntry
            ? throw new InvalidOperationException("A Gbx inside an archive cannot be written back to the archive. Use -o to write it to a file.")
            : gbx;
    }

    public async Task WriteSummaryAsync(CliConsole console)
    {
        await console.WriteLineAsync($"{CliConsole.Label}File:{CliConsole.Reset} {CliConsole.Escape(InputPath)}");

        switch (Value)
        {
            case Gbx gbx:
                await console.WriteLineAsync($"{CliConsole.Label}File size:{CliConsole.Reset} {FileSize:N0} bytes");
                await new GbxSummary(console).WriteAsync(gbx);
                break;
            case Pak pak:
                await console.WriteLineAsync($"{CliConsole.Label}Format:{CliConsole.Reset} {Pak.Magic}");
                await console.WriteLineAsync($"{CliConsole.Label}Version:{CliConsole.Reset} {pak.Version}");
                await console.WriteLineAsync($"{CliConsole.Label}Header encryption:{CliConsole.Reset} {(pak.IsHeaderEncrypted ? "Encrypted" : "Unencrypted")}");
                await console.WriteLineAsync(pak.IsHeaderPrivate && !HasKey
                    ? $"{CliConsole.Label}Files:{CliConsole.Reset} Unavailable (private header requires a key)."
                    : $"{CliConsole.Label}Files:{CliConsole.Reset} {pak.Files.Count}");
                break;
            case ZipArchive zip:
                await console.WriteLineAsync($"{CliConsole.Label}Format:{CliConsole.Reset} ZIP");
                await console.WriteLineAsync($"{CliConsole.Label}Files:{CliConsole.Reset} {zip.Select(pattern: null).Count()}");
                break;
        }
    }

    public ValueTask DisposeAsync() => DisposeAllAsync(owned);

    private static async Task<Opened> ParseAsync(Stream stream, string name, string? entryPath, OpenMode mode, List<object> owned, CancellationToken cancellationToken)
    {
        switch (await FileSignature.ReadAsync(stream, cancellationToken))
        {
            case FileKind.Gbx:
                if (entryPath is not null)
                {
                    throw new InvalidDataException($"'{name}' is a Gbx file, so it has no entry '{entryPath}'.");
                }

                if (mode == OpenMode.Archive)
                {
                    throw new InvalidDataException($"'{name}' is a Gbx file, not a PAK (NadeoPak) or ZIP archive.");
                }

                return new(await Gbx.ParseAsync(stream, cancellationToken: cancellationToken), name, stream.Length);
            case FileKind.Pak:
                return await ParsePakAsync(stream, name, entryPath, mode, owned, cancellationToken);
            case FileKind.Zip:
                return await ParseZipAsync(stream, name, entryPath, mode, owned, cancellationToken);
            default:
                throw new InvalidDataException($"'{name}' has an unsupported format. Expected a Gbx (GBX), PAK (NadeoPak) or ZIP file.");
        }
    }

    private static async Task<Opened> ParsePakAsync(Stream stream, string name, string? entryPath, OpenMode mode, List<object> owned, CancellationToken cancellationToken)
    {
        var (pak, hasKey) = await PakLoader.ParseAsync(stream, name, cancellationToken);
        owned.Add(pak);

        if (mode == OpenMode.Archive)
        {
            pak.EnsureIndex(hasKey);
            return new(pak, name, stream.Length, hasKey, entryPath);
        }

        if (entryPath is null)
        {
            return new(pak, name, stream.Length, hasKey);
        }

        pak.EnsureIndex(hasKey);

        var entry = pak.Find(entryPath)
            ?? throw new FileNotFoundException($"File '{entryPath}' was not found in PAK '{name}'.", name);

        return new(await OpenEntryAsync(pak, entry, cancellationToken), entry.Path, entry.File.UncompressedSize);
    }

    private static async Task<Opened> ParseZipAsync(Stream stream, string name, string? entryPath, OpenMode mode, List<object> owned, CancellationToken cancellationToken)
    {
        var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
        owned.Add(zip);

        if (entryPath is null)
        {
            return new(zip, name, stream.Length);
        }

        ZipArchiveEntry entry;
        string? innerPath;

        if (zip.Find(entryPath, requireInnerPath: mode == OpenMode.Archive) is var (found, rest))
        {
            (entry, innerPath) = (found, rest);
        }
        else if (mode == OpenMode.Archive)
        {
            // The rest of the path selects entries of this archive.
            return new(zip, name, stream.Length, EntryPattern: entryPath);
        }
        else
        {
            var matches = zip.Select(entryPath).Take(11).ToArray();

            entry = matches.Length switch
            {
                0 => throw new FileNotFoundException($"File '{entryPath}' was not found in ZIP '{name}'.", name),
                1 => matches[0],
                _ => throw ZipArchiveExtensions.Ambiguous(entryPath, matches)
            };
            innerPath = null;
        }

        var memory = await BufferAsync(entry, cancellationToken);
        owned.Add(memory);

        // The entry can be an archive itself, so whatever path is left continues inside of it.
        return await ParseAsync(memory, entry.FullName, innerPath, mode, owned, cancellationToken);
    }

    /// <summary>
    /// Disposes in reverse order, so archives go before the streams they read from.
    /// </summary>
    private static async ValueTask DisposeAllAsync(List<object> owned)
    {
        for (var i = owned.Count - 1; i >= 0; i--)
        {
            switch (owned[i])
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        owned.Clear();
    }

    private sealed record Opened(object Value, string Name, long FileSize, bool HasKey = false, string? EntryPattern = null);
}
