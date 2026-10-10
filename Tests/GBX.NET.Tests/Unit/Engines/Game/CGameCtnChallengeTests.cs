using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeTests
{
    [Test]
    [Arguments(5, false)]
    [Arguments(8, false)]
    [Arguments(9, false)]
    [Arguments(10, false)]
    [Arguments(5, true)]
    [Arguments(8, true)]
    [Arguments(9, true)]
    [Arguments(10, true)]
    public async Task EmptyLightmapFramesOmitCachePayload(int version, bool modernChunk)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            if (modernChunk) writer.Write(0);
            writer.Write(1);
            if (modernChunk)
            {
                writer.Write(0);
                writer.Write(0);
            }
            writer.Write(version);
            writer.Write(0);
        }
        var payload = stream.ToArray();

        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x12345678);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        var map = new CGameCtnChallenge();
        var chunk = modernChunk
            ? new CGameCtnChallenge.Chunk0304305B()
            : new CGameCtnChallenge.Chunk0304303D();

        chunk.ReadWrite(map, rw);

        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(map.HasLightmaps).IsTrue();
        await Assert.That(map.LightmapVersion).IsEqualTo(version);
        await Assert.That(map.LightmapFrames).IsNotNull();
        await Assert.That(map.LightmapFrames).IsEmpty();
        await Assert.That(map.LightmapCacheData).IsNull();

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(map, writerWriter);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }

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
    public void PlaceBlock_UninitializedBlocks_ThrowsMemberNullException()
    {
        var map = new CGameCtnChallenge();

        Assert.Throws<MemberNullException>(() => map.PlaceBlock("Start", new Int3(), Direction.North));
    }

    [Test]
    public void PlaceAnchoredObject_UninitializedItems_ThrowsMemberNullException()
    {
        var map = new CGameCtnChallenge();

        Assert.Throws<MemberNullException>(() => map.PlaceAnchoredObject(new Ident("Item"), Vec3.Zero, Vec3.Zero));
    }

    [Test]
    public async Task GetBlocks_SharedPosition_ReturnsMatchingBlocksInPlacementOrder()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        var position = new Int3(1, 2, 3);
        var first = map.PlaceBlock("Start", position, Direction.North);
        var ghost = map.PlaceBlock("Ghost", position, Direction.East);
        ghost.IsGhost = true;
        map.PlaceBlock("Elsewhere", new Int3(9, 8, 7), Direction.South);

        var blocks = map.GetBlocks(position).ToArray();

        await Assert.That(blocks).IsEquivalentTo(new[] { first, ghost }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task PlaceBlock_WithVariants_PreservesPlacementSettings()
    {
        var map = new CGameCtnChallenge { Blocks = [] };

        var first = map.PlaceBlock("Start", new Int3(1, 2, 3), Direction.North, isGround: true, variant: 4, subVariant: 5);

        await Assert.That(first.IsGround).IsTrue();
        await Assert.That(first.Variant).IsEqualTo((byte)4);
        await Assert.That(first.SubVariant).IsEqualTo((byte)5);
        await Assert.That(map.NbBlocks).IsEqualTo(1);
    }

    [Test]
    public async Task GetGhostBlocks_WithMixedBlocks_ReturnsOnlyGhostBlocks()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        map.PlaceBlock("Start", new Int3(), Direction.North);
        var ghost = map.PlaceBlock("Ghost", new Int3(), Direction.East);
        ghost.IsGhost = true;

        var ghosts = map.GetGhostBlocks().ToArray();

        await Assert.That(ghosts).HasSingleItem();
        await Assert.That(ghosts[0]).IsSameReferenceAs(ghost);
    }

    [Test]
    public async Task GetBlock_SharedPosition_ReturnsFirstPlacedBlock()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        var position = new Int3(1, 2, 3);
        var first = map.PlaceBlock("Start", position, Direction.North);
        map.PlaceBlock("Later", position, Direction.East);

        var block = map.GetBlock(position);

        await Assert.That(block).IsSameReferenceAs(first);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RemoveBlock_WithMatchingBlock_RemovesOnlyThatBlock(bool usePredicate)
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        var remaining = map.PlaceBlock("Start", new Int3(), Direction.North);
        var removed = map.PlaceBlock("Ghost", new Int3(), Direction.East);
        removed.IsGhost = true;

        var result = usePredicate ? map.RemoveBlock(block => block.IsGhost) : map.RemoveBlock(removed);

        await Assert.That(result).IsTrue();
        await Assert.That(map.Blocks!).HasSingleItem();
        await Assert.That(map.Blocks![0]).IsSameReferenceAs(remaining);
    }

    [Test]
    public async Task RemoveBlocks_WithMatchingBlocks_ReturnsRemovedCount()
    {
        var map = new CGameCtnChallenge { Blocks = [] };
        map.PlaceBlock("First", new Int3(), Direction.North);
        map.PlaceBlock("Second", new Int3(), Direction.East);

        var removedCount = map.RemoveBlocks(_ => true);

        await Assert.That(removedCount).IsEqualTo(2);
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
    public async Task PlaceAnchoredObject_WithTransform_SetsTransformAndRequiredChunks()
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
    }

    [Test]
    public async Task RemoveAnchoredObject_WithPlacedItem_RemovesItem()
    {
        var map = new CGameCtnChallenge { AnchoredObjects = [] };
        var item = map.PlaceAnchoredObject(new Ident("Item"), Vec3.Zero, Vec3.Zero);

        var removed = map.RemoveAnchoredObject(item);

        await Assert.That(removed).IsTrue();
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
