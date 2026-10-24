using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCharPhyMaterialLayoutTests
{
    [Test]
    public async Task ReadWrite_Version11_KeepsNativeOrder()
    {
        var node = new CPlugCharPhyMaterial();
        var chunk = new CPlugCharPhyMaterial.Chunk090CA001();
        var payload = Payload(w =>
        {
            w.Write(11);
            w.Write((byte)CPlugSurface.MaterialId.Ice);
            w.Write(0.5f);
            w.Write(1); // SlideSurf
            w.Write(0); // Climbable
            w.Write(1); // WallJumpable
            w.Write(1);
            w.Write(45f);
            w.Write(1.2f);
            w.Write(70f);
            w.Write(1);
            w.Write(0.8f);
            w.Write(30f);
            w.Write(5f);
            w.Write(1); // IgnoreBlows
            w.Write(0.6f);
            w.Write(0.7f);
            w.Write(0.9f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.SurfaceMaterialId).IsEqualTo(CPlugSurface.MaterialId.Ice);
        await Assert.That(chunk.U04).IsEqualTo(0.5f);
        await Assert.That(node.SlideSurf).IsTrue();
        await Assert.That(node.Climbable).IsFalse();
        await Assert.That(node.WallJumpable).IsTrue();
        await Assert.That(chunk.U08).IsTrue();
        await Assert.That(node.MaxWalkSlope).IsEqualTo(45f);
        await Assert.That(node.SpeedCoef).IsEqualTo(1.2f);
        await Assert.That(node.MaxSlideSlope).IsEqualTo(70f);
        await Assert.That(chunk.U12).IsTrue();
        await Assert.That(node.BlowHorizontalCoef).IsEqualTo(0.8f);
        await Assert.That(node.AccelMax).IsEqualTo(30f);
        await Assert.That(node.FrictionMax).IsEqualTo(5f);
        await Assert.That(node.IgnoreBlows).IsTrue();
        await Assert.That(chunk.U17).IsEqualTo(0.6f);
        await Assert.That(node.GroundBlowCoef).IsEqualTo(0.7f);
        await Assert.That(chunk.U19).IsEqualTo(0.9f);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP4 | GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_Version2_ReadsLegacyCurves()
    {
        var node = new CPlugCharPhyMaterial();
        var chunk = new CPlugCharPhyMaterial.Chunk090CA001();
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write((byte)CPlugSurface.MaterialId.Grass);
            w.Write(-1);
            w.Write(-1);
            w.Write(0.5f);
            w.Write(0);
            w.Write(1); // Climbable
            w.Write(0);
            w.Write(0);
            w.Write(50f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.SurfaceMaterialId).IsEqualTo(CPlugSurface.MaterialId.Grass);
        await Assert.That(node.Climbable).IsTrue();
        await Assert.That(node.MaxWalkSlope).IsEqualTo(50f);
        await Assert.That(node.SpeedCoef).IsEqualTo(1f);
    }

    [Test]
    public async Task ReadWrite_LegacyChunk_KeepsNativeOrder()
    {
        var node = new CPlugCharPhyMaterial();
        var chunk = new CPlugCharPhyMaterial.Chunk090CA000();
        var payload = Payload(w =>
        {
            w.Write((byte)CPlugSurface.MaterialId.Metal);
            w.Write(-1);
            w.Write(-1);
            w.Write(0.5f);
            w.Write(1); // SlideSurf
            w.Write(1); // Climbable
            w.Write(0); // WallJumpable
            w.Write(1);
            w.Write(1);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.SurfaceMaterialId).IsEqualTo(CPlugSurface.MaterialId.Metal);
        await Assert.That(chunk.U04).IsEqualTo(0.5f);
        await Assert.That(node.SlideSurf).IsTrue();
        await Assert.That(node.Climbable).IsTrue();
        await Assert.That(node.WallJumpable).IsFalse();
        await Assert.That(chunk.U08).IsTrue();
        await Assert.That(chunk.U09).IsTrue();
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugCharPhyMaterial();
        var chunk = new CPlugCharPhyMaterial.Chunk090CA001();

        await Assert.That(node.MaxWalkSlope).IsEqualTo(60f);
        await Assert.That(node.SpeedCoef).IsEqualTo(1f);
        await Assert.That(node.MaxSlideSlope).IsEqualTo(60f);
        await Assert.That(node.BlowHorizontalCoef).IsEqualTo(1f);
        await Assert.That(node.AccelMax).IsEqualTo(-1f);
        await Assert.That(node.FrictionMax).IsEqualTo(-1f);
        await Assert.That(node.GroundBlowCoef).IsEqualTo(1f);
        await Assert.That(chunk.U04).IsEqualTo(1f);
        await Assert.That(chunk.U17).IsEqualTo(1f);
        await Assert.That(chunk.U19).IsEqualTo(1f);
    }
}
