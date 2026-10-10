using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockInfoLayoutTests
{
    [Test]
    [Arguments(0x0304E005u)]
    public async Task LegacyBlockDimensions_PreserveOriginalFieldOrder(uint chunkId)
    {
        var node = await RoundTrip(Payload(w =>
        {
            w.Write(chunkId);
            w.WriteIdAsString("LegacyBlock");
            w.Write(2);
            w.Write(3);
            w.Write(4);
            w.Write(true);
            w.Write(1); // Native Medpi selection.
            w.Write(6);
            w.Write(-1); // Pillar.
            w.Write(0); // Ground units.
            w.Write(0); // Air units.
            w.Write(0); // Ground mobil groups.
            w.Write(0); // Air mobil groups.
            if (chunkId == 0x0304E000u) w.Write(-1);
            if (chunkId is 0x0304E004u or 0x0304E005u or 0x0304E008u)
            {
                w.Write((byte)7);
                w.Write(8);
                w.Write((short)9);
            }
            if (chunkId is 0x0304E005u or 0x0304E008u) w.Write((short)10);
            if (chunkId == 0x0304E008u) w.Write("Legacy text");
        }));

        await Assert.That(node.SizeX).IsEqualTo(2);
        await Assert.That(node.SizeY).IsEqualTo(3);
        await Assert.That(node.SizeZ).IsEqualTo(4);
        await Assert.That(node.Selection).IsEqualTo(CGameCtnBlockInfo.ESelection.Medpi);
    }

    [Test]
    [Arguments(GameVersion.TM10, 1)]
    [Arguments(GameVersion.TMPU, 1)]
    [Arguments(GameVersion.TMSX, 0)]
    public async Task LegacyBlockDimensions_UseOriginalGameDefaults(GameVersion gameVersion, int size)
    {
        var node = new LegacyBlockInfo(gameVersion);
        await Assert.That(node.SizeX).IsEqualTo(size);
        await Assert.That(node.SizeY).IsEqualTo(size);
        await Assert.That(node.SizeZ).IsEqualTo(size);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Sounds_NullReferencesUseVersionSpecificLocations(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(0x0304E02Au);
            w.Write(version);
            w.Write(-1);
            if (version >= 1)
            {
                w.Write(-1);
                if (version <= 2)
                {
                    w.Write(Iso4.Identity);
                    w.Write(Iso4.Identity);
                }
            }
        });

        var node = await RoundTrip(payload);
        await Assert.That(node.Sound1Loc).IsEqualTo(Iso4.Identity);
        await Assert.That(node.Sound2Loc).IsEqualTo(Iso4.Identity);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PrecalculatedParticles_PreserveReferenceAndLocation(bool external)
    {
        var file = external ? new GbxRefTableFile(new(), 0, true, "Particles.Gbx") : null;
        var location = Iso4.Identity with { TX = 7, TY = 8, TZ = 9 };
        var payload = Payload(w =>
        {
            w.Write(0x0304E015u);
            w.Write(external ? 1 : -1);
            w.Write(location);
            w.Write(0x0304E017u);
            w.Write(true);
        });

        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        if (file is not null) reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnBlockInfoClassic();
        await Assert.That(node.PrecalcPartLoc).IsEqualTo(Iso4.Identity);
        node.Read(rw);

        await Assert.That(node.PrecalcPartParams).IsNull();
        await Assert.That(node.PrecalcPartParamsFile).IsEqualTo(file);
        await Assert.That(node.PrecalcPartLoc).IsEqualTo(location);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(5, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    [Arguments(8, false)]
    [Arguments(8, true)]
    public async Task SpecialProperties_PreserveVersionBranchesAndPlacementTag(int version, bool hasTag)
    {
        var payload = Payload(w =>
        {
            w.Write(0x0304E020u);
            w.Write(version);
            w.Write(-1);
            if (version < 7) w.Write(-1);
            if (version >= 2) w.Write(-1);
            if (version >= 3) w.Write(-1);
            if (version >= 4) w.Write(true);
            if (version == 5) w.Write(true);
            if (version >= 8)
            {
                w.Write(hasTag);
                if (hasTag)
                {
                    w.Write("MatModifier");
                    w.Write("Grass");
                }
            }
        });

        var node = await RoundTrip(payload);
        await Assert.That(node.SpawnUnderground).IsEqualTo(version == 5);
        await Assert.That(node.CharPhySpecialPropertyCustomizable).IsEqualTo(version >= 4);
        await Assert.That(node.MatModifierPlacementTag?.Group).IsEqualTo(hasTag ? "MatModifier" : null);
        await Assert.That(node.MatModifierPlacementTag?.Tag).IsEqualTo(hasTag ? "Grass" : null);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task PillarShape_UsesByteBooleans(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(0x0304E02Fu);
            w.Write(version);
            w.Write((byte)1);
            w.Write((byte)CGameCtnBlockInfo.EMultiDir.OpposedDirOnly);
            if (version >= 1) w.Write((byte)1);
        });

        var node = await RoundTrip(payload);
        await Assert.That(node.IsPillar).IsTrue();
        await Assert.That(node.PillarShapeMultiDir).IsEqualTo(CGameCtnBlockInfo.EMultiDir.OpposedDirOnly);
        await Assert.That(node.IsMultiHeightPillarOrVFC).IsEqualTo(version >= 1);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task BlockModelAndMaterialModifiers_PreserveExternalReferencesAndVersionBoundary(int version, bool external)
    {
        var modelFile = new GbxRefTableFile(new(), 0, true, "Model.Block.Gbx");
        var modifierFile = new GbxRefTableFile(new(), 1, true, "Modifier.Gbx");
        var modifier2File = new GbxRefTableFile(new(), 2, true, "Modifier2.Gbx");
        var payload = Payload(w =>
        {
            w.Write(0x0304E031u);
            w.Write(version);
            w.Write(external ? 1 : -1);
            w.Write(external ? 2 : -1);
            if (version >= 1) w.Write(external ? 3 : -1);
            w.Write(0x0304E017u);
            w.Write(true);
        });

        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode>
        {
            [1] = modelFile,
            [2] = modifierFile,
            [3] = modifier2File
        });
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnBlockInfoClassic();
        node.Read(rw);

        await Assert.That(node.BlockInfoModelFid).IsNull();
        await Assert.That(node.BlockInfoModelFidFile).IsEqualTo(external ? modelFile : null);
        await Assert.That(node.MaterialModifierFile).IsEqualTo(external ? modifierFile : null);
        await Assert.That(node.MaterialModifier2File).IsEqualTo(external && version >= 1 ? modifier2File : null);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 1)]
    [Arguments(0, 2)]
    [Arguments(0, -1)]
    [Arguments(1, 0)]
    public async Task BaseType_OnlyLegacyValueOneOverridesConductorAndPreservesExtraInteger(int version, int legacyValue)
    {
        byte[] CreatePayload(CGameCtnBlockInfo.EBaseType baseType) => Payload(w =>
        {
            w.Write(0x0304E02Bu);
            w.Write(version);
            w.Write((int)baseType);
            if (version == 0) w.Write(legacyValue);
            w.Write(0x0304E017u);
            w.Write(true);
        });

        using var stream = new MemoryStream(CreatePayload(CGameCtnBlockInfo.EBaseType.Conductor));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnBlockInfoClassic();
        node.Read(rw);
        var expectedBaseType = version == 0 && legacyValue == 1
            ? CGameCtnBlockInfo.EBaseType.Generator
            : CGameCtnBlockInfo.EBaseType.Conductor;
        await Assert.That(node.BaseType).IsEqualTo(expectedBaseType);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(CreatePayload(expectedBaseType), CollectionOrdering.Matching);
    }

    [Test]
    public async Task LegacySymmetry_PreservesDirectionAndFlagBoundaries()
    {
        var node = await RoundTrip(Payload(w =>
        {
            w.Write(0x0304E021u);
            w.Write(-1);
            w.Write((int)Direction.East);
            w.Write(0);
            w.Write(true);
            w.Write(false);
            w.Write(true);
            w.Write(false);
            w.Write(0);
            w.Write(false);
            w.Write(true);
            w.Write(false);
            w.Write(true);
        }));
        await Assert.That(node.SymmetryGround!.HasManualSymmetryH).IsTrue();
        await Assert.That(node.SymmetryAir!.HasManualSymmetryV).IsTrue();
    }

    [Test]
    public async Task LegacySpawn_AppliesToGroundAndAir()
    {
        var location = Iso4.Identity with { TX = 7, TY = 8, TZ = 9 };
        var node = await RoundTrip(Payload(w =>
        {
            w.Write(0x0304E007u);
            w.Write(location);
        }));

        await Assert.That(node.SpawnLocGround).IsEqualTo(location);
        await Assert.That(node.SpawnLocAir).IsEqualTo(location);
    }

    [Test]
    public async Task DefaultsAndWriterMetadata_MatchVerifiedBinaries()
    {
        var node = new CGameCtnBlockInfoClassic();
        await Assert.That(node.WayPointType).IsEqualTo(CGameCtnBlockInfo.EWayPointType.None);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E020(GameVersion.MP4).Version).IsEqualTo(7);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E020(GameVersion.TM2020).Version).IsEqualTo(8);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E02F(GameVersion.MP4).Version).IsEqualTo(0);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E02F(GameVersion.TM2020).Version).IsEqualTo(1);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E015().GameVersion).IsEqualTo(GameVersion.MP3 | GameVersion.TMT);
        await Assert.That(new CGameCtnBlockInfo.Chunk0304E022().GameVersion).IsEqualTo(GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020);
    }

    private static byte[] Payload(Action<GbxWriter> write, bool terminator = true)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        if (terminator) writer.Write(0xFACADE01u);
        return stream.ToArray();
    }

    private sealed class LegacyBlockInfo(GameVersion gameVersion) : CGameCtnBlockInfo(gameVersion);

    private static async Task<CGameCtnBlockInfoClassic> RoundTrip(byte[] payload)
    {
        const uint followingWord = 0xDEADBEEF;
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(followingWord);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        var node = new CGameCtnBlockInfoClassic();
        node.Read(rw);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(followingWord);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
        return node;
    }
}
