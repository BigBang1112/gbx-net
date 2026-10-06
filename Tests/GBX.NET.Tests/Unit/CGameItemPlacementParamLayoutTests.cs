using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameItemPlacementParamLayoutTests
{
    private const int sentinel = 0x11223344;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task LegacyPlacementFieldsRoundTrip(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            WritePlacementGroups(writer);
            writer.Write(true);
            if (version >= 1) writer.Write(false);
            writer.Write(sentinel);
        }

        var node = new CGameItemPlacementParam();
        var chunk = new CGameItemPlacementParam.Chunk2E020003();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.PlacementClass!.SizeGroup).IsEqualTo("Size");
        await Assert.That(node.PlacementClass.CompatibleGroupsIds!).IsEquivalentTo(new[] { "GroupA", "GroupB" }, CollectionOrdering.Matching);
        await Assert.That(node.PlacementClass.AlwaysUp).IsTrue();
        await Assert.That(node.PlacementClass.AlignToInterior).IsEqualTo(version == 0);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

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
    public async Task InlinePlacementArchivePreservesVersionBranches(int archiveVersion)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(3); // Chunk version, independent of the placement archive version.
            writer.Write(archiveVersion);
            WritePlacementGroups(writer);
            writer.Write(true);
            writer.Write(false);
            if (archiveVersion >= 8)
            {
                writer.Write(true);
                writer.Write(new Vec3(1, 2, 3));
            }

            writer.Write(1); // Patch count.
            if (archiveVersion < 4)
            {
                writer.Write(11);
                writer.Write(12);
                writer.Write(13.5f);
                writer.Write(14);
                writer.Write(15);
                writer.Write(16.5f);
                writer.Write(17.5f);
                if (archiveVersion == 3) writer.Write(18.5f);
            }
            else
            {
                writer.Write(21); // ItemCount.
                writer.Write(22.5f); // ItemSpacing.
                writer.Write(23); // FillAlign.
                writer.Write(24); // FillDir.
                writer.Write(25.5f); // NormedPos.
                if (archiveVersion >= 5) writer.Write(26.5f);
                if (archiveVersion >= 10)
                {
                    writer.Write(2);
                    writer.WriteIdAsString("OnlyA");
                    writer.WriteIdAsString("OnlyB");
                }
                else
                {
                    writer.WriteIdAsString("OnlyA");
                }
                if (archiveVersion >= 7) writer.Write(27.5f);
                if (archiveVersion >= 9) writer.Write(28.5f);
            }

            if (archiveVersion >= 6)
            {
                writer.Write(2);
                writer.Write(31);
                writer.Write(32);
            }
            else if (archiveVersion >= 2)
            {
                writer.Write(33); // Deprecated scalar, retained for writing.
            }
            writer.Write(sentinel);
        }

        var node = new CGameItemPlacementParam();
        var chunk = new CGameItemPlacementParam.Chunk2E020003();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        var placement = node.PlacementClass!;
        await Assert.That(chunk.Version).IsEqualTo(3);
        await Assert.That(placement.Version).IsEqualTo(archiveVersion);
        await Assert.That(placement.WorldDir).IsEqualTo(archiveVersion >= 8 ? new Vec3(1, 2, 3) : new Vec3(0, 0, 1));
        if (archiveVersion < 4)
        {
            await Assert.That(placement.LegacyPatchLayouts![0].U07).IsEqualTo(17.5f);
            await Assert.That(placement.LegacyPatchLayouts[0].U08).IsEqualTo(archiveVersion == 3 ? 18.5f : 0);
        }
        else
        {
            var patch = placement.PatchLayouts![0];
            await Assert.That(patch.ItemCount).IsEqualTo(21);
            await Assert.That(patch.NormedPos).IsEqualTo(25.5f);
            await Assert.That(patch.U01).IsEqualTo(archiveVersion >= 5 ? 26.5f : 0);
            await Assert.That(patch.Altitude).IsEqualTo(archiveVersion >= 7 ? 27.5f : 0);
            await Assert.That(patch.U02).IsEqualTo(archiveVersion >= 9 ? 28.5f : 0);
            if (archiveVersion >= 10)
                await Assert.That(patch.OnlyOnGroups!).IsEquivalentTo(new[] { "OnlyA", "OnlyB" }, CollectionOrdering.Matching);
            else
                await Assert.That(patch.OnlyOnGroup).IsEqualTo("OnlyA");
        }
        if (archiveVersion >= 6)
            await Assert.That(placement.GroupCurPatchLayouts!).IsEquivalentTo(new[] { 31, 32 }, CollectionOrdering.Matching);
        else if (archiveVersion >= 2)
            await Assert.That(placement.U01).IsEqualTo(33);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task PlacementReferencePreservesExternalFile(bool legacyChunk)
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "Placement.PlaceParam.Gbx");
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (legacyChunk) writer.Write(4);
            writer.Write(1);
            writer.Write(sentinel);
        }

        payload.Position = 0;
        var node = new CGameItemPlacementParam();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
            if (legacyChunk) new CGameItemPlacementParam.Chunk2E020003().ReadWrite(node, rw);
            else new CGameItemPlacementParam.Chunk2E020005().ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        await Assert.That(node.PlacementClassFile).IsSameReferenceAs(file);
        await AssertRoundTrip(payload, rw =>
        {
            if (legacyChunk) new CGameItemPlacementParam.Chunk2E020003 { Version = 4 }.ReadWrite(node, rw);
            else new CGameItemPlacementParam.Chunk2E020005().ReadWrite(node, rw);
        });
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task MagnetLocationsPreservePositionAndDegreeRotations(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(2);
            foreach (var value in new[] { 1f, 2f, 3f, 90f, -45f, 180f, 4f, 5f, 6f, -90f, 45f, -180f }) writer.Write(value);
            writer.Write(sentinel);
        }
        var node = new CGameItemPlacementParam();
        var chunk = new CGameItemPlacementParam.Chunk2E020004();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.MagnetLocs!.Length).IsEqualTo(2);
        await Assert.That(node.MagnetLocs[0].Position).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(node.MagnetLocs[1].YawPitchRoll).IsEqualTo(new Vec3(-90, 45, -180));
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task FlagsPreserveReservedBitsAndNativeDefaults()
    {
        var node = new CGameItemPlacementParam();
        await Assert.That(node.IsFreelyAnchorable).IsTrue();
        await Assert.That(node.PivotSnapDistance).IsEqualTo(-1f);
        node.Flags = unchecked((short)0x8000);
        node.GhostMode = true;
        node.HasPath = true;
        node.IsFreelyAnchorable = true;
        await Assert.That((ushort)node.Flags).IsEqualTo((ushort)0x8061);
        node.GhostMode = false;
        node.HasPath = false;
        node.IsFreelyAnchorable = false;
        await Assert.That((ushort)node.Flags).IsEqualTo((ushort)0x8000);
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    public async Task UnsupportedChunkVersionsStopBeforeReadingFields(int chunkId)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(chunkId == 3 ? 5 : 1);
            writer.Write(sentinel);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameItemPlacementParam();
        if (chunkId == 3)
            Assert.Throws<GBX.NET.Exceptions.ChunkVersionNotSupportedException>(() => new CGameItemPlacementParam.Chunk2E020003().ReadWrite(node, rw));
        else
            Assert.Throws<NotSupportedException>(() => new CGameItemPlacementParam.Chunk2E020000().ReadWrite(node, rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
    }

    [Test]
    [Arguments("MP4 001.Item.Gbx")]
    [Arguments("MP4 002.Item.Gbx")]
    [Arguments("MP4 003.Item.Gbx")]
    [Arguments("TM2020 001.Item.Gbx")]
    [Arguments("TM2020 002.Item.Gbx")]
    [Arguments("TM2020 003.Item.Gbx")]
    [Arguments("TM2020 004.Block.Gbx")]
    [Arguments("TM2020 005.Item.Gbx")]
    [Arguments("TM2020 006.Item.Gbx")]
    public async Task ExistingItemFixturesRoundTrip(string suffix)
    {
        var gbx = Gbx.Parse(TestFiles.Gbx($"CGameItemModel/GBX-NET 2 CGameItemModel {suffix}"));
        gbx.BodyCompression = GbxCompression.Uncompressed;
        using var saved = new MemoryStream();
        gbx.Save(saved);
        saved.Position = 0;
        var parsed = Gbx.Parse(saved);
        using var savedAgain = new MemoryStream();
        parsed.Save(savedAgain);
        await Assert.That(savedAgain.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }

    private static void WritePlacementGroups(GbxWriter writer)
    {
        writer.WriteIdAsString("Size");
        writer.Write(2);
        writer.WriteIdAsString("GroupA");
        writer.WriteIdAsString("GroupB");
    }

    private static void ReadChunk(MemoryStream payload, Action<GbxReaderWriter> readWrite)
    {
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        readWrite(rw);
        if (reader.ReadInt32() != sentinel || payload.Position != payload.Length)
            throw new InvalidDataException("Chunk did not consume its native payload exactly.");
    }

    private static async Task AssertRoundTrip(MemoryStream payload, Action<GbxReaderWriter> readWrite)
    {
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            readWrite(rw);
            writer.Write(sentinel);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
