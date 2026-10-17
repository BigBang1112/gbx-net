using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeTrafficLayoutTests
{
    public static IEnumerable<(int ChunkVersion, int ModelVersion)> ElementVersions()
    {
        for (var chunkVersion = -1; chunkVersion <= 1; chunkVersion++)
        {
            for (var modelVersion = 0; modelVersion <= 6; modelVersion++)
            {
                yield return (chunkVersion, modelVersion);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(ElementVersions))]
    public async Task ReadWrite_Element_PreservesLegacyAndReferencedModels(int chunkVersion, int modelVersion)
    {
        Chunk<CGameCtnChallengeTrafficElemDesc> chunk = chunkVersion < 0
            ? new CGameCtnChallengeTrafficElemDesc.Chunk0316E000()
            : new CGameCtnChallengeTrafficElemDesc.Chunk0316E001();
        var node = await RoundTrip(w => WriteElement(w, chunkVersion, modelVersion), chunk);

        await Assert.That(node.TrafficObject).IsEqualTo(new Ident("Car", "Stadium", "Author"));
        await Assert.That(node.PathId).IsEqualTo("Path");
        await Assert.That(node.ElemId).IsEqualTo(chunkVersion < 0 ? "Path" : "Element");
        await Assert.That(node.ModelVersion).IsEqualTo(modelVersion);
        if (modelVersion < 5)
        {
            await Assert.That(node.Model).IsNull();
            await Assert.That(node.LegacyModel!.U01).IsEqualTo(12.5f);
            if (modelVersion >= 3)
            {
                await Assert.That(node.LegacyModel.U05).IsEqualTo(Offset);
            }
        }
        else
        {
            await Assert.That(node.Model!.DefaultSpeed).IsEqualTo(12.5f);
            await Assert.That(node.Model.BaseOffset).IsEqualTo(Offset);
            await Assert.That(node.Model.DefaultAcceleration).IsEqualTo(17.5f);
            await Assert.That(node.Faction).IsEqualTo(3);
            await Assert.That(node.TimeOffset.TotalMilliseconds).IsEqualTo(modelVersion >= 6 ? 1234 : 0);
        }
        if (chunkVersion >= 1)
        {
            await Assert.That(node.PackDesc!.FilePath).IsEqualTo("Traffic/Car.Gbx");
        }
        else
        {
            await Assert.That(node.PackDesc).IsNull();
        }
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_PathSquares_PreservesCoordinatesDimensionsAndLegacyTail(int version)
    {
        Chunk<CPlugTrafficPathSquares> chunk = version < 0
            ? new CPlugTrafficPathSquares.Chunk090D0000()
            : new CPlugTrafficPathSquares.Chunk090D0001();
        var node = await RoundTrip(w =>
        {
            if (version >= 0) w.Write(version);
            WriteSquares(w, "Path");
            if (version == 0) w.Write(42);
        }, chunk);

        await Assert.That(node.PathId).IsEqualTo("Path");
        await Assert.That(node.Squares).IsEquivalentTo(Coordinates, CollectionOrdering.Matching);
        await Assert.That(node.SquareSize).IsEqualTo(new Vec3(32, 8, 32));
        await Assert.That(node.BaseHeight).IsEqualTo(16f);
    }

    [Test]
    public async Task ReadWrite_ZoneSquares_PreservesCoordinatesAndDimensions()
    {
        var node = await RoundTrip(w => WriteSquares(w, "Zone"), new CPlugTrafficZoneSquares.Chunk090D2000());

        await Assert.That(node.ZoneId).IsEqualTo("Zone");
        await Assert.That(node.Squares).IsEquivalentTo(Coordinates, CollectionOrdering.Matching);
        await Assert.That(node.SquareSize).IsEqualTo(new Vec3(32, 8, 32));
        await Assert.That(node.BaseHeight).IsEqualTo(16f);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task ReadWrite_Model_PreservesMovementVersions(int version)
    {
        Chunk<CPlugTrafficObjectParams> chunk = version < 0
            ? new CPlugTrafficObjectParams.Chunk090D1000()
            : new CPlugTrafficObjectParams.Chunk090D1001();
        var node = await RoundTrip(w =>
        {
            if (version >= 0) w.Write(version);
            w.Write(12.5f);
            w.Write(1);
            if (version < 0) return;
            w.Write(2);
            if (version >= 1) w.Write(Offset);
            if (version == 2) w.Write(42);
            if (version >= 4) w.Write(1234);
            if (version >= 5) w.Write(17.5f);
            if (version >= 6) w.Write(32.5f);
        }, chunk);

        await Assert.That(new CPlugTrafficObjectParams().DefaultSpeed).IsEqualTo(1f);
        await Assert.That(new CPlugTrafficObjectParams().BaseOffset).IsEqualTo(Iso4.Identity);
        await Assert.That(node.DefaultSpeed).IsEqualTo(12.5f);
        await Assert.That(node.TrafficPathType).IsEqualTo(1);
        if (version >= 1) await Assert.That(node.BaseOffset).IsEqualTo(Offset);
        if (version >= 4) await Assert.That(node.PointDelay.TotalMilliseconds).IsEqualTo(1234);
        if (version >= 5) await Assert.That(node.DefaultAcceleration).IsEqualTo(17.5f);
        if (version >= 6) await Assert.That(node.DefaultDecceleration).IsEqualTo(32.5f);
    }

    [Test]
    public async Task ReadWrite_MapTraffic_PreservesPopulatedInlineNodes()
    {
        var model = CreateModel();
        var node = await RoundTrip(w =>
        {
            w.Write(2);
            w.WriteEncapsulated(inner =>
            {
                inner.Write(10);
                inner.Write(0); // spline paths
                inner.Write(10);
                inner.Write(2); // Encapsulation writes each model inline.
                inner.Write(CGameCtnChallengeTrafficElemDesc.Id);
                inner.Write(0x0316E001u);
                WriteElement(inner, 1, 6, model);
                inner.Write(0xFACADE01u);
                inner.Write(CGameCtnChallengeTrafficElemDesc.Id);
                inner.Write(0x0316E001u);
                WriteElement(inner, 1, 6, model);
                inner.Write(0xFACADE01u);
                inner.Write(10);
                inner.Write(1);
                inner.Write(CPlugTrafficPathSquares.Id);
                inner.Write(0x090D0001u);
                inner.Write(1);
                WriteSquares(inner, "Path");
                inner.Write(0xFACADE01u);
                inner.Write(10);
                inner.Write(1);
                inner.Write(CPlugTrafficZoneSquares.Id);
                inner.Write(0x090D2000u);
                WriteSquares(inner, "Zone");
                inner.Write(0xFACADE01u);
            });
        }, new CGameCtnChallenge.Chunk0304304D());

        await Assert.That(node.TrafficElemDescs![0].Model!.DefaultSpeed).IsEqualTo(12.5f);
        await Assert.That(node.TrafficElemDescs[1].Model!.DefaultSpeed).IsEqualTo(12.5f);
        await Assert.That(node.TrafficPathSquares![0].Squares).IsEquivalentTo(Coordinates, CollectionOrdering.Matching);
        await Assert.That(node.TrafficZoneSquares![0].Squares).IsEquivalentTo(Coordinates, CollectionOrdering.Matching);
    }

    private static readonly Int3[] Coordinates = [new(1, 2, 3), new(-4, 5, -6)];
    private static readonly Iso4 Offset = new(1, 0, 0, 0, 1, 0, 0, 0, 1, 11, 22, 33);

    private static void WriteSquares(GbxWriter w, string id)
    {
        w.WriteIdAsString(id);
        w.Write(Coordinates.Length);
        foreach (var coordinate in Coordinates) w.Write(coordinate);
        w.Write(new Vec3(32, 8, 32));
        w.Write(16f);
    }

    private static void WriteElement(GbxWriter w, int chunkVersion, int modelVersion, CPlugTrafficObjectParams? model = null)
    {
        if (chunkVersion >= 0)
        {
            w.Write(chunkVersion);
            w.WriteIdAsString("Element");
        }
        w.Write(new Ident("Car", "Stadium", "Author"));
        w.WriteIdAsString("Path");
        w.Write(modelVersion);
        if (modelVersion < 5)
        {
            w.Write(12.5f);
            w.Write(1);
            w.Write(2);
            if (modelVersion >= 2) w.Write(3);
            if (modelVersion >= 3) w.Write(Offset);
            if (modelVersion >= 4) w.Write(1234);
        }
        else
        {
            w.WriteNodeRef(model ?? CreateModel());
            w.Write(3);
            if (modelVersion >= 6) w.Write(1234);
        }
        if (chunkVersion >= 1) w.Write(new PackDesc("Traffic/Car.Gbx"));
    }

    private static CPlugTrafficObjectParams CreateModel()
    {
        var model = new CPlugTrafficObjectParams
        {
            DefaultSpeed = 12.5f,
            TrafficPathType = 1,
            TrafficModel = 2,
            BaseOffset = Offset,
            DefaultAcceleration = 17.5f,
            DefaultDecceleration = 32.5f
        };
        model.CreateChunk<CPlugTrafficObjectParams.Chunk090D1001>().Version = 6;
        return model;
    }

    private static async Task<T> RoundTrip<T>(Action<GbxWriter> write, Chunk<T> chunk) where T : CMwNod, new()
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            write(writer);
            writer.Write(0x12345678);
        }
        input.Position = 0;
        var node = new T();
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
        return node;
    }
}
