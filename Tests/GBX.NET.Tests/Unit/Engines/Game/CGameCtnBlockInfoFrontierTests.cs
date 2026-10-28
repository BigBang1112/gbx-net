using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoFrontierTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_FrontierFlag_ReadsFourBytesAndWritesTrue(bool flag)
    {
        const int sentinel = 0x12345678;
        var node = new CGameCtnBlockInfoFrontier();
        var chunk = new CGameCtnBlockInfoFrontier.Chunk03050000();

        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(flag);
            writer.Write(sentinel);
        }
        input.Position = 0;

        using (var reader = new GbxReader(input))
        {
            using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
            await Assert.That(chunk.U01).IsEqualTo(flag);
            await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
            writer.Write(sentinel);
        }
        await Assert.That(output.ToArray().SequenceEqual(new byte[]
        {
            1, 0, 0, 0, 0x78, 0x56, 0x34, 0x12
        })).IsTrue();
        await Assert.That(chunk.U01).IsTrue();
    }

    [Test]
    public async Task Constructor_FrontierDefaults_MatchNativeConstructor()
    {
        var node = new CGameCtnBlockInfoFrontier();
        await Assert.That(node.IsInternal).IsTrue();
        await Assert.That(node.CatalogPosition).IsEqualTo(-1);
        await Assert.That(new CGameCtnBlockInfoFrontier.Chunk03050000().U01).IsFalse();
    }
}
