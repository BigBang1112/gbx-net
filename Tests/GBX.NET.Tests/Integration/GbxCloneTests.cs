using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxCloneTests
{
    [Test]
    public async Task ClonedMapFixtureSerializesLikeOriginal()
    {
        var path = TestFiles.Gbx("CGameCtnChallenge", "GBX-NET 2 CGameCtnChallenge TM2020 001.Map.Gbx");
        var original = Gbx.Parse<CGameCtnChallenge>(path);

#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        using var originalOutput = new MemoryStream();
        using var cloneOutput = new MemoryStream();

        original.Save(originalOutput);
        clone.Save(cloneOutput);

        await Assert.That(clone.Node).IsNotSameReferenceAs(original.Node);
        await Assert.That(cloneOutput.ToArray()).IsEquivalentTo(originalOutput.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ClonedMediaClipFixtureSerializesLikeOriginal()
    {
        var path = TestFiles.Gbx("CGameCtnMediaClip", "GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx");
        var original = Gbx.Parse<CGameCtnMediaClip>(path);

#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        using var originalOutput = new MemoryStream();
        using var cloneOutput = new MemoryStream();

        original.Save(originalOutput);
        clone.Save(cloneOutput);

        await Assert.That(clone.Node).IsNotSameReferenceAs(original.Node);
        await Assert.That(cloneOutput.ToArray()).IsEquivalentTo(originalOutput.ToArray(), CollectionOrdering.Matching);
    }
}
