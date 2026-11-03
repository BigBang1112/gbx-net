using SimplestGitSourceGenerator;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Serialization;

internal static class GbxSignatureChunk
{
    internal const uint Id = 0x01112000;

    private const uint Skip = 0x534B4950;
    private const uint Facade = 0xFACADE01;
    private const uint Magic = 0x4E584247; // GBXN
    private const int Version = 1;
    private const int HashSize = 32;
    private const int MinimumPayloadSize = 64;

    private static readonly byte[] chunkIdBytes = [0x00, 0x20, 0x11, 0x01];
    private static readonly byte[] chunkPrefixBytes = [0x00, 0x20, 0x11, 0x01, 0x50, 0x49, 0x4B, 0x53];
    private static readonly Encoding encoding = new UTF8Encoding(false, true);

    internal static bool TryAppend(MemoryStream body)
    {
        var length = checked((int)body.Length);
        if (length < 4 || BinaryPrimitives.ReadUInt32LittleEndian(body.GetBuffer().AsSpan(length - 4)) != Facade)
        {
            return false;
        }

        // Replace only the root terminator. Nested nodes have already been serialized.
        body.Position = length - 4;
        using var writer = new GbxWriter(body);
        writer.Write(Id);
        writer.Write(Skip);
        writer.Write(MinimumPayloadSize + encoding.GetByteCount(SimplestGit.CommitHash) + encoding.GetByteCount(SimplestGit.Tag));
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write(SimplestGit.CommitHash);
        writer.Write(SimplestGit.Tag);
        writer.Write(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        writer.Write(body.Position + sizeof(long));
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
