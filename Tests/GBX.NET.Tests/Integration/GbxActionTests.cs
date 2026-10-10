using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Engines.Script;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxActionTests
{
    [Test]
    [Arguments("MP3", GbxCompression.Uncompressed, false)]
    [Arguments("MP3", GbxCompression.Uncompressed, true)]
    [Arguments("MP3", GbxCompression.Compressed, false)]
    [Arguments("MP3", GbxCompression.Compressed, true)]
    [Arguments("MP4", GbxCompression.Uncompressed, false)]
    [Arguments("MP4", GbxCompression.Uncompressed, true)]
    [Arguments("MP4", GbxCompression.Compressed, false)]
    [Arguments("MP4", GbxCompression.Compressed, true)]
    public async Task ParseSaveParse_ActionFixture_PreservesActionData(string game, GbxCompression compression, bool async)
    {
        var path = TestFiles.Gbx("CGameActionModel", $"CGameActionModel {game} 001.Action.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = async ? await Gbx.ParseAsync<CGameActionModel>(path, settings) : Gbx.Parse<CGameActionModel>(path, settings);
        original.BodyCompression = compression;

        await Assert.That(original.Body.Exception).IsNull();
        await Assert.That(original.Node.Anim).IsNotNull();
        await Assert.That(original.Node.Chunks.Get<CGameActionModel.Chunk2E008000>()!.Version).IsEqualTo(game == "MP3" ? 18 : 31);
        using var saved = new MemoryStream();
        original.Save(saved);
        saved.Position = 0;
        var restored = async ? await Gbx.ParseAsync<CGameActionModel>(saved, settings) : Gbx.Parse<CGameActionModel>(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await GbxAssert.HaveEqualSerializedData(original, restored);
        await Assert.That(restored.BodyCompression).IsEqualTo(compression);

        using var secondSave = new MemoryStream();
        restored.Save(secondSave);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(saved.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("MP3")]
    [Arguments("MP4")]
    public async Task EditAndClone_ActionFixture_PreservesAnimationAndScriptChanges(string game)
    {
        var path = TestFiles.Gbx("CGameActionModel", $"CGameActionModel {game} 001.Action.Gbx");
        var settings = new GbxReadSettings { SafeSkippableChunks = false, IgnoreExceptionsInBody = false };
        var original = Gbx.Parse<CGameActionModel>(path, settings);
#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        await GbxAssert.HaveEqualSerializedData(original, clone);
        await Assert.That(clone.Node.Anim).IsNotSameReferenceAs(original.Node.Anim);

        clone.Node.ActionName = "EditedAction";
        clone.Node.Description = "Edited description";
        if (game == "MP4") clone.Node.Name = "Edited display name";
        var anim = clone.Node.Anim!;
        var edition = game == "MP3" ? anim.EditionClips![0].Edition : anim.Clips![0].Edition!;
        await Assert.That(edition).IsNotNull();
        edition.Duration += 1;
        edition.RootMotion![0] = new CPlugAnimSkelEdition.Curve
        {
            Type = 1,
            Real = new() { Value = 3, Keys = [new() { Time = 0, Value = 3 }, new() { Time = 1, Value = 4 }] }
        };
        if (game == "MP3") anim.LegacyClips![0].Name = "EditedClip";
        else anim.Clips![0].Name = "EditedClip";

        clone.Node.Script ??= new CPlugScriptWithSettings();
        clone.Node.Script.CreateChunk<CPlugScriptWithSettings.Chunk09083000>();
        clone.Node.Script.Script ??= new CPlugFileTextScript();
        clone.Node.Script.Script.Text += "\n// Edited script";
        clone.Node.Script.Settings = [new CScriptSetting { Name = "EditedSetting", Type = 1, Integer = 42 }];

        using var saved = new MemoryStream();
        clone.Save(saved);
        saved.Position = 0;
        var restored = Gbx.Parse<CGameActionModel>(saved, settings);
        await Assert.That(restored.Body.Exception).IsNull();
        await GbxAssert.AreDeeplyEqual(clone.Node, restored.Node);
        await Assert.That(restored.Node.ActionName).IsEqualTo("EditedAction");
        await Assert.That(restored.Node.Description).IsEqualTo("Edited description");
        if (game == "MP4") await Assert.That(restored.Node.Name).IsEqualTo("Edited display name");
        await Assert.That(restored.Node.Script!.Settings![0].Integer).IsEqualTo(42);
        await Assert.That(original.Node.ActionName).IsNotEqualTo("EditedAction");
    }
}
