using GBX.NET.Engines.Game;
using GBX.NET.Engines.Script;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxPlayerDataFixtureTests
{
    public static IEnumerable<string> FixtureFiles()
    {
        foreach (var game in new[] { "TM10", "TMNESWC", "TMF", "MP3", "MP4" })
        {
            yield return $"CGamePlayerProfile/CGamePlayerProfile {game} 001.Profile.Gbx";
        }

        foreach (var game in new[] { "TMNESWC", "TMF" })
        {
            yield return $"CGamePlayerScores/CGamePlayerScores {game} 001.Scores.Gbx";
        }

        foreach (var game in new[] { "MP3", "MP4" })
        {
            yield return $"CScriptTraitsPersistent/CScriptTraitsPersistent {game} 001.ScriptCache.Gbx";
        }
    }

    public static IEnumerable<(string FilePath, GbxCompression Compression, bool Async)> Fixtures()
    {
        foreach (var filePath in FixtureFiles())
        {
            foreach (var compression in new[] { GbxCompression.Uncompressed, GbxCompression.Compressed })
            {
                yield return (filePath, compression, false);
                yield return (filePath, compression, true);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task PlayerDataFixturesPreserveContentAndStableBytesAfterSavingAndReparsing(string filePath, GbxCompression compression, bool async)
    {
        var path = TestFiles.Gbx(filePath);
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = async ? await Gbx.ParseAsync(path, settings) : Gbx.Parse(path, settings);
        await Assert.That(original.Body.Exception).IsNull();
        await AssertFixtureContent(original, filePath);
        original.BodyCompression = compression;
        original.ClassIdRemapMode = ClassIdRemapMode.Latest;

        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = async ? await Gbx.ParseAsync(saved, settings) : Gbx.Parse(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await Assert.That(restored.BodyCompression).IsEqualTo(compression);
        await GbxAssert.HaveEqualSerializedData(original, restored);

        using var secondSave = new MemoryStream();
        restored.Save(secondSave);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [MethodDataSource(nameof(FixtureFiles))]
    public async Task PlayerDataFixturesPreserveEditsAfterSaving(string filePath)
    {
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse(TestFiles.Gbx(filePath), settings);
        switch (original.Node)
        {
            case CGamePlayerProfile profile:
                profile.ProfileName = "Integration test profile";
                break;
            case CGamePlayerScore scores:
                scores.PlayerName = "Integration test player";
                if (scores.Scores is { Length: > 0 })
                {
                    scores.Scores[0].PersonalBest = new(12345);
                }
                else
                {
                    scores.SoloRaceTimeSeconds = 12345;
                }
                break;
            case CScriptTraitsPersistent cache:
                cache.PersistentTraits.Add("IntegrationTest", new()
                {
                    ["Count"] = new CScriptTraitsMetadata.ScriptTrait<int>(
                        new CScriptTraitsMetadata.ScriptType(CScriptTraitsMetadata.EScriptType.Integer), 42)
                });
                break;
            default:
                throw new InvalidDataException($"Unexpected player data class in {filePath}.");
        }

        using var saved = new MemoryStream();
        original.ClassIdRemapMode = ClassIdRemapMode.Latest;
        original.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await GbxAssert.HaveEqualSerializedData(original, restored);
    }

    private static async Task AssertFixtureContent(Gbx gbx, string filePath)
    {
        if (filePath.StartsWith("CGamePlayerProfile/", StringComparison.Ordinal))
        {
            await Assert.That(gbx.Node).IsTypeOf<CGamePlayerProfile>();
            var profile = (CGamePlayerProfile)gbx.Node!;
            await Assert.That(profile.ProfileName).IsNotNullOrEmpty();
            await Assert.That(profile.ProfileId).IsNotNullOrEmpty();
            if (filePath.Contains(" MP3 ") || filePath.Contains(" MP4 "))
            {
                await Assert.That(profile.ProfileChunks).IsNotEmpty();
            }
        }
        else if (filePath.StartsWith("CGamePlayerScores/", StringComparison.Ordinal))
        {
            await Assert.That(gbx.Node).IsTypeOf<CGamePlayerScore>();
            var scores = (CGamePlayerScore)gbx.Node!;
            await Assert.That(scores.PlayerName).IsNotNullOrEmpty();
            await Assert.That(scores.Scores).IsNotNull();
            if (filePath.Contains(" TMNESWC "))
            {
                await Assert.That(scores.Scores).IsNotEmpty();
            }
            else
            {
                await Assert.That(scores.Chunks.Get<CGamePlayerScore.Chunk0308D011>()).IsNotNull();
            }
        }
        else
        {
            await Assert.That(gbx.Node).IsTypeOf<CScriptTraitsPersistent>();
            var cache = (CScriptTraitsPersistent)gbx.Node!;
            await Assert.That(cache.PersistentTraits).IsNotEmpty();
            await Assert.That(cache.PersistentTraits.Values.Any(traits => traits.Count > 0)).IsTrue();
        }
    }
}
