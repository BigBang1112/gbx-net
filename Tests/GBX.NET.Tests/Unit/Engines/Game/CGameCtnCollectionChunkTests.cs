using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

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

    private static void WriteIcon(GbxWriter writer, int kind, GbxRefTableFile? file)
    {
        writer.Write(kind != 0);
        if (kind != 0)
        {
            writer.WriteNodeRef(kind == 1 ? new CPlugBitmap() : null, file);
        }
    }
}
