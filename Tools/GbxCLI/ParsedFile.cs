using GBX.NET;
using GBX.NET.PAK;
using Spectre.Console;

namespace GbxCLI;

internal sealed class ParsedFile : IAsyncDisposable
{
    private readonly Stream stream;
    private readonly string path;
    private readonly bool hasPakKey;
    private readonly long fileSize;

    public object Value { get; }
    public string RootName => Value is Gbx ? "Gbx" : "Pak";
    public bool HasPakFileIndex => Value is Pak pak && (!pak.IsHeaderPrivate || hasPakKey);

    private ParsedFile(string path, Stream stream, object value, bool hasPakKey, long? fileSize = null)
    {
        this.path = path;
        this.stream = stream;
        Value = value;
        this.hasPakKey = hasPakKey;
        this.fileSize = fileSize ?? stream.Length;
    }

    public static async Task<ParsedFile> OpenAsync(string path, bool fullGbx)
    {
        var (filePath, entryPath) = ResolveInputPath(path);
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        try
        {
            var signature = new byte[Pak.Magic.Length];
            var bytesRead = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false);
            stream.Position = 0;
            object value;
            var hasPakKey = false;
            long? fileSize = null;

            if (entryPath is not null && !signature.AsSpan(0, bytesRead).SequenceEqual("NadeoPak"u8))
            {
                throw new InvalidDataException($"The archive path '{filePath}' must point to a PAK (NadeoPak) file.");
            }

            if (signature.AsSpan(0, bytesRead).StartsWith("GBX"u8))
            {
                value = fullGbx ? await Gbx.ParseAsync(stream) : Gbx.ParseHeader(stream);
            }
            else if (signature.AsSpan(0, bytesRead).SequenceEqual("NadeoPak"u8))
            {
                var result = await PakLoader.ParseAsync(stream, filePath);
                value = result.Pak;
                hasPakKey = result.HasKey;

                if (entryPath is not null)
                {
                    if (result.Pak.IsHeaderPrivate && !hasPakKey)
                    {
                        throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
                    }

                    var pakFile = new PakFileIndex(result.Pak).Find(entryPath);

                    if (pakFile is null)
                    {
                        throw new FileNotFoundException($"File '{entryPath}' was not found in PAK '{filePath}'.", path);
                    }

                    value = fullGbx ? await result.Pak.OpenGbxFileAsync(pakFile) : result.Pak.OpenGbxFileHeader(pakFile);
                    fileSize = pakFile.UncompressedSize;
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file format. Expected a Gbx (GBX) or PAK (NadeoPak) file.");
            }

            return new ParsedFile(path, stream, value, hasPakKey, fileSize);
        }
        catch
        {
            await stream.DisposeAsync();
            throw;
        }
    }

    internal static (string FilePath, string? EntryPath) ResolveInputPath(string path)
    {
        if (File.Exists(path))
        {
            return (path, null);
        }

        for (var i = path.Length - 1; i > 0; i--)
        {
            if ((path[i] == '/' || path[i] == '\\') && File.Exists(path[..i]))
            {
                return (path[..i], path[(i + 1)..]);
            }
        }

        return (path, null);
    }

    public async Task WriteSummaryAsync(CliConsole console)
    {
        await console.WriteLineAsync($"[grey]File:[/] {Markup.Escape(path)}");

        if (Value is Gbx gbx)
        {
            await console.WriteLineAsync($"[grey]File size:[/] {fileSize:N0} bytes");
            await new GbxSummary(console).WriteAsync(gbx);
        }
        else if (Value is Pak pak)
        {
            await console.WriteLineAsync($"[grey]Format:[/] {Pak.Magic}");
            await console.WriteLineAsync($"[grey]Version:[/] {pak.Version}");
            await console.WriteLineAsync($"[grey]Header encryption:[/] {(pak.IsHeaderEncrypted ? "Encrypted" : "Unencrypted")}");

            if (!HasPakFileIndex)
            {
                await console.WriteLineAsync("[grey]Files:[/] Unavailable (private header requires a key).");
            }
            else
            {
                await console.WriteLineAsync($"[grey]Files:[/] {pak.Files.Count}");
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Value is Pak pak)
            {
                await pak.DisposeAsync();
            }
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }
}
