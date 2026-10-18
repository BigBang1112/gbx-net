using GBX.NET;
using GBX.NET.PAK;
using Spectre.Console;

namespace GbxCLI;

internal sealed class ParsedFile : IAsyncDisposable
{
    private readonly Stream stream;
    private readonly string path;

    public object Value { get; }
    public string RootName => Value is Gbx ? "Gbx" : "Pak";

    private ParsedFile(string path, Stream stream, object value)
    {
        this.path = path;
        this.stream = stream;
        Value = value;
    }

    public static async Task<ParsedFile> OpenAsync(string path, bool fullGbx)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        try
        {
            var signature = new byte[Pak.Magic.Length];
            var bytesRead = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false);
            stream.Position = 0;
            object value;

            if (signature.AsSpan(0, bytesRead).StartsWith("GBX"u8))
            {
                value = fullGbx ? await Gbx.ParseAsync(stream) : Gbx.ParseHeader(stream);
            }
            else if (signature.AsSpan(0, bytesRead).SequenceEqual("NadeoPak"u8))
            {
                value = await Pak.ParseAsync(stream);
            }
            else
            {
                throw new InvalidDataException("Unsupported file format. Expected a Gbx (GBX) or PAK (NadeoPak) file.");
            }

            return new ParsedFile(path, stream, value);
        }
        catch
        {
            await stream.DisposeAsync();
            throw;
        }
    }

    public async Task WriteSummaryAsync(CliConsole console)
    {
        await console.WriteLineAsync($"[grey]File:[/] {Markup.Escape(path)}");

        if (Value is Gbx gbx)
        {
            await console.WriteLineAsync($"[grey]Header:[/] {Markup.Escape($"{gbx.Header}")}");
            await console.WriteLineAsync($"[grey]Version:[/] {gbx.Header.Basic.Version}");
            await console.WriteLineAsync($"[grey]Format:[/] {gbx.Header.Basic.Format}");
            await console.WriteLineAsync($"[grey]Body compression:[/] {gbx.Header.Basic.CompressionOfBody}");
        }
        else if (Value is Pak pak)
        {
            await console.WriteLineAsync($"[grey]Format:[/] {Pak.Magic}");
            await console.WriteLineAsync($"[grey]Version:[/] {pak.Version}");
            await console.WriteLineAsync($"[grey]Header encryption:[/] {(pak.IsHeaderEncrypted ? "Encrypted" : "Unencrypted")}");

            if (pak.IsHeaderPrivate)
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
