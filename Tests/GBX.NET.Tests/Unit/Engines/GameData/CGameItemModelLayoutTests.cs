using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.GameData;

[Category("Unit")]
public class CGameItemModelLayoutTests
{
    [Test]
    public async Task Constructor_UsesNativeItemDefaults()
    {
        var node = new CGameItemModel();

        await Assert.That(node.ItemType).IsEqualTo(CGameItemModel.EItemType.Vehicle);
        await Assert.That(node.WaypointType).IsEqualTo(CGameItemModel.EWaypointType.None);
        await Assert.That(node.DefaultCamIndex).IsEqualTo(-1);
        await Assert.That(node.OrbitalRadiusBase).IsEqualTo(-1f);
        await Assert.That(node.OrbitalPreviewAngle).IsEqualTo(0.15f);
        await Assert.That(node.DefaultPlacement).IsNotNull();
        await Assert.That(node.NadeoSkinsFids!.Count).IsEqualTo(7);
        await Assert.That(node.NadeoSkinsFids.All(x => x is null)).IsTrue();
        await Assert.That(new CGameItemModel.ItemGroupElement().Location).IsEqualTo(Iso4.Identity);
        await Assert.That(new CGameItemModel.Chunk2E002019(GameVersion.TM2020).Version).IsEqualTo(15);
        await Assert.That(new CGameItemModel.Chunk2E002019().Version).IsEqualTo(16);
    }

    [Test]
    public async Task ReadWrite_ItemTypeBodyChunk_UpdatesHeaderItemType()
    {
        var node = new CGameItemModel();
        var payload = Payload(w => w.Write((int)CGameItemModel.EItemType.Block));
        await RoundTrip(payload, rw => new CGameItemModel.Chunk2E002015().ReadWrite(node, rw));
        await Assert.That(node.ItemType).IsEqualTo(CGameItemModel.EItemType.Block);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) new CGameItemModel.HeaderChunk2E002000().ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_ModelActions_ContainDirectNodeBodies()
    {
        var payload = Payload(w =>
        {
            w.Write(6);
            w.WriteIdAsString("Weapon");
            w.Write(-1); // Physics reference.
            w.Write(-1); // Visual reference.
            w.Write(1); // Action count.
            w.Write(0xFACADE01U); // Direct action body without a reference index or class ID.
        });
        var node = new CGameItemModel { ItemType = CGameItemModel.EItemType.Block };
        var chunk = new CGameItemModel.Chunk2E002019();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Actions!.Length).IsEqualTo(1);
        await Assert.That(node.Actions[0].Chunks).IsEmpty();
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
    public async Task ReadWrite_Archetype_PreservesLegacyNamesAndSkinDirectory(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version <= 2)
            {
                w.Write("LegacyMesh.Gbx");
                w.Write("");
            }
            if (version >= 2) w.Write("Archetype.Item.Gbx");
            if (version >= 6) w.Write("Skins/MySkin");
            if (version >= 7) w.Write(-1);
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E00201E();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        if (version <= 2) await Assert.That(node.LegacyVisModelName).IsEqualTo("LegacyMesh.Gbx");
        if (version >= 2) await Assert.That(node.ArchetypeRef).IsEqualTo("Archetype.Item.Gbx");
        if (version >= 6) await Assert.That(node.SkinDirNameCustom).IsEqualTo("Skins/MySkin");
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
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    public async Task ReadWrite_Waypoint_VersionBoundariesPreserveFollowingData(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version <= 6)
            {
                if (version >= 5)
                {
                    w.Write("Inventory name");
                    w.Write("Inventory description");
                    w.Write(2);
                }
                if (version >= 4) w.Write(1000);
                if (version <= 2) w.Write((short)0x43);
            }
            w.Write((int)CGameItemModel.EWaypointType.Checkpoint);
            if (version >= 2 && version <= 7) w.Write(Iso4.Identity);
            if (version >= 6) w.Write(true);
            if (version >= 9 && version <= 12) w.Write(-1);
            if (version >= 11) w.Write((byte)2);
            if (version >= 12)
            {
                w.WriteNodeRef(new CPlugMediaClipList());
                w.WriteNodeRef(new CPlugMediaClipList());
            }
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E00201F();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.WaypointType).IsEqualTo(CGameItemModel.EWaypointType.Checkpoint);
        await Assert.That(node.DisableLightmap).IsEqualTo(version >= 6);
        if (version >= 11) await Assert.That(node.ItemFlags).IsEqualTo(CGameItemModel.EItemFlags.DisableAutoCreateSound);
        if (version >= 12)
        {
            await Assert.That(node.PodiumClipList).IsNotNull();
            await Assert.That(node.IntroClipList).IsNotNull();
            await Assert.That(node.PodiumClipList).IsNotSameReferenceAs(node.IntroClipList);
        }
    }

    [Test]
    [Arguments(0, "")]
    [Arguments(0, "Icon.dds")]
    [Arguments(1, "")]
    [Arguments(1, "Icon.dds")]
    [Arguments(2, "")]
    [Arguments(3, "Icon.dds")]
    public async Task ReadWrite_Icon_LegacyNodeOnlyFollowsEmptyReference(int version, string iconRef)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(iconRef);
            if (version <= 1 && iconRef.Length == 0) w.Write(-1);
            if (version >= 3) w.Write((byte)1);
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E002020();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.IconRef).IsEqualTo(iconRef);
    }

    [Test]
    public async Task ReadWrite_LegacyClipCounts_DoNotContainArrayElements()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(10);
            w.Write(3);
            w.Write(10);
            w.Write(5);
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E002022();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.LegacyItemClipIdCount).IsEqualTo(3);
        await Assert.That(node.LegacyItemClipIndexCount).IsEqualTo(5);
    }

    [Test]
    public async Task ReadWrite_ItemClips_PreserveTypeAndNonemptyElements()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write((byte)4); // Native Square clip type.
            w.Write(2);
            w.Write(Iso4.Identity);
            w.Write("Clip.Item.Gbx");
            w.Write(Iso4.Identity);
            w.Write("");
            w.WriteNodeRef(new CGameItemModel());
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E002023();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ItemClipType).IsEqualTo(CGameItemModel.EItemClipType.Square);
        await Assert.That(node.ItemClips!.Length).IsEqualTo(2);
        await Assert.That(node.ItemClips[0].ItemRef).IsEqualTo("Clip.Item.Gbx");
        await Assert.That(node.ItemClips[1].Item).IsNotNull();
    }

    [Test]
    public async Task ReadWrite_IconMacroblock_UsesEncapsulatedNodeReference()
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Encapsulation prefix.
            w.Write(8); // Encapsulated payload size.
            w.Write(0x0310D000U); // Class ID without a reference index.
            w.Write(0xFACADE01U);
        });
        var node = new CGameItemModel();
        var chunk = new CGameItemModel.Chunk2E002028();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.IconMacroBlockInfo).IsNotNull();
        await Assert.That(node.References.IconMacroBlockInfo.Kind).IsEqualTo(NodeRefKind.Embedded);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var suffixWriter = new GbxWriter(input)) suffixWriter.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            serialize(rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
