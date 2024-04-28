using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using KellermanSoftware.CompareNetObjects;
using Xunit.Abstractions;

namespace GBX.NET.Tests.Integration;

public class GbxEqualTests
{
    private readonly ITestOutputHelper output;

    public GbxEqualTests(ITestOutputHelper output)
    {
        this.output = output;

        Gbx.LZO = new MiniLZO();
        Gbx.StrictBooleans = true;
    }

    /// <summary>
    /// The goal is to test if the Gbx data is equal when parsed from a file and then saved and parsed again.
    /// It does not have to be equal to the original file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    [InlineData("CPlugPrefab/GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    [InlineData("CPlugTrainWagonModel/GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    [InlineData("CGameCtnBlockInfoClassic/GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualDataImplicit(string filePath)
    {
        using var logger = output.BuildLogger();

        using var fs = new FileStream(Path.Combine("Files", "Gbx", filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var inputGbx = Gbx.Parse(fs, new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }

    [Theory]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    [InlineData("CPlugPrefab/GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    [InlineData("CPlugTrainWagonModel/GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    [InlineData("CGameCtnBlockInfoClassic/GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualObjectsImplicit(string filePath)
    {
        using var logger = output.BuildLogger();

        using var inputGbxMs = new MemoryStream();

        using var fs = new FileStream(Path.Combine("Files", "Gbx", filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        Gbx.Decompress(fs, inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = Gbx.Parse(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        inputGbx.ShouldCompare(gbxFromSavedGbx, compareConfig: new() { MaxDifferences = 10 });
    }

    [Theory]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    [InlineData("CPlugPrefab/GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    [InlineData("CPlugTrainWagonModel/GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    [InlineData("CGameCtnBlockInfoClassic/GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualDataImplicitAsync(string filePath)
    {
        using var logger = output.BuildLogger();

        var inputGbx = await Gbx.ParseAsync(Path.Combine("Files", "Gbx", filePath), new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }

    [Theory]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("CPlugSolid/GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("CPlugSolid2Model/GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    [InlineData("CPlugPrefab/GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    [InlineData("CPlugTrainWagonModel/GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    [InlineData("CGameCtnBlockInfoClassic/GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualObjectsImplicitAsync(string filePath)
    {
        using var logger = output.BuildLogger();

        using var inputGbxMs = new MemoryStream();

        await Gbx.DecompressAsync(Path.Combine("Files", "Gbx", filePath), inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = await Gbx.ParseAsync(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        inputGbx.ShouldCompare(gbxFromSavedGbx, compareConfig: new() { MaxDifferences = 10 });
    }


    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnChallenge(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnChallenge>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    public void TestGbxEqualDataExplicitCGameItemModel(string filePath)
    {
        TestGbxEqualDataExplicit<CGameItemModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    public void TestGbxEqualDataExplicitCPlugSolid(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugSolid>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    public void TestGbxEqualDataExplicitCPlugSolid2Model(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugSolid2Model>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    public void TestGbxEqualDataExplicitCPlugPrefab(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugPrefab>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    public void TestGbxEqualDataExplicitCPlugTrainWagonModel(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugTrainWagonModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnBlockInfoClassic(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnBlockInfoClassic>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnMacroBlockInfo(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnChallenge(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnChallenge>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameItemModel(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameItemModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    public void TestGbxEqualObjectsExplicitCPlugSolid(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugSolid>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    public void TestGbxEqualObjectsExplicitCPlugSolid2Model(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugSolid2Model>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    public void TestGbxEqualObjectsExplicitCPlugPrefab(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugPrefab>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    public void TestGbxEqualObjectsExplicitCPlugTrainWagonModel(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugTrainWagonModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnBlockInfoClassic(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnBlockInfoClassic>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnMacroBlockInfo(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnMacroBlockInfo>(filePath);
    }


    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnChallengeAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnChallenge>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameItemModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameItemModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    public async Task TestGbxEqualDataExplicitCPlugSolidAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugSolid>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    public async Task TestGbxEqualDataExplicitCPlugSolid2ModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugSolid2Model>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    public async Task TestGbxEqualDataExplicitCPlugPrefabAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugPrefab>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    public async Task TestGbxEqualDataExplicitCPlugTrainWagonModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugTrainWagonModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnBlockInfoClassicAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnBlockInfoClassic>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnMacroBlockInfoAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnChallengeAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnChallenge>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameItemModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameItemModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid TM10 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMSX 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMNESWC 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMF 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid TMT 001.Solid.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid MP4 001.Solid.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCPlugSolidAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugSolid>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugSolid2Model TMT 001.Solid2.Gbx")]
    [InlineData("GBX-NET 2 CPlugSolid2Model MP4 001.Mesh.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCPlugSolid2ModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugSolid2Model>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugPrefab TM2020 001.Prefab.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCPlugPrefabAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugPrefab>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CPlugTrainWagonModel MP4 001.Wagon.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCPlugTrainWagonModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugTrainWagonModel>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnBlockInfo TMF 001.TMEDClassic.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnBlockInfoClassicAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnBlockInfoClassic>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnMacroBlockInfoAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnMacroBlockInfo>(filePath);
    }

    private void TestGbxEqualDataExplicit<T>(string filePath) where T : CMwNod, new()
    {
        using var logger = output.BuildLogger();

        using var fs = new FileStream(Path.Combine("Files", "Gbx", typeof(T).Name, filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var inputGbx = Gbx.Parse<T>(fs, new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse<T>(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }

    private void TestGbxEqualObjectsExplicit<T>(string filePath) where T : CMwNod, new()
    {
        using var logger = output.BuildLogger();

        using var inputGbxMs = new MemoryStream();

        using var fs = new FileStream(Path.Combine("Files", "Gbx", typeof(T).Name, filePath), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        Gbx.Decompress(fs, inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = Gbx.Parse<T>(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = Gbx.Parse<T>(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        inputGbx.ShouldCompare(gbxFromSavedGbx, compareConfig: new() { MaxDifferences = 10 });
    }

    private async Task TestGbxEqualDataExplicitAsync<T>(string filePath) where T : CMwNod, new()
    {
        using var logger = output.BuildLogger();

        var inputGbx = await Gbx.ParseAsync<T>(Path.Combine("Files", "Gbx", typeof(T).Name, filePath), new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync<T>(savedGbxMs, new() { Logger = logger });

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }

    private async Task TestGbxEqualObjectsExplicitAsync<T>(string filePath) where T : CMwNod, new()
    {
        using var logger = output.BuildLogger();

        using var inputGbxMs = new MemoryStream();

        await Gbx.DecompressAsync(Path.Combine("Files", "Gbx", typeof(T).Name, filePath), inputGbxMs);

        inputGbxMs.Position = 0;

        var inputGbx = await Gbx.ParseAsync<T>(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs);

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync<T>(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        inputGbx.ShouldCompare(gbxFromSavedGbx, compareConfig: new() { MaxDifferences = 10 });
    }
}
