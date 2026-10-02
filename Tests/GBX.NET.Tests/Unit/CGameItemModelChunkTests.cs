using GBX.NET.Attributes;
using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

public class CGameItemModelChunkTests
{
    [Test]
    [Arguments(CGameItemModel.EItemType.Undefined, 0, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 2, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 3, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 4, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 5, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 6, true)]
    [Arguments(CGameItemModel.EItemType.Undefined, 7, true)]
    [Arguments(CGameItemModel.EItemType.Ornament, 8, true)]
    [Arguments(CGameItemModel.EItemType.Ornament, 9, false)]
    [Arguments(CGameItemModel.EItemType.PickUp, 8, true)]
    [Arguments(CGameItemModel.EItemType.PickUp, 9, false)]
    [Arguments(CGameItemModel.EItemType.Spot, 8, true)]
    [Arguments(CGameItemModel.EItemType.Spot, 9, false)]
    [Arguments(CGameItemModel.EItemType.Vehicle, 9, true)]
    [Arguments(CGameItemModel.EItemType.Vehicle, 10, false)]
    [Arguments(CGameItemModel.EItemType.EntitySpawner, 11, true)]
    [Arguments(CGameItemModel.EItemType.EntitySpawner, 12, false)]
    [Arguments(CGameItemModel.EItemType.Block, 0, false)]
    [Arguments(CGameItemModel.EItemType.Block, 8, false)]
    [Arguments(CGameItemModel.EItemType.Block, 13, false)]
    [Arguments(CGameItemModel.EItemType.Block, 15, false)]
    public async Task Chunk2E002019_ReadsNativeLayoutAtItemTypeAndVersionBoundaries(CGameItemModel.EItemType itemType, int version, bool legacyModels)
    {
        var refTable = new GbxRefTable();
        var phyFile = new GbxRefTableFile(refTable, 0, true, "Physics.Gbx");
        var visFile = new GbxRefTableFile(refTable, 0, true, "Visual.Gbx");
        var editionFile = new GbxRefTableFile(refTable, 0, true, "Edition.Gbx");
        var materialFile = new GbxRefTableFile(refTable, 0, true, "Material.Gbx");
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            if (legacyModels)
            {
                writer.Write(1);
                writer.Write(2);
            }
            if (version >= 3) writer.WriteIdAsString("Weapon");
            if (version >= 4) writer.Write(-1);
            if (version >= 5) writer.Write(-1);
            if (version >= 6) writer.Write(0);
            if (version >= 7) writer.Write((int)CGameItemModel.EDefaultCam.Behind);
            if (version >= 8) writer.Write(3);
            if (version >= 13) writer.Write(-1);
            if (version >= 15) writer.Write(4);
            writer.Write(0x11223344);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode>
        {
            [1] = phyFile, [2] = visFile, [3] = editionFile, [4] = materialFile
        });
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameItemModel { ItemType = itemType };
        var chunk = new CGameItemModel.Chunk2E002019();
        chunk.ReadWrite(node, rw);

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.PhyModelCustomFile).IsEqualTo(legacyModels ? phyFile : null);
        await Assert.That(node.VisModelCustomFile).IsEqualTo(legacyModels ? visFile : null);
        await Assert.That(node.DefaultWeaponName).IsEqualTo(version >= 3 ? "Weapon" : null);
        if (version >= 6) await Assert.That(node.Actions).IsEmpty();
        await Assert.That(node.DefaultCam).IsEqualTo(version >= 7 ? CGameItemModel.EDefaultCam.Behind : default);
        await Assert.That(node.EntityModelEditionFile).IsEqualTo(version >= 8 ? editionFile : null);
        await Assert.That(node.EntityModel).IsNull();
        await Assert.That(node.MaterialModifierFile).IsEqualTo(version >= 15 ? materialFile : null);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task Chunk2E002019_PreservesExistingModelsAndWritesSentinels()
    {
        var phy = new CMwNod();
        var vis = new CMwNod();
        var node = new CGameItemModel { ItemType = CGameItemModel.EItemType.Block, PhyModelCustom = phy, VisModelCustom = vis };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(5);
            writer.WriteIdAsString("Weapon");
            writer.Write(42);
            writer.Write(43);
        }

        stream.Position = 0;
        using (var reader = new GbxReader(stream))
        using (var rw = new GbxReaderWriter(reader))
        {
            new CGameItemModel.Chunk2E002019().ReadWrite(node, rw);
        }
        await Assert.That(node.PhyModelCustom).IsSameReferenceAs(phy);
        await Assert.That(node.VisModelCustom).IsSameReferenceAs(vis);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            new CGameItemModel.Chunk2E002019 { Version = 5 }.ReadWrite(node, rw);
        }
        saved.Position = 0;
        using var savedReader = new GbxReader(saved);
        await Assert.That(savedReader.ReadInt32()).IsEqualTo(5);
        await Assert.That(savedReader.ReadIdAsString()).IsEqualTo("Weapon");
        await Assert.That(savedReader.ReadInt32()).IsEqualTo(-1);
        await Assert.That(savedReader.ReadInt32()).IsEqualTo(-1);
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
    }

    [Test]
    public async Task Chunk2E002019_RejectsBlockWithoutEditionModel()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        var node = new CGameItemModel { ItemType = CGameItemModel.EItemType.Block };

        var exception = Assert.Throws<Exception>(() => new CGameItemModel.Chunk2E002019 { Version = 8 }.ReadWrite(node, rw));
        await Assert.That(exception.Message).IsEqualTo("EntityModel cannot exist for Block");
    }

    [Test]
    [Arguments(nameof(CGameItemModel.PhyModelCustom), 0)]
    [Arguments(nameof(CGameItemModel.VisModelCustom), 0)]
    [Arguments(nameof(CGameItemModel.DefaultWeaponName), 3)]
    [Arguments(nameof(CGameItemModel.Actions), 6)]
    [Arguments(nameof(CGameItemModel.DefaultCam), 7)]
    [Arguments(nameof(CGameItemModel.EntityModelEdition), 8)]
    [Arguments(nameof(CGameItemModel.EntityModel), 8)]
    [Arguments(nameof(CGameItemModel.VFX), 13)]
    [Arguments(nameof(CGameItemModel.MaterialModifier), 15)]
    public async Task ModelPropertiesHaveChunkVersionMetadata(string name, int sinceVersion)
    {
        var attribute = typeof(CGameItemModel).GetProperty(name)!.GetCustomAttributes<AppliedWithChunkAttribute>().Single();
        await Assert.That(attribute.ChunkType).IsEqualTo(typeof(CGameItemModel.Chunk2E002019));
        await Assert.That(attribute.SinceVersion).IsEqualTo(sinceVersion);
        await Assert.That(attribute.UpToVersion).IsNull();
    }
}
