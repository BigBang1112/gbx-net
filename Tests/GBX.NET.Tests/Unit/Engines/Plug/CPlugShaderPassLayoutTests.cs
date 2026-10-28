using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugShaderPassLayoutTests
{
    private static readonly Vec4 Constants = new(1, 2, 3, 4);

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 1)]
    [Arguments(0, 2)]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(1, 2)]
    public async Task ReadWrite_Pipeline_PreservesAbsentEmbeddedAndExternalGpuPayloads(int version, int referenceKind)
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "Shader.Gbx");
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WritePipeline(writer, referenceKind);
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var pipeline = new CPlugShaderPass.PipelineGpu();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            if (referenceKind == 2) reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
            pipeline.ReadWrite(rw, version);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(pipeline.HasExternalFile).IsEqualTo(referenceKind == 2);
        if (referenceKind == 2) await Assert.That(pipeline.References.FileGpu.File).IsSameReferenceAs(file);
        else await Assert.That(pipeline.FileGpu is null).IsEqualTo(referenceKind == 0);
        await Assert.That(pipeline.GpuLoadFxs.Length).IsEqualTo(referenceKind == 0 ? 0 : 1);
        if (referenceKind != 0)
        {
            await Assert.That(pipeline.GpuLoadFxs[0].Name).IsEqualTo("Color");
            await Assert.That(pipeline.GpuLoadFxs[0].RegistersPerElement).IsEqualTo(4);
            await Assert.That(pipeline.GpuLoadFxs[0].ElementCount).IsEqualTo(2);
            await Assert.That(pipeline.GpuLoadFxs[0].ValueIndex).IsEqualTo(7);
            await Assert.That(pipeline.FxValues[0]).IsEqualTo(Constants);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) pipeline.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_Chunk00B_PreservesDomainHullAndVersionedGeometryStages(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            WritePipeline(writer, 1);
            WritePipeline(writer, 0);
            if (version >= 1) WritePipeline(writer, 1);
            writer.Write(0x11223344);
        }
        var node = new CPlugShaderPass();
        var chunk = new CPlugShaderPass.Chunk0906700B();
        await RoundTrip(payload, node, chunk);
        await Assert.That(node.DomainPipeline!.FxValues[0]).IsEqualTo(Constants);
        await Assert.That(node.HullPipeline!.FileGpu).IsNull();
        await Assert.That(node.GeometryPipeline is null).IsEqualTo(version == 0);
        if (version >= 1) await Assert.That(node.GeometryPipeline!.GpuLoadFxs[0].ValueIndex).IsEqualTo(7);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(8)]
    public async Task ReadWrite_LegacyChunks_PreserveShadersConstantsAndRenderState(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (offset is >= 2 and <= 5)
            {
                writer.Write(5);
                writer.Write(6);
                if (offset >= 3) writer.Write(2);
            }
            writer.WriteNodeRef<CPlugFileGPU>(new CPlugFileVHlsl());
            if (offset is >= 1 and <= 4) writer.WriteNodeRef<CMwNod>(null);
            if (offset is 5 or 8) WriteEffects(writer);
            else if (offset is 0 or 1 or 2 or 3 or 4)
            {
                writer.WriteNodeRef<CPlugFileGPU>(new CPlugFilePsh());
                writer.WriteArray(new[] { Constants });
            }
            if (offset is 5 or 8)
            {
                writer.WriteNodeRef<CPlugFileGPU>(new CPlugFilePsh());
                WriteEffects(writer);
            }
            if (offset == 4) writer.WriteArray(new[] { Constants });
            writer.Write(0x11223344);
        }
        var node = new CPlugShaderPass();
        Chunk<CPlugShaderPass> chunk = offset switch
        {
            0 => new CPlugShaderPass.Chunk09067000(),
            1 => new CPlugShaderPass.Chunk09067001(),
            2 => new CPlugShaderPass.Chunk09067002(),
            3 => new CPlugShaderPass.Chunk09067003(),
            4 => new CPlugShaderPass.Chunk09067004(),
            5 => new CPlugShaderPass.Chunk09067005(),
            8 => new CPlugShaderPass.Chunk09067008(),
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };
        await RoundTrip(payload, node, chunk);
        await Assert.That(node.VertexFxValues[0]).IsEqualTo(Constants);
        if (offset is 4 or 5 or 8) await Assert.That(node.PixelFxValues[0]).IsEqualTo(Constants);
        if (offset is >= 2 and <= 5)
        {
            await Assert.That(node.BlendSrc).IsEqualTo(5);
            await Assert.That(node.BlendDst).IsEqualTo(6);
            await Assert.That(node.Flags & 0xFF).IsEqualTo(0x65u);
        }
    }

    [Test]
    public async Task ReadWrite_TextureBindings_PreserveAllFiveStageArrays()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            for (var stage = 0; stage < 5; stage++)
            {
                writer.Write(1);
                writer.WriteNodeRef(new CPlugBitmap());
                writer.WriteIdAsString($"Texture{stage}");
            }
            writer.Write(0x11223344);
        }
        var node = new CPlugShaderPass();
        await RoundTrip(payload, node, new CPlugShaderPass.Chunk0906700D());
        var stages = new[] { node.VertexSamplers, node.PixelSamplers, node.DomainSamplers, node.HullSamplers, node.GeometrySamplers };
        for (var stage = 0; stage < stages.Length; stage++)
        {
            await Assert.That(stages[stage][0].Name).IsEqualTo($"Texture{stage}");
            await Assert.That(stages[stage][0].Bitmap).IsNotNull();
        }
    }

    private static void WritePipeline(GbxWriter writer, int referenceKind)
    {
        writer.Write(referenceKind == 2);
        if (referenceKind == 2) writer.Write(1);
        else writer.WriteNodeRef<CPlugFileGPU>(referenceKind == 1 ? new CPlugFileVHlsl() : null);
        if (referenceKind != 0) WriteEffects(writer);
    }

    private static void WriteEffects(GbxWriter writer)
    {
        writer.Write(1);
        writer.WriteIdAsString("Color");
        writer.Write(3);
        writer.Write(4);
        writer.Write(2);
        writer.Write(7);
        writer.WriteArray(new[] { Constants });
    }

    private static async Task RoundTrip(MemoryStream payload, CPlugShaderPass node, Chunk<CPlugShaderPass> chunk)
    {
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
