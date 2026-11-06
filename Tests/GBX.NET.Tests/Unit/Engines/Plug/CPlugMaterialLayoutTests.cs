using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugMaterialLayoutTests
{
    [Test]
    [Arguments(2, 0)]
    [Arguments(2, 3)]
    [Arguments(10, 0)]
    [Arguments(10, 3)]
    public async Task LegacySurface_PreservesMaterialTypeAndPackedFlags(int chunkId, int materialType)
    {
        var payload = Payload(w =>
        {
            w.Write((byte)CPlugSurface.MaterialId.Grass);
            w.Write((byte)materialType);
            w.Write((ushort)0x4567);
        });
        var node = new CPlugMaterial();
        Chunk<CPlugMaterial> chunk = chunkId == 2
            ? new CPlugMaterial.Chunk09079002()
            : new CPlugMaterial.Chunk0907900A();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SurfaceId).IsEqualTo(CPlugSurface.MaterialId.Grass);
        await Assert.That(node.LegacySurfaceMaterialType).IsEqualTo((byte)materialType);
        await Assert.That(node.SurfaceFlags).IsEqualTo((ushort)0x4567);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task GrassFencesStepSize_OnlyReadsAnOverrideForCountOne(int count)
    {
        var payload = Payload(w =>
        {
            w.Write(0); // Chunk version.
            w.Write(count);
            if (count == 1) w.Write(0.375f);
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079019();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.GrassFencesStepSizeCount).IsEqualTo(count);
        await Assert.That(node.GrassFencesStepSize).IsEqualTo(count == 1 ? 0.375f : 0f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task WaterIds_UsePlainStringsWithoutAnIdTable(int count)
    {
        string[] waterIds = ["Water/Lake", "Water/River"];
        var payload = Payload(w =>
        {
            w.Write(count);
            for (var i = 0; i < count; i++) w.Write(waterIds[i]);
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079013();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.WaterIds).IsEquivalentTo(waterIds.Take(count), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task MaterialVertexIndex_PreservesNullableSourceFileWithoutResolving(bool external)
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["source.Material_VId.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var descriptor = external ? table.CreateFile("source.Material_VId.Gbx") : null;
        var payload = Payload(w => w.Write(external ? 1 : -1));
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079014();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw), descriptor);
        await Assert.That(node.References.MaterialVertexIndex.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(node.References.MaterialVertexIndex.Kind).IsEqualTo(external ? NodeRefKind.ExternalFile : NodeRefKind.None);
        await Assert.That(node.References.MaterialVertexIndex.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(3, true)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    [Arguments(8, false)]
    [Arguments(9, false)]
    [Arguments(12, true)]
    [Arguments(13, false)]
    [Arguments(21, true)]
    public async Task DeviceMat_PreservesDeviceEncodingAndShaderSlots(int version, bool external)
    {
        var shader = external ? null : new CPlugShader();
        var payload = Payload(w =>
        {
            w.Write((short)3);
            if (version <= 12) w.Write((short)7);
            else
            {
                w.Write((byte)7);
                w.Write((byte)4);
            }
            if (version >= 4) w.Write(external);
            w.WriteNodeRef(shader);
            if (version >= 9)
            {
                w.WriteNodeRef(shader);
                w.WriteNodeRef<CPlugShader>(null);
            }
        });
        var entry = new CPlugMaterial.DeviceMat();

        await RoundTrip(payload, rw => entry.ReadWrite(rw, version));
        await Assert.That(entry.RenderDevice).IsEqualTo((short)3);
        await Assert.That(entry.SubDevice).IsEqualTo(7);
        await Assert.That(entry.RenderQuality).IsEqualTo((byte)(version >= 13 ? 4 : 0));
        await Assert.That(entry.IsShaderExternal).IsEqualTo(version >= 4 && external);
        if (!external)
        {
            await Assert.That(entry.Shader).IsNotNull();
            if (version >= 9) await Assert.That(entry.ShaderDay).IsSameReferenceAs(entry.Shader);
        }
        await Assert.That(entry.ShaderNight).IsNull();
    }

    [Test]
    [Arguments(9)]
    [Arguments(13)]
    [Arguments(21)]
    public async Task MaterialModel_ExternalReferenceSkipsDeviceArraysWithoutResolving(int chunkId)
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["model.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var descriptor = table.CreateFile("model.Gbx");
        var payload = Payload(w =>
        {
            if (chunkId == 21) w.Write(7);
            w.Write(1);
            if (chunkId == 21)
            {
                w.Write(0); // Color target tables.
                w.Write(-1); // Water array.
            }
        });
        var node = new CPlugMaterial();
        Chunk<CPlugMaterial> chunk = chunkId switch
        {
            9 => new CPlugMaterial.Chunk09079009(),
            13 => new CPlugMaterial.Chunk0907900D(),
            _ => new CPlugMaterial.Chunk09079015()
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw), descriptor);
        await Assert.That(node.References.MaterialModel.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(node.References.MaterialModel.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(node.DeviceMaterials).IsNull();
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    [Arguments(4)]
    [Arguments(9)]
    [Arguments(13)]
    public async Task LegacyChunks_UseTheMatchingDeviceLayout(int chunkId)
    {
        var payload = Payload(w =>
        {
            if (chunkId >= 8) w.Write(-1); // Material model.
            w.Write(1); // Device materials.
            w.Write((short)3);
            if (chunkId <= 12) w.Write((short)7);
            else
            {
                w.Write((byte)7);
                w.Write((byte)4);
            }
            if (chunkId >= 4) w.Write(false);
            w.WriteNodeRef(new CPlugShader());
            if (chunkId >= 9)
            {
                w.Write(-1);
                w.Write(-1);
            }
            if (chunkId >= 12)
            {
                w.Write(2);
                w.Write(17);
                w.Write(23);
            }
        });
        var node = new CPlugMaterial();
        Chunk<CPlugMaterial> chunk = chunkId switch
        {
            4 => new CPlugMaterial.Chunk09079004(),
            9 => new CPlugMaterial.Chunk09079009(),
            _ => new CPlugMaterial.Chunk0907900D()
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DeviceMaterials!.Single().SubDevice).IsEqualTo(7);
        await Assert.That(node.DeviceMaterials!.Single().Shader).IsNotNull();
    }

    [Test]
    public async Task MaterialModel_VersionSevenPreservesColorTablesAndWaterArray()
    {
        var table = new CPlugMaterialColorTargetTable
        {
            ColorTargets = [new() { Colors = [0x11223344], ColorBlindColors = [0x55667788] }]
        };
        var payload = Payload(w =>
        {
            w.Write(7);
            w.WriteNodeRef(new CPlugMaterial());
            w.Write(2);
            w.WriteNodeRef(table);
            w.WriteNodeRef(table);
            w.WriteNodeRef(new CPlugMaterialWaterArray());
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079015();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ColorTargetTables!.Count).IsEqualTo(2);
        await Assert.That(node.ColorTargetTables[0]).IsSameReferenceAs(node.ColorTargetTables[1]);
        await Assert.That(node.ColorTargetTables[0].ColorTargets![0].ColorBlindColors![0]).IsEqualTo(0x55667788);
        await Assert.That(node.MaterialWaterArray).IsNotNull();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ShadingDesc_PreservesRequirementsAndPreLightData(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version == 0) return;
            w.Write("Materials/Model.Gbx");
            w.Write(0xFEDCBA9876543210ul);
            w.Write(17);
            w.Write(0x123456789ABCDEF0ul);
            w.Write(0x11223344u);
            w.Write(0x55667788u);
            w.Write(2);
            w.Write(1.5f);
            w.Write(3);
            if (version >= 2) w.Write(0xAABBCCDDu);
        });
        var node = new CPlugMaterial();
        var chunk = new CPlugMaterial.Chunk09079012();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        if (version == 0) return;
        await Assert.That(node.Shading!.MaterialModelFileName).IsEqualTo("Materials/Model.Gbx");
        await Assert.That(node.Shading.CustomMaterialCrc64).IsEqualTo(0xFEDCBA9876543210ul);
        await Assert.That(node.Shading.VertexDeclarationFlags).IsEqualTo(0x9ABCDEF0u);
        await Assert.That(node.Shading.U05).IsEqualTo(0x12345678u);
        await Assert.That(node.Shading.PreLightGenScale).IsEqualTo(1.5f);
    }

    [Test]
    public async Task Defaults_MatchNativeMaterialAndUnavailableShading()
    {
        await Assert.That(new CPlugMaterial().PreLightGenScale).IsEqualTo(1f);
        await Assert.That(new CPlugMaterial().ScreenAspectRatio).IsEqualTo(-1f);
        await Assert.That(new CPlugMaterial().ScreenSkinEnabled).IsFalse();
        await Assert.That(new CPlugMaterial().SurfaceFlags).IsEqualTo((ushort)0x800E);
        await Assert.That(new CPlugMaterial(GameVersion.TM10).SurfaceFlags).IsEqualTo((ushort)0x000E);
        await Assert.That(new CPlugMaterial(GameVersion.TMPU).SurfaceFlags).IsEqualTo((ushort)0x000E);
        await Assert.That(new CPlugMaterial(GameVersion.TMSX).SurfaceFlags).IsEqualTo((ushort)0x000E);
        await Assert.That(new CPlugMaterial(GameVersion.TMNESWC).SurfaceFlags).IsEqualTo((ushort)0x000E);
        await Assert.That(new CPlugMaterial().Flags).IsEqualTo(0u);
        await Assert.That(new CPlugMaterial(GameVersion.TM2020).Flags).IsEqualTo(0x240u);
        var shading = new CPlugMaterial.ShadingDesc();
        await Assert.That(shading.VertexDeclarationFlags).IsEqualTo(uint.MaxValue);
        await Assert.That(shading.Flags).IsEqualTo(0xFFFFFFFDu);
        await Assert.That(shading.PreLightGenTexCoordIndex).IsEqualTo(0x3FF);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize, GbxRefTableNode? descriptor = null)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            if (descriptor is not null) reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = descriptor });
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
