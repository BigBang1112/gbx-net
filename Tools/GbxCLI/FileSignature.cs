namespace GbxCLI;

internal static class FileSignature
{
    /// <summary>
    /// Detects the file type from its signature and moves the stream back to the start.
    /// </summary>
    public static async Task<FileKind> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var signature = new byte[8];
        var length = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false, cancellationToken);
        stream.Position = 0;

        return Detect(signature.AsSpan(0, length));
    }

    public static FileKind Detect(ReadOnlySpan<byte> signature)
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
}
