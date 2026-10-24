using SimplestGitSourceGenerator;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.Serialization;

internal static class GbxValidationChunk
{
    internal const uint Id = 0x01112000;

    private const uint Magic = 0x4E584247; // GBXN
    private const int Version = 1;
    private const int MinimumPayloadSize = 64;
    private const int HashSize = 32;

    private static readonly byte[] ChunkIdBytes = [0x00, 0x20, 0x11, 0x01];
    private static readonly Encoding SignatureEncoding = new UTF8Encoding(false, true);

    internal static void Write(GbxWriter writer)
    {
        var stream = (MemoryStream)writer.BaseStream;
        var size = MinimumPayloadSize + Encoding.UTF8.GetByteCount(SimplestGit.CommitHash)
            + Encoding.UTF8.GetByteCount(SimplestGit.Tag);
        writer.WriteHexUInt32(Id);
        writer.WriteHexUInt32(0x534B4950);
        writer.Write(size);
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write(SimplestGit.CommitHash);
        writer.Write(SimplestGit.Tag);
        writer.Write(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        var byteCount = checked((int)stream.Position + sizeof(long));
        writer.Write((long)byteCount);

        writer.Write(ComputeHash(stream.GetBuffer(), byteCount));
    }

    internal static bool TryRead(byte[] body,
#if NET6_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        out GbxSignature? signature, out GbxSignatureStatus status, GbxReadSettings settings)
    {
        signature = null;
        status = GbxSignatureStatus.Missing;

        // The body need not have a known class layout. Locate a chunk whose framing
        // ends exactly at the root's final marker, then validate its complete payload.
        ReadOnlySpan<byte> search = body;
        var maxSize = settings.MaxSkippableChunkSize ?? GbxReader.MaxDataSize;
        while (!search.IsEmpty)
        {
            var chunkStart = search.LastIndexOf(ChunkIdBytes);
            if (chunkStart < 0)
            {
                break;
            }
            search = search.Slice(0, chunkStart);
            status = GbxSignatureStatus.Malformed;
            var payloadSize = body.Length - chunkStart - 16;
            if (payloadSize < 8
                || payloadSize > maxSize
                || BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(chunkStart + 4)) != 0x534B4950
                || BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(chunkStart + 8)) != payloadSize
                || BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(chunkStart + 12)) != Magic
                || BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(body.Length - 4)) != 0xFACADE01)
            {
                continue;
            }

            var version = BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(chunkStart + 16));
            if (version != Version)
            {
                status = GbxSignatureStatus.Unsupported;
                return false;
            }

            if (payloadSize < MinimumPayloadSize)
            {
                continue;
            }

            var hashStart = body.Length - HashSize - 4;
            var byteCount = BinaryPrimitives.ReadInt64LittleEndian(body.AsSpan(hashStart - 8));
            if (byteCount != hashStart)
            {
                continue;
            }

            ReadOnlySpan<byte> strings = body.AsSpan(chunkStart + 20, hashStart - 16 - chunkStart - 20);
            var maxStringSize = settings.MaxDataSize ?? GbxReader.MaxDataSize;
            if (!TryReadString(ref strings, maxStringSize, out var commitHash)
                || !TryReadString(ref strings, maxStringSize, out var tag)
                || !strings.IsEmpty)
            {
                return false;
            }

            var milliseconds = BinaryPrimitives.ReadInt64LittleEndian(body.AsSpan(hashStart - 16));
            DateTimeOffset timestamp;
            try
            {
                timestamp = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            var hash = new Checksum256(body.AsSpan(hashStart, HashSize));
            if (hash != new Checksum256(ComputeHash(body, hashStart)))
            {
                status = GbxSignatureStatus.Mismatched;
                return false;
            }

            signature = new GbxSignature(version, commitHash, tag, timestamp, byteCount, hash);
            status = GbxSignatureStatus.Valid;
            return true;
        }

        return false;
    }

    private static bool TryReadString(ref ReadOnlySpan<byte> data, int maxSize, out string value)
    {
        value = string.Empty;
        if (data.Length < sizeof(int))
        {
            return false;
        }

        var length = BinaryPrimitives.ReadInt32LittleEndian(data);
        if (length < 0 || length > maxSize || length > data.Length - sizeof(int))
        {
            return false;
        }

        try
        {
#if NET6_0_OR_GREATER
            value = SignatureEncoding.GetString(data.Slice(sizeof(int), length));
#else
            value = SignatureEncoding.GetString(data.Slice(sizeof(int), length).ToArray());
#endif
        }
        catch (DecoderFallbackException)
        {
            return false;
        }
        data = data.Slice(sizeof(int) + length);
        return true;
    }

    private static byte[] ComputeHash(byte[] data, int byteCount)
    {
#if NET6_0_OR_GREATER
        return SHA256.HashData(data.AsSpan(0, byteCount));
#else
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(data, 0, byteCount);
#endif
    }
}
