using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CPlugBitmapLayoutTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 4)]
    [Arguments(false, 5)]
    [Arguments(false, 7)]
    [Arguments(false, 8)]
    [Arguments(true, 0)]
    [Arguments(true, 4)]
    [Arguments(true, 5)]
    [Arguments(true, 7)]
    [Arguments(true, 8)]
    public async Task ImagePayload_SelectsSuffixFromPixelUpdate(bool modern, int pixelUpdate)
    {
        using var payload = new MemoryStream();
        var flags = 0xAABBCCDD00000012UL | (ulong)pixelUpdate << 8;
        using (var writer = new GbxWriter(payload))
        {
            if (modern) writer.Write(5);
            writer.Write(-1); // No image; the pixel-update suffix still follows.
            writer.Write(flags);
            if (modern) writer.Write(0x87654321u);
            writer.Write(0.25f);
            writer.Write(2f);
            writer.Write(-0.5f);
            writer.Write(0x11223344);
            switch (pixelUpdate)
            {
                case 4:
                    writer.Write(-1); // Render node reference.
                    break;
                case 5:
                    writer.Write(0x09080002u); // Direct shader chunk bodies.
                    writer.Write(-1);
                    writer.Write(-1);
                    writer.Write(0xFACADE01u);
                    break;
                case 7:
                    writer.Write(1);
                    writer.Write(0.1f);
                    writer.Write(0.2f);
                    writer.Write(0.3f);
                    writer.Write(12f);
                    break;
                case 8:
                    writer.Write(0xFF123456u);
                    break;
            }
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        Chunk<CPlugBitmap> chunk = modern ? new CPlugBitmap.Chunk09011030() : new CPlugBitmap.Chunk09011018();
        await ReadAndRoundTrip(payload, node, chunk);
        await Assert.That(node.Flags).IsEqualTo(flags);
        await Assert.That(node.Usage).IsEqualTo(CPlugBitmap.EUsage.LightAlpha);
        await Assert.That(node.PixelUpdate).IsEqualTo(pixelUpdate);
        await Assert.That(node.MipMapLodBiasDefault).IsEqualTo(-0.5f);
        if (modern) await Assert.That(node.FlagsHigh).IsEqualTo(0x87654321u);
        if (pixelUpdate == 5) await Assert.That(node.BitmapShader).IsNotNull();
        if (pixelUpdate == 7) await Assert.That(node.SpecularSubMapCats![0].SpecularExp).IsEqualTo(12f);
        if (pixelUpdate == 8) await Assert.That(node.ClearColor).IsEqualTo(0xFF123456u);
    }

    [Test]
    public async Task LegacyFlags_ReadsFourBytesAndUpdatesPixelUpdate()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(-1);
            writer.Write(0xBEEF0503u);
            writer.Write(0.5f);
            writer.Write(1f);
            writer.Write(-1); // Legacy shader reference.
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        await ReadAndRoundTrip(payload, node, new CPlugBitmap.Chunk09011000());
        await Assert.That(node.Usage).IsEqualTo(CPlugBitmap.EUsage.Render);
        await Assert.That(node.PixelUpdate).IsEqualTo(5);
        await Assert.That(node.Flags).IsEqualTo(0xBEEF0503UL);
    }

    [Test]
    public async Task AtlasCounts_ReadsUnsigned16BitPairs()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(2);
            writer.Write((ushort)60000);
            writer.Write((ushort)65535);
            writer.Write((ushort)3);
            writer.Write((ushort)4);
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        await ReadAndRoundTrip(payload, node, new CPlugBitmap.Chunk0901101E());
        await Assert.That(node.AtlasCountUVs![0].U).IsEqualTo((ushort)60000);
        await Assert.That(node.AtlasCountUVs[0].V).IsEqualTo(ushort.MaxValue);
        await Assert.That(node.AtlasCountUVs[1].V).IsEqualTo((ushort)4);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task CubeAtlas_ReadsNestedVersionAndEntries(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0); // Bitmap chunk version.
            writer.Write(-1); // Atlas node reference.
            writer.Write(1u);
            writer.Write(version); // Cube archive version.
            if (version == 0)
            {
                writer.Write(123u);
                writer.Write(456u);
            }
            else writer.Write(2u);
            writer.Write(4096u);
            writer.Write(2048u);
            writer.Write(1); // Entry count.
            for (var i = 1u; i <= 8; i++) writer.Write(i);
            if (version >= 2) writer.Write(-1); // Obsolete image FID.
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        await ReadAndRoundTrip(payload, node, new CPlugBitmap.Chunk09011038());
        await Assert.That(node.AtlasCubeIn!.Version).IsEqualTo(version);
        await Assert.That(node.AtlasCubeIn.SizeY).IsEqualTo(2048u);
        await Assert.That(node.AtlasCubeIn.Entries![0].U08).IsEqualTo(8u);
    }

    [Test]
    public async Task GrassImage_ExternalReferenceSelectsTextureCoordinates()
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "GrassId.dds");
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(1);
            writer.Write(1); // External image reference.
            writer.Write(2f);
            writer.Write(3f);
            writer.Write(0.25f);
            writer.Write(0.5f);
            writer.Write(1.25f);
            writer.Write(-1);
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        await ReadAndRoundTrip(payload, node, new CPlugBitmap.Chunk09011036(),
            new Dictionary<int, GbxRefTableNode> { [1] = file });
        await Assert.That(node.GrassIdImageFile).IsSameReferenceAs(file);
        await Assert.That(node.GrassIdTexCoordRotate).IsEqualTo(1.25f);
    }

    [Test]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ImageArray_ReadsExternalFids(int version)
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "Layer.dds");
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(-1);
            writer.Write("Suffix");
            writer.Write(1);
            writer.Write(1); // External FID, rather than a string.
            if (version >= 3)
            {
                writer.Write(-1);
                writer.Write("Layer");
            }
            if (version == 4) writer.Write(0u);
            writer.Write(Sentinel);
        }

        var node = new CPlugBitmap();
        await ReadAndRoundTrip(payload, node, new CPlugBitmap.Chunk09011034(),
            new Dictionary<int, GbxRefTableNode> { [1] = file });
        await Assert.That(node.ImageArrayFids![0].File).IsSameReferenceAs(file);
    }

    private static async Task ReadAndRoundTrip(MemoryStream payload, CPlugBitmap node,
        Chunk<CPlugBitmap> chunk, IReadOnlyDictionary<int, GbxRefTableNode>? files = null)
    {
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            if (files is not null) reader.LoadRefTable(files);
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(Sentinel);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
