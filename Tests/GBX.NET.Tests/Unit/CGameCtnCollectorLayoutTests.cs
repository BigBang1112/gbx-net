using GBX.NET.Attributes;
using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnCollectorLayoutTests
{
    private const int Sentinel = 0x11223344;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task DescriptionHeaderPreservesAllVersionBranches(int version)
    {
        var payload = Payload(writer =>
        {
            writer.Write(new Ident("Collector", new Id("Stadium"), "Author"));
            writer.Write(version);
            writer.Write("Page");
            if (version == 5) writer.WriteIdAsString("Deprecated");
            if (version >= 4) writer.WriteIdAsString("Parent");
            if (version is 1 or 2) writer.Write(true);
            if (version == 2) writer.Write(true);
            if (version >= 3) writer.Write(4);
            if (version >= 2) writer.Write((short)-1234);
            if (version is >= 2 and <= 5)
            {
                writer.Write((byte)200);
                writer.Write(-123456);
                writer.Write((short)-2345);
            }
            if (version >= 7) writer.Write("Display Name");
            if (version >= 8) writer.Write((byte)3);
        });
        var node = new CGameCtnCollector();
        var chunk = new CGameCtnCollector.HeaderChunk2E001003();
        Read(payload, node, chunk);
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.CatalogPosition).IsEqualTo(version >= 2 ? -1234 : 1);
        await Assert.That(node.Flags).IsEqualTo(version >= 3
            ? CGameCtnCollector.ECollectorFlags.IsAdvanced
            : version == 2 ? CGameCtnCollector.ECollectorFlags.IsInternal : CGameCtnCollector.ECollectorFlags.None);
        await Assert.That(node.Name).IsEqualTo(version >= 7 ? "Display Name" : "Collector");
        if (version is >= 2 and <= 5)
        {
            await Assert.That(node.NbAvailableMin).IsEqualTo(200);
            await Assert.That(node.NbAvailableMax).IsEqualTo(-2345);
            await Assert.That(node.CopperPrice).IsEqualTo(-123456);
        }
        await RoundTrip(payload, node, chunk);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    public async Task SkinChunkPreservesFolderPathsAndFileReferences(int version, bool hasDirectory)
    {
        var files = new Dictionary<int, GbxRefTableNode>();
        var refTable = new GbxRefTable();
        var payload = Payload(writer =>
        {
            void Reference(string path)
            {
                var index = files.Count + 1;
                files.Add(index, new GbxRefTableFile(refTable, 0, true, path));
                writer.Write(index);
            }

            writer.Write(version);
            Reference("DefaultSkin.zip");
            if (version >= 1) writer.Write(hasDirectory ? "Skins/" : "");
            if (version >= 2 && !hasDirectory) Reference("LegacySkin.zip");
            if (version >= 3) writer.Write("ModelKit/");
            if (version >= 4)
            {
                Reference("ModelKitDb.Gbx");
                Reference("ModelKitDb.Release.Gbx");
            }
        });
        var node = new CGameCtnCollector();
        var chunk = new CGameCtnCollector.Chunk2E001010();
        Read(payload, node, chunk, files, isRelease: false);
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.DefaultSkinFile!.FilePath).IsEqualTo("DefaultSkin.zip");
        await Assert.That(node.SkinDirectory).IsEqualTo(version == 0 ? null : hasDirectory ? "Skins/" : "");
        if (version >= 2 && !hasDirectory) await Assert.That(chunk.U01File!.FilePath).IsEqualTo("LegacySkin.zip");
        if (version >= 3) await Assert.That(chunk.U02).IsEqualTo("ModelKit/");
        if (version >= 4)
        {
            await Assert.That(node.ModelKitDbFile!.FilePath).IsEqualTo("ModelKitDb.Gbx");
            await Assert.That(node.ModelKitDbReleaseFile!.FilePath).IsEqualTo("ModelKitDb.Release.Gbx");
        }
        await RoundTrip(payload, node, chunk, isRelease: false);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(9)]
    public async Task IconChunksPreserveExternalReferences(int offset)
    {
        var file = new GbxRefTableFile(new(), 0, true, "Icon.dds");
        var files = new Dictionary<int, GbxRefTableNode> { [1] = file };
        var payload = Payload(writer =>
        {
            writer.Write("Page");
            if (offset >= 3) writer.Write(true);
            writer.Write(1);
            if (offset == 4) writer.Write(true);
            if (offset == 9) writer.WriteIdAsString("Parent");
        });
        Chunk<CGameCtnCollector> chunk = offset switch
        {
            0 => new CGameCtnCollector.Chunk2E001000(),
            1 => new CGameCtnCollector.Chunk2E001001(),
            3 => new CGameCtnCollector.Chunk2E001003(),
            4 => new CGameCtnCollector.Chunk2E001004(),
            9 => new CGameCtnCollector.Chunk2E001009(),
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };
        var node = new CGameCtnCollector();
        Read(payload, node, chunk, files);
        if (offset >= 3) await Assert.That(node.IconFidFile).IsSameReferenceAs(file);
        await RoundTrip(payload, node, chunk);
    }

    [Test]
    public async Task RenderChunkHasAVersionAndFloatPhase()
    {
        var payload = Payload(writer =>
        {
            writer.Write(0);
            writer.Write(true);
            writer.Write(-2);
            writer.Write(0.375f);
        });
        var node = new CGameCtnCollector();
        var chunk = new CGameCtnCollector.Chunk2E001012();
        Read(payload, node, chunk);
        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(node.IconUseAutoRender).IsTrue();
        await Assert.That(node.IconQuarterRotationY).IsEqualTo(-2);
        await Assert.That(node.IconPhase01).IsEqualTo(0.375f);
        await RoundTrip(payload, node, chunk);
    }

    [Test]
    [Arguments(3, GbxMode.Release, GbxCompression.Uncompressed)]
    [Arguments(3, GbxMode.Release, GbxCompression.Compressed)]
    [Arguments(3, GbxMode.Editor, GbxCompression.Uncompressed)]
    [Arguments(3, GbxMode.Editor, GbxCompression.Compressed)]
    [Arguments(4, GbxMode.Release, GbxCompression.Uncompressed)]
    [Arguments(4, GbxMode.Release, GbxCompression.Compressed)]
    [Arguments(4, GbxMode.Editor, GbxCompression.Uncompressed)]
    [Arguments(4, GbxMode.Editor, GbxCompression.Compressed)]
    public async Task SkinChunkUsesHeaderModeThroughFullGbxRoundTrip(int version, GbxMode mode, GbxCompression compression)
    {
        var node = new CGameCtnCollector { CatalogPosition = 77, Icon = new Color[3, 2] };
        node.Chunks.Create<CGameCtnCollector.HeaderChunk2E001004>();
        var chunk = node.Chunks.Create<CGameCtnCollector.Chunk2E001010>();
        chunk.Version = version;
        chunk.U02 = "ModelKit/";
        node.Chunks.Create<CGameCtnCollector.Chunk2E001011>().Version = 1;
        var gbx = new Gbx<CGameCtnCollector>(node, GbxHeaderBasic.Create(mode: mode))
        {
            BodyCompression = compression
        };
        using var stream = new MemoryStream();
        gbx.Save(stream);
        stream.Position = 0;
        var parsed = Gbx.Parse<CGameCtnCollector>(stream);
        await Assert.That(parsed.Node.CatalogPosition).IsEqualTo(77);
        await Assert.That(parsed.Node.Icon!.GetLength(0)).IsEqualTo(3);
        await Assert.That(parsed.Node.Icon.GetLength(1)).IsEqualTo(2);
        await Assert.That(parsed.Node.Chunks.Get<CGameCtnCollector.Chunk2E001010>()!.U02)
            .IsEqualTo(mode == GbxMode.Editor ? "ModelKit/" : null);
        using var saved = new MemoryStream();
        parsed.Save(saved);
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ConstructorAndChunkAttributesMatchVerifiedMetadata()
    {
        var node = new CGameCtnCollector();
        await Assert.That(node.CatalogPosition).IsEqualTo(1);
        await Assert.That(node.CopperPrice).IsEqualTo(100);
        await Assert.That(node.NbAvailableMax).IsEqualTo(10);
        await Assert.That(node.ProdState).IsEqualTo(CGameCtnCollector.EProdState.Release);
        await Assert.That(new CGameCtnCollector.Chunk2E001008().GameVersion.HasFlag(GameVersion.TM2020)).IsTrue();
        await Assert.That(new CGameCtnCollector.HeaderChunk2E001008().GameVersion.HasFlag(GameVersion.TM2020)).IsTrue();
        var attributes = typeof(CGameCtnCollector).GetProperty(nameof(CGameCtnCollector.CatalogPosition))!
            .GetCustomAttributes(typeof(AppliedWithChunkAttribute), true).Cast<AppliedWithChunkAttribute>();
        await Assert.That(attributes.Select(x => x.ChunkType).Contains(typeof(CGameCtnCollector.HeaderChunk2E001003))).IsTrue();
    }

    private static byte[] Payload(Action<GbxWriter> action)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        action(writer);
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static void Read(byte[] payload, CGameCtnCollector node, Chunk<CGameCtnCollector> chunk,
        Dictionary<int, GbxRefTableNode>? files = null, bool isRelease = true)
    {
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        reader.IsRelease = isRelease;
        using var rw = new GbxReaderWriter(reader);
        if (files is not null) reader.LoadRefTable(files);
        chunk.ReadWrite(node, rw);
        if (reader.ReadInt32() != Sentinel || stream.Position != stream.Length)
        {
            throw new InvalidDataException("Chunk did not consume its exact payload.");
        }
    }

    private static async Task RoundTrip(byte[] payload, CGameCtnCollector node, Chunk<CGameCtnCollector> chunk, bool isRelease = true)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.IsRelease = isRelease;
        using var rw = new GbxReaderWriter(writer);
        chunk.ReadWrite(node, rw);
        writer.Write(Sentinel);
        await Assert.That(stream.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
