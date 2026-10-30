using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSoundEngine2LayoutTests
{
    [Test]
    public async Task ReadWrite_Chunk001Version5_ReadsLowPassFiltersAfterInlineCurves()
    {
        var node = new CPlugSoundEngine2();
        var chunk = new CPlugSoundEngine2.Chunk09029001();
        var payload = Payload(w =>
        {
            w.Write(5);
            w.Write(9000f);
            for (var i = 0; i < 4; i++) WriteCurve(w);
            for (var i = 0; i < 6; i++) WriteCurve(w); // Perspective volumes.
            w.Write(1); // Loops_RpmClamp
            w.Write(0); // Loops_UseVolumeCorrection
            w.Write(1.5f); // RpmGamma
            WriteCurve(w);
            WriteCurve(w);
            WriteLowPassFilters(w);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.RpmMaxFromEngine).IsEqualTo(9000f);
        await Assert.That(node.RpmGamma).IsEqualTo(1.5f);
        await AssertLowPassFilters(node);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_Chunk001Version4_ReadsLowPassFiltersAfterDirectCurves()
    {
        var node = new CPlugSoundEngine2();
        var chunk = new CPlugSoundEngine2.Chunk09029001();
        var payload = Payload(w =>
        {
            w.Write(4);
            w.Write(9000f);
            for (var i = 0; i < 4; i++) w.Write(0xFACADE01);
            for (var i = 0; i < 6; i++) w.Write(0xFACADE01);
            w.Write(1);
            w.Write(0);
            w.Write(1.5f);
            w.Write(0xFACADE01); // Volume_Gear
            w.Write(0xFACADE01); // RpmFactor_Gear
            WriteLowPassFilters(w);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Volume_Gear).IsNotNull();
        await AssertLowPassFilters(node);
    }

    [Test]
    public async Task ReadWrite_Chunk002Version4_ReadsCutoffBeforeLoopVolumes()
    {
        var node = new CPlugSoundEngine2();
        var chunk = new CPlugSoundEngine2.Chunk09029002();
        var payload = Payload(w =>
        {
            w.Write(4);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++) w.Write(-1); // Motors and loops.
                w.Write(0.5f + i);
                w.Write(0.8f);
                w.Write(0.6f);
            }
            w.Write(0xFACADE01);
            w.Write(0xFACADE01);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.AudioMotors![2].LowPassCutoffRatio).IsEqualTo(2.5f);
        await Assert.That(node.AudioMotors[0].IdleVolume).IsEqualTo(0.8f);
        await Assert.That(node.AudioMotors[0].LimiterVolume).IsEqualTo(0.6f);
    }

    [Test]
    public async Task ReadWrite_Chunk002Version6_SkipsLegacyCutoff()
    {
        var node = new CPlugSoundEngine2();
        var chunk = new CPlugSoundEngine2.Chunk09029002();
        var payload = Payload(w =>
        {
            w.Write(6);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++) w.Write(-1);
                w.Write(0.8f);
                w.Write(0.6f);
            }
            WriteCurve(w);
            WriteCurve(w);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.AudioMotors![1].IdleVolume).IsEqualTo(0.8f);
        await Assert.That(node.AudioMotors[1].LimiterVolume).IsEqualTo(0.6f);
    }

    private static void WriteLowPassFilters(BinaryWriter w)
    {
        w.Write(0.5f);
        w.Write(1);
        w.Write(1f);
        w.Write(0);
        w.Write(0.7f);
        w.Write(1);
    }

    private static async Task AssertLowPassFilters(CPlugSoundEngine2 node)
    {
        await Assert.That(node.LowPassFilters!.Length).IsEqualTo(3);
        await Assert.That(node.LowPassFilters[0].CutoffRatio).IsEqualTo(0.5f);
        await Assert.That(node.LowPassFilters[0].Enabled).IsTrue();
        await Assert.That(node.LowPassFilters[1].Enabled).IsFalse();
        await Assert.That(node.LowPassFilters[2].CutoffRatio).IsEqualTo(0.7f);
    }

    private static void WriteCurve(BinaryWriter w)
    {
        w.Write(3); // Curve version.
        w.Write(1);
        w.Write((ushort)0);
        w.Write(0f);
        w.Write(1f);
    }
}
