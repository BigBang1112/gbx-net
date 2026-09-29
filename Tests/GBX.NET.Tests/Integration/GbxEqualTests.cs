using KellermanSoftware.CompareNetObjects;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxEqualTests
{
    public static IEnumerable<string> Fixtures() =>
    [
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx",
        "CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel MP4 003.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx",
        "CGameItemModel/GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx",
        "CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx",
        "CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx",
        "CSystemConfig/GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx",
        "CSystemConfig/GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx",
        "CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx",
        "CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx",
        "CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx",
        "CGameCtnGhost/GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx",
        "CGameCtnGhost/GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx",
    ];

    /// <summary>
    /// The goal is to test if the Gbx data is equal when parsed from a file and then saved and parsed again.
    /// It does not have to be equal to the original file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task TestGbxEqualDataImplicit(string filePath)
    {
        var logger = new TestLogger();

        using var fs = new FileStream(TestFiles.Gbx(filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var inputGbx = Gbx.Parse(fs, new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        await Assert.That(savedGbxAgainMs.ToArray()).IsEquivalentTo(savedGbxMs.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task TestGbxEqualObjectsImplicit(string filePath)
    {
        var logger = new TestLogger();

        using var inputGbxMs = new MemoryStream();

        using var fs = new FileStream(TestFiles.Gbx(filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        Gbx.Decompress(fs, inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = Gbx.Parse(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        var comparison = new CompareLogic(new ComparisonConfig() { MaxDifferences = 10, MembersToIgnore = ["Exception.StackTrace"] })
            .Compare(inputGbx, gbxFromSavedGbx);
        await Assert.That(comparison.AreEqual).IsTrue().Because(comparison.DifferencesString);
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task TestGbxEqualDataImplicitAsync(string filePath)
    {
        var logger = new TestLogger();

        var inputGbx = await Gbx.ParseAsync(TestFiles.Gbx(filePath), new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        await Assert.That(savedGbxAgainMs.ToArray()).IsEquivalentTo(savedGbxMs.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task TestGbxEqualObjectsImplicitAsync(string filePath)
    {
        var logger = new TestLogger();

        using var inputGbxMs = new MemoryStream();

        await Gbx.DecompressAsync(TestFiles.Gbx(filePath), inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = await Gbx.ParseAsync(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        var comparison = new CompareLogic(new ComparisonConfig() { MaxDifferences = 10, MembersToIgnore = ["Exception.StackTrace"] })
            .Compare(inputGbx, gbxFromSavedGbx);
        await Assert.That(comparison.AreEqual).IsTrue().Because(comparison.DifferencesString);
    }
}
