using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugGpuCompileCacheTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    public async Task ReadWrite_Version18_ReadsCompileDataAndPreservesBytes()
    {
        var payload = Version18Payload();
        var node = new CPlugGpuCompileCache();
        var chunk = new CPlugGpuCompileCache.Chunk09053008();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(18);
        await Assert.That(node.Type).IsEqualTo(2);
        await Assert.That(node.EntryPoint).IsEqualTo("psMain");
        await Assert.That(node.Hashes).IsEquivalentTo([0x0123456789ABCDEFUL]);
        await Assert.That(node.Includes?.Single().Path).IsEqualTo("Common.hlsli");
        await Assert.That(node.HasSlots).IsTrue();
        await Assert.That(node.Slots).IsNotNull();
        await Assert.That(node.Data?.Params?.Length).IsEqualTo(1);
        await Assert.That(node.Data?.Defines?.Single().Name).IsEqualTo("DGbxVersion");
        await Assert.That(node.Data?.Defines?.Single().Value).IsEqualTo("2");
        await Assert.That(node.Data?.ByteCode).IsEquivalentTo("DXBC"u8.ToArray());
    }

    [Test]
    public async Task ReadWrite_LegacyChunk_ReadsVersion12CompileData()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(12);
            WriteCompileData(writer, version: 12, type: 1);
            writer.Write(Sentinel);
        }
        var node = new CPlugGpuCompileCache();
        var chunk = new CPlugGpuCompileCache.Chunk09053007();

        await Verify(stream.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Data?.Params?.Length).IsEqualTo(1);
        await Assert.That(node.Data?.ByteCode).IsEquivalentTo("DXBC"u8.ToArray());
    }

    private static byte[] Version18Payload()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write(18);
        writer.Write(78);
        writer.Write(1); // v15+
        writer.Write(2); // Type selects 64-bit parameter values.
        writer.Write(0); // v7+
        writer.Write(0x000A000042EE000CUL);
        writer.Write(1);
        writer.Write(0x0123456789ABCDEFUL);
        writer.Write("psMain");
        writer.Write(1);
        writer.Write((ushort)7);
        writer.Write(1); // Includes
        writer.Write("Common.hlsli");
        writer.Write(0x1122334455667788UL);
        writer.Write(3); // v8+
        writer.Write(4); // v11+
        writer.Write(""); // v12+
        writer.Write(1);
        writer.Write(5);
        writer.Write(1); // HasSlots
        writer.Write(0);
        writer.Write(1);
        writer.Write(9UL);
        writer.Write(1);
        writer.Write(10);
        writer.Write(0);
        WriteCompileData(writer, version: 18, type: 2);
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static void WriteCompileData(GbxWriter writer, int version, int type)
    {
        writer.Write(1); // Params
        for (var i = 0; i < 4; i++) writer.Write(i + 1);
        writer.Write(5);
        writer.Write(6);
        writer.Write((ushort)0x500);
        writer.Write(7); // v3+
        writer.Write(8); // v9+
        writer.Write(9); // v5+
        writer.Write(10); // v8+
        if (version is >= 11 and <= 13)
        {
            writer.Write(11);
            writer.Write(12);
        }
        if (version >= 14 && type == 2)
        {
            writer.Write(11UL);
            writer.Write(12UL);
        }
        writer.Write(13); // v12+
        if (version >= 16)
        {
            writer.Write(14);
            writer.Write(15);
        }
        writer.Write(1); // Defines
        writer.WriteIdAsString("DGbxVersion");
        writer.Write("2");
        writer.Write(4);
        writer.Write("DXBC"u8);
        writer.Write(0); // v10+
        if (version >= 16)
        {
            writer.Write(0);
            writer.Write(0);
        }
        if (version >= 18)
        {
            writer.Write((byte)1);
        }
        writer.Write(0); // v7+ ids
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
