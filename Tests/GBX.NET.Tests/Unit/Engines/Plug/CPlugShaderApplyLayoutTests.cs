using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugShaderApplyLayoutTests
{
    [Test]
    [Arguments(0x003)]
    [Arguments(0x005)]
    [Arguments(0x006)]
    [Arguments(0x007)]
    [Arguments(0x008)]
    [Arguments(0x009)]
    [Arguments(0x00A)]
    [Arguments(0x012)]
    public async Task ReadWrite_ApplyFields_PreserveNativeWidthsAndPackedBits(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new BinaryWriter(payload, Encoding.UTF8, leaveOpen: true))
        {
            if (offset == 0x012) writer.Write(0);
            writer.Write(0xFEDCBA98u);
            if (offset >= 0x008) writer.Write(0x89ABCDEFu);
            if (offset >= 0x009) writer.Write(0x87654321u);
            writer.Write(0x11223344);
        }
        var node = new CPlugShaderApply();
        var chunk = CreateChunk(offset);
        await ReadAndRoundTrip(node, chunk, payload);
        await Assert.That(offset <= 0x006 ? node.LegacyApplyField : node.ApplyField).IsEqualTo(0xFEDCBA98u);
        if (offset >= 0x008) await Assert.That(node.ApplyField2).IsEqualTo(0x89ABCDEFu);
        if (offset >= 0x009) await Assert.That(node.ApplyField3).IsEqualTo(0x87654321u);
    }

    [Test]
    [Arguments(0x000)]
    [Arguments(0x001)]
    public async Task ReadWrite_LegacyBlendFactors_PrecedeTheCountedTexturesAndAlphaFlags(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new BinaryWriter(payload, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(4);
            writer.Write(5);
            writer.Write(0); // Texture count.
            if (offset == 0x001) writer.Write(0xA5A501FFu);
            writer.Write(0x11223344);
        }
        var node = new CPlugShaderApply();
        await ReadAndRoundTrip(node, CreateChunk(offset), payload);
        await Assert.That(node.BlendSrc).IsEqualTo(4);
        await Assert.That(node.BlendDst).IsEqualTo(5);
        if (offset == 0x001) await Assert.That(node.LegacyAlphaFlags).IsEqualTo(0xA5A501FFu);
    }

    [Test]
    [Arguments(0x09063000)]
    [Arguments(0x09063001)]
    [Arguments(0x09063002)]
    public async Task ReadWrite_LegacyMaterials_KeepFiveTextureSlotsAndTheirFlags(int id)
    {
        using var payload = new MemoryStream();
        using (var writer = new BinaryWriter(payload, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(0xA5A5003Fu);
            writer.Write(3); // Environment-map texture operation.
            for (var i = 0; i < 5; i++) writer.Write(-1);
            writer.Write(0x11223344);
        }
        Chunk<CPlugShaderApply> chunk = id switch
        {
            0x09063000 => new CPlugShaderApply.Chunk09063000(),
            0x09063001 => new CPlugShaderApply.Chunk09063001(),
            0x09063002 => new CPlugShaderApply.Chunk09063002(),
            _ => throw new ArgumentOutOfRangeException(nameof(id))
        };
        var node = new CPlugShaderApply();
        await ReadAndRoundTrip(node, chunk, payload);
        await Assert.That(id == 0x09063000 ? node.LegacyMaterialFlags0 : node.LegacyMaterialFlags).IsEqualTo(0xA5A5003Fu);
        await Assert.That(node.EnvMapTexOp).IsEqualTo(3);
    }

    [Test]
    public async Task ReadWrite_UnusedBitmapAddresses_DoNotReplaceActiveTextures()
    {
        var active = new CPlugBitmapAddress();
        var node = new CPlugShaderApply { BitmapAddresses = [active] };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteDeprecVersion();
            writer.Write(0); // Unused texture count.
            writer.Write(0x11223344);
        }
        await ReadAndRoundTrip(node, new CPlugShaderApply.Chunk0902600C(), payload);
        await Assert.That(node.BitmapAddresses[0]).IsSameReferenceAs(active);
        await Assert.That(node.UnusedBitmapAddresses).IsEmpty();
    }

    [Test]
    public async Task ReadWrite_BitmapApplyArrayReference_PreservesTypedNodeAndDeprecatedArray()
    {
        var bitmapArray = new CPlugBitmapApplyArray { BitmapAddresses = [new CPlugBitmapAddress()] };
        bitmapArray.CreateChunk<CPlugBitmapApplyArray.Chunk090B8000>();
        var node = new CPlugShaderApply { BitmapApplyArray = bitmapArray };
        node.CreateChunk<CPlugShaderApply.Chunk09026011>().Version = 0;
        var gbx = new Gbx<CPlugShaderApply>(node) { BodyCompression = GbxCompression.Uncompressed };
        using var stream = new MemoryStream();
        gbx.Save(stream);
        stream.Position = 0;
        var parsed = Gbx.Parse<CPlugShaderApply>(stream);
        await Assert.That(parsed.Node.BitmapApplyArray).IsNotNull();
        await Assert.That(parsed.Node.BitmapApplyArray!.BitmapAddresses).Count().IsEqualTo(1);
        await Assert.That(parsed.Node.BitmapApplyArray.BitmapAddresses![0]).IsTypeOf<CPlugBitmapAddress>();
        using var saved = new MemoryStream();
        parsed.Save(saved);
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_DefaultSettings_MatchVerifiedPackedState()
    {
        var legacy = new CPlugShaderApply();
        var current = new CPlugShaderApply(GameVersion.TM2020);
        await Assert.That(legacy.ApplyField).IsEqualTo(0x06000001u);
        await Assert.That(legacy.ApplyField2).IsEqualTo(0x007F007Fu);
        await Assert.That(legacy.ApplyField3).IsEqualTo(2u);
        await Assert.That(legacy.TexOpConstant).IsEqualTo(0x01010101u);
        await Assert.That(current.ApplyField).IsEqualTo(0x06200001u);
        await Assert.That(current.ApplyField2).IsEqualTo(0x007F807Fu);
    }

    private static async Task ReadAndRoundTrip(CPlugShaderApply node, Chunk<CPlugShaderApply> chunk, MemoryStream payload)
    {
        var expected = payload.ToArray()[..^4];
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
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    private static Chunk<CPlugShaderApply> CreateChunk(int offset) => offset switch
    {
        0x000 => new CPlugShaderApply.Chunk09026000(),
        0x001 => new CPlugShaderApply.Chunk09026001(),
        0x003 => new CPlugShaderApply.Chunk09026003(),
        0x005 => new CPlugShaderApply.Chunk09026005(),
        0x006 => new CPlugShaderApply.Chunk09026006(),
        0x007 => new CPlugShaderApply.Chunk09026007(),
        0x008 => new CPlugShaderApply.Chunk09026008(),
        0x009 => new CPlugShaderApply.Chunk09026009(),
        0x00A => new CPlugShaderApply.Chunk0902600A(),
        0x012 => new CPlugShaderApply.Chunk09026012(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };
}
