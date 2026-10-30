using GBX.NET.Engines.Plug;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugTrainWagonModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version12_KeepsNativeOrder()
    {
        var node = new CPlugTrainWagonModel();
        var chunk = new CPlugTrainWagonModel.Chunk0911C000();
        var payload = Payload(w =>
        {
            w.Write(12);
            w.Write(-1); // Shape
            w.Write(-1); // Mesh
            w.Write(1); // IsLoco
            foreach (var value in new[] { 20f, 2f, 3f }) w.Write(value);
            for (var i = 0; i < 4; i++) w.Write(-1); // Sounds
            w.Write(12.5f);
            w.Write(0.5f);
            w.Write(0); // GenLengthFromShape
            w.Write(3); // Curve version.
            w.Write(1);
            w.Write((ushort)0);
            w.Write(0f);
            w.Write(4f);
            w.Write(-1); // SmokeEmitterModel
            w.Write(-1); // Discarded surface.
            w.Write(-1); // DustEmitterModel
            w.Write(-1); // SparkleParticle
            w.Write(0.75f);
            w.Write(-1); // AnimFile
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.IsLoco).IsTrue();
        await Assert.That(chunk.U01).IsEqualTo(new Vec3(20, 2, 3));
        await Assert.That(node.WagonLength).IsEqualTo(12.5f);
        await Assert.That(node.WagonColOffset).IsEqualTo(0.5f);
        await Assert.That(node.GenLengthFromShape).IsFalse();
        await Assert.That(node.AccelCurve!.Values![0].Y).IsEqualTo(4f);
        await Assert.That(chunk.U04).IsEqualTo(0.75f);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
    }

    [Test]
    public async Task ReadWrite_Version11_ReadsDiscardedNodeAndInlineCurve()
    {
        var node = new CPlugTrainWagonModel();
        var chunk = new CPlugTrainWagonModel.Chunk0911C000();
        var payload = Payload(w =>
        {
            w.Write(11);
            w.Write(-1);
            w.Write(-1);
            w.Write(0);
            foreach (var value in new[] { 15f, 1f, 1f }) w.Write(value);
            for (var i = 0; i < 4; i++) w.Write(-1);
            w.Write(1f);
            w.Write(0f);
            w.Write(1);
            w.Write(3);
            w.Write(0);
            w.Write((ushort)0);
            w.Write(-1);
            w.Write(-1);
            w.Write(-1);
            w.Write(-1);
            w.Write(0.25f);
            w.Write(-1); // Discarded node.
            w.Write(-1); // AnimFile
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.U04).IsEqualTo(0.25f);
        await Assert.That(node.GenLengthFromShape).IsTrue();
    }

    [Test]
    public async Task ReadWrite_Version4_SkipsLegacyData()
    {
        var node = new CPlugTrainWagonModel();
        var chunk = new CPlugTrainWagonModel.Chunk0911C000();
        var payload = Payload(w =>
        {
            w.Write(4);
            w.Write(7);
            w.Write(8);
            w.Write(9);
            w.Write(-1);
            w.Write(-1);
            w.Write(1);
            foreach (var value in new[] { 15f, 1f, 1f }) w.Write(value);
            for (var i = 0; i < 4; i++) w.Write(-1);
            w.Write(10);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.U05).IsEquivalentTo([7, 8, 9], CollectionOrdering.Matching);
        await Assert.That(chunk.U06).IsEqualTo(10);
        await Assert.That(node.IsLoco).IsTrue();
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugTrainWagonModel();
        var chunk = new CPlugTrainWagonModel.Chunk0911C000();

        await Assert.That(node.WagonLength).IsEqualTo(1f);
        await Assert.That(node.GenLengthFromShape).IsTrue();
        await Assert.That(chunk.U01).IsEqualTo(new Vec3(15, 1, 1));
    }
}
