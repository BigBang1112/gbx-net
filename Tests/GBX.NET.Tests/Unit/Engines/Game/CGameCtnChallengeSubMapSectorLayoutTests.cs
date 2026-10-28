using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeSubMapSectorLayoutTests
{
    [Test]
    public async Task ReadWrite_EmptySectors_OmitsEncapsulationAndClearsPreviousData()
    {
        var node = new CGameCtnChallenge
        {
            SubMapSectors = [new()],
            SubMapSectorGrid = new() { Indices = [0] }
        };
        await RoundTrip(w =>
        {
            w.Write(0);
            w.Write(0);
        }, node);
        await Assert.That(node.SubMapSectors).IsEmpty();
        await Assert.That(node.SubMapSectorGrid).IsNull();
    }

    [Test]
    [Arguments((byte)0)]
    [Arguments((byte)1)]
    [Arguments((byte)2)]
    public async Task ReadWrite_Sectors_PreservesGridReferencesAndTransforms(byte format)
    {
        var child = new CGameCtnChallenge { MapName = "Child map" };
        child.CreateChunk<CGameCtnChallenge.Chunk03043012>();
        byte[] indices = [255, 0, 1, 2, 255, 0];
        var node = new CGameCtnChallenge { Size = new Int3(2, 8, 3) };
        await RoundTrip(w =>
        {
            w.Write(0);
            w.Write(3);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(format);
                if (format != 1) inner.WriteData(indices);
                for (var i = 0; i < 3; i++)
                {
                    inner.Write(i == 2 ? string.Empty : "Maps/žlutá.Map.Gbx");
                    inner.WriteNodeRef(i == 2 ? null : child);
                    inner.Write(i + 4);
                    inner.Write(new Vec3(i + 1.5f, i + 2.5f, i + 3.5f));
                    inner.Write(new Vec3(i - 4.5f, i - 5.5f, i - 6.5f));
                    inner.Write(i * -90f);
                }
            });
        }, node);

        await Assert.That(node.SubMapSectorGrid!.Format).IsEqualTo(format);
        if (format == 1)
        {
            await Assert.That(node.SubMapSectorGrid.Indices).IsNull();
        }
        else
        {
            await Assert.That(node.SubMapSectorGrid.Indices!).IsEquivalentTo(indices, CollectionOrdering.Matching);
        }
        await Assert.That(node.SubMapSectors).Count().IsEqualTo(3);
        for (var i = 0; i < 3; i++)
        {
            var sector = node.SubMapSectors![i];
            await Assert.That(sector.MapFilePath).IsEqualTo(i == 2 ? string.Empty : "Maps/žlutá.Map.Gbx");
            await Assert.That(sector.GroundHeight).IsEqualTo(i + 4);
            await Assert.That(sector.Position).IsEqualTo(new Vec3(i + 1.5f, i + 2.5f, i + 3.5f));
            await Assert.That(sector.Pivot).IsEqualTo(new Vec3(i - 4.5f, i - 5.5f, i - 6.5f));
            await Assert.That(sector.Yaw).IsEqualTo(i * -90f);
        }
        await Assert.That(node.SubMapSectors![0].Map!.MapName).IsEqualTo("Child map");
        await Assert.That(node.SubMapSectors[1].Map!.MapName).IsEqualTo("Child map");
        await Assert.That(node.SubMapSectors[2].Map).IsNull();
        await Assert.That(new CGameCtnChallenge.SubMapSector().GroundHeight).IsEqualTo(-1);
    }

    [Test]
    public async Task ReadWrite_NegativeCount_FailsBeforeReadingEncapsulation()
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(0);
            writer.Write(-1);
            writer.Write(0x12345678);
        }
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<InvalidDataException>(() => new CGameCtnChallenge.Chunk0304305A().ReadWrite(new(), rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }

    [Test]
    public async Task ReadNode_NestedSectors_PreservesRecursiveMapChunks()
    {
        var grandchild = new CGameCtnChallenge { MapName = "Grandchild map" };
        grandchild.CreateChunk<CGameCtnChallenge.Chunk03043012>();
        var child = CreateMapWithSector(grandchild);
        var parent = CreateMapWithSector(child);
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input)) writer.WriteNode(parent);
        input.Position = 0;
        using var reader = new GbxReader(input);
        var restored = reader.ReadNode<CGameCtnChallenge>()!;
        await Assert.That(input.Position).IsEqualTo(input.Length);
        await Assert.That(restored.SubMapSectors![0].Map!.SubMapSectors![0].Map!.MapName).IsEqualTo("Grandchild map");
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) writer.WriteNode(restored);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);

        static CGameCtnChallenge CreateMapWithSector(CGameCtnChallenge child)
        {
            var map = new CGameCtnChallenge { SubMapSectors = [new() { Map = child }] };
            map.CreateChunk<CGameCtnChallenge.Chunk0304305A>();
            return map;
        }
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
        var chunk = new CGameCtnChallenge.Chunk0304305A();
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(chunk.Version).IsEqualTo(0);
            await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
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
