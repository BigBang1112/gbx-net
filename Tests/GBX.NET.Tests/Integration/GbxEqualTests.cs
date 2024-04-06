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
    public async Task TestGbxEqualData(string filePath)
    {
        using var logger = output.BuildLogger();

        var inputGbx = await Gbx.ParseAsync(Path.Combine("Files", "Gbx", filePath), new() { Logger = logger });
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs, new() { LeaveOpen = true });

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
    public async Task TestGbxEqualObjects(string filePath)
    {
        using var logger = output.BuildLogger();

        using var inputGbxMs = new MemoryStream();

        await Gbx.DecompressAsync(Path.Combine("Files", "Gbx", filePath), inputGbxMs, leaveOpen: true);

        inputGbxMs.Position = 0;

        var inputGbx = await Gbx.ParseAsync(inputGbxMs, new() { Logger = logger });

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs, new() { LeaveOpen = true });

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs, new() { Logger = logger });

        inputGbx.FilePath = null;

        inputGbx.ShouldCompare(gbxFromSavedGbx, compareConfig: new() { MaxDifferences = 10 });
    }
}
