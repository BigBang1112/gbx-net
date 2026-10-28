using GBX.NET.Engines.Plug;
using TmEssentials;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCharPhyRecoilModelLayoutTests
{
    [Test]
    public async Task ReadWrite_RecoilChunk_KeepsNativeOrder()
    {
        var node = new CPlugCharPhyRecoilModel();
        var chunk = new CPlugCharPhyRecoilModel.Chunk090E0000();
        var payload = Payload(w =>
        {
            w.Write(3); // Id table version.
            NewId(w, "Rifle");
            w.Write(200);
            w.Write(1.1f);
            w.Write(0.05f);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.BulletModelId).IsEqualTo("Rifle");
        await Assert.That(node.Duration).IsEqualTo(TimeInt32.FromMilliseconds(200));
        await Assert.That(node.FovScale).IsEqualTo(1.1f);
        await Assert.That(node.ViewKick).IsEqualTo(0.05f);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP4 | GameVersion.TM2020);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugCharPhyRecoilModel();

        await Assert.That(node.Duration).IsEqualTo(TimeInt32.FromMilliseconds(150));
        await Assert.That(node.FovScale).IsEqualTo(1.04f);
        await Assert.That(node.ViewKick).IsEqualTo(0f);
    }
}
