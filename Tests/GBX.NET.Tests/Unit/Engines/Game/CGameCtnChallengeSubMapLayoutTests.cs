using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeSubMapLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_EmptySubMaps_OmitsEncapsulationAndClearsPreviousEntries(int version)
    {
        var node = new CGameCtnChallenge
        {
            SubMapsInfos = [new() { BlockIndex = 3, SubMapDataRefForEditor = "Old.Map.Gbx" }]
        };
        await RoundTrip(w =>
        {
            w.Write(version);
            w.Write(0);
        }, node);

        await Assert.That(node.SubMapsInfos).IsEmpty();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_SubMaps_PreservesIndicesDataReferencesAndVersionedNodes(int version)
    {
        var child = new CGameCtnChallenge { MapName = "Child map" };
        child.CreateChunk<CGameCtnChallenge.Chunk03043012>();
        var node = new CGameCtnChallenge();
        await RoundTrip(w =>
        {
            w.Write(version);
            w.Write(2);
            if (version >= 1)
            {
                w.WriteEncapsulated(WriteEntries);
            }
            else
            {
                WriteEntries(w);
            }
        }, node);

        await Assert.That(node.SubMapsInfos).Count().IsEqualTo(2);
        var first = node.SubMapsInfos![0];
        await Assert.That(first.BlockIndex).IsEqualTo(3);
        await Assert.That(first.U01).IsEqualTo(7);
        await Assert.That(first.SubMapDataRefForEditor).IsEqualTo("Maps/é.Map.Gbx");
        if (version >= 1)
        {
            await Assert.That(first.SubMapNod!.MapName).IsEqualTo("Child map");
        }
        else
        {
            await Assert.That(first.SubMapNod).IsNull();
        }
        var second = node.SubMapsInfos[1];
        await Assert.That(second.BlockIndex).IsEqualTo(1);
        await Assert.That(second.U01).IsEqualTo(-9);
        await Assert.That(second.SubMapDataRefForEditor).IsEqualTo(string.Empty);
        await Assert.That(second.SubMapNod).IsNull();
        await Assert.That(new CGameCtnChallenge.Chunk03043058(GameVersion.MP4).Version).IsEqualTo(1);

        void WriteEntries(GbxWriter w)
        {
            w.Write(3);
            w.Write(7);
            w.Write("Maps/é.Map.Gbx");
            if (version >= 1) w.WriteNodeRef(child);
            w.Write(1);
            w.Write(-9);
            w.Write(string.Empty);
            if (version >= 1) w.Write(-1);
        }
    }

    [Test]
    public async Task ReadNode_NestedSubMaps_PreservesRecursiveMapChunks()
    {
        var grandchild = new CGameCtnChallenge { MapName = "Grandchild map" };
        grandchild.CreateChunk<CGameCtnChallenge.Chunk03043012>();
        var child = new CGameCtnChallenge
        {
            SubMapsInfos = [new() { BlockIndex = 0, SubMapDataRefForEditor = "Grandchild.Map.Gbx", SubMapNod = grandchild }]
        };
        child.CreateChunk<CGameCtnChallenge.Chunk03043058>().Version = 1;
        var parent = new CGameCtnChallenge
        {
            SubMapsInfos = [new() { BlockIndex = 2, SubMapDataRefForEditor = "Child.Map.Gbx", SubMapNod = child }]
        };
        parent.CreateChunk<CGameCtnChallenge.Chunk03043058>().Version = 1;

        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input)) writer.WriteNode(parent);
        input.Position = 0;
        using var reader = new GbxReader(input);
        var restored = reader.ReadNode<CGameCtnChallenge>()!;
        await Assert.That(input.Position).IsEqualTo(input.Length);
        var restoredChild = restored.SubMapsInfos![0].SubMapNod!;
        await Assert.That(restoredChild.SubMapsInfos![0].SubMapNod!.MapName).IsEqualTo("Grandchild map");

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) writer.WriteNode(restored);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_NegativeCount_FailsBeforeReadingEntries(int version)
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(version);
            writer.Write(-1);
            writer.Write(0x12345678);
        }
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        var chunk = new CGameCtnChallenge.Chunk03043058();
        Assert.Throws<InvalidDataException>(() => chunk.ReadWrite(new CGameCtnChallenge(), rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }

    private static async Task RoundTrip(Action<GbxWriter> write, CGameCtnChallenge node)
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            write(writer);
            writer.Write(0x12345678);
        }
        input.Position = 0;
        var chunk = new CGameCtnChallenge.Chunk03043058();
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(0x12345678);
        }
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }
}
