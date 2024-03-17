using GBX.NET.LZO;

namespace GBX.NET.Tests.Integration;

public class GbxEqualDataTests
{
    public GbxEqualDataTests()
    {
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
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMSX 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMNESWC 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TMT 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP3 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge MP4 001.Map.Gbx")]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx")]
    public async Task TestGbxEqualData(string filePath)
    {
        var inputGbx = await Gbx.ParseAsync(Path.Combine("Files", "Gbx", filePath));
        inputGbx.BodyCompression = GbxCompression.Uncompressed;

        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs, new() { LeaveOpen = true });

        savedGbxMs.Position = 0;

        var gbxFromSavedGbx = await Gbx.ParseAsync(savedGbxMs);

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        File.WriteAllBytes("before.gbx", savedGbxMs.ToArray());
        File.WriteAllBytes("after.gbx", savedGbxAgainMs.ToArray());

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }
}
