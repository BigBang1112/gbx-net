using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnCollectionChunkTests
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 1)]
    [Arguments(0, 2)]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(1, 2)]
    [Arguments(2, 0)]
    [Arguments(2, 1)]
    [Arguments(2, 2)]
    public async Task Chunk0303300D_RoundTripsAbsentInternalAndExternalIcons(int iconKind, int smallIconKind)
    {
        // Reference kinds: 0 = absent, 1 = internal bitmap, 2 = external file.
        var refTable = new GbxRefTable();
        var iconFile = iconKind == 2 ? new GbxRefTableFile(refTable, 0, true, "Icon.Gbx") : null;
        var smallIconFile = smallIconKind == 2 ? new GbxRefTableFile(refTable, 0, true, "SmallIcon.Gbx") : null;
        var files = new Dictionary<int, GbxRefTableNode>();
        var index = 1;
        if (iconFile is not null) files[index] = iconFile;
        if (iconKind != 0) index++;
        if (smallIconFile is not null) files[index] = smallIconFile;

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            WriteIcon(writer, iconKind, iconFile);
            WriteIcon(writer, smallIconKind, smallIconFile);
            writer.Write(0x12345678);
        }

        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(files);
        using var rw = new GbxReaderWriter(reader);
        var restored = new CGameCtnCollection();
        var chunk = new CGameCtnCollection.Chunk0303300D();
        chunk.ReadWrite(restored, rw);

        await Assert.That(restored.IconFidFile).IsEqualTo(iconFile);
        await Assert.That(restored.IconSmallFidFile).IsEqualTo(smallIconFile);
        if (iconKind == 0) await Assert.That(restored.IconFid).IsNull();
        if (iconKind == 1) await Assert.That(restored.IconFid).IsNotNull();
        if (smallIconKind == 0) await Assert.That(restored.IconSmallFid).IsNull();
        if (smallIconKind == 1) await Assert.That(restored.IconSmallFid).IsNotNull();
        await Assert.That(stream.Position).IsEqualTo(payloadLength);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(restored, writerWriter);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray().Take((int)payloadLength))).IsTrue();
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
    public async Task Chunk03033038_ReadsNativeWaterVersions(int version)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            if (version < 4) writer.Write(12.5f);
            if (version == 0)
            {
                writer.Write(13.5f);
                writer.Write(14.5f);
            }
            else
            {
                if (version >= 8)
                {
                    writer.Write(-1); // WaterArray
                    writer.Write(0); // Water0 archive version
                }
                var waterCount = version >= 8 ? 1 : version < 4 ? 2 : 4;
                for (var i = 0; i < waterCount; i++)
                {
                    writer.WriteIdAsString($"Water{i}");
                    writer.Write(20f + i);
                    writer.Write(-20f - i);
                    if (version >= 3) writer.Write(0.5f + i);
                    if (version >= 2) writer.Write(-1); // First water material FID
                    if (version >= 6) writer.Write(-1); // Second water material FID
                }
            }
            if (version >= 5)
            {
                writer.WriteNodeRef(new CPlugBitmap());
                writer.Write(1.25f);
                writer.Write(2.25f);
                writer.Write(3.25f);
                writer.Write(4.25f);
                if (version >= 7) writer.Write(true);
            }
            writer.Write(-123f);
            if (version < 4) writer.Write(true);
            writer.Write(true);
            if (version < 3) writer.Write(0.75f);
            writer.Write(0.125f);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnCollection.Chunk03033038());
        await Assert.That(restored.CameraMinHeight).IsEqualTo(-123f);
        await Assert.That(restored.IsWaterMultiHeight).IsTrue();
        await Assert.That(restored.WaterFogClampAboveDist).IsEqualTo(0.125f);
        if (version > 0) await Assert.That(restored.Water1?.Id).IsEqualTo("Water0");
        if (version >= 5) await Assert.That(restored.WaterG_BitmapNormal).IsNotNull();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Chunk0303303D_ReadsNativeTvProgramOrder(int version)
    {
        var refTable = new GbxRefTable();
        var files = new Dictionary<int, GbxRefTableNode>();
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            if (version < 2) WriteFile("Legacy", writer, refTable, files);
            WriteFile("64x10A", writer, refTable, files);
            WriteFile("64x10B", writer, refTable, files);
            WriteFile("64x10C", writer, refTable, files);
            WriteFile("2x3", writer, refTable, files);
            if (version > 0) WriteFile("155", writer, refTable, files);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnCollection.Chunk0303303D(), files);
        await Assert.That(restored.BitmapDisplayControlDefaultTVProgram_64x10AFile?.FilePath).IsEqualTo("64x10A.Gbx");
        await Assert.That(restored.BitmapDisplayControlDefaultTVProgram_2x3File?.FilePath).IsEqualTo("2x3.Gbx");
        if (version > 0) await Assert.That(restored.BitmapDisplayControlDefaultTVProgram_155File?.FilePath).IsEqualTo("155.Gbx");
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
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
    [Arguments(19)]
    [Arguments(20)]
    [Arguments(21)]
    [Arguments(22)]
    public async Task Chunk03033039_ReadsNativeInventoryVersions(int version)
    {
        var refTable = new GbxRefTable();
        var files = new Dictionary<int, GbxRefTableNode>();
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            writer.Write(-1); // VehicleStyles
            if (version >= 2) writer.Write(-1); // ItemPlacementGroups
            if (version >= 3) writer.Write(-1); // AdnRandomGenList
            if (version >= 4) WriteFile("Groups", writer, refTable, files);
            if (version is >= 5 and <= 10)
            {
                writer.WriteNodeRef(new CPlugBitmap());
                if (version >= 8) writer.Write(-1); // Legacy image
            }
            if (version >= 6) WriteFile("Blocks", writer, refTable, files);
            if (version is 7 or 8) writer.Write(-1); // Legacy inventory
            if (version >= 10) WriteFile("Items", writer, refTable, files);
            if (version == 12) writer.Write("Materials");
            if (version >= 11)
            {
                for (var i = 0; i < 5; i++) writer.WriteNodeRef(new CPlugBitmap());
            }
            if (version >= 20) WriteFile("SponsorBig", writer, refTable, files);
            if (version >= 21)
            {
                WriteFile("SponsorWide", writer, refTable, files);
                WriteFile("Screen16x9", writer, refTable, files);
                WriteFile("Screen8x1", writer, refTable, files);
                WriteFile("Screen16x1", writer, refTable, files);
            }
            if (version >= 22)
            {
                foreach (var name in new[] { "Advertisement16x9", "Advertisement1x1", "Advertisement2x1", "Advertisement2x3", "Advertisement4x1", "ItemFlag" })
                    WriteFile(name, writer, refTable, files);
            }
            if (version >= 13) WriteFile("DefaultMaterial", writer, refTable, files);
            if (version >= 14) WriteFile("Macroblocks", writer, refTable, files);
            if (version >= 15) WriteFile("SpawnClips", writer, refTable, files);
            if (version >= 16) writer.Write(new Ident("Snow", "Stadium", "Nadeo"));
            if (version >= 17) writer.Write(new Ident("Rally", "Stadium", "Nadeo"));
            if (version >= 18) writer.Write(new Ident("Desert", "Stadium", "Nadeo"));
            if (version >= 19) writer.Write(42);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnCollection.Chunk03033039(), files);
        if (version >= 10) await Assert.That(restored.FidItemModelInventoryFile?.FilePath).IsEqualTo("Items.Gbx");
        if (version == 12) await Assert.That(restored.LegacyFolderMaterial).IsEqualTo("Materials");
        if (version >= 11) await Assert.That(restored.CustomDeco?.DecorationScreen16x1).IsNotNull();
        if (version >= 13) await Assert.That(restored.DefaultMaterialFile?.FilePath).IsEqualTo("DefaultMaterial.Gbx");
        if (version >= 14) await Assert.That(restored.FidMacroBlockInfoInventoryFile?.FilePath).IsEqualTo("Macroblocks.Gbx");
        if (version >= 15) await Assert.That(restored.DefaultSpawnClipListFile?.FilePath).IsEqualTo("SpawnClips.Gbx");
        if (version >= 22) await Assert.That(restored.BlockSkins_Default_FidAdvertisement16x9File?.FilePath).IsEqualTo("Advertisement16x9.Gbx");
    }

    [Test]
    public async Task Chunk03033039_ReadsVehicleStylesAndRandomGenerators()
    {
        var group = new CPlugVehicleVisStyleRandomGroup
        {
            Name = "Traffic",
            MatchName = "Car*",
            GroupProba = 0.75f,
            Styles = [new() { Color = 0xFF123456, Proba = 0.25f }]
        };
        group.CreateChunk<CPlugVehicleVisStyleRandomGroup.Chunk09147000>().Version = 4;
        var styles = new CPlugVehicleVisStyles { RandomGroups = [group], UnwantedTrafficVehiclesMatchName = "Truck*" };
        styles.CreateChunk<CPlugVehicleVisStyles.Chunk09146000>().Version = 1;

        var generator = new CPlugAdnRandomGen
        {
            RandomGenId = "Traffic",
            RandSeed = 42,
            cModel = 3,
            Sets = [new() { RequiredTags = [new() { Type = "Vehicle", Value = "Car" }] }]
        };
        generator.CreateChunk<CPlugAdnRandomGen.Chunk09140000>().Version = 6;
        var generators = new CPlugAdnRandomGenList { Datas = [generator] };
        generators.CreateChunk<CPlugAdnRandomGenList.Chunk09157000>().Version = 1;

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(3);
            writer.WriteNodeRef(styles);
            writer.Write(-1); // ItemPlacementGroups
            writer.WriteNodeRef(generators);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnCollection.Chunk03033039());
        await Assert.That(restored.VehicleStyles?.RandomGroups?[0]?.Name).IsEqualTo("Traffic");
        await Assert.That(restored.VehicleStyles?.RandomGroups?[0]?.Styles?[0].Proba).IsEqualTo(0.25f);
        await Assert.That(restored.AdnRandomGenList?.Datas?[0]?.Sets[0].RequiredTags?[0].Value).IsEqualTo("Car");
    }

    [Test]
    public async Task Chunk03033030_ReadsCarMarksModels()
    {
        var subModel = new CSceneVehicleCarMarksModelSub { Width = 0.6f, WidthMax = 25f, CondMaterialId = 5 };
        subModel.CreateChunk<CSceneVehicleCarMarksModelSub.Chunk0A082003>();
        var model = new CSceneVehicleCarMarksModel { Models = [subModel], Disabled = true };
        model.CreateChunk<CSceneVehicleCarMarksModel.Chunk0A081000>();
        model.CreateChunk<CSceneVehicleCarMarksModel.Chunk0A081001>();

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteNodeRef(model);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnCollection.Chunk03033030());
        await Assert.That(restored.MarksModel?.Disabled).IsEqualTo(true);
        await Assert.That(restored.MarksModel?.Models?[0]?.Width).IsEqualTo(0.6f);
        await Assert.That(restored.MarksModel?.Models?[0]?.WidthMax).IsEqualTo(25f);
        await Assert.That(restored.MarksModel?.Models?[0]?.CondMaterialId).IsEqualTo((byte)5);
    }

    private static void WriteFile(string name, GbxWriter writer, GbxRefTable refTable, Dictionary<int, GbxRefTableNode> files)
    {
        var file = new GbxRefTableFile(refTable, 0, true, name + ".Gbx");
        // Writer indices also include the internal bitmap nodes.
        var index = writer.NodeDict.Count + 1;
        files[index] = file;
        writer.WriteNodeRef<CMwNod>(null, file);
    }

    private static async Task<CGameCtnCollection> ReadAndRewrite(
        MemoryStream stream, Chunk<CGameCtnCollection> chunk, Dictionary<int, GbxRefTableNode>? files = null)
    {
        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        if (files is not null) reader.LoadRefTable(files);
        using var rw = new GbxReaderWriter(reader);
        var restored = new CGameCtnCollection();
        chunk.ReadWrite(restored, rw);
        await Assert.That(stream.Position).IsEqualTo(payloadLength);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(restored, writerWriter);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray().Take((int)payloadLength))).IsTrue();
        return restored;
    }

    private static void WriteIcon(GbxWriter writer, int kind, GbxRefTableFile? file)
    {
        writer.Write(kind != 0);
        if (kind != 0)
        {
            writer.WriteNodeRef(kind == 1 ? new CPlugBitmap() : null, file);
        }
    }
}
