using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockTests
{
    [Test]
    public async Task NameUpdatesBlockModelWithoutLosingCollectionOrAuthor()
    {
        var block = new CGameCtnBlock { BlockModel = new Ident("Old", "Stadium", "Maker") };

        block.Name = "New";

        await Assert.That(block.BlockModel).IsEqualTo(new Ident("New", "Stadium", "Maker"));
        await Assert.That(block.Name).IsEqualTo("New");
    }

    [Test]
    public async Task VariantAndSubVariantKeepOtherFlagsIntact()
    {
        var block = new CGameCtnBlock { Flags = (1 << 12) | (1 << 28) };

        block.Variant = 63;
        block.SubVariant = 42;

        await Assert.That(block.Variant).IsEqualTo((byte)63);
        await Assert.That(block.SubVariant).IsEqualTo((byte)42);
        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 28) | (42 << 6) | 63);

        block.Variant = 7;
        await Assert.That(block.SubVariant).IsEqualTo((byte)42);
        await Assert.That(block.IsGhost).IsTrue();
    }

    [Test]
    public async Task FlagPropertiesCanBeSetAndClearedIndependently()
    {
        var block = new CGameCtnBlock
        {
            IsGround = true,
            IsClip = true,
            IsPillar = true,
            IsReplacement = true,
            IsGhost = true,
            IsFree = true
        };

        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 13) | (1 << 14) | (1 << 16) | (1 << 28) | (1 << 29));

        block.IsClip = false;
        block.IsGhost = false;

        await Assert.That(block.IsGround).IsTrue();
        await Assert.That(block.IsPillar).IsTrue();
        await Assert.That(block.IsReplacement).IsTrue();
        await Assert.That(block.IsFree).IsTrue();
        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 14) | (1 << 16) | (1 << 29));
    }

    [Test]
    public async Task WaypointAndDecalFlagsFollowTheirValues()
    {
        var block = new CGameCtnBlock();
        block.WaypointSpecialProperty = new CGameWaypointSpecialProperty();
        block.DecalId = "decal";

        await Assert.That(block.Flags & (1 << 20)).IsEqualTo(1 << 20);
        await Assert.That(block.Flags & (1 << 17)).IsEqualTo(1 << 17);

        block.WaypointSpecialProperty = null;
        block.DecalId = null;

        await Assert.That(block.Flags & ((1 << 20) | (1 << 17))).IsEqualTo(0);
    }
}
