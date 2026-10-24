using SimplestGitSourceGenerator;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Serialization;

internal static class GbxValidationChunk
{
    internal const uint Id = 0x01112000;

    private const uint Magic = 0x4E584247; // GBXN
    private const int Version = 1;

    internal static void Write(GbxWriter writer)
    {
        // Gbx.Save already buffers the uncompressed body. Hash only the bytes before this chunk,
        // excluding both its own framing and the root node's final FACADE marker.
        var stream = (MemoryStream)writer.BaseStream;
        var byteCount = checked((int)stream.Position);
#if NET6_0_OR_GREATER
        var hash = SHA256.HashData(stream.GetBuffer().AsSpan(0, byteCount));
#else
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(stream.GetBuffer(), 0, byteCount);
#endif

        writer.WriteHexUInt32(Id);
        writer.WriteHexUInt32(0x534B4950);
        writer.Write(56 + Encoding.UTF8.GetByteCount(SimplestGit.CommitHash)
            + Encoding.UTF8.GetByteCount(SimplestGit.Tag));
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write((long)byteCount);
        writer.Write(hash);
        writer.Write(SimplestGit.CommitHash);
        writer.Write(SimplestGit.Tag);
    }
}
