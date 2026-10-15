using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugBitmapSamplerLayoutTests
{
    [Test]
    [Arguments(1, 0x80000605u, false)]
    [Arguments(2, 0x80000605u, false)]
    [Arguments(3, 0x80000605u, false)]
    [Arguments(4, 0x80000605u, false)]
    [Arguments(5, 0x80000605u, false)]
    [Arguments(6, 0x80000605u, false)]
    [Arguments(7, 0x80000605u, false)]
    [Arguments(8, 0x605u, false)]
    [Arguments(8, 0x18u, true)]
    [Arguments(8, 0x60u, true)]
    [Arguments(8, 0x1800u, true)]
    [Arguments(8, 0x30u, false)]
    [Arguments(8, 0x180u, false)]
    [Arguments(8, 0xC00u, false)]
    [Arguments(9, 0x18u, true)]
    [Arguments(9, 0x605u, false)]
    [Arguments(10, 0x60u, true)]
    [Arguments(10, 0x605u, false)]
    [Arguments(11, 0x1C018005u, false)]
    [Arguments(11, 0x30u, true)]
    [Arguments(11, 0x180u, true)]
    [Arguments(11, 0xC00u, true)]
    [Arguments(11, 0x18u, false)]
    [Arguments(11, 0x60u, false)]
    [Arguments(11, 0x1800u, false)]
    public async Task ReadWrite_SamplerStates_PreserveFlagsAndUseTheCorrectBorderCondition(int offset, uint flags, bool hasBorder)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSampler(writer, offset, flags, hasBorder);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugBitmapSampler();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Name).IsEqualTo(offset >= 2 ? "Sampler" : null);
        await Assert.That(node.Bitmap).IsNull();
        await Assert.That(node.References.Bitmap.File).IsNull();
        await Assert.That(offset == 11 ? node.Flags : node.LegacyFlags).IsEqualTo(flags);
        await Assert.That(node.MipMapLodBias).IsEqualTo(-1.25f);
        await Assert.That(node.BorderColor).IsEqualTo(hasBorder ? 0x80402010u : 0u);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_SeparateLegacyFields_PreserveFilteringAndAddressModes()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(-1); // No bitmap reference.
            writer.Write(1); // Bilinear filtering.
            writer.Write(2); // Clamp U.
            writer.Write(3); // Border V.
            writer.Write(2.5f);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugBitmapSampler();
        var chunk = new CPlugBitmapSampler.Chunk0907E000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.WantedFiltering).IsEqualTo(CPlugBitmapSampler.ETexFilter.Bilinear);
        await Assert.That(node.TexAddressU).IsEqualTo(CPlugBitmapSampler.ETexAddress.Clamp);
        await Assert.That(node.TexAddressV).IsEqualTo(CPlugBitmapSampler.ETexAddress.Border);
        await Assert.That(node.MipMapLodBias).IsEqualTo(2.5f);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(2, 5u, false)]
    [Arguments(5, 0x605u, false)]
    [Arguments(6, 0x605u, false)]
    [Arguments(7, 0x605u, false)]
    [Arguments(8, 0x18u, true)]
    [Arguments(9, 0x60u, true)]
    [Arguments(10, 0x1800u, true)]
    [Arguments(11, 0xC00u, true)]
    public async Task ReadNode_NodeBodies_RecognizeLegacyAndModernChunks(int offset, uint flags, bool hasBorder)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugBitmapSampler.Id + (uint)offset);
            WriteSampler(writer, offset, flags, hasBorder);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugBitmapSampler>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstruction()
    {
        var node = new CPlugBitmapSampler();
        await Assert.That(node.Flags).IsEqualTo(0x1C018005u);
        await Assert.That(node.LegacyFlags).IsEqualTo(0x605u);
        await Assert.That(new CPlugBitmapSampler(GameVersion.TM10).LegacyFlags).IsEqualTo(5u);
        await Assert.That(new CPlugBitmapSampler(GameVersion.TMPU).LegacyFlags).IsEqualTo(5u);
        await Assert.That(node.WantedFiltering).IsEqualTo(CPlugBitmapSampler.ETexFilter.Trilinear);
        await Assert.That(node.TexAddressU).IsEqualTo(CPlugBitmapSampler.ETexAddress.Wrap);
        await Assert.That(node.TexAddressV).IsEqualTo(CPlugBitmapSampler.ETexAddress.Wrap);
        await Assert.That(node.MipMapLodBias).IsEqualTo(0f);
        await Assert.That(node.BorderColor).IsEqualTo(0u);
    }

    private static Chunk<CPlugBitmapSampler> CreateChunk(int offset) => offset switch
    {
        1 => new CPlugBitmapSampler.Chunk0907E001(),
        2 => new CPlugBitmapSampler.Chunk0907E002(),
        3 => new CPlugBitmapSampler.Chunk0907E003(),
        4 => new CPlugBitmapSampler.Chunk0907E004(),
        5 => new CPlugBitmapSampler.Chunk0907E005(),
        6 => new CPlugBitmapSampler.Chunk0907E006(),
        7 => new CPlugBitmapSampler.Chunk0907E007(),
        8 => new CPlugBitmapSampler.Chunk0907E008(),
        9 => new CPlugBitmapSampler.Chunk0907E009(),
        10 => new CPlugBitmapSampler.Chunk0907E00A(),
        11 => new CPlugBitmapSampler.Chunk0907E00B(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static void WriteSampler(GbxWriter writer, int offset, uint flags, bool hasBorder)
    {
        if (offset == 11) writer.Write(0);
        if (offset >= 2) writer.WriteIdAsString("Sampler");
        writer.Write(-1); // No bitmap reference.
        writer.Write(flags);
        writer.Write(-1.25f);
        if (hasBorder) writer.Write(0x80402010u);
    }
}
