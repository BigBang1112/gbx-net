using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class CGameCtnBlockFixtureTests
{
    public static IEnumerable<string> Fixtures() => TestFiles.GbxFixtures()
        .Where(path => path.StartsWith("CGameCtnChallenge/", StringComparison.Ordinal));

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task MapRoundTrip_PreservesEveryBlockFlagWord(string filePath)
    {
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CGameCtnChallenge>(TestFiles.Gbx(filePath), settings);
        await Assert.That(original.Body.Exception).IsNull();
        var flags = GetBlockFlags(original.Node);
        await Assert.That(flags.Length).IsGreaterThan(0);
        original.ClassIdRemapMode = ClassIdRemapMode.Latest;
        using var saved = new MemoryStream();

        original.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse<CGameCtnChallenge>(saved, settings);

        await Assert.That(restored.Body.Exception).IsNull();
        await Assert.That(GetBlockFlags(original.Node).SequenceEqual(flags)).IsTrue();
        await Assert.That(GetBlockFlags(restored.Node).SequenceEqual(flags)).IsTrue();
    }

    private static int[] GetBlockFlags(CGameCtnChallenge map) => map.GetBlocks()
        .Concat(map.GetBakedBlocks()).Select(block => block.Flags).ToArray();
}
