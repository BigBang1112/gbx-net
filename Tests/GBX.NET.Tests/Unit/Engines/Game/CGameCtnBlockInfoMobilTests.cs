using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoMobilTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    [Arguments(21)]
    [Arguments(22)]
    [Arguments(23)]
    public async Task ReadWrite_MobilVersions_PreservePayloadAndChunkBoundary(int version)
    {
        var payload = NativePayload(version, reading: true);
        var node = new CGameCtnBlockInfoMobil();
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122003();

        await Verify(payload, NativePayload(version, reading: false), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SolidFrequency).IsEqualTo(7);
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.HasGeomTransformation).IsEqualTo(version >= 1);
        if (version >= 18)
        {
            await Assert.That(node.VFXScale).IsEqualTo(2.5f);
            await Assert.That(node.VFXScale2).IsEqualTo(3.5f);
            await Assert.That(node.VFXPosition).IsEqualTo(new Vec3(10, 11, 12));
            await Assert.That(node.VFXDirection).IsEqualTo(new Vec3(3, 6, 9));
            await Assert.That(node.VFXAngle).IsEqualTo(13f);
        }
    }

    [Test]
    [Arguments(18, 1)]
    [Arguments(19, 1)]
    [Arguments(23, 1)]
    [Arguments(18, 2)]
    [Arguments(19, 3)]
    [Arguments(23, 3)]
    public async Task ReadWrite_VfxScaleModes_ConsumeOnlyTheirScaleValues(int version, int scaleType)
    {
        var node = new CGameCtnBlockInfoMobil();
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122003();
        await Verify(NativePayload(version, reading: true, scaleType: (byte)scaleType),
            NativePayload(version, reading: false, scaleType: (byte)scaleType), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VFXScaleType).IsEqualTo((byte)scaleType);
        await Assert.That(node.VFXScale).IsEqualTo(scaleType == 1 ? 2.5f : 0f);
        await Assert.That(node.VFXScale2).IsEqualTo(0f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_LegacyReferenceKind_NormalizesToOldMobilOnWrite(int version)
    {
        var node = new CGameCtnBlockInfoMobil();
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122003();
        await Verify(NativePayload(version, reading: true, legacyReferenceKind: true),
            NativePayload(version, reading: false), rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0f, false)]
    [Arguments(0.000009f, false)]
    [Arguments(-0.000009f, false)]
    [Arguments(0.000011f, true)]
    [Arguments(-0.000011f, true)]
    public async Task Write_GeometryFlag_UsesNativeTolerance(float translation, bool expectedFlag)
    {
        var node = new CGameCtnBlockInfoMobil
        {
            GeomTranslation = new Vec3(translation, 0, 0),
            HasGeomTransformation = !expectedFlag
        };
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122003 { Version = 1 };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);

        var bytes = stream.ToArray();
        await Assert.That(bytes[16]).IsEqualTo(expectedFlag ? (byte)1 : (byte)0);
        await Assert.That(bytes.Length).IsEqualTo(expectedFlag ? 41 : 17);
        await Assert.That(node.HasGeomTransformation).IsEqualTo(expectedFlag);
    }

    [Test]
    public async Task ReadWrite_DecalFrequency_PreservesFourByteValue()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(10);
            writer.Write(0);
            writer.Write(17);
            writer.Write(Sentinel);
        }
        var node = new CGameCtnBlockInfoMobil();
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122002();
        await Verify(payload.ToArray(), payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.NoDecalFrequency).IsEqualTo(17);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(4)]
    public async Task ReadWrite_LegacyAndLinkChunks_PreserveReferencesAndFrequency(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (offset == 0)
            {
                writer.Write(-1);
                writer.Write(19);
            }
            else
            {
                if (offset == 4) writer.Write(0); // Link chunk version.
                writer.Write(10);
                writer.Write(1);
                writer.Write(-1);
            }
            writer.Write(Sentinel);
        }
        var node = new CGameCtnBlockInfoMobil();
        Chunk<CGameCtnBlockInfoMobil> chunk = offset switch
        {
            0 => new CGameCtnBlockInfoMobil.Chunk03122000(),
            1 => new CGameCtnBlockInfoMobil.Chunk03122001(),
            _ => new CGameCtnBlockInfoMobil.Chunk03122004()
        };
        await Verify(payload.ToArray(), payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        if (offset == 0) await Assert.That(node.SolidFrequency).IsEqualTo(19);
        if (offset == 1) await Assert.That(node.SolidDecals?.Length).IsEqualTo(1);
        if (offset == 4) await Assert.That(node.DynaLinks?.Length).IsEqualTo(1);
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(17)]
    [Arguments(20)]
    [Arguments(23)]
    public async Task ReadWrite_NonemptyReferenceLists_PreserveCountsAndPrefixRules(int version)
    {
        var node = new CGameCtnBlockInfoMobil();
        var chunk = new CGameCtnBlockInfoMobil.Chunk03122003();
        await Verify(NativePayload(version, reading: true, nonemptyLists: true),
            NativePayload(version, reading: false, nonemptyLists: true), rw => chunk.ReadWrite(node, rw));
        if (version <= 5)
        {
            await Assert.That(node.LegacyRailPolylines?.Length).IsEqualTo(1);
            await Assert.That(node.RailPath?.PolyLines?.Length).IsEqualTo(1);
        }
        if (version >= 17) await Assert.That(node.CitizenRoadChunks?.Length).IsEqualTo(1);
        if (version >= 20) await Assert.That(node.PlacementPatches?.Length).IsEqualTo(1);
    }

    [Test]
    [Arguments(9)]
    [Arguments(17)]
    [Arguments(20)]
    [Arguments(22)]
    [Arguments(23)]
    [Arguments(24)]
    public async Task ReadWrite_RoadReferences_UseSpecificClassesAndPreserveLegacyPayload(int version)
    {
        CPlugRoadChunk road = version < 23 ? new CPlugRoadChunk() : new CPlugRoadChunkTraffic();
        CPlugRoadChunk citizen = version < 23 ? new CPlugRoadChunk() : new CPlugRoadChunkCitizen();
        CPlugRoadChunk placement = version <= 23 ? new CPlugRoadChunk() : new CPlugPlacementPatch();
        foreach (var source in new[] { road, citizen, placement })
        {
            var data = source.Chunks.Create<CPlugRoadChunk.Chunk09128000>();
            data.U01 = 73;
            data.U03 = [new Vec3(1, 2, 3)];
        }
        var recast = new CPlugRecastPolyMeshData();
        recast.Chunks.Create<CPlugRecastPolyMeshData.Chunk09150000>().Version = 5;
        var payload = NativePayload(version, reading: true, road: road, citizen: citizen,
            placement: placement, recast: recast);
        var node = new CGameCtnBlockInfoMobil();
        using (var input = new MemoryStream(payload))
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            new CGameCtnBlockInfoMobil.Chunk03122003().ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        await VerifyRoadReferences(node.RoadChunks);
        if (version >= 17) await VerifyRoadReferences(node.CitizenRoadChunks);
        if (version >= 20) await VerifyRoadReferences(node.PlacementPatches);
        if (version >= 15) await Assert.That(node.RecastPolyMeshData).IsNotNull();

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
            new CGameCtnBlockInfoMobil.Chunk03122003 { Version = 24 }.ReadWrite(node, rw);
        output.Position = 0;
        var restored = new CGameCtnBlockInfoMobil();
        using (var reader = new GbxReader(output))
        using (var rw = new GbxReaderWriter(reader))
            new CGameCtnBlockInfoMobil.Chunk03122003().ReadWrite(restored, rw);
        await VerifyRoadReferences(restored.RoadChunks);
        if (version >= 17) await VerifyRoadReferences(restored.CitizenRoadChunks);
        if (version >= 20) await VerifyRoadReferences(restored.PlacementPatches);
    }

    private static async Task VerifyRoadReferences(CPlugRoadChunk[]? roads)
    {
        await Assert.That(roads?.Length).IsEqualTo(3);
        await Assert.That(ReferenceEquals(roads![0], roads[1])).IsTrue();
        await Assert.That(roads[2]).IsNull();
        var data = roads[0].Chunks.Get<CPlugRoadChunk.Chunk09128000>();
        await Assert.That(data?.U01).IsEqualTo(73);
        await Assert.That(data?.U03?[0]).IsEqualTo(new Vec3(1, 2, 3));
    }

    private static byte[] NativePayload(int version, bool reading, byte scaleType = 0,
        bool legacyReferenceKind = false, bool nonemptyLists = false, CPlugRoadChunk? road = null,
        CPlugRoadChunk? citizen = null, CPlugRoadChunk? placement = null, CPlugRecastPolyMeshData? recast = null)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write(version);
        if (version <= 1)
        {
            writer.Write(reading && legacyReferenceKind);
            writer.Write(-1); // OldMobil or the discarded legacy node.
        }
        writer.Write(7); // SolidFrequency.
        if (version >= 1)
        {
            writer.Write((byte)1);
            for (var i = 1; i <= 6; i++) writer.Write((float)i);
        }
        if (version >= 2)
        {
            writer.Write(-1); // SolidFid.
            writer.Write(-1); // OldMobil, because SolidFid is null.
        }
        if (version >= 14) writer.Write(-1); // PrefabFid.
        if (version >= 3) writer.Write(-1); // OldSolidAggreg.
        if (version is 4 or 5) WriteList(writer, nonemptyLists); // LegacyRailPolylines.
        if (version is >= 5 and <= 10) writer.Write(false);
        if (version >= 6) writer.Write(-1); // RailPath.
        if (version is >= 7 and <= 22) writer.Write(-1); // TrafficPath.
        if (version >= 15) writer.WriteNodeRef(recast);
        if (version is >= 8 and <= 12) writer.Write(101);
        if (version >= 9)
        {
            if (version <= 22) writer.Write(10);
            WriteList(writer, nonemptyLists, road);
        }
        if (version is >= 10 and <= 12) writer.Write(102);
        if (version == 12) writer.Write(103);
        if (version is >= 16 and <= 22) writer.Write(-1); // CitizenNetworkPath.
        if (version >= 17)
        {
            if (version <= 22) writer.Write(10);
            WriteList(writer, nonemptyLists, citizen);
        }
        if (version >= 18)
        {
            writer.Write(-1); // VFXs.
            writer.Write(scaleType);
            if (scaleType is 0 or 1) writer.Write(2.5f);
            if (scaleType == 0) writer.Write(3.5f);
            if (version == 18)
            {
                for (var i = 1; i <= 12; i++) writer.Write((float)i); // Matrix, then position.
            }
            else
            {
                writer.Write(10f);
                writer.Write(11f);
                writer.Write(12f);
                writer.Write(3f);
                writer.Write(6f);
                writer.Write(9f);
            }
            writer.Write(13f); // VFXAngle.
        }
        if (version >= 20)
        {
            if (version < 23 || (version == 23 && reading)) writer.Write(10);
            WriteList(writer, nonemptyLists, placement);
        }
        if (version >= 21) writer.Write(-1); // Additional external asset.
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static void WriteList(GbxWriter writer, bool nonempty, CPlugRoadChunk? road = null)
    {
        if (road is not null)
        {
            writer.Write(3);
            writer.WriteNodeRef(road);
            writer.WriteNodeRef(road);
            writer.Write(-1);
            return;
        }
        writer.Write(nonempty ? 1 : 0);
        if (nonempty) writer.Write(-1);
    }

    private static async Task Verify(byte[] payload, byte[] expectedOutput, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(expectedOutput)).IsTrue();
    }
}
