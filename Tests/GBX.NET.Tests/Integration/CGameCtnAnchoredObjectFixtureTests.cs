using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class CGameCtnAnchoredObjectFixtureTests
{
    [Test]
    [Arguments("MP3")]
    [Arguments("TMT")]
    [Arguments("MP4")]
    [Arguments("TM2020")]
    public async Task MapItems_PreserveDataAfterSavingAndReparsing(string game)
    {
        var settings = new GbxReadSettings { SafeSkippableChunks = false };
        var map = Gbx.ParseNode<CGameCtnChallenge>(TestFiles.Gbx("CGameCtnChallenge", $"CGameCtnChallenge {game} 001.Map.Gbx"), settings);
        using var stream = new MemoryStream();
        map.Save(stream);
        stream.Position = 0;
        var restored = Gbx.ParseNode<CGameCtnChallenge>(stream, settings);
        var originalItems = map.GetAnchoredObjects().ToArray();
        var restoredItems = restored.GetAnchoredObjects().ToArray();
        await Assert.That(restoredItems.Length).IsEqualTo(originalItems.Length);
        for (var i = 0; i < originalItems.Length; i++)
        {
            await GbxAssert.AreDeeplyEqual(originalItems[i], restoredItems[i]);
        }
    }
}
