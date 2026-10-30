using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleCameraHelicoModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version9_ReadsInlineCurves()
    {
        var node = new CPlugVehicleCameraHelicoModel();
        var chunk = new CPlugVehicleCameraHelicoModel.Chunk0914D000();
        var payload = Payload(w =>
        {
            w.Write(9);
            WriteScalars(w);
            WriteCurve(w, 1f);
            w.Write(3.5f);
            w.Write(-6.5f);
            for (var i = 0; i < 7; i++) WriteCurve(w, i + 2f);
            w.Write(0xFACADE01); // Discarded direct curve.
            w.Write(0xFACADE01); // Discarded direct curve.
            w.Write(500);
            w.Write(600);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await AssertScalars(chunk);
        await Assert.That(chunk.U18!.Values![0].Y).IsEqualTo(1f);
        await Assert.That(chunk.U19).IsEqualTo(3.5f);
        await Assert.That(chunk.U20).IsEqualTo(-6.5f);
        await Assert.That(chunk.U21!.Values![0].Y).IsEqualTo(2f);
        await Assert.That(chunk.U27!.Values![0].Y).IsEqualTo(8f);
        await Assert.That(chunk.U30).IsEqualTo(500);
        await Assert.That(chunk.U31).IsEqualTo(600);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_Version8_ReadsDirectLegacyCurves()
    {
        var node = new CPlugVehicleCameraHelicoModel();
        var chunk = new CPlugVehicleCameraHelicoModel.Chunk0914D000();
        var payload = Payload(w =>
        {
            w.Write(8);
            WriteScalars(w);
            w.Write(0xFACADE01);
            w.Write(3.5f);
            w.Write(-6.5f);
            for (var i = 0; i < 9; i++) w.Write(0xFACADE01); // Seven curves and two discarded curves.
            w.Write(500);
            w.Write(600);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await AssertScalars(chunk);
        await Assert.That(chunk.U18!.Curve).IsTypeOf<CPlugCurveSimpleNod>();
        await Assert.That(chunk.U27!.Values).IsEmpty();
        await Assert.That(chunk.U19).IsEqualTo(3.5f);
        await Assert.That(chunk.U31).IsEqualTo(600);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var chunk = new CPlugVehicleCameraHelicoModel.Chunk0914D000();

        await Assert.That(chunk.U01).IsEqualTo(12f);
        await Assert.That(chunk.U02).IsEqualTo(30f);
        await Assert.That(chunk.U03).IsEqualTo(80f);
        await Assert.That(chunk.U05).IsEqualTo(1f);
        await Assert.That(chunk.U06).IsEqualTo(4f);
        await Assert.That(chunk.U09).IsEqualTo(4f);
        await Assert.That(chunk.U10).IsEqualTo(0.3f);
        await Assert.That(chunk.U13).IsEqualTo(1f);
        await Assert.That(chunk.U14).IsEqualTo(1f);
        await Assert.That(chunk.U19).IsEqualTo(3f);
        await Assert.That(chunk.U20).IsEqualTo(-6f);
        await Assert.That(chunk.U30).IsEqualTo(1000);
        await Assert.That(chunk.U31).IsEqualTo(1000);
    }

    private static void WriteScalars(BinaryWriter w)
    {
        for (var i = 1; i <= 17; i++) w.Write(i + 0.5f);
    }

    private static async Task AssertScalars(CPlugVehicleCameraHelicoModel.Chunk0914D000 chunk)
    {
        await Assert.That(chunk.U01).IsEqualTo(1.5f);
        await Assert.That(chunk.U08).IsEqualTo(8.5f);
        await Assert.That(chunk.U14).IsEqualTo(14.5f);
        await Assert.That(chunk.U17).IsEqualTo(17.5f);
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
