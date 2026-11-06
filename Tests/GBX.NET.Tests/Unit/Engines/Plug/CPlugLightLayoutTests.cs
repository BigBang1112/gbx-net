using GBX.NET.Engines.Function;
using GBX.NET.Engines.Graphic;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugLightLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_004_PreservesInheritedReferencesFlagsAndColorTargetTable(bool populated)
    {
        var light = populated ? new GxLightBall { RadiusIndex = 12.5f } : null;
        light?.CreateChunk<GxLightBall.Chunk04002009>();
        var funcLight = populated ? new CFuncLightColor { Color1 = new Vec4(1, 2, 3, 4) } : null;
        funcLight?.CreateChunk<CFuncLightColor.Chunk05019001>();
        var bitmap = populated ? new CPlugBitmap() : null;
        var table = populated ? new CPlugMaterialColorTargetTable
        {
            ColorTargets = [new() { Colors = [0x11223344, 0x55667788], ColorBlindColors = [0x12345678] }]
        } : null;
        var flags = (CPlugLight.EFlags)0x4000001F;
        var payload = Payload(w =>
        {
            w.Write(0); // Chunk version.
            w.WriteNodeRef<GxLight>(light);
            w.WriteNodeRef<CFuncLight>(funcLight);
            w.WriteNodeRef(bitmap);
            w.WriteNodeRef(bitmap); // Shared flare/projector reference.
            w.Write((int)flags);
            w.WriteNodeRef(table);
        });
        var node = new CPlugLight();
        var chunk = new CPlugLight.Chunk0901D004();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await Assert.That(node.Flags).IsEqualTo(flags);
        await Assert.That(node.SceneLightAlwaysActive).IsTrue();
        if (populated)
        {
            await Assert.That(node.GxLightModel).IsTypeOf<GxLightBall>();
            await Assert.That(((GxLightBall)node.GxLightModel!).RadiusIndex).IsEqualTo(12.5f);
            await Assert.That(node.FuncLight).IsTypeOf<CFuncLightColor>();
            await Assert.That(((CFuncLightColor)node.FuncLight!).Color1).IsEqualTo(new Vec4(1, 2, 3, 4));
            await Assert.That(node.BitmapFlare).IsNotNull();
            await Assert.That(node.BitmapProjector).IsSameReferenceAs(node.BitmapFlare);
            await Assert.That(node.ColorTargetTable!.ColorTargets![0].Colors[1]).IsEqualTo(0x55667788);
            await Assert.That(node.ColorTargetTable.ColorTargets[0].ColorBlindColors[0]).IsEqualTo(0x12345678);
        }
        else
        {
            await Assert.That(node.GxLightModel).IsNull();
            await Assert.That(node.FuncLight).IsNull();
            await Assert.That(node.BitmapFlare).IsNull();
            await Assert.That(node.BitmapProjector).IsNull();
            await Assert.That(node.ColorTargetTable).IsNull();
        }
    }

    [Test]
    public async Task Defaults_MatchNativeConstructor()
    {
        var node = new CPlugLight();

        await Assert.That(node.Flags).IsEqualTo(CPlugLight.EFlags.NightOnly);
        await Assert.That(node.NightOnly).IsTrue();
        await Assert.That(node.SceneLightAlwaysActive).IsFalse();
        await Assert.That(node.GxLightModel).IsNull();
        await Assert.That(node.ColorTargetTable).IsNull();
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            serialize(rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
