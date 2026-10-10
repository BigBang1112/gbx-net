using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class GbxApiTests
{
    [Test]
    public async Task ParseAsync_PartialNonSeekableReads_MatchesSynchronousParsing()
    {
        // Arrange
        var bytes = CreateClipBytes();
        using var input = new FragmentedReadStream(bytes, maxReadSize: 7);
        using var expectedInput = new MemoryStream(bytes);
        var expected = Gbx.Parse<CGameCtnMediaClip>(expectedInput);

        // Act
        var actual = await Gbx.ParseAsync<CGameCtnMediaClip>(input);

        // Assert
        await GbxAssert.HaveEqualSerializedData(expected, actual);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ParseAsync_CloseStreamSetting_HonorsStreamOwnership(bool closeStream)
    {
        // Arrange
        using var stream = new MemoryStream(CreateClipBytes());

        // Act
        await Gbx.ParseAsync<CGameCtnMediaClip>(stream, new() { CloseStream = closeStream });

        // Assert
        await Assert.That(stream.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Save_CloseStreamSetting_HonorsStreamOwnership(bool closeStream)
    {
        // Arrange
        var gbx = new Gbx<CGameCtnMediaClip>(new CGameCtnMediaClip());
        using var stream = new MemoryStream();

        // Act
        gbx.Save(stream, new() { CloseStream = closeStream });

        // Assert
        await Assert.That(stream.CanWrite).IsEqualTo(!closeStream);
    }

    [Test]
    public async Task ParseHeader_ReadRawBody_PreservesBytesWhenSaved()
    {
        // Arrange
        var original = CreateClipBytes();
        using var input = new MemoryStream(original);
        var header = Gbx.ParseHeader<CGameCtnMediaClip>(input, new() { ReadRawBody = true });
        using var saved = new MemoryStream();

        // Act
        header.Save(saved);

        // Assert
        await Assert.That(saved.ToArray()).IsEquivalentTo(original, CollectionOrdering.Matching);
    }

    [Test]
    public void Parse_UnrelatedNodeType_ThrowsInvalidCastException()
    {
        // Arrange
        using var input = new MemoryStream(CreateClipBytes());

        // Act / Assert
        Assert.Throws<InvalidCastException>(() => Gbx.Parse<CPlugSurface>(input));
    }

    private static byte[] CreateClipBytes()
    {
        var clip = new CGameCtnMediaClip { Tracks = [], Name = "Test clip" };
        clip.Chunks.Create<CGameCtnMediaClip.Chunk0307900D>();
        using var output = new MemoryStream();
        new Gbx<CGameCtnMediaClip>(clip).Save(output);
        return output.ToArray();
    }
}
