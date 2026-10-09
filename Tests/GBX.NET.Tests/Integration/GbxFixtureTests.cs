using GBX.NET.Engines.Game;
using GBX.NET.Engines.Hms;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxFixtureTests
{
    [Test]
    [Arguments("TMF", "Challenge")]
    [Arguments("MP3", "Map")]
    [Arguments("MP4", "Map")]
    [Arguments("TM2020", "Map")]
    public async Task ParseSaveParse_MapFixture_PreservesChallengeParameters(string game, string extension)
    {
        // Arrange
        var path = TestFiles.Gbx("CGameCtnChallenge", $"CGameCtnChallenge {game} 001.{extension}.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false };
        var map = Gbx.ParseNode<CGameCtnChallenge>(path, settings);
        using var stream = new MemoryStream();

        // Act
        map.Save(stream);
        stream.Position = 0;
        var restored = Gbx.ParseNode<CGameCtnChallenge>(stream, settings);

        // Assert
        await Assert.That(map.ChallengeParameters).IsNotNull();
        await GbxAssert.AreDeeplyEqual(map.ChallengeParameters, restored.ChallengeParameters);
    }

    [Test]
    public async Task Parse_TM2020Replay_ReturnsCompleteGhostDataSynchronouslyAndAsynchronously()
    {
        // Arrange
        var path = TestFiles.Gbx("CGameCtnReplayRecord", "CGameCtnReplayRecord TM2020 001.Replay.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        using var synchronousInput = File.OpenRead(path);
        using var asynchronousInput = File.OpenRead(path);

        // Act
        var synchronous = Gbx.Parse<CGameCtnReplayRecord>(synchronousInput, settings);
        var asynchronous = await Gbx.ParseAsync<CGameCtnReplayRecord>(asynchronousInput, settings);

        // Assert
        await Assert.That(synchronous.Body.Exception).IsNull();
        await Assert.That(asynchronous.Body.Exception).IsNull();
        await Assert.That(synchronous.Node.MapInfo).IsNotNull();
        await Assert.That(synchronous.Node.PlayerNickname).IsNotNull();
        var ghost = synchronous.Node.GetGhosts().First();
        await Assert.That(ghost.Chunks.Get<CGameCtnGhost.Chunk03092000>()!.U01).IsEqualTo(2);
        await GbxAssert.HaveEqualSerializedData(synchronous, asynchronous);
    }

    [Test]
    [Arguments("TM10")]
    [Arguments("TMPU")]
    [Arguments("TMSX")]
    [Arguments("TMNESWC")]
    [Arguments("TMU")]
    [Arguments("TMF")]
    [Arguments("MP3")]
    [Arguments("TMT")]
    [Arguments("MP4")]
    [Arguments("TM2020")]
    public async Task Parse_ReplayFixture_ReturnsGhostsWithoutIgnoringChunkFailures(string game)
    {
        var path = TestFiles.Gbx("CGameCtnReplayRecord", $"CGameCtnReplayRecord {game} 001.Replay.Gbx");
        var replay = Gbx.ParseNode<CGameCtnReplayRecord>(path, new GbxReadSettings { SafeSkippableChunks = false });
        await Assert.That(replay.GetGhosts().Any()).IsTrue();
    }

    [Test]
    [Arguments("MP3 001")]
    [Arguments("MP4 001")]
    [Arguments("MP4 002")]
    [Arguments("TM2020 001")]
    public async Task LightmapCache_MapFixture_LoadsEmbeddedCacheAndMapping(string suffix)
    {
        var map = Gbx.ParseNode<GBX.NET.Engines.Game.CGameCtnChallenge>(
            TestFiles.Gbx("CGameCtnChallenge", $"CGameCtnChallenge {suffix}.Map.Gbx"));
        await Assert.That(map.LightmapCacheData).IsNotNull();
        var cache = map.LightmapCache;
        await Assert.That(cache).IsNotNull();
        await Assert.That(cache!.Chunks.Any(c => c.Id == 0x0602201A)).IsTrue();
        var chunk = (CHmsLightMapCache.Chunk0602201A)cache.Chunks.Single(c => c.Id == 0x0602201A);
        if (chunk.Version >= 3) await Assert.That(cache.Frames).IsNotNull();
        await Assert.That(cache.Mapping).IsNotNull();
        _ = cache.Mapping!.ZlibData2Decompressed1;
    }

    [Test]
    public async Task Parse_MP4Replay_ReturnsActionModelsAndItemNames()
    {
        var path = TestFiles.Gbx("CGameCtnReplayRecord", "CGameCtnReplayRecord MP4 001.Replay.Gbx");

        var replay = Gbx.ParseNode<CGameCtnReplayRecord>(path);

        await Assert.That(replay.ActionModels).IsNotNull();
        await Assert.That(replay.ItemNames).IsNotNull();
    }
}
