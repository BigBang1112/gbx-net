using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoVariantChunkTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 2)]
    [Arguments(1, 0)]
    [Arguments(1, 2)]
    public async Task ReadWrite_WaterVolumes_PreservesVersionedPayload(int version, int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(count);
            for (var i = 0; i < count; i++)
            {
                writer.Write(i + 1);
                for (var j = 0; j <= i; j++)
                {
                    for (var coordinate = 0; coordinate < 6; coordinate++) writer.Write(j * 10 + coordinate);
                }
                for (var component = 0; component < 7; component++) writer.Write(i * 10f + component + 0.5f);
                if (version >= 1) writer.WriteIdAsString("Water");
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B00B();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.WaterVolumes!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            var volume = node.WaterVolumes[i];
            await Assert.That(volume.Boxes!.Length).IsEqualTo(i + 1);
            await Assert.That(volume.Boxes[i]).IsEqualTo(new BoxInt3(i * 10, i * 10 + 1, i * 10 + 2, i * 10 + 3, i * 10 + 4, i * 10 + 5));
            await Assert.That(volume.Offset).IsEqualTo(new Vec3(i * 10 + 0.5f, i * 10 + 1.5f, i * 10 + 2.5f));
            await Assert.That(volume.RotationY).IsEqualTo(i * 10 + 3.5f);
            await Assert.That(volume.BoxSize).IsEqualTo(new Vec3(i * 10 + 4.5f, i * 10 + 5.5f, i * 10 + 6.5f));
            await Assert.That(volume.WaterId).IsEqualTo(version >= 1 ? "Water" : null);
        }
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 2)]
    [Arguments(1, 0)]
    [Arguments(1, 2)]
    [Arguments(2, 0)]
    [Arguments(2, 2)]
    public async Task ReadWrite_SubMapSpots_PreservesVersionedPayload(int version, int count)
    {
        var location = new Iso4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(count);
            for (var i = 0; i < count; i++)
            {
                writer.Write(location with { TX = i });
                if (version >= 1) writer.WriteIdAsString("Stadium");
                if (version >= 2) writer.WriteIdAsString("LegacySpot");
            }
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B00C();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SubMapSpots!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            var spot = node.SubMapSpots[i];
            await Assert.That(spot.Location).IsEqualTo(location with { TX = i });
            await Assert.That(spot.EnvironmentId).IsEqualTo(version >= 1 ? "Stadium" : null);
            await Assert.That(spot.U01).IsEqualTo(version >= 2 ? "LegacySpot" : null);
        }
    }

    [Test]
    [Arguments(0, "")]
    [Arguments(0, "DefaultItem")]
    [Arguments(1, "")]
    [Arguments(1, "DefaultItem")]
    public async Task ReadWrite_DefaultPlacementItem_PreservesId(int version, string item)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.WriteIdAsString(item);
            writer.Write(Sentinel);
        }

        var node = new CGameCtnBlockInfoVariant();
        var chunk = new CGameCtnBlockInfoVariant.Chunk0315B00D();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.DefaultPlacementItem).IsEqualTo(item);
    }

    [Test]
    public async Task Constructor_SubMapChunk_PreservesWriterVersionAndLegacyReadLimit()
    {
        await Assert.That(new CGameCtnBlockInfoVariant.Chunk0315B00C(GameVersion.MP4).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnBlockInfoVariant.Chunk0315B00C().Version).IsEqualTo(2);
        await Assert.That(new CGameCtnBlockInfoVariant.Chunk0315B00C().GameVersion).IsEqualTo(GameVersion.MP4);
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
