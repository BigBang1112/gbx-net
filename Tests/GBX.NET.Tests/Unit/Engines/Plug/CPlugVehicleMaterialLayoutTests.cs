using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleMaterialLayoutTests
{
    [Test]
    public async Task ReadWrite_Version14_KeepsNativeOrderAndInlineCurves()
    {
        var node = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F1010();
        var payload = Payload(w =>
        {
            w.Write(14);
            WriteBase(w);
            w.Write(0.1f); // U21
            WriteCurve(w, 9f);
            WriteCurve(w, 10f);
            w.Write(11f); // U25
            WriteCurve(w, 13f);
            WriteCurve(w, 13.5f);
            w.Write(0.2f);
            w.Write(0.3f);
            w.Write(4); // U30
            WriteCurve(w, 14f);
            w.Write(5); // U32
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await AssertBase(node);
        await Assert.That(chunk.U21).IsEqualTo(0.1f);
        await Assert.That(chunk.U23!.Values![0].Y).IsEqualTo(9f);
        await Assert.That(chunk.U24!.Values![0].Y).IsEqualTo(10f);
        await Assert.That(chunk.U25).IsEqualTo(11f);
        await Assert.That(chunk.U26!.Values![0].Y).IsEqualTo(13f);
        await Assert.That(chunk.U27!.Values![0].Y).IsEqualTo(13.5f);
        await Assert.That(chunk.U28).IsEqualTo(0.2f);
        await Assert.That(chunk.U29).IsEqualTo(0.3f);
        await Assert.That(chunk.U30).IsEqualTo(4);
        await Assert.That(chunk.U31!.Values![0].Y).IsEqualTo(14f);
        await Assert.That(chunk.U32).IsEqualTo(5);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_Version11_ReadsLegacyValueAndCurveReferences()
    {
        var node = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F1010();
        var payload = Payload(w =>
        {
            w.Write(11);
            WriteBase(w);
            w.Write(0.1f); // U21
            w.Write(0.9f); // U22
            w.Write(-1);
            w.Write(-1);
            w.Write(11f); // U25
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await AssertBase(node);
        await Assert.That(chunk.U21).IsEqualTo(0.1f);
        await Assert.That(chunk.U22).IsEqualTo(0.9f);
        await Assert.That(chunk.U25).IsEqualTo(11f);
    }

    [Test]
    public async Task ReadWrite_Version1_ReadsVibrationAfterMaterialId()
    {
        var node = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F1010();
        var payload = Payload(w =>
        {
            w.Write(1);
            foreach (var value in new[] { 1.1f, 1.2f, 1.3f, 1.4f }) w.Write(value);
            w.Write((byte)3);
            w.Write(0.6f);
            w.Write(0.7f);
            foreach (var value in new[] { 0.01f, 0.02f, 4f }) w.Write(value);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.MaterialId).IsEqualTo((byte)3);
        await Assert.That(node.VibrationSpeedCoef).IsEqualTo(0.6f);
        await Assert.That(node.VibrationMax).IsEqualTo(0.7f);
        await Assert.That(node.M7_BumpCellSize).IsEqualTo(4f);
        await Assert.That(node.M7_BumpReplacementSpeedCoef).IsEqualTo(0.05f);
    }

    [Test]
    public async Task ReadWrite_RumbleChunk_KeepsNativeOrder()
    {
        var node = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F100F();
        var payload = Payload(w =>
        {
            w.Write(3f);
            w.Write(0.8f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.Rumble_BumpsSpacing).IsEqualTo(3f);
        await Assert.That(node.Rumble_VibrationCorrection).IsEqualTo(0.8f);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F1010();

        await Assert.That(node.Speed).IsEqualTo(1f);
        await Assert.That(node.Grip).IsEqualTo(1f);
        await Assert.That(node.AccelerationCoef).IsEqualTo(1f);
        await Assert.That(node.BrakeCoef).IsEqualTo(1f);
        await Assert.That(node.VibrationMax).IsEqualTo(0.5f);
        await Assert.That(node.M7_BumpReplacementSpeedCoef).IsEqualTo(0.05f);
        await Assert.That(node.M6_MaterialHeightDetail_Scale).IsEqualTo(new Vec2(32, 32));
        await Assert.That(node.Drifting_TransfertForceCoef).IsEqualTo(-1f);
        await Assert.That(node.Drifting_InputSteerSensitivityFromSpeed).IsEqualTo(-1f);
        await Assert.That(node.Rumble_BumpsSpacing).IsEqualTo(2.5f);
        await Assert.That(node.Rumble_VibrationCorrection).IsEqualTo(1f);
        await Assert.That(chunk.U25).IsEqualTo(3f);
        await Assert.That(chunk.U29).IsEqualTo(1f);
        await Assert.That(chunk.U30).IsEqualTo(1);
    }

    private static void WriteBase(BinaryWriter w)
    {
        foreach (var value in new[] { 1.1f, 1.2f, 1.3f, 1.4f }) w.Write(value); // Speed, grip, acceleration, brake.
        w.Write((byte)3);
        w.Write(0.6f);
        w.Write(0.7f);
        foreach (var value in new[] { 0.01f, 0.02f, 4f, 0.06f }) w.Write(value); // Bump replacement.
        foreach (var value in new[] { 10f, 100f, 2f }) w.Write(value); // Friction.
        w.Write(1);
        w.Write(16f);
        w.Write(24f);
        w.Write(0.4f);
        w.Write(0.5f);
    }

    private static async Task AssertBase(CPlugVehicleMaterial node)
    {
        await Assert.That(node.Speed).IsEqualTo(1.1f);
        await Assert.That(node.Grip).IsEqualTo(1.2f);
        await Assert.That(node.AccelerationCoef).IsEqualTo(1.3f);
        await Assert.That(node.BrakeCoef).IsEqualTo(1.4f);
        await Assert.That(node.MaterialId).IsEqualTo((byte)3);
        await Assert.That(node.VibrationSpeedCoef).IsEqualTo(0.6f);
        await Assert.That(node.VibrationMax).IsEqualTo(0.7f);
        await Assert.That(node.M7_BumpReplacementMax).IsEqualTo(0.01f);
        await Assert.That(node.M7_BumpReplacementMin).IsEqualTo(0.02f);
        await Assert.That(node.M7_BumpCellSize).IsEqualTo(4f);
        await Assert.That(node.M7_BumpReplacementSpeedCoef).IsEqualTo(0.06f);
        await Assert.That(node.FrictionSpeedMin).IsEqualTo(10f);
        await Assert.That(node.FrictionSpeedMax).IsEqualTo(100f);
        await Assert.That(node.FrictionMax).IsEqualTo(2f);
        await Assert.That(node.M6_MaterialHeightDetail_Enable).IsTrue();
        await Assert.That(node.M6_MaterialHeightDetail_Scale).IsEqualTo(new Vec2(16, 24));
        await Assert.That(node.Drifting_TransfertForceCoef).IsEqualTo(0.4f);
        await Assert.That(node.Drifting_InputSteerSensitivityFromSpeed).IsEqualTo(0.5f);
    }

    private static void WriteCurve(BinaryWriter w, float y)
    {
        w.Write(3); // Curve version.
        w.Write(1);
        w.Write((ushort)0);
        w.Write(0f);
        w.Write(y);
    }
}
