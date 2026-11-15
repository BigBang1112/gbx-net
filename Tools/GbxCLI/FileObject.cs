using GBX.NET;
using GBX.NET.PAK;
using Spectre.Console;
using System.IO.Compression;

namespace GbxCLI;

/// <summary>
/// Opens a <see cref="Gbx"/>, <see cref="Pak"/> or <see cref="ZipArchive"/> from a path. The path can point to the file
/// itself (<c>My/Zip.zip</c>, <c>My/Gbx.Gbx</c>, <c>My/Pak.pak</c>) or to an entry inside a ZIP or PAK archive
/// (<c>My/Zip.zip/My/Gbx.Gbx</c>, <c>My/Pak.pak/My/Map.Gbx</c>). The file type is detected from its signature, not its extension.
/// </summary>
internal sealed class FileObject : IAsyncDisposable
{
    private enum FileKind { Unknown, Gbx, Pak, Zip }

    private readonly List<object> owned;

    /// <summary>
    /// A <see cref="Gbx"/>, <see cref="Pak"/> or <see cref="ZipArchive"/>. For paths with an entry, this is the parsed entry.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// The path as it was passed to <see cref="OpenAsync"/>.
    /// </summary>
    public string InputPath { get; }

    /// <summary>
    /// Size of the opened file or archive entry (uncompressed) in bytes.
    /// </summary>
    public long FileSize { get; }

    public string RootName => Value switch
    {
        Gbx => "Gbx",
        Pak => "Pak",
        ZipArchive => "Zip",
        _ => throw new InvalidOperationException("Unexpected file object value.")
    };

    private FileObject(string inputPath, object value, long fileSize, List<object> owned)
    {
        InputPath = inputPath;
        Value = value;
        FileSize = fileSize;
        this.owned = owned;
    }

    /// <param name="path">Path to a file, or to an entry inside a ZIP or PAK archive.</param>
    /// <param name="fullGbx">If false, only the header of a Gbx is parsed.</param>
    public static async Task<FileObject> OpenAsync(string path, bool fullGbx = true, CancellationToken cancellationToken = default)
    {
        var (filePath, entryPath) = Split(path);
        var owned = new List<object>();

        try
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            owned.Add(stream);

            var (value, fileSize) = await ParseAsync(stream, filePath, entryPath, fullGbx, owned, cancellationToken);

            return new FileObject(path, value, fileSize, owned);
        }
        catch
        {
            await DisposeAllAsync(owned);
            throw;
        }
    }

    public async Task WriteSummaryAsync(CliConsole console)
    {
        await console.WriteLineAsync($"[grey]File:[/] {Markup.Escape(InputPath)}");

        switch (Value)
        {
            case Gbx gbx:
                await console.WriteLineAsync($"[grey]File size:[/] {FileSize:N0} bytes");
                await new GbxSummary(console).WriteAsync(gbx);
                break;
            case Pak pak:
                await console.WriteLineAsync($"[grey]Format:[/] {Pak.Magic}");
                await console.WriteLineAsync($"[grey]Version:[/] {pak.Version}");
                await console.WriteLineAsync($"[grey]Header encryption:[/] {(pak.IsHeaderEncrypted ? "Encrypted" : "Unencrypted")}");

                // Without a matching key, a private header is left unread and no files are listed.
                if (pak.IsHeaderPrivate && pak.Files.Count == 0)
                {
                    await console.WriteLineAsync("[grey]Files:[/] Unavailable (private header requires a key).");
                }
                else
                {
                    await console.WriteLineAsync($"[grey]Files:[/] {pak.Files.Count}");
                }

                break;
            case ZipArchive zip:
                await console.WriteLineAsync("[grey]Format:[/] ZIP");
                await console.WriteLineAsync($"[grey]Files:[/] {zip.Entries.Count(entry => !entry.FullName.EndsWith('/'))}");
                break;
        }
    }

    public ValueTask DisposeAsync() => DisposeAllAsync(owned);

    /// <summary>
    /// Splits the path into the file on disk and the entry path inside of it, using the longest prefix that is an existing file.
    /// </summary>
    internal static (string FilePath, string? EntryPath) Split(string path)
    {
        if (File.Exists(path))
        {
            return (path, null);
        }

        for (var i = path.Length - 1; i > 0; i--)
        {
            if ((path[i] == '/' || path[i] == '\\') && File.Exists(path[..i]))
            {
                return (path[..i], i + 1 < path.Length ? path[(i + 1)..] : null);
            }
        }

        throw new FileNotFoundException($"File '{path}' was not found.", path);
    }

    private static async Task<(object Value, long FileSize)> ParseAsync(
        Stream stream, string name, string? entryPath, bool fullGbx, List<object> owned, CancellationToken cancellationToken)
    {
        var signature = new byte[Pak.Magic.Length];
        var bytesRead = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false, cancellationToken);
        stream.Position = 0;

        switch (GetKind(signature.AsSpan(0, bytesRead)))
        {
            case FileKind.Gbx:
                if (entryPath is not null)
                {
                    throw new InvalidDataException($"'{name}' is a Gbx file, so it has no entry '{entryPath}'.");
                }

                return (fullGbx ? await Gbx.ParseAsync(stream, cancellationToken: cancellationToken) : Gbx.ParseHeader(stream), stream.Length);

            case FileKind.Pak:
                return await ParsePakAsync(stream, name, entryPath, fullGbx, owned, cancellationToken);

            case FileKind.Zip:
                return await ParseZipAsync(stream, name, entryPath, fullGbx, owned, cancellationToken);

            default:
                throw new InvalidDataException("Unsupported file format. Expected a Gbx (GBX), PAK (NadeoPak) or ZIP file.");
        }
    }

    private static async Task<(object Value, long FileSize)> ParsePakAsync(
        Stream stream, string name, string? entryPath, bool fullGbx, List<object> owned, CancellationToken cancellationToken)
    {
        var (pak, hasKey) = await PakLoader.ParseAsync(stream, name, cancellationToken);
        owned.Add(pak);

        if (entryPath is null)
        {
            return (pak, stream.Length);
        }

        if (pak.IsHeaderPrivate && !hasKey)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        var pakFile = pak.Find(entryPath)
            ?? throw new FileNotFoundException($"File '{entryPath}' was not found in PAK '{name}'.", name);

        object gbx = fullGbx ? await pak.OpenGbxFileAsync(pakFile, cancellationToken: cancellationToken) : pak.OpenGbxFileHeader(pakFile);

        return (gbx, pakFile.UncompressedSize);
    }

    private static async Task<(object Value, long FileSize)> ParseZipAsync(
        Stream stream, string name, string? entryPath, bool fullGbx, List<object> owned, CancellationToken cancellationToken)
    {
        var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
        owned.Add(zip);

        if (entryPath is null)
        {
            return (zip, stream.Length);
        }

        var (entry, innerPath) = FindZipEntry(zip, name, entryPath);

        // Gbx and PAK parsing need a seekable stream, which a compressed entry stream is not.
        var memory = new MemoryStream(capacity: (int)Math.Min(entry.Length, int.MaxValue));
        owned.Add(memory);

        await using (var entryStream = entry.Open())
        {
            await entryStream.CopyToAsync(memory, cancellationToken);
        }

        memory.Position = 0;

        // The entry can be an archive itself, so whatever path is left continues inside of it.
        return await ParseAsync(memory, entry.FullName, innerPath, fullGbx, owned, cancellationToken);
    }

    /// <summary>
    /// Finds the entry matching the longest leading part of <paramref name="entryPath"/>
    /// and returns the rest of the path, which points inside of that entry.
    /// </summary>
    private static (ZipArchiveEntry Entry, string? InnerPath) FindZipEntry(ZipArchive zip, string name, string entryPath)
    {
        var pattern = entryPath.Replace('\\', '/').Trim('/');

        for (var end = pattern.Length; end > 0; end = pattern.LastIndexOf('/', end - 1))
        {
            var prefix = pattern[..end];
            var matches = zip.Select(prefix).ToArray();

            if (matches.Length > 1)
            {
                throw new InvalidDataException($"ZIP entry '{prefix}' is ambiguous ({matches.Length} matches). Specify a full path. First matches: {string.Join(", ", matches.Take(10).Select(entry => entry.FullName))}");
            }

            if (matches.Length == 1)
            {
                return (matches[0], end < pattern.Length ? pattern[(end + 1)..] : null);
            }
        }

        throw new FileNotFoundException($"File '{entryPath}' was not found in ZIP '{name}'.", name);
    }

    private static FileKind GetKind(ReadOnlySpan<byte> signature)
    {
        if (signature.StartsWith("GBX"u8))
        {
            return FileKind.Gbx;
        }

        if (signature.StartsWith("NadeoPak"u8))
        {
            return FileKind.Pak;
        }

        // Local file header, empty archive and spanned archive markers
        if (signature.StartsWith("PK\x03\x04"u8) || signature.StartsWith("PK\x05\x06"u8) || signature.StartsWith("PK\x07\x08"u8))
        {
            return FileKind.Zip;
        }

        return FileKind.Unknown;
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
}
