using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugMaterialCustomLayoutTests
{
    [Test]
    [Arguments(0, 3, 2)]
    [Arguments(0, 4, 0)]
    [Arguments(0, 0, 3)]
    [Arguments(1, 4, 1)]
    [Arguments(2, 4, 1)]
    [Arguments(3, 2, 1)]
    public async Task ReadWrite_GpuFx_PreservesLegacyFormatsAndEmptyDimensions(int version, int components, int registers)
    {
        var payload = Payload(w =>
        {
            w.WriteIdAsString("Tint");
            if (version == 1) w.Write(17);
            if (version is 0 or 3) w.Write(components);
            if (version == 0) w.Write(registers);
            if (version is 0 or 3) w.Write(true);
            for (var i = 0; i < registers; i++)
            {
                for (var j = 0; j < components; j++) w.Write(i * 10 + j + 0.5f);
            }
        });
        var fx = new CPlugMaterialCustom.GpuFx();

        await RoundTrip(payload, rw => fx.ReadWrite(rw, version));
        await Assert.That(fx.Name).IsEqualTo("Tint");
        await Assert.That(fx.ComponentCount).IsEqualTo(components);
        await Assert.That(fx.Values.Length).IsEqualTo(registers);
        foreach (var register in fx.Values) await Assert.That(register.Length).IsEqualTo(components);
        if (version == 1) await Assert.That(fx.U04).IsEqualTo(17);
    }

    [Test]
    [Arguments(0x00A, 0)]
    public async Task ReadWrite_GpuFxArrays_ReadsColorsBeforeReals(int offset, int version)
    {
        var payload = Payload(w =>
        {
            foreach (var name in new[] { "Tint", "UsePBR" })
            {
                w.Write(1);
                w.WriteIdAsString(name);
                if (version == 1) w.Write(0);
                if (version is 0 or 3) w.Write(4);
                if (version == 0) w.Write(1);
                if (version is 0 or 3) w.Write(false);
                for (var i = 0; i < 4; i++) w.Write(name == "Tint" ? 0.5f : 1f);
            }
        });
        var node = new CPlugMaterialCustom();
        var chunk = (Chunk<CPlugMaterialCustom>)Activator.CreateInstance(
            typeof(CPlugMaterialCustom).GetNestedType($"Chunk{0x0903A000 + offset:X8}")!)!;

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.GpuFxColors![0].Name).IsEqualTo("Tint");
        await Assert.That(node.GpuFxReals![0].Name).IsEqualTo("UsePBR");
        await Assert.That(node.GpuFxColors[0].Values[0][0]).IsEqualTo(0.5f);
        await Assert.That(node.GpuFxReals[0].Values[0][0]).IsEqualTo(1f);
    }

    [Test]
    public async Task ReadWrite_FuncShaders_PreservesInlineNodesAndNullReferences()
    {
        var payload = Payload(w =>
        {
            w.Write(2);
            w.WriteNodeRef(new CFuncShaderLayerUV());
            w.WriteNodeRef<CFuncShader>(null);
        });
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A004();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.FuncShaders![0]).IsTypeOf<CFuncShaderLayerUV>();
        await Assert.That(node.FuncShaders[1]).IsNull();
    }

    [Test]
    [Arguments(0x009, 0, false)]
    [Arguments(0x009, 0, true)]
    [Arguments(0x00B, 0, false)]
    [Arguments(0x00B, 0, true)]
    [Arguments(0x00D, 0, false)]
    [Arguments(0x00D, 0, true)]
    [Arguments(0x016, 0, false)]
    [Arguments(0x016, 0, true)]
    [Arguments(0x016, 1, false)]
    [Arguments(0x016, 1, true)]
    [Arguments(0x016, 2, false)]
    [Arguments(0x016, 2, true)]
    public async Task ReadWrite_Flags_PreservesWidthsAndConditionalFilters(int offset, int version, bool filter)
    {
        var flags = (offset < 0x00D ? 0xA5A50000UL : 0x7F80A5A50000UL) | (filter ? 1UL : 0UL);
        const ulong requirements = 0xFEDCBA9876543210;
        var payload = Payload(w =>
        {
            if (offset == 0x016) w.Write(version);
            if (offset < 0x00D) w.Write((uint)flags);
            else w.Write(flags);
            w.Write(requirements);
            if (offset == 0x016 && version >= 1) w.Write(19);
            if (filter)
            {
                w.Write((short)-123);
                if (offset >= 0x00B) w.Write((short)456);
            }
        });
        var node = new CPlugMaterialCustom();
        var chunk = (Chunk<CPlugMaterialCustom>)Activator.CreateInstance(
            typeof(CPlugMaterialCustom).GetNestedType($"Chunk{0x0903A000 + offset:X8}")!)!;

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Flags).IsEqualTo(flags);
        await Assert.That(node.ShaderRequirements).IsEqualTo(requirements);
        if (filter && offset >= 0x00B)
        {
            await Assert.That(node.VisibleFilter!.BitMask).IsEqualTo((short)-123);
            await Assert.That(node.VisibleFilter.BitValue).IsEqualTo((short)456);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_Visibility_PreservesVersionedDefines(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(250f);
            w.Write(0.05f);
            if (version >= 1) w.Write(0.75f);
            if (version >= 2)
            {
                w.Write(1);
                w.WriteIdAsString("Quality");
                w.Write(3);
            }
        });
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A00F();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VisibleMaxDist).IsEqualTo(250f);
        await Assert.That(node.VisibleMinScreenHeight).IsEqualTo(0.05f);
        if (version >= 1) await Assert.That(node.LightThroughRatioBounce).IsEqualTo(0.75f);
        if (version >= 2)
        {
            await Assert.That(node.DefineNats![0].Name).IsEqualTo("Quality");
            await Assert.That(node.DefineNats[0].Value).IsEqualTo(3);
        }
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task ReadWrite_ConstantBuffers_PreservesAbsentByteSize(int version, bool absent)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1);
            w.Write(2);
            w.Write(3);
            if (version >= 1) w.Write(absent);
            if (!absent) w.Write(new byte[] { 10, 20, 30 });
        });
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A014();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ConstantBuffers![0].Stage).IsEqualTo(CPlugMaterialCustom.EGpuStage.Domain);
        await Assert.That(node.ConstantBuffers[0].ByteSize).IsEqualTo(3);
        await Assert.That(node.ConstantBuffers[0].IsDataAbsent).IsEqualTo(absent);
        if (absent) await Assert.That(node.ConstantBuffers[0].Data).IsNull();
        else await Assert.That(node.ConstantBuffers[0].Data!).IsEquivalentTo(new byte[] { 10, 20, 30 }, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(1, 4)]
    [Arguments(2, 0)]
    [Arguments(2, 4)]
    public async Task ReadWrite_SubTexture_PreservesCountAndVersionedNames(int version, int count)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            if (version >= 1) w.Write(count);
            if (count == 0)
            {
                w.Write("Side");
                w.Write("Top");
                if (version >= 2)
                {
                    w.Write("Detail");
                    w.Write("Height");
                }
            }
        });
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A015();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.IndexCountPerVertex).IsEqualTo(count);
        if (count == 0)
        {
            await Assert.That(node.SubTextureNamePxz).IsEqualTo("Side");
            await Assert.That(node.SubTextureNamePy).IsEqualTo("Top");
            if (version >= 2)
            {
                await Assert.That(node.SubTextureNamePyX2).IsEqualTo("Detail");
                await Assert.That(node.SubTextureNamePyH2).IsEqualTo("Height");
            }
        }
    }

    [Test]
    public async Task ReadWrite_Bitmaps_PreservesNodeReferencesAndAddressModes()
    {
        var payload = Payload(w =>
        {
            w.Write(0);
            w.Write(1);
            w.WriteIdAsString("Diffuse");
            w.Write(17);
            w.WriteNodeRef(new CPlugBitmap());
            w.Write(3);
            w.Write(5);
        });
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A013();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Bitmaps![0].LayerName).IsEqualTo("Diffuse");
        await Assert.That(node.Bitmaps[0].BitmapOrImage).IsTypeOf<CPlugBitmap>();
        await Assert.That(node.Bitmaps[0].TexAddressU).IsEqualTo(3);
        await Assert.That(node.Bitmaps[0].TexAddressV).IsEqualTo(5);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(7)]
    [Arguments(31)]
    public async Task ReadWrite_VSplitIndexSemantic_PreservesShaderDefaultSentinelAndOverrides(int semantic)
    {
        var node = new CPlugMaterialCustom();
        var chunk = new CPlugMaterialCustom.Chunk0903A011();

        await RoundTrip(Payload(w => w.Write(semantic)), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VSplitIndexSemantic).IsEqualTo(semantic);
    }

    [Test]
    public async Task Defaults_MatchNativeConstructors()
    {
        var node = new CPlugMaterialCustom();
        await Assert.That(node.Flags).IsEqualTo(0x7F0001FC0000UL);
        await Assert.That(new CPlugMaterialCustom(GameVersion.TM2020).Flags).IsEqualTo(0x7F8001FC0000UL);
        await Assert.That(node.VisibleMaxDist).IsEqualTo(-1f);
        await Assert.That(node.VisibleMinScreenHeight).IsEqualTo(-1f);
        await Assert.That(new CPlugMaterialCustom.Bitmap().TexAddressU).IsEqualTo(4);
        await Assert.That(new CPlugMaterialCustom.Bitmap().TexAddressV).IsEqualTo(4);
        await Assert.That(new CPlugMaterialCustom.CBuffer().Stage).IsEqualTo(CPlugMaterialCustom.EGpuStage.Unknown);
        await Assert.That(node.VSplitIndexSemantic).IsEqualTo(-1);
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
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
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
