using GBX.NET.Engines.Game;
using System.Text.RegularExpressions;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public partial class GbxGameVersionFixtureTests
{
    public static IEnumerable<(string FilePath, GameVersion Game)> Fixtures()
    {
        foreach (var filePath in TestFiles.GbxFixtures())
        {
            var match = FixtureName().Match(Path.GetFileName(filePath));
            if (match.Success && Enum.TryParse<GameVersion>(match.Groups["game"].Value, out var game))
            {
                yield return (filePath, game);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task GameVersionIncludesTheGameFromTheFileName(string filePath, GameVersion game)
    {
        // Arrange
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };

        // Act
        var gbx = Gbx.Parse(TestFiles.Gbx(filePath), settings);

        // Assert
        await Assert.That(gbx.Node).IsNotNull();
        await Assert.That(gbx.Node!.GameVersion).HasFlag(game);
    }

    [Test]
    [Arguments("CGameCtnGhost MP3 001.Ghost.Gbx", GameVersion.MP3)]
    [Arguments("CGameCtnGhost MP4 001.Ghost.Gbx", GameVersion.MP4)]
    [Arguments("CGameCtnGhost TM2020 001.Ghost.Gbx", GameVersion.TM2020)]
    public async Task GameVersionDifferentiatesGhostsByChunkVersions(string fileName, GameVersion game)
    {
        var gbx = Gbx.Parse(TestFiles.Gbx("CGameCtnGhost", fileName));

        await Assert.That(gbx.Node!.GameVersion).IsEqualTo(game);
    }

    [Test]
    [Arguments("CGameCtnChallenge MP3 001.Map.Gbx", GameVersion.MP3)]
    [Arguments("CGameCtnChallenge TMT 001.Map.Gbx", GameVersion.TMT)]
    public async Task GameVersionDifferentiatesMapsByTitle(string fileName, GameVersion game)
    {
        var gbx = Gbx.Parse(TestFiles.Gbx("CGameCtnChallenge", fileName));

        await Assert.That(gbx.Node!.GameVersion).IsEqualTo(game);
    }

    [Test]
    public async Task GameVersionUsesTheTurboTitleWhenChunksFitBothGames()
    {
        var gbx = Gbx.Parse<CGameCtnChallenge>(TestFiles.Gbx("CGameCtnChallenge", "CGameCtnChallenge MP3 001.Map.Gbx"));
        gbx.Node.TitleId = "TMTurbo@nadeolabs";

        await Assert.That(gbx.Node.GameVersion).IsEqualTo(GameVersion.TMT);
    }

    [GeneratedRegex(@"^\S+ (?<game>[A-Z][A-Z0-9]+) \d+\.")]
    private static partial Regex FixtureName();
}
