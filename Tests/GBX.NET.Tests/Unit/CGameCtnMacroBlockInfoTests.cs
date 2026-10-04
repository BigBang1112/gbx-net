using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMacroBlockInfoTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task LegacyBlockSpawn_PreservesMobilAndVariantFields(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(new Ident("Block", "Stadium", "Author"));
            w.Write(new Int3(1, 2, 3));
            w.Write(2);
            w.Write(4);
            w.Write(5);
            w.Write(true);
            if (version == 1) w.Write(6);
        });
        var spawn = new CGameCtnMacroBlockInfo.BlockSpawn();
        await VerifyArchive(payload, spawn);
        await Assert.That(spawn.MobilIndex).IsEqualTo(4);
        await Assert.That(spawn.MobilVariantIndex).IsEqualTo(5);
        await Assert.That(spawn.IsGround).IsTrue();
        await Assert.That(spawn.BlockInfoVariantIndex).IsEqualTo(version == 1 ? 6 : 0);
    }

    [Test]
    [Arguments(7)]
    [Arguments(8)]
    public async Task BlockSpawn_ColorAndLightmapQualityHaveIndependentVersionGates(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(new Ident("Block", "Stadium", "Author"));
            w.Write(0x00030201); // Mobil indices and variant, followed by block flags.
            w.Write(new byte[] { 1, 2, 3, 2 });
            w.Write(-1); // Waypoint node reference.
            w.Write((byte)4);
            if (version == 8) w.Write((byte)2);
        });
        var spawn = new CGameCtnMacroBlockInfo.BlockSpawn();
        await VerifyArchive(payload, spawn);
        await Assert.That(spawn.Color).IsEqualTo((byte)4);
        await Assert.That(spawn.LightmapQuality).IsEqualTo((byte)(version == 8 ? 2 : 0));
    }

    [Test]
    public async Task LegacyBlockSkinSpawn_EndsAfterCoordinate()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(-1); // Skin node reference.
            w.Write(new Int3(1, 2, 3));
        });
        var spawn = new CGameCtnMacroBlockInfo.BlockSkinSpawn();
        await VerifyArchive(payload, spawn);
        await Assert.That(spawn.Coord).IsEqualTo(new Int3(1, 2, 3));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task ObjectSpawn_LegacyFieldsUseExactVersionRanges(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(new Ident("Item", "Stadium", "Author"));
            if (version < 3)
            {
                w.Write((byte)2);
                if (version >= 1) w.Write((byte)1);
            }
            else w.Write(new Vec3(1, 2, 3));
            w.Write(new Int3(4, 5, 6));
            w.WriteIdAsString("Anchor");
            w.Write(new Vec3(7, 8, 9));
            if (version is >= 2 and < 5) w.Write(10);
            if (version is >= 4 and < 7) w.Write(11);
            if (version >= 6) w.Write((short)12);
        });
        var spawn = new CGameCtnMacroBlockInfo.ObjectSpawn();
        await VerifyArchive(payload, spawn);
        await Assert.That(spawn.NeighbourSpawnIndex).IsEqualTo(version is >= 2 and < 5 ? 10 : -1);
        await Assert.That(spawn.U02).IsEqualTo(version is >= 4 and < 7 ? 11 : 0);
        await Assert.That(spawn.Flags).IsEqualTo((short)(version >= 6 ? 12 : 0));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task SplineChunk_ReadsOnlyTheArraysForItsVersion(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            var count = version == 0 ? 4 : version == 1 ? 1 : 0;
            for (var i = 0; i < count; i++)
            {
                w.Write(10); // Deprecated array marker.
                w.Write(0);
            }
            if (version == 0)
            {
                w.Write(1);
                w.Write(2);
                w.Write(3);
            }
        });
        await VerifyChunk(payload, new CGameCtnMacroBlockInfo.Chunk0310D00C());
    }

    [Test]
    public async Task AutoTerrainChunk_PreservesNoncanonicalBoolean()
    {
        var payload = Payload(w =>
        {
            w.Write(10);
            w.Write(0); // Empty deprecated auto-terrain array.
            w.Write(1);
            w.Write(2); // The existing MP4 fixture contains this native DoBool value.
        });
        await VerifyChunk(payload, new CGameCtnMacroBlockInfo.Chunk0310D008());
    }

    [Test]
    public async Task LegacyMediaTrackerChunk_PreservesTheLeadingCount()
    {
        var payload = Payload(w =>
        {
            w.Write(3);
            w.Write(-1);
            w.Write(-1);
            w.Write(-1);
        });
        await VerifyChunk(payload, new CGameCtnMacroBlockInfo.Chunk0310D007());
    }

    [Test]
    public async Task LegacyDecalChunk_WritesTheCountBeforeTheVersion()
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write("Decal.dds");
            w.Write(1); // Decal count.
            w.Write(1); // Version.
            w.Write(0);
            w.WriteIdAsString("Group");
            w.Write(Iso4.Identity);
            w.Write(new Vec3(1, 2, 3));
        });
        await VerifyChunk(payload, new CGameCtnMacroBlockInfo.Chunk0310D005());
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static Task VerifyArchive(byte[] payload, IReadableWritable archive)
        => Verify(payload, rw => archive.ReadWrite(rw));

    private static Task VerifyChunk<T>(byte[] payload, T chunk)
        where T : IReadableWritableChunk<CGameCtnMacroBlockInfo>
    {
        var node = new CGameCtnMacroBlockInfo();
        return Verify(payload, rw => chunk.ReadWrite(node, rw));
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
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
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
