using GBX.NET.Engines.System;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class CSystemConfigFixtureTests
{
    [Test]
    [Arguments("TMF")]
    [Arguments("MP3")]
    [Arguments("MP4")]
    public async Task ParseSaveParse_SystemConfigFixture_PreservesSettingsAndVersions(string game)
    {
        var path = TestFiles.Gbx("CSystemConfig", $"CSystemConfig {game} 001.SystemConfig.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CSystemConfig>(path, settings);
        await Assert.That(original.Body.Exception).IsNull();
        if (game != "TMF")
        {
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505B>()!.Version).IsEqualTo(1);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505C>()!.Version).IsEqualTo(0);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505D>()!.Version).IsEqualTo(0);
            await Assert.That(original.Node.Chunks.Get<CSystemConfig.Chunk0B00505E>()!.Version).IsEqualTo(1);
        }

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
