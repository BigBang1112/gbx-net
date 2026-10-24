using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using static GBX.NET.Engines.Plug.CPlugBitmapAddress;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugBitmapAddressTests
{
    private const int Sentinel = 0x12345678;
    private static readonly Iso3 Transform = new(1, 2, 3, 4, 5, 6);
    private static readonly Mat4 Matrix = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Test]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(3, 0)]
    [Arguments(3, 1)]
    [Arguments(3, 2)]
    [Arguments(3, 3)]
    [Arguments(4, 0)]
    [Arguments(4, 1)]
    [Arguments(4, 2)]
    [Arguments(4, 3)]
    [Arguments(5, 0)]
    [Arguments(5, 1)]
    [Arguments(5, 2)]
    [Arguments(5, 3)]
    [Arguments(6, 0)]
    [Arguments(6, 1)]
    [Arguments(6, 2)]
    [Arguments(6, 3)]
    [Arguments(7, 0)]
    [Arguments(7, 1)]
    [Arguments(7, 2)]
    [Arguments(7, 3)]
    public async Task ReadWrite_Transforms_PreservePresenceAndPayload(int offset, int presence)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (offset >= 6) writer.Write(0x80211100u | (2u << 15));
            writer.Write(2); // TexCoordIndex.
            if (offset == 1) writer.Write(presence != 0);
            else writer.Write((byte)presence);
            if (offset < 6)
            {
                if (offset is 1 or 3) writer.Write(1); // Bilinear filtering.
                writer.Write(0); // NoGenerate.
                writer.Write(true); // AreUVProjected.
                if (offset is 1 or 3)
                {
                    writer.Write(2); // Clamp U.
                    writer.Write(3); // Border V.
                }
                writer.Write(true);
                writer.Write(false);
                if (offset is 1 or 3) writer.Write(-1.25f);
            }
            if (offset >= 5)
            {
                // Write scalar values independently of the transform writer being tested.
                if ((presence & 1) != 0)
                {
                    for (var i = 1; i <= 6; i++) writer.Write((float)i);
                }
                if ((presence & 2) != 0)
                {
                    for (var i = 1; i <= 16; i++) writer.Write((float)i);
                }
            }
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmapAddress { TransfoIso3 = Transform, UVTransfoMat4 = Matrix };
        var chunk = CreateChunk(offset);
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.TexCoordIndex).IsEqualTo(2);
        await Assert.That(node.AreUVProjected).IsTrue();
        await Assert.That(node.TransfoIso3).IsEqualTo((presence & 1) == 0 ? (Iso3?)null : offset >= 5 ? Transform : Iso3.Identity);
        await Assert.That(node.UVTransfoMat4).IsEqualTo((presence & 2) == 0 ? (Mat4?)null : offset >= 5 ? Matrix : Mat4.Identity);
        if (offset >= 6)
        {
            await Assert.That(node.AddressFlags).IsEqualTo(0x80211100u | (2u << 15));
            await Assert.That(node.UseBitmapTcScale).IsTrue();
            await Assert.That(node.DirectTransfo).IsTrue();
        }
        if (offset is 1 or 3)
        {
            await Assert.That(node.WantedFiltering).IsEqualTo(CPlugBitmapSampler.ETexFilter.Bilinear);
            await Assert.That(node.TexAddressU).IsEqualTo(CPlugBitmapSampler.ETexAddress.Clamp);
            await Assert.That(node.TexAddressV).IsEqualTo(CPlugBitmapSampler.ETexAddress.Border);
            await Assert.That(node.MipMapLodBias).IsEqualTo(-1.25f);
        }
    }

    [Test]
    [Arguments(2)]
    [Arguments(8)]
    public async Task ReadWrite_MapEffects_PreserveNonemptyTwentyByteRecords(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(2);
            for (var i = 0; i < 10; i++) writer.Write(unchecked((int)0x81234567) + i);
            if (offset == 8) writer.Write(0.75f);
            writer.Write(Sentinel);
        }
        var node = new CPlugBitmapAddress();
        var chunk = CreateChunk(offset);
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.MapFxDescs!.Length).IsEqualTo(2);
        await Assert.That(node.MapFxDescs[0].U05).IsEqualTo(unchecked((int)0x81234567) + 4);
        await Assert.That(node.MapFxDescs[1].U01).IsEqualTo(unchecked((int)0x81234567) + 5);
        await Assert.That(node.BumpEnvScale).IsEqualTo(offset == 8 ? 0.75f : 1f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(9)]
    public async Task ReadWrite_BitmapAndBumpScale_PreservePayload(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (offset == 0) writer.Write(-1);
            else writer.Write(0.25f);
            writer.Write(Sentinel);
        }
        var node = new CPlugBitmapAddress();
        var chunk = CreateChunk(offset);
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Bitmap).IsNull();
        await Assert.That(node.BumpEnvScale).IsEqualTo(offset == 9 ? 0.25f : 1f);
    }

    [Test]
    [Arguments(0, 4)]
    [Arguments(1, -1)]
    [Arguments(13, 4)]
    [Arguments(20, 4)]
    [Arguments(24, -1)]
    [Arguments(25, -1)]
    [Arguments(26, -1)]
    public async Task Read_GenerateUV_UsesSerializedIndexOnlyWhenTexCoordsAreRequired(int generateUV, int expectedIndex)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x800F8000u | (uint)generateUV);
            writer.Write(4);
            writer.Write((byte)0);
            writer.Write(Sentinel);
        }
        payload.Position = 0;
        var node = new CPlugBitmapAddress();
        using var reader = new GbxReader(payload);
        using (var rw = new GbxReaderWriter(reader)) new Chunk09047007().ReadWrite(node, rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(node.GenerateUV).IsEqualTo((CPlugBitmap.EGxUVGenerate)generateUV);
        await Assert.That(node.TexCoordIndex).IsEqualTo(expectedIndex);
        await Assert.That(node.AddressFlags & 0x80000000).IsEqualTo(0x80000000u);
    }

    [Test]
    public async Task Properties_PackAddressStateWithoutChangingSamplerFlags()
    {
        var node = new CPlugBitmapAddress();
        await Assert.That(node.AddressFlags).IsEqualTo(0xF8000u);
        await Assert.That(node.TexCoordIndex).IsEqualTo(-1);
        await Assert.That(node.GenerateUV).IsEqualTo(CPlugBitmap.EGxUVGenerate.NoGenerate);
        await Assert.That(node.BumpEnvScale).IsEqualTo(1f);
        node.TexCoordIndex = 7;
        node.GenerateUV = CPlugBitmap.EGxUVGenerate.WorldPos_PyPxz;
        node.AreUVProjected = true;
        node.UseBitmapTcScale = true;
        node.DirectTransfo = true;
        await Assert.That(node.AddressFlags).IsEqualTo(0x23911Au);
        await Assert.That(node.Flags).IsEqualTo(0x1C018005u);
        node.AreUVProjected = false;
        node.UseBitmapTcScale = false;
        node.DirectTransfo = false;
        node.TexCoordIndex = -1;
        await Assert.That(node.AddressFlags).IsEqualTo(0xF801Au);
    }

    [Test]
    public async Task SaveParse_UsesTheInheritedBitmapReferenceAndBothTransforms()
    {
        var node = new CPlugBitmapAddress { TransfoIso3 = Transform, UVTransfoMat4 = Matrix, TexCoordIndex = 2 };
        node.CreateChunk<Chunk09047000>();
        node.CreateChunk<Chunk09047007>();
        node.References.Bitmap.Descriptor = new GbxRefTableFile(new GbxRefTable(), 32, true, "textures/bitmap.Gbx");
        using var stream = new MemoryStream();
        var gbx = new Gbx<CPlugBitmapAddress>(node) { BodyCompression = GbxCompression.Uncompressed };
        gbx.Save(stream);

        stream.Position = 0;
        var parsed = Gbx.Parse<CPlugBitmapAddress>(stream);
        await Assert.That(parsed.Node.References.Bitmap.File!.FilePath.Replace('\\', '/')).IsEqualTo("textures/bitmap.Gbx");
        await Assert.That(parsed.Node.References.Bitmap.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(parsed.Node.TransfoIso3).IsEqualTo((Iso3?)Transform);
        await Assert.That(parsed.Node.UVTransfoMat4).IsEqualTo((Mat4?)Matrix);
        await Assert.That(parsed.Node.TexCoordIndex).IsEqualTo(2);
        using var saved = new MemoryStream();
        parsed.Save(saved);
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task LegacyIso3Property_AdaptsTheSameSixValues()
    {
#pragma warning disable CS0618
        var node = new CPlugBitmapAddress { UVTransfoIso3 = new BoxAligned(1, 2, 3, 4, 5, 6) };
        await Assert.That(node.TransfoIso3).IsEqualTo((Iso3?)Transform);
        node.TransfoIso3 = Iso3.Identity;
        await Assert.That(node.UVTransfoIso3).IsEqualTo((BoxAligned?)new BoxAligned(1, 0, 0, 1, 0, 0));
        node.UVTransfoIso3 = null;
        await Assert.That(node.TransfoIso3).IsNull();
#pragma warning restore CS0618
    }

    private static Chunk<CPlugBitmapAddress> CreateChunk(int offset) => offset switch
    {
        0 => new Chunk09047000(),
        1 => new Chunk09047001(),
        2 => new Chunk09047002(),
        3 => new Chunk09047003(),
        4 => new Chunk09047004(),
        5 => new Chunk09047005(),
        6 => new Chunk09047006(),
        7 => new Chunk09047007(),
        8 => new Chunk09047008(),
        9 => new Chunk09047009(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
