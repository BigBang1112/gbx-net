using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit;

public class CGameCtnChallengeParametersChunkTests
{
    [Test]
    public async Task ConstructorUsesLayoutDefaults()
    {
        var parameters = new CGameCtnChallengeParameters();

        await Assert.That(parameters.BronzeTime).IsNull();
        await Assert.That(parameters.SilverTime).IsNull();
        await Assert.That(parameters.GoldTime).IsNull();
        await Assert.That(parameters.AuthorTime).IsNull();
        await Assert.That(parameters.AuthorScore).IsNull();
        await Assert.That(parameters.TimeLimit.TotalMilliseconds).IsEqualTo(60000);
    }

    [Test]
    public async Task MedalChunkReadsTheTipAndAllMedalParameters()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write("Tip");
            writer.Write(60000);
            writer.Write(50000);
            writer.Write(40000);
            writer.Write(-1);
            writer.Write(120000);
            writer.Write(12345);
            writer.Write(0x12345678);
        }

        Chunk<CGameCtnChallengeParameters> chunk = new CGameCtnChallengeParameters.Chunk0305B00A();
        await Assert.That(chunk is ISkippableChunk).IsTrue();
        var restored = await ReadAndRewrite(stream, chunk);

        await Assert.That(restored.Tip).IsEqualTo("Tip");
        await Assert.That(restored.BronzeTime?.TotalMilliseconds).IsEqualTo(60000);
        await Assert.That(restored.SilverTime?.TotalMilliseconds).IsEqualTo(50000);
        await Assert.That(restored.GoldTime?.TotalMilliseconds).IsEqualTo(40000);
        await Assert.That(restored.AuthorTime).IsNull();
        await Assert.That(restored.TimeLimit.TotalMilliseconds).IsEqualTo(120000);
        await Assert.That(restored.AuthorScore).IsEqualTo(12345);
    }

    [Test]
    public async Task LegacyTimeLimitUsesTheSamePropertyAsStunts()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(90000);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnChallengeParameters.Chunk0305B007());
        await Assert.That(restored.TimeLimit.TotalMilliseconds).IsEqualTo(90000);
    }

    [Test]
    [Arguments(0, 8)]
    [Arguments(2, 16)]
    [Arguments(3, 6)]
    [Arguments(4, 5)]
    [Arguments(5, 3)]
    [Arguments(6, 3)]
    public async Task DiscardedLegacyDataIsPreservedForRewriting(int offset, int count)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            if (offset == 6) writer.Write(count);
            for (var i = 0; i < count; i++) writer.Write(0x3F800001 + i);
            writer.Write(0x12345678);
        }

        Chunk<CGameCtnChallengeParameters> chunk = offset switch
        {
            0 => new CGameCtnChallengeParameters.Chunk0305B000(),
            2 => new CGameCtnChallengeParameters.Chunk0305B002(),
            3 => new CGameCtnChallengeParameters.Chunk0305B003(),
            4 => new CGameCtnChallengeParameters.Chunk0305B004(),
            5 => new CGameCtnChallengeParameters.Chunk0305B005(),
            6 => new CGameCtnChallengeParameters.Chunk0305B006(),
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };
        await ReadAndRewrite(stream, chunk);
    }

    [Test]
    public async Task LegacyGhostArrayConsumesTheDeprecatedVersionPrefix()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(10);
            writer.Write(2);
            writer.WriteNodeRef(new CGameCtnGhost());
            writer.WriteNodeRef<CGameCtnGhost>(null);
            writer.Write(0x12345678);
        }

        var restored = await ReadAndRewrite(stream, new CGameCtnChallengeParameters.Chunk0305B00B());
        await Assert.That(restored.LegacyValidateGhosts!.Length).IsEqualTo(2);
        await Assert.That(restored.LegacyValidateGhosts[0]).IsNotNull();
        await Assert.That(restored.LegacyValidateGhosts[1]).IsNull();
    }

    [Test]
    public async Task LegacyMapTypeChunkConsumesItsVersion()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(0);
            writer.Write("TrackMania\\TM_Race");
            writer.Write("TimeAttack");
            writer.Write(0x12345678);
        }

        var chunk = new CGameCtnChallengeParameters.Chunk0305B00C();
        var restored = await ReadAndRewrite(stream, chunk);
        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(restored.MapType).IsEqualTo("TrackMania\\TM_Race");
        await Assert.That(restored.MapStyle).IsEqualTo("TimeAttack");
    }

    [Test]
    [Arguments("TMF", "Challenge")]
    [Arguments("MP3", "Map")]
    [Arguments("MP4", "Map")]
    [Arguments("TM2020", "Map")]
    public async Task MapFixturesPreserveParametersWhenSaved(string game, string extension)
    {
        var path = TestFiles.Gbx("CGameCtnChallenge", $"GBX-NET 2 CGameCtnChallenge {game} 001.{extension}.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false };
        var map = Gbx.ParseNode<CGameCtnChallenge>(path, settings);
        await Assert.That(map.ChallengeParameters).IsNotNull();

        using var stream = new MemoryStream();
        map.Save(stream);
        stream.Position = 0;
        var restored = Gbx.ParseNode<CGameCtnChallenge>(stream, settings);
        var original = map.ChallengeParameters!;
        var rewritten = restored.ChallengeParameters!;

        await Assert.That(rewritten.BronzeTime).IsEqualTo(original.BronzeTime);
        await Assert.That(rewritten.SilverTime).IsEqualTo(original.SilverTime);
        await Assert.That(rewritten.GoldTime).IsEqualTo(original.GoldTime);
        await Assert.That(rewritten.AuthorTime).IsEqualTo(original.AuthorTime);
        await Assert.That(rewritten.AuthorScore).IsEqualTo(original.AuthorScore);
        await Assert.That(rewritten.TimeLimit).IsEqualTo(original.TimeLimit);
        await Assert.That(rewritten.Tip).IsEqualTo(original.Tip);
        await Assert.That(rewritten.MapType).IsEqualTo(original.MapType);
        await Assert.That(rewritten.MapStyle).IsEqualTo(original.MapStyle);
        await Assert.That(rewritten.IsValidatedForScriptModes).IsEqualTo(original.IsValidatedForScriptModes);
    }

    private static async Task<CGameCtnChallengeParameters> ReadAndRewrite(
        MemoryStream stream, Chunk<CGameCtnChallengeParameters> chunk)
    {
        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var restored = new CGameCtnChallengeParameters();
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
