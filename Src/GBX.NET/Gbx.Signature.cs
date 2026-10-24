using GBX.NET.Components;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET;

public partial class Gbx
{
    /// <summary>
    /// Reads and verifies the GBX.NET validation signature without parsing the node body.
    /// </summary>
    /// <param name="stream">Gbx data, starting at the stream's current position.</param>
    /// <param name="settings">Read limits and stream ownership settings.</param>
    /// <param name="cancellationToken">Token to cancel reading.</param>
    /// <returns>The signature status and verified metadata, when available.</returns>
    /// <remarks>
    /// The hash covers the uncompressed body through the signature's byte count, excluding the digest and final FACADE marker.
    /// Reading consumes the body. Uncompressed bodies are read to the end of the stream.
    /// Compressed bodies require <see cref="LZO"/>. I/O, decompression, cancellation, and body read-limit errors can throw.
    /// </remarks>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<GbxSignatureReadResult> ReadSignatureAsync(
        Stream stream,
        GbxReadSettings settings = default,
        CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));
        using var reader = new GbxReader(stream, settings with { SkipUserData = true, ReadRawBody = false });
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var header = GbxHeader.Parse(reader, out _);
            if (header.Basic.Format != GbxFormat.Binary)
            {
                return new GbxSignatureReadResult(GbxSignatureStatus.Unsupported, null);
            }
            GbxRefTable.Parse(reader, header, fileSystemPath: null);
            var body = await GbxBody.ParseAsync(reader, header.Basic.CompressionOfBody, cancellationToken);
            byte[] data;

            if (body.CompressedSize is int compressedSize)
            {
                var lzo = LZO ?? throw new LzoNotDefinedException();
                var compressed = await reader.ReadBytesAsync(compressedSize, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                data = new byte[body.UncompressedSize];
                lzo.Decompress(compressed, data);
            }
            else
            {
                using var output = new MemoryStream();
                var buffer = new byte[81920];
                var maxSize = settings.MaxUncompressedBodySize ?? GbxReader.MaxDataSize;
                int count;
                while ((count = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    if (output.Length + count > maxSize)
                    {
                        throw new LengthLimitException($"Uncompressed body size exceeds maximum allowed size {maxSize}.");
                    }
                    output.Write(buffer, 0, count);
                }
                data = output.ToArray();
            }

            cancellationToken.ThrowIfCancellationRequested();
            GbxValidationChunk.TryRead(data, out var signature, out var status, settings);
            return new GbxSignatureReadResult(status, signature);
        }
        catch (Exception ex) when (ex is FormatNotSupportedException or VersionNotSupportedException)
        {
            return new GbxSignatureReadResult(GbxSignatureStatus.Unsupported, null);
        }
        catch (Exception ex) when (ex is NotAGbxException or EndOfStreamException or InvalidDataException)
        {
            return new GbxSignatureReadResult(GbxSignatureStatus.Malformed, null);
        }
    }

    /// <summary>
    /// Reads and verifies the GBX.NET validation signature in a file without parsing its node body.
    /// </summary>
    /// <param name="filePath">Path of the Gbx file.</param>
    /// <param name="settings">Read limits.</param>
    /// <param name="cancellationToken">Token to cancel reading.</param>
    /// <returns>The signature status and verified metadata, when available.</returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<GbxSignatureReadResult> ReadSignatureAsync(
        string filePath,
        GbxReadSettings settings = default,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
#if SYNC_ONLY
        using var stream = global::System.IO.File.OpenRead(filePath);
#else
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
#endif
        return await ReadSignatureAsync(stream, settings, cancellationToken);
    }

    /// <summary>
    /// Reads and verifies the GBX.NET validation signature without parsing the node body.
    /// </summary>
    /// <param name="stream">Gbx data, starting at the stream's current position.</param>
    /// <param name="signature">The signature metadata on success, otherwise <see langword="null"/>.</param>
    /// <param name="settings">Read limits and stream ownership settings.</param>
    /// <returns><see langword="true"/> if a supported signature is present and its SHA-256 hash matches.</returns>
    /// <remarks>
    /// Returns <see langword="false"/> for missing, malformed, unsupported, or mismatched signatures.
    /// Use the overload with an output <see cref="GbxSignatureStatus"/> to distinguish these cases.
    /// The hash covers the uncompressed body through the signature's byte count, excluding the digest and final FACADE marker.
    /// Reading consumes the body. Uncompressed bodies are read to the end of the stream.
    /// Compressed bodies require <see cref="LZO"/>. I/O, decompression, and read-limit errors can throw.
    /// </remarks>
    public static bool TryReadSignature(Stream stream,
#if NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        out GbxSignature? signature, GbxReadSettings settings = default)
    {
        return TryReadSignature(stream, out signature, out _, settings);
    }

    /// <summary>
    /// Reads and verifies the GBX.NET validation signature and reports its status without parsing the node body.
    /// </summary>
    /// <param name="stream">Gbx data, starting at the stream's current position.</param>
    /// <param name="signature">The verified signature metadata on success, otherwise <see langword="null"/>.</param>
    /// <param name="status">Whether the signature is valid, missing, malformed, unsupported, or mismatched.</param>
    /// <param name="settings">Read limits and stream ownership settings.</param>
    /// <returns><see langword="true"/> only when <paramref name="status"/> is <see cref="GbxSignatureStatus.Valid"/>.</returns>
    /// <remarks>
    /// Reading consumes the body. Uncompressed bodies are read to the end of the stream.
    /// Compressed bodies require <see cref="LZO"/>. I/O, decompression, and body read-limit errors can throw.
    /// </remarks>
    public static bool TryReadSignature(Stream stream,
#if NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        out GbxSignature? signature, out GbxSignatureStatus status, GbxReadSettings settings = default)
    {
        var result = ReadSignature(stream, settings);
        signature = result.Signature;
        status = result.Status;
        return status == GbxSignatureStatus.Valid && signature is not null;
    }

    /// <summary>
    /// Reads and verifies the GBX.NET validation signature in a file without parsing its node body.
    /// </summary>
    /// <param name="filePath">Path of the Gbx file.</param>
    /// <param name="signature">The signature metadata on success, otherwise <see langword="null"/>.</param>
    /// <param name="settings">Read limits.</param>
    /// <returns><see langword="true"/> if a supported signature is present and its SHA-256 hash matches.</returns>
    public static bool TryReadSignature(string filePath,
#if NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        out GbxSignature? signature, GbxReadSettings settings = default)
    {
        return TryReadSignature(filePath, out signature, out _, settings);
    }

    /// <summary>
    /// Reads and verifies the GBX.NET validation signature in a file and reports its status without parsing its node body.
    /// </summary>
    /// <param name="filePath">Path of the Gbx file.</param>
    /// <param name="signature">The verified signature metadata on success, otherwise <see langword="null"/>.</param>
    /// <param name="status">Whether the signature is valid, missing, malformed, unsupported, or mismatched.</param>
    /// <param name="settings">Read limits.</param>
    /// <returns><see langword="true"/> only when <paramref name="status"/> is <see cref="GbxSignatureStatus.Valid"/>.</returns>
    public static bool TryReadSignature(string filePath,
#if NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        out GbxSignature? signature, out GbxSignatureStatus status, GbxReadSettings settings = default)
    {
        var result = ReadSignature(filePath, settings);
        signature = result.Signature;
        status = result.Status;
        return status == GbxSignatureStatus.Valid && signature is not null;
    }
}
