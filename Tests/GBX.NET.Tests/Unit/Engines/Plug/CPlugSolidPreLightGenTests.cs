using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSolidPreLightGenTests
{
    [Test]
    [Arguments(0, 0, true)]
    [Arguments(1, 0, true)]
    [Arguments(2, 0, true)]
    [Arguments(3, 0, true)]
    [Arguments(3, 1, true)]
    [Arguments(3, 1, false)]
    public async Task ReadWrite_PreLightGen_PreservesFieldOrderAndIndependentArchiveVersion(int chunkVersion, int archiveVersion, bool hasData)
    {
        using var input = new MemoryStream();
        using (var binary = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true))
        {
            binary.Write(0x09005017u);
            binary.Write(chunkVersion);
            if (chunkVersion >= 3) binary.Write(hasData ? 1 : 0);
            if (chunkVersion < 3 || hasData)
            {
                if (chunkVersion >= 3)
                {
                    binary.Write(archiveVersion);
                    binary.Write(4); // TileCountU is 32 bits in the archive.
                }
                else
                {
                    binary.Write((byte)4);
                }

                binary.Write(2.5f); // MeterByUv.
                binary.Write(1); // IsComputed.
                for (var i = 1; i <= 8; i++) binary.Write((float)i); // UvBBox and UvBBoxSH.
                binary.Write(9);
                binary.Write(10); // SpriteCount.
                if (chunkVersion >= 1)
                {
                    binary.Write(1); // ProbeBoxes count.
                    for (var i = 11; i <= 16; i++) binary.Write((float)i);
                }
                if (chunkVersion >= 3 && archiveVersion >= 1)
                {
                    binary.Write(1); // UvGroups count.
                    for (var i = 17; i <= 21; i++) binary.Write((float)i);
                }
            }
            if (chunkVersion >= 2) binary.Write(132537600000000000L); // FileWriteTime.
            binary.Write(0xFACADE01u);
        }
        var payload = input.ToArray();
        input.Position = 0;
        var node = new CPlugSolid();
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        node.Read(readWrite);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        if (chunkVersion >= 3) await Assert.That(node.HasPreLightGen).IsEqualTo(hasData);

        if (chunkVersion >= 3 && hasData)
        {
            var data = node.SolidPreLightGen!;
            await Assert.That(data.Version).IsEqualTo(archiveVersion);
            await Assert.That(data.TileCountU).IsEqualTo(4);
            await Assert.That(data.MeterByUv).IsEqualTo(2.5f);
            await Assert.That(data.IsComputed).IsTrue();
            await Assert.That(data.UvBBox).IsEqualTo(new Rect(1, 2, 3, 4));
            await Assert.That(data.UvBBoxSH).IsEqualTo(new Rect(5, 6, 7, 8));
            await Assert.That(data.SpriteCount).IsEqualTo(new Int2(9, 10));
            await Assert.That(data.ProbeBoxes![0]).IsEqualTo(new BoxAligned(11, 12, 13, 14, 15, 16));
            if (archiveVersion >= 1)
            {
                await Assert.That(data.UvGroups![0].MeterByUv).IsEqualTo(17f);
                await Assert.That(data.UvGroups[0].UvBBox).IsEqualTo(new Rect(18, 19, 20, 21));
            }
            else
            {
                await Assert.That(data.UvGroups).IsNull();
            }
        }
        else if (chunkVersion >= 3)
        {
            await Assert.That(node.SolidPreLightGen).IsNull();
        }
        else
        {
            await Assert.That(node.TileCountU).IsEqualTo((byte)4);
            await Assert.That(node.MeterByUv).IsEqualTo(2.5f);
            await Assert.That(node.UvBBox).IsEqualTo(new Rect(1, 2, 3, 4));
            await Assert.That(node.SpriteCount).IsEqualTo(new Int2(9, 10));
        }

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_PreLightGen_UsesNativeDefaults()
    {
        var data = new CPlugSolid.PreLightGen();
        var emptyBounds = new Rect(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
        await Assert.That(data.MeterByUv).IsEqualTo(-1f);
        await Assert.That(data.UvBBox).IsEqualTo(emptyBounds);
        await Assert.That(data.UvBBoxSH).IsEqualTo(emptyBounds);
        await Assert.That(data.TileCountU).IsEqualTo(0);
        await Assert.That(data.IsComputed).IsFalse();
        await Assert.That(data.SpriteCount).IsEqualTo(new Int2());
        await Assert.That(new CPlugSolid.Chunk09005017(GameVersion.TM2020).Version).IsEqualTo(3);
    }
}
