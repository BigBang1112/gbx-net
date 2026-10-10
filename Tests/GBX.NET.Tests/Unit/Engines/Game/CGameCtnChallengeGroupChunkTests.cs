using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnChallengeGroupChunkTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task LegacyMapsKeepTheThreeMedalRequirements(int offset)
    {
        var metadata = new Ident("Map", "Desert", "Author");
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(1);
            writer.Write(2);
            writer.Write(3);
            writer.Write(1);
            if (offset == 0) writer.WriteNodeRef<CGameCtnChallenge>(null);
            else writer.Write(metadata);
            writer.Write(0x12345678);
        }

        Chunk<CGameCtnChallengeGroup> chunk = offset == 0
            ? new CGameCtnChallengeGroup.Chunk0308F000()
            : new CGameCtnChallengeGroup.Chunk0308F001();
        var restored = await ReadAndRewrite(stream, chunk);
        await Assert.That(restored.NbBronzeRequired).IsEqualTo(1);
        await Assert.That(restored.NbSilverRequired).IsEqualTo(2);
        await Assert.That(restored.NbGoldRequired).IsEqualTo(3);
        if (offset == 0) await Assert.That(restored.MapFiles!).HasSingleItem();
        else await Assert.That(restored.MapInfos!.Single().Metadata).IsEqualTo(metadata);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyMapListSelectsIdentifiersOrExternalFiles(bool usesMapFiles)
    {
        var refTable = new GbxRefTable();
        var file = new GbxRefTableFile(refTable, 0, true, "Track.Challenge.Gbx");
        var metadata = new Ident("Map", "Stadium", "Author");
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(usesMapFiles);
            writer.Write(1);
            if (usesMapFiles) writer.WriteNodeRef<CGameCtnChallenge>(null, file);
            else writer.Write(metadata);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnChallengeGroup.Chunk0308F006(), file);
        await Assert.That(restored.UsesMapFiles).IsEqualTo(usesMapFiles);
        if (usesMapFiles)
        {
            await Assert.That(restored.MapFiles!.Single().File).IsEqualTo(file);
            await Assert.That(restored.MapInfos).IsNull();
        }
        else
        {
            await Assert.That(restored.MapInfos!.Single().Metadata).IsEqualTo(metadata);
            await Assert.That(restored.MapFiles).IsNull();
        }
    }

    [Test]
    [Arguments(3)]
    [Arguments(7)]
    public async Task MedalValuesAndUnlockRequirementsKeepDistinctSignedFields(int offset)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(int.MinValue + 1);
            writer.Write(2);
            writer.Write(3);
            if (offset == 7)
            {
                writer.Write(4);
                writer.Write(5);
                writer.Write(6);
            }
            writer.Write(0x12345678);
        }

        Chunk<CGameCtnChallengeGroup> chunk = offset == 3
            ? new CGameCtnChallengeGroup.Chunk0308F003()
            : new CGameCtnChallengeGroup.Chunk0308F007();
        var restored = await ReadAndRewrite(stream, chunk);
        var values = offset == 3
            ? new[] { restored.AllBronzeValue, restored.AllSilverValue, restored.AllGoldValue }
            : new[] { restored.NbBronzeMedalRequired, restored.NbSilverMedalRequired,
                restored.NbGoldMedalRequired, restored.NbBronzeCupRequired,
                restored.NbSilverCupRequired, restored.NbGoldCupRequired };
        var expected = offset == 3 ? new[] { int.MinValue + 1, 2, 3 } : [int.MinValue + 1, 2, 3, 4, 5, 6];
        await Assert.That(values.SequenceEqual(expected)).IsTrue();
    }

    [Test]
    [Arguments(4)]
    [Arguments(8)]
    public async Task LegacyFileReferencesPreserveTheReferenceTableEntry(int offset)
    {
        var refTable = new GbxRefTable();
        var file = new GbxRefTableFile(refTable, 0, true, "Collection.Gbx");
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            if (offset == 4) writer.Write(1);
            writer.WriteNodeRef<CMwNod>(null, file);
            writer.Write(0x12345678);
        }

        Chunk<CGameCtnChallengeGroup> chunk = offset == 4
            ? new CGameCtnChallengeGroup.Chunk0308F004()
            : new CGameCtnChallengeGroup.Chunk0308F008();
        var restored = await ReadAndRewrite(stream, chunk, file);
        await Assert.That(offset == 4 ? restored.Files!.Single().File : restored.AssociatedCollectionFile).IsEqualTo(file);
    }

    [Test]
    public async Task LinkedCampaignReadsANodeReference()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteNodeRef(new CGameCtnCampaign());
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnChallengeGroup.Chunk0308F005());
        await Assert.That(restored.LinkedCampaign).IsNotNull();
    }

    private static async Task<CGameCtnChallengeGroup> ReadAndRewrite(
        MemoryStream stream, Chunk<CGameCtnChallengeGroup> chunk, GbxRefTableFile? file = null)
    {
        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        if (file is not null) reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
        using var rw = new GbxReaderWriter(reader);
        var restored = new CGameCtnChallengeGroup();
        chunk.ReadWrite(restored, rw);
        await Assert.That(stream.Position).IsEqualTo(payloadLength);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(restored, writerWriter);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray().Take((int)payloadLength))).IsTrue();
        return restored;
    }
}
