using SimplestGitSourceGenerator;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Serialization;

internal static class GbxSignatureChunk
{
    internal const uint Id = 0xBB111200;

    private const uint Skip = 0x534B4950;
    private const uint Facade = 0xFACADE01;
    private const int Version = 1;
    private const int HashSize = 32;
    private const int MinimumPayloadSize = 60;

    private static readonly Encoding encoding = new UTF8Encoding(false, true);

    internal static GbxSignatureValidationResult Validate(byte[] body, GbxReadSettings settings)
    {
        if (body.Length < 4 || BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(body.Length - 4)) != Facade)
        {
            return new GbxSignatureValidationResult(GbxSignatureStatus.Missing);
        }

        // The body may contain arbitrary node data. Only a final signature chunk is relevant.
        for (var start = body.Length - 16; start >= 0; start--)
        {
            if (BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(start)) != Id
                || BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(start + 4)) != Skip)
            {
                continue;
            }

            var payloadSize = BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(start + 8));
            if (payloadSize < MinimumPayloadSize || payloadSize != body.Length - start - 16
                || payloadSize > (settings.MaxSkippableChunkSize ?? CMwNod.MaxSkippableChunkSize))
            {
                return new GbxSignatureValidationResult(GbxSignatureStatus.Malformed);
            }

            try
            {
                using var stream = new MemoryStream(body, start + 12, payloadSize, writable: false);
                using var reader = new GbxReader(stream, settings with { CloseStream = false });

                var version = reader.ReadInt32();
                if (version != Version)
                {
                    return new GbxSignatureValidationResult(GbxSignatureStatus.Unsupported);
                }

                var commitHash = ReadString(reader);
                var tag = ReadString(reader);
                var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(reader.ReadInt64());
                var byteCount = reader.ReadInt64();
                var hash = reader.ReadChecksum256();
                if (stream.Position != stream.Length || byteCount != body.Length - HashSize - 4)
                {
                    return new GbxSignatureValidationResult(GbxSignatureStatus.Malformed);
                }

                if (hash != new Checksum256(ComputeHash(body, (int)byteCount)))
                {
                    return new GbxSignatureValidationResult(GbxSignatureStatus.Mismatched);
                }

                return new GbxSignatureValidationResult(new GbxSignature(version, commitHash, tag, timestamp, byteCount, hash));
            }
            catch (Exception ex) when (ex is EndOfStreamException or InvalidDataException or ArgumentException or LengthLimitException)
            {
                return new GbxSignatureValidationResult(GbxSignatureStatus.Malformed);
            }
        }

        return new GbxSignatureValidationResult(GbxSignatureStatus.Missing);
    }

    private static string ReadString(GbxReader reader)
    {
        var length = reader.ReadInt32();
        if (length < 0 || length > reader.BaseStream.Length - reader.BaseStream.Position)
        {
            throw new InvalidDataException("Invalid GBX.NET signature string length.");
        }
        return encoding.GetString(reader.ReadBytes(length));
    }

    internal static void Read(GbxReader reader)
    {
        var version = reader.ReadInt32();
        if (version != Version)
        {
            throw new VersionNotSupportedException(version);
        }

        var commitHash = reader.ReadString();
        var tag = reader.ReadString();
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(reader.ReadInt64());
        var byteCount = reader.ReadInt64();
        var hash = reader.ReadChecksum256();

        reader.Signature = new GbxSignature(version, commitHash, tag, timestamp, byteCount, hash);
    }

    internal static bool Write(GbxWriter writer)
    {
        var body = (MemoryStream)writer.BaseStream;
        var length = checked((int)body.Length);
        if (length < 4 || BinaryPrimitives.ReadUInt32LittleEndian(body.GetBuffer().AsSpan(length - 4)) != Facade)
        {
            return false;
        }

        // Replace only the root terminator. Nested nodes have already been serialized.
        writer.BaseStream.Position = length - 4;
        writer.Write(Id);
        writer.Write(Skip);
        writer.Write(MinimumPayloadSize + encoding.GetByteCount(SimplestGit.CommitHash) + encoding.GetByteCount(SimplestGit.Tag));
        writer.Write(Version);
        writer.Write(SimplestGit.CommitHash);
        writer.Write(SimplestGit.Tag);
        writer.Write(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        writer.Write(writer.BaseStream.Position + sizeof(long));
        writer.Write(ComputeHash(body.GetBuffer(), checked((int)body.Position)));
        writer.Write(Facade);
        return true;
    }

    private static byte[] ComputeHash(byte[] data, int length)
    {
#if NET6_0_OR_GREATER
        return SHA256.HashData(data.AsSpan(0, length));
#else
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(data, 0, length);
#endif
    }
}
