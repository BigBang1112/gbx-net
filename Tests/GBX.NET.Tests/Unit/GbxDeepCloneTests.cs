using GBX.NET.Engines.Game;
using GBX.NET.Engines.Script;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class GbxDeepCloneTests
{
    [Test]
    public async Task RootClonePreservesSharedReferencesAndCyclesWithoutSharingMutableState()
    {
        const uint chunkId = 0x030430FE;
        var block = new CGameCtnBlock { Name = "Start", Coord = new Int3(1, 2, 3) };
        var item = new CGameCtnAnchoredObject { SnappedOnBlock = block };
        item.SnappedOnItem = item;
        var map = new CGameCtnChallenge
        {
            Blocks = [block],
            AnchoredObjects = [item],
            Offzones = [new BoxInt3(0, 0, 0, 1, 1, 1)]
        };
        map.Chunks.Add(new SkippableChunk(chunkId) { Data = [1, 2, 3] });
        Gbx original = new Gbx<CGameCtnChallenge>(map);
        original.Header.NumNodes = 3;

#pragma warning disable GBXNET10001
        var clonedGbx = original.DeepClone();
#pragma warning restore GBXNET10001
        var clone = (Gbx<CGameCtnChallenge>)clonedGbx;
        var clonedBlock = clone.Node.Blocks![0];
        var clonedItem = clone.Node.AnchoredObjects![0];
        var clonedChunk = (SkippableChunk)clone.Node.Chunks.Get(chunkId)!;

        await Assert.That(clone).IsNotSameReferenceAs(original);
        await Assert.That(clone.Node).IsNotSameReferenceAs(map);
        await Assert.That(clone.Header).IsNotSameReferenceAs(original.Header);
        await Assert.That(clonedBlock).IsNotSameReferenceAs(block);
        await Assert.That(clonedItem.SnappedOnBlock).IsSameReferenceAs(clonedBlock);
        await Assert.That(clonedItem.SnappedOnItem).IsSameReferenceAs(clonedItem);
        await Assert.That(clonedChunk).IsNotSameReferenceAs(map.Chunks.Get(chunkId));
        await Assert.That(clone.Node.Offzones).IsNotSameReferenceAs(map.Offzones);
        await Assert.That(clone.Header.NumNodes).IsEqualTo(3);

        clonedBlock.Name = "Changed";
        clonedChunk.Data![0] = 99;
        clone.Node.Blocks.Add(new CGameCtnBlock());
        clone.Node.Offzones!.Clear();
        clone.Header.NumNodes = 4;

        await Assert.That(block.Name).IsEqualTo("Start");
        await Assert.That(((SkippableChunk)map.Chunks.Get(chunkId)!).Data![0]).IsEqualTo((byte)1);
        await Assert.That(map.Blocks.Count).IsEqualTo(1);
        await Assert.That(map.Offzones.Count).IsEqualTo(1);
        await Assert.That(original.Header.NumNodes).IsEqualTo(3);
    }

    [Test]
    public async Task TypedRootCloneRetainsAliasesInsideMediaTracks()
    {
        var ghost = new CGameCtnGhost();
        var first = new CGameCtnMediaBlockGhost { GhostModel = ghost };
        var second = new CGameCtnMediaBlockGhost { GhostModel = ghost };
        var clip = new CGameCtnMediaClip
        {
            Name = "Intro",
            Tracks = [new CGameCtnMediaTrack { Blocks = [first, second] }]
        };
        var original = new Gbx<CGameCtnMediaClip>(clip);

#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        var clonedTrack = clone.Node.Tracks[0];
        var clonedFirst = (CGameCtnMediaBlockGhost)clonedTrack.Blocks[0];
        var clonedSecond = (CGameCtnMediaBlockGhost)clonedTrack.Blocks[1];

        await Assert.That(clone.Node).IsNotSameReferenceAs(clip);
        await Assert.That(clonedTrack).IsNotSameReferenceAs(clip.Tracks[0]);
        await Assert.That(clonedFirst.GhostModel).IsNotSameReferenceAs(ghost);
        await Assert.That(clonedFirst.GhostModel).IsSameReferenceAs(clonedSecond.GhostModel);

        clonedTrack.Blocks.RemoveAt(0);
        await Assert.That(clip.Tracks[0].Blocks.Count).IsEqualTo(2);
    }

    [Test]
    public async Task CloneRemapsTriangleKeyBackReferenceToClonedBlock()
    {
        var triangles = new CGameCtnMediaBlockTriangles { Vertices = new Vec4[3] };
        triangles.Keys.Add(new CGameCtnMediaBlockTriangles.Key(triangles));
        var clip = new CGameCtnMediaClip
        {
            Tracks = [new CGameCtnMediaTrack { Blocks = [triangles] }]
        };
        var original = new Gbx<CGameCtnMediaClip>(clip);

#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        var clonedTriangles = (CGameCtnMediaBlockTriangles)clone.Node.Tracks[0].Blocks[0];
        clonedTriangles.Keys[0].Positions = new Vec3[5];

        await Assert.That(clonedTriangles.Vertices.Length).IsEqualTo(5);
        await Assert.That(triangles.Vertices.Length).IsEqualTo(3);
        await Assert.That(triangles.Keys[0].Positions.Length).IsEqualTo(3);
    }

    [Test]
    public async Task InterfaceCloneUsesGeneratedEngineFieldCopying()
    {
        IClass original = new CGameCtnMediaClip
        {
            Tracks = [new CGameCtnMediaTrack { Blocks = [new CGameCtnMediaBlockText()] }]
        };

#pragma warning disable GBXNET10001
        var clone = (CGameCtnMediaClip)original.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone).IsNotSameReferenceAs(original);
        await Assert.That(clone.Tracks[0]).IsNotSameReferenceAs(((CGameCtnMediaClip)original).Tracks[0]);
        await Assert.That(clone.Tracks[0].Blocks[0]).IsNotSameReferenceAs(((CGameCtnMediaClip)original).Tracks[0].Blocks[0]);
    }

    [Test]
    public async Task CloneCopiesMutablePayloadsAndScriptTraits()
    {
        var trait = new CScriptTraitsMetadata.ScriptTrait<int>(
            new CScriptTraitsMetadata.ScriptType(CScriptTraitsMetadata.EScriptType.Integer), 7);
        var structType = new CScriptTraitsMetadata.ScriptStructType("Record", new Dictionary<string, CScriptTraitsMetadata.ScriptTrait>());
        var arrayType = new CScriptTraitsMetadata.ScriptArrayType(
            new CScriptTraitsMetadata.ScriptType(CScriptTraitsMetadata.EScriptType.Integer), structType);
        var metadata = new CScriptTraitsMetadata();
        metadata.Traits.Add("Count", trait);
        metadata.Traits.Add("NestedType", new CScriptTraitsMetadata.ScriptTrait<int>(arrayType, 1));
        var map = new CGameCtnChallenge
        {
            LightmapCacheData = new ZlibData(3, [1, 2, 3], null),
            ZoneGenealogyData = new RawData([4, 5, 6], null),
            ScriptMetadata = metadata
        };

#pragma warning disable GBXNET10001
        var clone = new Gbx<CGameCtnChallenge>(map).DeepClone();
#pragma warning restore GBXNET10001

        clone.Node.LightmapCacheData!.Data[0] = 9;
        clone.Node.ZoneGenealogyData!.Data[0] = 8;
        ((CScriptTraitsMetadata.ScriptTrait<int>)clone.Node.ScriptMetadata!.Traits["Count"]).Value = 12;

        await Assert.That(map.LightmapCacheData.Data[0]).IsEqualTo((byte)1);
        await Assert.That(map.ZoneGenealogyData.Data[0]).IsEqualTo((byte)4);
        await Assert.That(trait.Value).IsEqualTo(7);
        var clonedArrayType = (CScriptTraitsMetadata.ScriptArrayType)clone.Node.ScriptMetadata.Traits["NestedType"].Type;
        await Assert.That(clonedArrayType.ValueType).IsNotSameReferenceAs(structType);
    }
}
