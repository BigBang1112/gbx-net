using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;

namespace GBX.NET.Tests.Unit;

public class CGameCtnChallengeTests
{
    [Test]
    public async Task EmptyMapQueriesReturnNoElements()
    {
        var map = new CGameCtnChallenge();

        await Assert.That(map.GetBlocks()).IsEmpty();
        await Assert.That(map.GetAnchoredObjects()).IsEmpty();
        await Assert.That(map.GetBakedBlocks()).IsEmpty();
        await Assert.That(map.GetBlock(new Int3(1, 2, 3))).IsNull();
        await Assert.That(map.GetGhostBlocks()).IsEmpty();
    }

    [Test]
    public void PlacementRequiresInitializedCollections()
    {
        var map = new CGameCtnChallenge();

        Assert.Throws<MemberNullException>(() => map.PlaceBlock("Start", new Int3(), Direction.North));
        Assert.Throws<MemberNullException>(() => map.PlaceAnchoredObject(new Ident("Item"), Vec3.Zero, Vec3.Zero));
    }

    [Test]
    public async Task PlacedBlocksCanBeQueriedAndRemovedByPositionAndGhostFlag()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        var position = new Int3(1, 2, 3);
        var first = map.PlaceBlock("Start", position, Direction.North, isGround: true, variant: 4, subVariant: 5);
        var ghost = map.PlaceBlock("Ghost", position, Direction.East);
        ghost.IsGhost = true;
        var elsewhere = map.PlaceBlock("Elsewhere", new Int3(9, 8, 7), Direction.South);

        await Assert.That(map.GetBlock(position)).IsSameReferenceAs(first);
        await Assert.That(map.GetBlocks(position).ToArray()).IsEquivalentTo(new[] { first, ghost }, CollectionOrdering.Matching);
        await Assert.That(map.GetGhostBlocks().Single()).IsSameReferenceAs(ghost);
        await Assert.That(first.IsGround).IsTrue();
        await Assert.That(first.Variant).IsEqualTo((byte)4);
        await Assert.That(first.SubVariant).IsEqualTo((byte)5);
        await Assert.That(map.NbBlocks).IsEqualTo(3);

        await Assert.That(map.RemoveBlock(block => block.IsGhost)).IsTrue();
        await Assert.That(map.RemoveBlock(first)).IsTrue();
        await Assert.That(map.RemoveBlocks(block => block == elsewhere)).IsEqualTo(1);
        await Assert.That(map.NbBlocks).IsEqualTo(0);
    }

    [Test]
    public async Task IdentPlacementKeepsModelMetadataAndCreatesLegacyChunk()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        var model = new Ident("Start", "Stadium", "Maker");

        var block = map.PlaceBlock(model, new Int3(2, 3, 4), Direction.West);

        await Assert.That(block.BlockModel).IsEqualTo(model);
        await Assert.That(block.Chunks.Get<CGameCtnBlock.Chunk03057002>()).IsNotNull();
        await Assert.That(map.Blocks![0]).IsSameReferenceAs(block);
    }

    [Test]
    public async Task AnchoredObjectPlacementSetsTransformAndRequiredChunks()
    {
        var map = new CGameCtnChallenge { AnchoredObjects = [] };
        var model = new Ident("Item", "Stadium", "Maker");
        var position = new Vec3(1, 2, 3);
        var rotation = new Vec3(4, 5, 6);
        var pivot = new Vec3(7, 8, 9);

        var item = map.PlaceAnchoredObject(model, position, rotation, pivot);

        await Assert.That(item.ItemModel).IsEqualTo(model);
        await Assert.That(item.AbsolutePositionInMap).IsEqualTo(position);
        await Assert.That(item.YawPitchRoll).IsEqualTo(rotation);
        await Assert.That(item.PivotPosition).IsEqualTo(pivot);
        await Assert.That(map.Chunks.Get<CGameCtnChallenge.Chunk03043040>()).IsNotNull();
        await Assert.That(item.Chunks.Get<CGameCtnAnchoredObject.Chunk03101002>()!.Version).IsEqualTo(7);
        await Assert.That(map.RemoveAnchoredObject(item)).IsTrue();
        await Assert.That(map.GetAnchoredObjects()).IsEmpty();
    }

    [Test]
    public async Task RemoveAllClearsBlocksItemsAndOffzones()
    {
        var map = new CGameCtnChallenge { Blocks = [], AnchoredObjects = [], Offzones = [] };
        map.PlaceBlock("Start", new Int3(), Direction.North);
        map.PlaceAnchoredObject(new Ident("Item"), Vec3.Zero, Vec3.Zero);
        map.Offzones.Add(new BoxInt3(0, 0, 0, 1, 1, 1));

        map.RemoveAll();

        await Assert.That(map.Blocks).IsEmpty();
        await Assert.That(map.AnchoredObjects).IsEmpty();
        await Assert.That(map.Offzones).IsEmpty();
    }
}
