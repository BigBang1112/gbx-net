namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxCloneTests
{
    [Test]
    [Arguments("CGameCtnChallenge/CGameCtnChallenge TM2020 001.Map.Gbx")]
    [Arguments("CGameCtnMediaClip/CGameCtnMediaClip TM2020 001.Clip.Gbx")]
    public async Task DeepClone_RealFixture_SerializesLikeIndependentOriginal(string filePath)
    {
        // Arrange
        var original = Gbx.Parse(TestFiles.Gbx(filePath));

        // Act
#pragma warning disable GBXNET10001
        var clone = original.DeepClone();
#pragma warning restore GBXNET10001
        using var originalOutput = new MemoryStream();
        using var cloneOutput = new MemoryStream();
        original.Save(originalOutput);
        clone.Save(cloneOutput);
        originalOutput.Position = 0;
        cloneOutput.Position = 0;

        // Assert
        await Assert.That(clone.Node).IsNotSameReferenceAs(original.Node);
        await Assert.That(cloneOutput.ToArray()).IsEquivalentTo(originalOutput.ToArray(), CollectionOrdering.Matching);
        await GbxAssert.HaveEqualSerializedData(Gbx.Parse(originalOutput), Gbx.Parse(cloneOutput));
    }
}
