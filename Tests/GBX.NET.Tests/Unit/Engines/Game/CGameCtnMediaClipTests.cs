using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
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
    public async Task GetGhosts_EmptyClip_ReturnsNoGhosts()
    {
        var clip = new CGameCtnMediaClip();

        await Assert.That(clip.GetGhosts()).IsEmpty();
    }

    [Test]
    [Arguments(null, "CGameCtnMediaClip: (unnamed)")]
    [Arguments("Intro", "CGameCtnMediaClip: Intro")]
    public async Task ToString_WithOptionalName_ReturnsClipLabel(string? name, string expected)
    {
        var clip = new CGameCtnMediaClip { Name = name };

        var label = clip.ToString();

        await Assert.That(label).IsEqualTo(expected);
    }
}
