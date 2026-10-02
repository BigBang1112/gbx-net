using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GbxApiTests
{
    private static string ClipFixture => TestFiles.Gbx("CGameCtnMediaClip", "GBX-NET 2 CGameCtnMediaClip TM2020 001.Clip.Gbx");

    [Test]
    public async Task AsyncParsingFromPartialNonSeekableReadsMatchesSynchronousParsing()
    {
        var bytes = await File.ReadAllBytesAsync(ClipFixture);
        using var input = new FragmentedReadStream(bytes, maxReadSize: 7);
        var parsed = await Gbx.ParseAsync<CGameCtnMediaClip>(input);
        var expected = Gbx.Parse<CGameCtnMediaClip>(ClipFixture);
        parsed.BodyCompression = GbxCompression.Uncompressed;
        expected.BodyCompression = GbxCompression.Uncompressed;
        using var actualOutput = new MemoryStream();
        using var expectedOutput = new MemoryStream();

        parsed.Save(actualOutput);
        expected.Save(expectedOutput);

        await Assert.That(actualOutput.ToArray()).IsEquivalentTo(expectedOutput.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ParseHonorsStreamOwnership(bool closeStream)
    {
        using var stream = File.OpenRead(ClipFixture);
        var parsed = await Gbx.ParseAsync<CGameCtnMediaClip>(stream, new() { CloseStream = closeStream });

        await Assert.That(parsed.Node.Tracks).IsNotNull();
        await Assert.That(stream.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SaveHonorsStreamOwnership(bool closeStream)
    {
        var gbx = Gbx.Parse<CGameCtnMediaClip>(ClipFixture);
        using var stream = new MemoryStream();

        gbx.Save(stream, new() { CloseStream = closeStream });

        await Assert.That(stream.CanWrite).IsEqualTo(!closeStream);
        await Assert.That(stream.ToArray().Length).IsGreaterThan(0);
    }

    [Test]
    public async Task HeaderOnlyParsingCanPreserveAndSaveTheRawBody()
    {
        var original = await File.ReadAllBytesAsync(ClipFixture);
        using var input = new MemoryStream(original);
        var header = Gbx.ParseHeader<CGameCtnMediaClip>(input, new() { ReadRawBody = true });
        using var saved = new MemoryStream();

        header.Save(saved);

        await Assert.That(saved.ToArray()).IsEquivalentTo(original, CollectionOrdering.Matching);
    }

    [Test]
    public void TypedParsingRejectsAnUnrelatedNodeType()
    {
        Assert.Throws<InvalidCastException>(() => Gbx.Parse<CPlugSurface>(ClipFixture));
    }
}
