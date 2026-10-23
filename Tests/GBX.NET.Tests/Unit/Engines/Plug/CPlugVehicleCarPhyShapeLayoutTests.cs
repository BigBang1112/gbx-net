using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleCarPhyShapeLayoutTests
{
    [Test]
    public async Task ReadWrite_Version6_KeepsNativeOrder()
    {
        var node = new CPlugVehicleCarPhyShape();
        var chunk = new CPlugVehicleCarPhyShape.Chunk0910E000();
        var payload = Payload(w =>
        {
            w.Write(6);
            w.Write(0.25f); // OffsetX
            w.Write(-1); // SurfSolid
            w.Write(-1); // MoveShape
            w.Write(1); // Wheels
            w.Write(1); // IsDriving
            w.Write(0); // IsSteering
            w.Write(3); // Id table version.
            NewId(w, "FL");
            foreach (var value in new[] { 1f, 2f, 3f, 4f, 5f, 6f }) w.Write(value); // VehicleBBox
            w.Write(0); // UseSimplifiedCarModel
            foreach (var value in new[] { 0.1f, 1.5f, -1.5f, 0.9f, 0.8f, 0.4f, 0.45f, 0.3f, 0.35f }) w.Write(value);
            w.Write(1); // VolumeSpheres
            foreach (var value in new[] { 0f, 1f, 2f, 0.5f }) w.Write(value);
            foreach (var value in new[] { 0f, 0.7f, 0.2f, 1.1f, 0.7f, 2.2f }) w.Write(value); // CarVsCarShape_Box
            for (var i = 0; i < 12; i++) w.Write(0); // Values always saved as zero, except U25.
            w.Write(60f);
            w.Write(2f);
            w.Write(0.02f);
            String(w, "");
            w.Write(-1); // U34
            w.Write(1);
            foreach (var value in new[] { 7f, 8f, 9f }) w.Write(value);
            w.Write(-1); // CarVsCarShape
            w.Write(-1); // HitShape
            w.Write(-1); // WaterShape
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.OffsetX).IsEqualTo(0.25f);
        await Assert.That(node.Wheels!.Length).IsEqualTo(1);
        await Assert.That(node.Wheels[0].IsDriving).IsTrue();
        await Assert.That(node.Wheels[0].WheelId).IsEqualTo("FL");
        await Assert.That(node.VehicleBBox).IsEqualTo(new BoxAligned(1, 2, 3, 4, 5, 6));
        await Assert.That(node.UseSimplifiedCarModel).IsFalse();
        await Assert.That(node.GroundY).IsEqualTo(0.1f);
        await Assert.That(node.FrontWheelZ).IsEqualTo(1.5f);
        await Assert.That(node.BackWheelZ).IsEqualTo(-1.5f);
        await Assert.That(node.FrontWheelX).IsEqualTo(0.9f);
        await Assert.That(node.BackWheelX).IsEqualTo(0.8f);
        await Assert.That(node.FrontWheelRadius).IsEqualTo(0.4f);
        await Assert.That(node.BackWheelRadius).IsEqualTo(0.45f);
        await Assert.That(node.FrontWheelHalfWidth).IsEqualTo(0.3f);
        await Assert.That(node.BackWheelHalfWidth).IsEqualTo(0.35f);
        await Assert.That(node.VolumeSpheres).IsEquivalentTo([new Vec4(0, 1, 2, 0.5f)], CollectionOrdering.Matching);
        await Assert.That(node.CarVsCarShape_Box).IsEqualTo(new BoxAligned(0, 0.7f, 0.2f, 1.1f, 0.7f, 2.2f));
        await Assert.That(chunk.U30).IsEqualTo(60f);
        await Assert.That(chunk.U31).IsEqualTo(2f);
        await Assert.That(chunk.U32).IsEqualTo(0.02f);
        await Assert.That(chunk.U35).IsEquivalentTo([new Vec3(7, 8, 9)], CollectionOrdering.Matching);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP4 | GameVersion.TM2020);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugVehicleCarPhyShape();
        var chunk = new CPlugVehicleCarPhyShape.Chunk0910E000();

        await Assert.That(node.VehicleBBox).IsEqualTo(new BoxAligned(0, 0, 0, -1, -1, -1));
        await Assert.That(node.UseSimplifiedCarModel).IsTrue();
        await Assert.That(node.FrontWheelZ).IsEqualTo(1f);
        await Assert.That(node.BackWheelZ).IsEqualTo(-1f);
        await Assert.That(node.FrontWheelX).IsEqualTo(1f);
        await Assert.That(node.BackWheelX).IsEqualTo(1f);
        await Assert.That(node.FrontWheelRadius).IsEqualTo(0.5f);
        await Assert.That(node.BackWheelRadius).IsEqualTo(0.5f);
        await Assert.That(node.FrontWheelHalfWidth).IsEqualTo(0.5f);
        await Assert.That(node.BackWheelHalfWidth).IsEqualTo(0.5f);
        await Assert.That(node.CarVsCarShape_Box).IsEqualTo(new BoxAligned(0, 0.6f, 0.1f, 1, 0.6f, 2.1f));
        await Assert.That(chunk.U25).IsEqualTo(0.05f);
        await Assert.That(chunk.U30).IsEqualTo(50f);
        await Assert.That(chunk.U31).IsEqualTo(1.5f);
        await Assert.That(chunk.U32).IsEqualTo(0.01f);
    }
}
