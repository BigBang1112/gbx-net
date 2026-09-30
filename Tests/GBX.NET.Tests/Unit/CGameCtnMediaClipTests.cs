using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMediaClipTests
{
    [Test]
    public async Task GetGhostsReturnsOnlyGhostModelsInTrackOrder()
    {
        var first = new CGameCtnGhost();
        var second = new CGameCtnGhost();
        var clip = new CGameCtnMediaClip
        {
            Tracks =
            [
                new CGameCtnMediaTrack
                {
                    Blocks =
                    [
                        new CGameCtnMediaBlockText(),
                        new CGameCtnMediaBlockGhost { GhostModel = first },
                        new CGameCtnMediaBlockGhost()
                    ]
                },
                new CGameCtnMediaTrack
                {
                    Blocks = [new CGameCtnMediaBlockGhost { GhostModel = second }]
                }
            ]
        };

        var ghosts = clip.GetGhosts().ToArray();

        await Assert.That(ghosts.Length).IsEqualTo(2);
        await Assert.That(ghosts[0]).IsSameReferenceAs(first);
        await Assert.That(ghosts[1]).IsSameReferenceAs(second);
    }

    [Test]
    public async Task EmptyClipHasNoGhostsAndUsesUnnamedLabel()
    {
        var clip = new CGameCtnMediaClip();

        await Assert.That(clip.GetGhosts()).IsEmpty();
        await Assert.That(clip.ToString()).IsEqualTo("CGameCtnMediaClip: (unnamed)");

        clip.Name = "Intro";
        await Assert.That(clip.ToString()).IsEqualTo("CGameCtnMediaClip: Intro");
    }
}
