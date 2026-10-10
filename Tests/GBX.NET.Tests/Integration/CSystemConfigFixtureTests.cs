using GBX.NET.Engines.System;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class CSystemConfigFixtureTests
{
    [Test]
    [Arguments("TMNESWC")]
    [Arguments("TMF")]
    [Arguments("MP3")]
    [Arguments("MP4")]
    public async Task ParseSaveParse_SystemConfigFixture_PreservesSettingsAndVersions(string game)
    {
        var path = TestFiles.Gbx("CSystemConfig", $"CSystemConfig {game} 001.SystemConfig.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CSystemConfig>(path, settings);
        await Assert.That(original.Body.Exception).IsNull();
        if (game is "MP3" or "MP4")
        {
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505B>()!.Version).IsEqualTo(1);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505C>()!.Version).IsEqualTo(0);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505D>()!.Version).IsEqualTo(0);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505E>()!.Version).IsEqualTo(1);
        }

        if (game == "TMNESWC")
        {
            await Assert.That(original.Header.Basic.Format).IsEqualTo(GbxFormat.Text);
            await Assert.That(original.Node.Display).IsNotNull();
            await Assert.That(original.Node.Display!.DisableColorWMask).IsFalse();
            await Assert.That(original.Node.Display.ScreenSizeFS).IsEqualTo(new Int2(1920, 1080));
            await Assert.That(original.Node.Display.IgnoreDriverCrashes).IsTrue();
            await Assert.That(original.Node.Display.GeomLodScaleZ).IsEqualTo(1.0f);
        }

        if (game == "MP3")
        {
            await Assert.That(original.Node.Display!.DisableColorWMask).IsTrue();
        }

        original.Header.Basic = original.Header.Basic with { Format = GbxFormat.Binary };
        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse<CSystemConfig>(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await GbxAssert.HaveEqualSerializedData(original, restored);
        using var secondSave = new MemoryStream();
        restored.Save(secondSave);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }
}
