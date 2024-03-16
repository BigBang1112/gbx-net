using GBX.NET.LZO;

namespace GBX.NET.Tests.Integration;

public class GbxEqualDataTests
{
    public GbxEqualDataTests()
    {
        Gbx.LZO = new MiniLZO();
    }

    /// <summary>
    /// The goal is to test if the Gbx data is equal when parsed from a file and then saved and parsed again.
    /// It does not have to be equal to the original file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("CGameCtnChallenge/GBX-NET 2 CGameCtnChallenge TM10 001.Challenge.Gbx")]
    public void TestGbxEqualData(string filePath)
    {
        var inputGbx = Gbx.Parse(Path.Combine("Files", "Gbx", filePath));
        
        using var savedGbxMs = new MemoryStream();
        inputGbx.Save(savedGbxMs, new() { LeaveOpen = true });

        savedGbxMs.Position = 0;


        var gbxFromSavedGbx = Gbx.Parse(savedGbxMs);

        using var savedGbxAgainMs = new MemoryStream();
        gbxFromSavedGbx.Save(savedGbxAgainMs);

        Assert.Equal(savedGbxMs.ToArray(), savedGbxAgainMs.ToArray());
    }
}
