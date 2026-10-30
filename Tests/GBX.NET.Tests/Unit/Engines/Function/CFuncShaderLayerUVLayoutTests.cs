using GBX.NET.Engines.Function;
using GBX.NET.Serialization.Chunking;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Function;

[Category("Unit")]
public class CFuncShaderLayerUVLayoutTests
{
    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CFuncShaderLayerUV();

        await Assert.That(node.LayerName).IsEqualTo("EnvMap");
        await Assert.That(node.SignalType).IsEqualTo(CFuncShaderLayerUV.EType.TransLinear);
        await Assert.That(node.Amplitude).IsEqualTo(new Vec2(1, 1));
    }

    [Test]
    public async Task ReadWrite_Chunk005_SplitsPackedSignalTypeAndFlags()
    {
        var node = new CFuncShaderLayerUV();
        var chunk = new CFuncShaderLayerUV.Chunk05015005();
        var payload = Payload(w =>
        {
            String(w, "Diffuse");
            w.Write((byte)CFuncShaderLayerUV.EType.TransLinearScaleRotate);
            w.Write((byte)0b11); // EnablePingPong, EnableSmooth
            w.Write((short)0x1234);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.LayerName).IsEqualTo("Diffuse");
        await Assert.That(node.SignalType).IsEqualTo(CFuncShaderLayerUV.EType.TransLinearScaleRotate);
        await Assert.That(node.EnablePingPong).IsTrue();
        await Assert.That(node.EnableSmooth).IsTrue();
        await Assert.That(chunk.U01).IsEqualTo((short)0x1234);
    }

    [Test]
    public async Task ReadWrite_LegacyChunk003_SetsInheritedPeriodAndSignalType()
    {
        var node = new CFuncShaderLayerUV();
        var chunk = new CFuncShaderLayerUV.Chunk05015003();
        var payload = Payload(w =>
        {
            w.Write((byte)CFuncShaderLayerUV.EType.Rotate);
            w.Write((byte)7); // Legacy layer.
            w.Write((short)0);
            w.Write(2.5f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.SignalType).IsEqualTo(CFuncShaderLayerUV.EType.Rotate);
        await Assert.That(node.Period).IsEqualTo(2.5f);
        await Assert.That(chunk.U01).IsEqualTo((byte)7);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.Unspecified);
    }

    [Test]
    public async Task ReadWrite_LegacyChunk00C_StoresEnablePingPongInFlags()
    {
        var node = new CFuncShaderLayerUV();
        var chunk = new CFuncShaderLayerUV.Chunk0501500C();
        var payload = Payload(w =>
        {
            w.Write(16);
            w.Write(4);
            w.Write(1); // EnablePingPong
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.NbSubTexture).IsEqualTo(16);
        await Assert.That(node.NbSubTexturePerLine).IsEqualTo(4);
        await Assert.That(node.EnablePingPong).IsTrue();
        await Assert.That(node.EnableSmooth).IsFalse();
    }

    [Test]
    public async Task ReadWrite_Chunk015_ReadsTopToBottomAsBool()
    {
        var node = new CFuncShaderLayerUV();
        var chunk = new CFuncShaderLayerUV.Chunk05015015();
        var payload = Payload(w =>
        {
            w.Write(16);
            w.Write(4);
            w.Write(5);
            w.Write(1);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.NbSubTexturePerColumn).IsEqualTo(5);
        await Assert.That(node.TopToBottom).IsTrue();
    }

    [Test]
    public async Task ReadWrite_Chunk016_ExposesWriteComponents()
    {
        var node = new CFuncShaderLayerUV();
        var chunk = new CFuncShaderLayerUV.Chunk05015016();
        var payload = Payload(w => w.Write(0b1010u));

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.WriteX).IsFalse();
        await Assert.That(node.WriteY).IsTrue();
        await Assert.That(node.WriteZ).IsFalse();
        await Assert.That(node.WriteW).IsTrue();
    }

    [Test]
    [Arguments(0x00A, GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    [Arguments(0x015, GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    [Arguments(0x016, GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    [Arguments(0x017, GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020)]
    public async Task GameVersion_ConditionalWriters_ListEachWritingGame(int offset, GameVersion expected)
    {
        var chunk = (IChunk)Activator.CreateInstance(
            typeof(CFuncShaderLayerUV).GetNestedType($"Chunk{0x05015000 + offset:X8}")!)!;

        await Assert.That(chunk.GameVersion).IsEqualTo(expected);
    }
}
