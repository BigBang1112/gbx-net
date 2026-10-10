using GBX.NET.Engines.Game;
using GBX.NET.Engines.Hms;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxFixtureTests
{
    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task ParseSaveParse_MP4Crystal_PreservesEditableGeometry(GbxCompression compression, bool async)
    {
        var path = TestFiles.Gbx("CPlugCrystal", "CPlugCrystal MP4 001.Crystal.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = async ? await Gbx.ParseAsync<CPlugCrystal>(path, settings) : Gbx.Parse<CPlugCrystal>(path, settings);
        original.BodyCompression = compression;
        var geometry = original.Node.Layers.OfType<CPlugCrystal.GeometryLayer>().First().Crystal!;

        await Assert.That(original.Body.Exception).IsNull();
        await Assert.That(geometry.IsEmbeddedCrystal).IsFalse();
        await Assert.That(geometry.Positions).IsNotEmpty();
        await Assert.That(geometry.Faces).IsNotEmpty();
        // Saving may update serialization caches, so keep an independent expected graph.
        var expected = Gbx.Parse<CPlugCrystal>(path, settings);
        expected.BodyCompression = compression;

        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = async ? await Gbx.ParseAsync<CPlugCrystal>(saved, settings) : Gbx.Parse<CPlugCrystal>(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await GbxAssert.HaveEqualSerializedData(expected, restored);
        await Assert.That(restored.BodyCompression).IsEqualTo(compression);

        using var secondSave = new MemoryStream();
        restored.Save(secondSave);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Save_MP4Crystal_PreservesEditedPositionsAndTexCoords()
    {
        var path = TestFiles.Gbx("CPlugCrystal", "CPlugCrystal MP4 001.Crystal.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CPlugCrystal>(path, settings);
        var geometry = original.Node.Layers.OfType<CPlugCrystal.GeometryLayer>().First().Crystal!;
        geometry.Positions[0] = new Vec3(12, 34, 56);
        var face = geometry.Faces[0];
        face.Vertices[0] = face.Vertices[0] with { TexCoord = new Vec2(0.25f, 0.75f) };

        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse<CPlugCrystal>(saved, settings);
        var restoredGeometry = restored.Node.Layers.OfType<CPlugCrystal.GeometryLayer>().First().Crystal!;
        await Assert.That(restoredGeometry.Positions[0]).IsEqualTo(new Vec3(12, 34, 56));
        await Assert.That(restoredGeometry.Faces[0].Vertices[0].TexCoord).IsEqualTo(new Vec2(0.25f, 0.75f));
    }

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
