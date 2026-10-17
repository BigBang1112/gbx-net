using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeWorldDistortionLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_NoBitmap_PreservesScalesAndVersionThreeTail(int version)
    {
        var node = new CGameCtnChallenge { WorldDistortXYZ = new() };
        var chunk = await RoundTrip(w =>
        {
            WritePrefix(w, version);
            if (version == 0) w.Write(-1);
            else w.Write(false);
            if (version >= 3)
            {
                w.Write(123);
                w.Write(-456);
            }
        }, node);
        await Assert.That(node.WorldDistortScales).IsEqualTo(new Vec3(1.25f, -2.5f, 3.75f));
        if (version >= 1) await Assert.That(node.WorldDistortXYZ).IsNull();
        if (version >= 3)
        {
            await Assert.That(chunk.U01).IsEqualTo(123);
            await Assert.That(chunk.U02).IsEqualTo(-456);
        }
        await Assert.That(new CGameCtnChallenge.Chunk03043059(GameVersion.MP4).Version).IsEqualTo(2);
        await Assert.That(new CGameCtnChallenge.Chunk03043059(GameVersion.TM2020).Version).IsEqualTo(3);
    }

    [Test]
    public async Task ReadWrite_VersionZero_PreservesLegacyBitmapReference()
    {
        var bitmap = new CPlugBitmap();
        var node = new CGameCtnChallenge();
        await RoundTrip(w =>
        {
            WritePrefix(w, 0);
            w.WriteNodeRef(bitmap);
        }, node);
        await Assert.That(node.References.WorldDistortBitmap.Resolve()).IsTypeOf<CPlugBitmap>();
    }

    [Test]
    [Arguments((byte)2, (byte)24)]
    [Arguments((byte)2, (byte)32)]
    [Arguments((byte)3, (byte)8)]
    [Arguments((byte)10, (byte)24)]
    [Arguments((byte)10, (byte)32)]
    [Arguments((byte)11, (byte)8)]
    public async Task ReadWrite_VersionOne_PreservesTgaHeaderIdentifierAndEncodedPixels(byte imageType, byte pixelDepth)
    {
        var pixelSize = pixelDepth / 8;
        var pixels = imageType < 9
            ? Enumerable.Range(0, 6 * pixelSize).Select(x => (byte)x).ToArray()
            : new byte[] { 0x82 }.Concat(Enumerable.Repeat((byte)123, pixelSize))
                .Concat(new byte[] { 0x02 }).Concat(Enumerable.Range(0, 3 * pixelSize).Select(x => (byte)x)).ToArray();
        var node = new CGameCtnChallenge();
        await RoundTrip(w =>
        {
            WritePrefix(w, 1);
            w.Write(true);
            WriteTga(w, imageType, pixelDepth, pixels);
        }, node);
        var image = node.WorldDistortXYZ!.Tga!;
        await Assert.That(image.Width).IsEqualTo((ushort)3);
        await Assert.That(image.Height).IsEqualTo((ushort)2);
        await Assert.That(image.ImageType).IsEqualTo(imageType);
        await Assert.That(image.PixelDepth).IsEqualTo(pixelDepth);
        await Assert.That(image.XOrigin).IsEqualTo((ushort)4);
        await Assert.That(image.YOrigin).IsEqualTo((ushort)5);
        await Assert.That(image.ImageDescriptor).IsEqualTo((byte)0x20);
        await Assert.That(image.ImageId!).IsEquivalentTo(new byte[] { 9, 8 }, CollectionOrdering.Matching);
        await Assert.That(image.EncodedPixels!).IsEquivalentTo(pixels, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(2, 3)]
    [Arguments(0, 3)]
    public async Task ReadWrite_VersionTwo_PreservesRgba16PixelsWithoutLengthPrefix(int width, int height)
    {
        var pixels = Enumerable.Range(0, width * height * 8).Select(x => (byte)x).ToArray();
        var node = new CGameCtnChallenge();
        await RoundTrip(w =>
        {
            WritePrefix(w, 2);
            w.Write(true);
            w.Write(new Int2(width, height));
            w.WriteData(pixels, pixels.Length);
        }, node);
        await Assert.That(node.WorldDistortXYZ!.Size).IsEqualTo(new Int2(width, height));
        await Assert.That(node.WorldDistortXYZ.Pixels!).IsEquivalentTo(pixels, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(-1, 1)]
    [Arguments(1, -1)]
    [Arguments(268435456, 1)]
    [Arguments(65536, 65536)]
    public async Task ReadWrite_InvalidBitmapDimensions_FailsBeforeReadingPixels(int width, int height)
    {
        using var input = Payload(w =>
        {
            WritePrefix(w, 2);
            w.Write(true);
            w.Write(new Int2(width, height));
            w.Write(0x12345678);
        });
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<InvalidDataException>(() => new CGameCtnChallenge.Chunk03043059().ReadWrite(new(), rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }

    [Test]
    public async Task ReadWrite_VersionThreeWithBitmap_RemainsUnsupportedByTheNativeFormat()
    {
        using var input = Payload(w =>
        {
            WritePrefix(w, 3);
            w.Write(true);
            w.Write(0x12345678);
        });
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<NotSupportedException>(() => new CGameCtnChallenge.Chunk03043059().ReadWrite(new(), rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }

    [Test]
    public void ReadWrite_OversizedRlePacket_RejectsThePacket()
    {
        using var input = Payload(w =>
        {
            WritePrefix(w, 1);
            w.Write(true);
            WriteTga(w, 10, 32, [0x86]);
        });
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<InvalidDataException>(() => new CGameCtnChallenge.Chunk03043059().ReadWrite(new(), rw));
    }

    [Test]
    public void ReadWrite_TruncatedRlePacket_RejectsMissingPixels()
    {
        using var input = Payload(w =>
        {
            WritePrefix(w, 1);
            w.Write(true);
            WriteTga(w, 10, 32, [0x85, 1, 2]);
        });
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<EndOfStreamException>(() => new CGameCtnChallenge.Chunk03043059().ReadWrite(new(), rw));
    }

    private static void WritePrefix(GbxWriter writer, int version)
    {
        writer.Write(version);
        writer.Write(new Vec3(1.25f, -2.5f, 3.75f));
    }

    private static void WriteTga(GbxWriter writer, byte imageType, byte pixelDepth, byte[] pixels)
    {
        writer.Write((byte)2); // image ID length
        writer.Write((byte)0); // no color map
        writer.Write(imageType);
        writer.Write((ushort)0); // color map first index
        writer.Write((ushort)0); // color map length
        writer.Write((byte)0); // color map entry size
        writer.Write((ushort)4); // X origin
        writer.Write((ushort)5); // Y origin
        writer.Write((ushort)3); // width
        writer.Write((ushort)2); // height
        writer.Write(pixelDepth);
        writer.Write((byte)0x20); // top-left origin
        writer.WriteData([9, 8], 2);
        writer.WriteData(pixels, pixels.Length);
    }

    private static MemoryStream Payload(Action<GbxWriter> write)
    {
        var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) write(writer);
        stream.Position = 0;
        return stream;
    }

    private static async Task<CGameCtnChallenge.Chunk03043059> RoundTrip(Action<GbxWriter> write, CGameCtnChallenge node)
    {
        using var input = Payload(writer =>
        {
            write(writer);
            writer.Write(0x12345678);
        });
        var chunk = new CGameCtnChallenge.Chunk03043059();
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(0x12345678);
        }
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
        return chunk;
    }
}
