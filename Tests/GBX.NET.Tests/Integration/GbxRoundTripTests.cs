using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxRoundTripTests
{
    public static IEnumerable<(string FilePath, GbxCompression Compression, bool Async)> Fixtures()
    {
        foreach (var filePath in TestFiles.GbxFixtures())
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
    public async Task ParseSaveParse_RealFixture_PreservesObjectGraph(string filePath, GbxCompression compression, bool async)
    {
        // Arrange
        SkipReadOnlyReplay(filePath);
        using var input = File.OpenRead(TestFiles.Gbx(filePath));
        var original = await Parse(input, async);
        original.BodyCompression = compression;
        using var saved = new MemoryStream();

        // Act
        original.Save(saved);
        saved.Position = 0;
        var reparsed = await Parse(saved, async);

        // Assert
        await Assert.That(reparsed.BodyCompression).IsEqualTo(compression);
        await GbxAssert.HaveEqualSerializedData(original, reparsed);
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task Save_AfterRealFixtureRoundTrip_ProducesStableBytes(string filePath, GbxCompression compression, bool async)
    {
        // Arrange
        SkipReadOnlyReplay(filePath);
        using var input = File.OpenRead(TestFiles.Gbx(filePath));
        var original = await Parse(input, async);
        original.BodyCompression = compression;
        using var firstSave = new MemoryStream();
        original.Save(firstSave);
        firstSave.Position = 0;
        var reparsed = await Parse(firstSave, async);
        using var secondSave = new MemoryStream();

        // Act
        reparsed.Save(secondSave);

        // Assert
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(firstSave.ToArray(), CollectionOrdering.Matching);
    }

    private static async Task<Gbx> Parse(Stream stream, bool async)
    {
        var settings = new GbxReadSettings
        {
            Logger = new TestLogger(),
            SafeSkippableChunks = false,
            IgnoreExceptionsInBody = false
        };

        var gbx = async ? await Gbx.ParseAsync(stream, settings) : Gbx.Parse(stream, settings);
        await Assert.That(gbx.Node).IsNotNull();
        await Assert.That(gbx.Body.Exception).IsNull();
        return gbx;
    }

    private static void SkipReadOnlyReplay(string filePath)
    {
        if (filePath.StartsWith("CGameCtnReplayRecord/", StringComparison.Ordinal)
            && !new CGameCtnReplayRecord().IsWriteSupported)
        {
            Skip.Test("CGameCtnReplayRecord writing is disabled. Replay fixtures retain parsing coverage in GbxFixtureTests.");
        }
    }
}
