using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;
using System.IO.Compression;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeEmbeddedItemsTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_UnchangedZip_PreservesExpectedItemsAndOrder(int version)
    {
        using var payload = CreatePayload(version, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));

        await Assert.That(node.ExpectedEmbeddedItemModels!).IsEquivalentTo(ExpectedItems(), CollectionOrdering.Matching);
        if (version >= 1)
        {
            await Assert.That(node.EmbeddedItemModelsExternalDataRefs!).IsEquivalentTo(ExternalDataRefs(), CollectionOrdering.Matching);
        }
        else
        {
            await Assert.That(node.EmbeddedItemModelsExternalDataRefs).IsNull();
        }
        await payload.AssertRoundTrip(rw => chunk.Write(node, rw.Writer!));
        await payload.AssertRoundTrip(rw => chunk.Write(node, rw.Writer!));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_IdenticalReplacementZip_PreservesExpectedItems(int version)
    {
        using var payload = CreatePayload(version, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
        node.EmbeddedZipData = (byte[])node.EmbeddedZipData!.Clone();

        await payload.AssertRoundTrip(rw => chunk.Write(node, rw.Writer!));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_EmptyZip_PreservesExpectedItems(int version)
    {
        using var payload = CreatePayload(version, [], ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));

        await payload.AssertRoundTrip(rw => chunk.Write(node, rw.Writer!));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_UnchangedZip_DoesNotRequireReadableArchive(bool emptyExpectedItems)
    {
        using var payload = CreatePayload(1, [1, 2, 3], emptyExpectedItems ? [] : ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));

        await payload.AssertRoundTrip(rw => chunk.Write(node, rw.Writer!));
    }

    [Test]
    public async Task ReadWrite_ChangedZip_RebuildsExpectedItems()
    {
        using var payload = CreatePayload(1, [1, 2, 3], ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
        node.EmbeddedZipData = CreateZip();

        var restored = WriteAndRead(node, chunk);
        await Assert.That(restored.ExpectedEmbeddedItemModels!).IsEmpty();
        await Assert.That(restored.EmbeddedZipData!).IsEquivalentTo(node.EmbeddedZipData, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_NewOrReplacedZip_DiscoversItemIdentifiers(bool replaceOriginal)
    {
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054 { Version = 1 };
        if (replaceOriginal)
        {
            using var payload = CreatePayload(1, CreateZip(), ExpectedItems());
            payload.Read(rw => chunk.Read(node, rw.Reader!));
        }

        using var stream = new MemoryStream();
        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            var item = new CGameItemModel { Ident = new Ident("Header.Item.Gbx", "Stadium", "Author") };
            item.CreateChunk<CGameCtnCollector.HeaderChunk2E001003>().Version = 8;
            using var entryStream = zip.CreateEntry("Items/Folder/Written.Item.Gbx").Open();
            new Gbx<CGameItemModel>(item) { BodyCompression = GbxCompression.Uncompressed }.Save(entryStream);
        }
        node.EmbeddedZipData = stream.ToArray();

        var restored = WriteAndRead(node, chunk);
        await Assert.That(restored.ExpectedEmbeddedItemModels!).IsEquivalentTo(
            new[] { new Ident("Folder\\Written.Item.Gbx", "Stadium", "Author") }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_InPlaceZipMutation_RebuildsExpectedItems()
    {
        using var payload = CreatePayload(1, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
        node.EmbeddedZipData![Array.IndexOf(node.EmbeddedZipData, (byte)0xB1)] = 0xB2;

        var restored = WriteAndRead(node, chunk);
        await Assert.That(restored.ExpectedEmbeddedItemModels!).IsEmpty();
        await Assert.That(restored.EmbeddedZipData!).IsEquivalentTo(node.EmbeddedZipData, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_RemovedZip_DoesNotReuseExpectedItems(bool useNull)
    {
        using var payload = CreatePayload(1, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
        node.EmbeddedZipData = useNull ? null : [];

        var restored = WriteAndRead(node, chunk);
        await Assert.That(restored.ExpectedEmbeddedItemModels!).IsEmpty();
        await Assert.That(restored.EmbeddedZipData!).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_ZipUpdate_RebuildsExpectedItems(bool asyncUpdate)
    {
        using var payload = CreatePayload(1, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
        if (asyncUpdate)
        {
            await node.UpdateEmbeddedZipDataAsync(zip =>
            {
                zip.CreateEntry("Extra.txt");
                return Task.CompletedTask;
            });
        }
        else
        {
            node.UpdateEmbeddedZipData(zip => zip.CreateEntry("Extra.txt"));
        }

        var restored = WriteAndRead(node, chunk);
        await Assert.That(restored.ExpectedEmbeddedItemModels!).IsEmpty();
    }

    [Test]
    public async Task DeepClone_Node_PreservesExpectedItemsForUnchangedZip()
    {
        using var payload = CreatePayload(1, CreateZip(), ExpectedItems());
        var node = new CGameCtnChallenge();
        var chunk = new CGameCtnChallenge.Chunk03043054();
        payload.Read(rw => chunk.Read(node, rw.Reader!));
#pragma warning disable GBXNET10001
        var clone = (CGameCtnChallenge)node.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone.EmbeddedZipData).IsNotSameReferenceAs(node.EmbeddedZipData);
        await payload.AssertRoundTrip(rw => chunk.Write(clone, rw.Writer!));
    }

    private static Ident[] ExpectedItems() =>
    [
        new("Original.Item.Gbx", "Stadium", "Author"),
        new("Folder\\Other.Item.Gbx", "Stadium", "OtherAuthor"),
        new("Original.Item.Gbx", "Stadium", "Author")
    ];

    private static ObjectModelPayload CreatePayload(int version, byte[] zipData, Ident[] expectedItems)
    {
        var payload = new ObjectModelPayload();
        payload.Writer.Write(version);
        payload.Writer.WriteEncapsulated(writer =>
        {
            writer.WriteArray(expectedItems);
            writer.WriteData(zipData);
            if (version >= 1)
            {
                writer.WriteArray(ExternalDataRefs());
            }
        });
        payload.Finish();
        return payload;
    }

    private static string[] ExternalDataRefs() =>
    [
        "Materials\\Example.Material.Gbx",
        "Models\\Příliš.Solid.Gbx",
        "Materials\\Example.Material.Gbx",
        ""
    ];

    private static byte[] CreateZip()
    {
        using var stream = new MemoryStream();
        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            using var entryStream = zip.CreateEntry("NotAnItem.txt", CompressionLevel.NoCompression).Open();
            entryStream.WriteByte(0xB1);
            entryStream.WriteByte(0xC2);
            entryStream.WriteByte(0xD3);
        }
        return stream.ToArray();
    }

    private static CGameCtnChallenge WriteAndRead(CGameCtnChallenge node, CGameCtnChallenge.Chunk03043054 chunk)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            chunk.Write(node, writer);
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var restored = new CGameCtnChallenge();
        new CGameCtnChallenge.Chunk03043054().Read(restored, reader);
        return restored;
    }
}
