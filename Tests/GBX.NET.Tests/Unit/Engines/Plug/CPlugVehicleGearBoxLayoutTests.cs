using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleGearBoxLayoutTests
{
    [Test]
    public async Task ReadWrite_Version2_KeepsNativeOrder()
    {
        var node = new CPlugVehicleGearBox();
        var chunk = new CPlugVehicleGearBox.Chunk09094001();
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(3);
            foreach (var value in new[] { 300f, 200f, 100f }) w.Write(value);
            w.Write(1); // Sequential
            w.Write(500);
            w.Write(60);
            foreach (var value in new[] { 90f, 7500f, 6000f, 0.2f, 0.5f, 1.6f }) w.Write(value);
            w.Write(80f);
            w.Write(1.2f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.GearRatios).IsEquivalentTo([300f, 200f, 100f], CollectionOrdering.Matching);
        await Assert.That(node.Sequential).IsTrue();
        await Assert.That(node.ChangeGearPeriod).IsEqualTo(500);
        await Assert.That(node.ChangeGearDeclutchDuration).IsEqualTo(60);
        await Assert.That(node.MinSpeedKmhForDrive).IsEqualTo(90f);
        await Assert.That(node.OptimalRpm_Throttle).IsEqualTo(7500f);
        await Assert.That(node.OptimalRpm_Release).IsEqualTo(6000f);
        await Assert.That(node.MotorRpmInertiaOnLoad).IsEqualTo(0.2f);
        await Assert.That(node.MotorRpmInertiaDecluthGas).IsEqualTo(0.5f);
        await Assert.That(node.MotorRpmInertiaDecluthFree).IsEqualTo(1.6f);
        await Assert.That(node.TakeOffSpeedKmh).IsEqualTo(80f);
        await Assert.That(node.RpmGammaCorrection).IsEqualTo(1.2f);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP4 | GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_LegacyChunk_ReadsGearRatiosAfterDiscardedValues()
    {
        var node = new CPlugVehicleGearBox();
        var chunk = new CPlugVehicleGearBox.Chunk09094000();
        var payload = Payload(w =>
        {
            w.Write(0);
            for (var i = 0; i < 8; i++) w.Write(i + 0.5f);
            w.Write(2);
            w.Write(250f);
            w.Write(150f);
            w.Write(1);
            w.Write(1f);
            w.Write(0);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.GearRatios).IsEquivalentTo([250f, 150f], CollectionOrdering.Matching);
        await Assert.That(chunk.U08).IsEqualTo(7.5f);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugVehicleGearBox();

        await Assert.That(node.GearRatios).IsEquivalentTo([338f, 297f, 207f, 143f, 100f, 84f, 56f], CollectionOrdering.Matching);
        await Assert.That(node.Sequential).IsFalse();
        await Assert.That(node.ChangeGearPeriod).IsEqualTo(400);
        await Assert.That(node.ChangeGearDeclutchDuration).IsEqualTo(50);
        await Assert.That(node.MinSpeedKmhForDrive).IsEqualTo(100f);
        await Assert.That(node.OptimalRpm_Throttle).IsEqualTo(7000f);
        await Assert.That(node.OptimalRpm_Release).IsEqualTo(6500f);
        await Assert.That(node.MotorRpmInertiaOnLoad).IsEqualTo(0.1f);
        await Assert.That(node.MotorRpmInertiaDecluthGas).IsEqualTo(0.4f);
        await Assert.That(node.MotorRpmInertiaDecluthFree).IsEqualTo(1.5f);
        await Assert.That(node.TakeOffSpeedKmh).IsEqualTo(85f);
        await Assert.That(node.RpmGammaCorrection).IsEqualTo(1f);
    }
}
