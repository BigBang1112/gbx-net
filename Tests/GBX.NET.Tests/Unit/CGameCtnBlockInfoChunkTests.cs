using GBX.NET.Attributes;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameCtnBlockInfoChunkTests
{
    [Test]
    public async Task Collector_NameAndIdentIdFillEachOtherWhenMissing()
    {
        var named = new CGameCtnCollector { Name = "NameOnly" };
        var identified = new CGameCtnCollector { Ident = new("IdOnly") };
        var distinct = new CGameCtnCollector { Name = "DisplayName", Ident = new("InternalId") };

        await Assert.That(named.Ident.Id).IsEqualTo("NameOnly");
        await Assert.That(identified.Name).IsEqualTo("IdOnly");
        await Assert.That(distinct.Name).IsEqualTo("DisplayName");
        await Assert.That(distinct.Ident.Id).IsEqualTo("InternalId");
    }

    [Test]
    public async Task BlockInfo_NameUsesCollectorPropertyWithoutChangingIdent()
    {
        CGameCtnCollector collector = new CGameCtnBlockInfoClassic
        {
            Ident = new("OldId", new Id("Stadium"), "Author")
        };

        collector.Name = "NewId";

        await Assert.That(collector.Name).IsEqualTo("NewId");
        await Assert.That(collector.Ident.Id).IsEqualTo("OldId");
        await Assert.That(collector.Ident.Collection).IsEqualTo(new Id("Stadium"));
        await Assert.That(collector.Ident.Author).IsEqualTo("Author");
    }

    [Test]
    public async Task BlockInfo_InheritedNameRetainsCollectorChunkAttributes()
    {
        var property = typeof(CGameCtnBlockInfo).GetProperty(nameof(CGameCtnBlockInfo.Name))!;
        await Assert.That(property.DeclaringType).IsEqualTo(typeof(CGameCtnCollector));
        var chunkTypes = Attribute.GetCustomAttributes(property, typeof(AppliedWithChunkAttribute), inherit: true)
            .Cast<AppliedWithChunkAttribute>()
            .Select(attribute => attribute.ChunkType)
            .ToHashSet();

        await Assert.That(chunkTypes.Contains(typeof(CGameCtnCollector.HeaderChunk2E001003))).IsTrue();
        await Assert.That(chunkTypes.Contains(typeof(CGameCtnCollector.Chunk2E00100C))).IsTrue();
        await Assert.That(chunkTypes.Contains(typeof(CGameCtnBlockInfo.Chunk0304E005))).IsTrue();
    }

    [Test]
    public async Task Chunk0304E005_RoundTripsFieldsAndJaggedMobils()
    {
        var original = new CGameCtnBlockInfoClassic
        {
            Ident = new("BlockId"),
            IsPillar = true,
            Selection = CGameCtnBlockInfo.ESelection.AutoRotate,
            GroundBlockUnitInfos = [],
            AirBlockUnitInfos = [],
            GroundMobils = [[new(null, null)], []],
            AirMobils = [[], [new(null, null)]]
        };
        var chunk = new CGameCtnBlockInfo.Chunk0304E005
        {
            U02 = 2,
            U03 = 3,
            U04 = 4,
            U06 = 6,
            U07 = 7,
            U08 = 8,
            U09 = 9,
            U10 = 10
        };

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(original, writerWriter);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readerWriter = new GbxReaderWriter(reader);
        var restored = new CGameCtnBlockInfoClassic
        {
            Ident = new("PreviousId", new Id("Stadium"), "Author")
        };
        var restoredChunk = new CGameCtnBlockInfo.Chunk0304E005();
        restoredChunk.ReadWrite(restored, readerWriter);

        await Assert.That(restored.Ident.Id).IsEqualTo("PreviousId");
        await Assert.That(restored.Name).IsEqualTo("BlockId");
        await Assert.That(restored.Ident.Collection).IsEqualTo(new Id("Stadium"));
        await Assert.That(restored.Ident.Author).IsEqualTo("Author");
        await Assert.That(restored.IsPillar).IsTrue();
        await Assert.That(restored.Selection).IsEqualTo(CGameCtnBlockInfo.ESelection.AutoRotate);
        await Assert.That(restored.GroundBlockUnitInfos).IsEmpty();
        await Assert.That(restored.AirBlockUnitInfos).IsEmpty();
        await Assert.That(restored.GroundMobils).Count().IsEqualTo(2);
        await Assert.That(restored.GroundMobils![0]).Count().IsEqualTo(1);
        await Assert.That(restored.GroundMobils[0][0].Node).IsNull();
        await Assert.That(restored.GroundMobils[1]).IsEmpty();
        await Assert.That(restored.AirMobils).Count().IsEqualTo(2);
        await Assert.That(restored.AirMobils![0]).IsEmpty();
        await Assert.That(restored.AirMobils[1]).Count().IsEqualTo(1);
        await Assert.That(restored.AirMobils[1][0].Node).IsNull();
        await Assert.That(restoredChunk.U02).IsEqualTo(2);
        await Assert.That(restoredChunk.U03).IsEqualTo(3);
        await Assert.That(restoredChunk.U04).IsEqualTo(4);
        await Assert.That(restoredChunk.U06).IsEqualTo(6);
        await Assert.That(restoredChunk.U07).IsEqualTo((byte)7);
        await Assert.That(restoredChunk.U08).IsEqualTo(8);
        await Assert.That(restoredChunk.U09).IsEqualTo((short)9);
        await Assert.That(restoredChunk.U10).IsEqualTo((short)10);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task Chunk0304E005_UsesNameWhenIdentIdIsEmpty()
    {
        var original = new CGameCtnBlockInfoClassic { Name = "BlockName" };

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            new CGameCtnBlockInfo.Chunk0304E005().ReadWrite(original, writerWriter);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readerWriter = new GbxReaderWriter(reader);
        var restored = new CGameCtnBlockInfoClassic();
        new CGameCtnBlockInfo.Chunk0304E005().ReadWrite(restored, readerWriter);

        await Assert.That(original.Ident.Id).IsEqualTo("BlockName");
        await Assert.That(restored.Name).IsEqualTo("BlockName");
        await Assert.That(restored.Ident.Id).IsEqualTo("BlockName");
    }
}
