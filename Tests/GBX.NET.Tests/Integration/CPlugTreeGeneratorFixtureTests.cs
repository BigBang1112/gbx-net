using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class CPlugTreeGeneratorFixtureTests
{
    [Test]
    [Arguments("MP4")]
    [Arguments("TM2020")]
    public async Task CrystalFixturePreservesEditedGeneratorFlags(string game)
    {
        var path = TestFiles.Gbx("CPlugCrystal", $"CPlugCrystal {game} 001.Crystal.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CPlugCrystal>(path, settings);
        await Assert.That(original.Body.Exception).IsNull();
        // The TM2020 fixture omits the optional generator chunk.
        if (!original.Node.Chunks.Any(x => x.Id == 0x09051000))
        {
            original.Node.CreateChunk<CPlugTreeGenerator.Chunk09051000>();
        }

        original.Node.Flags = 0x80000000u;
        original.Node.IsSaveGenerated = true;
        original.Node.IsToKeepInSaveAsRelease = true;
        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse<CPlugCrystal>(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await Assert.That(restored.Node.Flags).IsEqualTo(0x80000003u);

        using var secondSave = new MemoryStream();
        restored.Save(secondSave);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }
}
