using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Serialization;

internal static class GbxValidationChunk
{
    internal const uint Id = 0x01112000;

    private const uint Magic = 0x4E584247; // GBXN
    private const int Version = 1;
    private static readonly string writerVersion = typeof(Gbx).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;

    internal static void Write(GbxWriter writer)
    {
        // Gbx.Save already buffers the uncompressed body. Hash only the bytes before this chunk,
        // excluding both its own framing and the root node's final FACADE marker.
        var stream = (MemoryStream)writer.BaseStream;
        var byteCount = checked((int)stream.Position);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(stream.GetBuffer(), 0, byteCount);

        writer.WriteHexUInt32(Id);
        writer.WriteHexUInt32(0x534B4950);
        writer.Write(52 + Encoding.UTF8.GetByteCount(writerVersion));
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write(writerVersion);
        writer.Write((long)byteCount);
        writer.Write(hash);
    }
}
