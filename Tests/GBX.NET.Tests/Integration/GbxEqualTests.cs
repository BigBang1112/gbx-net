using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Engines.System;
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

        Gbx.LZO = new Lzo();
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
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
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
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
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
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
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
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 002.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("CGameItemModel/GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("CGameCtnMacroBlockInfo/GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("CSystemConfig/GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("CGameCtnMediaClip/GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("CGameCtnGhost/GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
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
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
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
    [InlineData("GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    public void TestGbxEqualDataExplicitCGameItemModel(string filePath)
    {
        TestGbxEqualDataExplicit<CGameItemModel>(filePath);
    }

    /*[Theory]
    public void TestGbxEqualDataExplicitCPlugSolid(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugSolid>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualDataExplicitCPlugSolid2Model(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugSolid2Model>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualDataExplicitCPlugPrefab(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugPrefab>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualDataExplicitCPlugTrainWagonModel(string filePath)
    {
        TestGbxEqualDataExplicit<CPlugTrainWagonModel>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualDataExplicitCGameCtnBlockInfoClassic(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnBlockInfoClassic>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnMacroBlockInfo(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    public void TestGbxEqualDataExplicitCSystemConfig(string filePath)
    {
        TestGbxEqualDataExplicit<CSystemConfig>(filePath);
    }

    /*[Theory]
    public void TestGbxEqualDataExplicitCGameSpawnModel(string filePath)
    {
        TestGbxEqualDataExplicit<CGameSpawnModel>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnMediaClip(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnMediaClip>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
    public void TestGbxEqualDataExplicitCGameCtnGhost(string filePath)
    {
        TestGbxEqualDataExplicit<CGameCtnGhost>(filePath);
    }


    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
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
    [InlineData("GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameItemModel(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameItemModel>(filePath);
    }

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCPlugSolid(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugSolid>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCPlugSolid2Model(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugSolid2Model>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCPlugPrefab(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugPrefab>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCPlugTrainWagonModel(string filePath)
    {
        TestGbxEqualObjectsExplicit<CPlugTrainWagonModel>(filePath);
    }*/

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCGameCtnBlockInfoClassic(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnBlockInfoClassic>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnMacroBlockInfo(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    public void TestGbxEqualObjectsExplicitCSystemConfig(string filePath)
    {
        TestGbxEqualObjectsExplicit<CSystemConfig>(filePath);
    }

    /*[Theory]
    public void TestGbxEqualObjectsExplicitCGameSpawnModel(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameSpawnModel>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnMediaClip(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnMediaClip>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
    public void TestGbxEqualObjectsExplicitCGameCtnGhost(string filePath)
    {
        TestGbxEqualObjectsExplicit<CGameCtnGhost>(filePath);
    }


    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
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
    [InlineData("GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameItemModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameItemModel>(filePath);
    }

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCPlugSolidAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugSolid>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCPlugSolid2ModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugSolid2Model>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCPlugPrefabAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugPrefab>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCPlugTrainWagonModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CPlugTrainWagonModel>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCGameCtnBlockInfoClassicAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnBlockInfoClassic>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnMacroBlockInfoAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    public async Task TestGbxEqualDataExplicitCSystemConfigAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CSystemConfig>(filePath);
    }

    /*[Theory]
    public async Task TestGbxEqualDataExplicitCGameSpawnModelAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameSpawnModel>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnMediaClipAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnMediaClip>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
    public async Task TestGbxEqualDataExplicitCGameCtnGhostAsync(string filePath)
    {
        await TestGbxEqualDataExplicitAsync<CGameCtnGhost>(filePath);
    }


    [Theory]
    [InlineData("GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMPU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 001.Challenge.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnChallenge TMF 002.Challenge.Gbx")]
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
    [InlineData("GBX-NET 2 CGameItemModel MP4 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 001.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 002.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 003.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 004.Block.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 005.Item.Gbx")]
    [InlineData("GBX-NET 2 CGameItemModel TM2020 006.Item.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameItemModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameItemModel>(filePath);
    }

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCPlugSolidAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugSolid>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCPlugSolid2ModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugSolid2Model>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCPlugPrefabAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugPrefab>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCPlugTrainWagonModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CPlugTrainWagonModel>(filePath);
    }*/

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCGameCtnBlockInfoClassicAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnBlockInfoClassic>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo MP4 001.Macroblock.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMacroBlockInfo TM2020 001.Macroblock.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnMacroBlockInfoAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnMacroBlockInfo>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CSystemConfig TMF 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP3 001.SystemConfig.Gbx")]
    [InlineData("GBX-NET 2 CSystemConfig MP4 001.SystemConfig.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCSystemConfigAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CSystemConfig>(filePath);
    }

    /*[Theory]
    public async Task TestGbxEqualObjectsExplicitCGameSpawnModelAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameSpawnModel>(filePath);
    }*/

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TMF 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip MP4 001.Clip.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnMediaClipAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnMediaClip>(filePath);
    }

    [Theory]
    [InlineData("GBX-NET 2 CGameCtnGhost MP4 001.Ghost.Gbx")]
    [InlineData("GBX-NET 2 CGameCtnGhost TM2020 001.Ghost.Gbx")]
    public async Task TestGbxEqualObjectsExplicitCGameCtnGhostAsync(string filePath)
    {
        await TestGbxEqualObjectsExplicitAsync<CGameCtnGhost>(filePath);
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
