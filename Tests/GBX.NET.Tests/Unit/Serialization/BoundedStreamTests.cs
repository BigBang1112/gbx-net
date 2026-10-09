using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class BoundedStreamTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadsOnlyTheDeclaredRegionAndLeavesFollowingBytes(bool seekable)
    {
        using Stream source = seekable ? new MemoryStream([10, 20, 30, 99]) : new FragmentedReadStream([10, 20, 30, 99]);
        using var bounded = new BoundedStream(source, 3);
        var buffer = new byte[8];

        var count = bounded.Read(buffer, 2, 5);

        await Assert.That(count).IsEqualTo(3);
        await Assert.That(buffer).IsEquivalentTo(new byte[] { 0, 0, 10, 20, 30, 0, 0, 0 }, CollectionOrdering.Matching);
        await Assert.That(bounded.Position).IsEqualTo(3L);
        await Assert.That(bounded.Remaining).IsEqualTo(0L);
        await Assert.That(bounded.ReadByte()).IsEqualTo(-1);
        bounded.EnsureFullyRead();
        await Assert.That(source.ReadByte()).IsEqualTo(99);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SeekingReplaysDataWithinTheRegion(bool seekable)
    {
        using Stream source = seekable ? new MemoryStream([10, 20, 30, 99]) : new FragmentedReadStream([10, 20, 30, 99]);
        using var bounded = new BoundedStream(source, 3);

        await Assert.That(bounded.Seek(-1, SeekOrigin.End)).IsEqualTo(2L);
        await Assert.That(bounded.ReadByte()).IsEqualTo(30);
        await Assert.That(bounded.Seek(-2, SeekOrigin.Current)).IsEqualTo(1L);
        await Assert.That(bounded.ReadByte()).IsEqualTo(20);
        bounded.Position = 0;
        await Assert.That(bounded.ReadByte()).IsEqualTo(10);
        Assert.Throws<IOException>(() => bounded.Seek(-1, SeekOrigin.Begin));
    }

    [Test]
    public async Task UsesTheInitialOffsetWhenOtherReadersMoveTheUnderlyingStream()
    {
        using var source = new MemoryStream([99, 10, 20, 30, 88]);
        source.Position = 1;
        using var bounded = new BoundedStream(source, 3);
        source.Position = 4;

        await Assert.That(bounded.ReadByte()).IsEqualTo(10);
        source.Position = 0;
        await Assert.That(bounded.ReadByte()).IsEqualTo(20);
        await Assert.That(bounded.Length).IsEqualTo(3L);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task AsyncReadsRespectTheBoundary(bool seekable, bool memoryOverload)
    {
        using Stream source = seekable ? new MemoryStream([10, 20, 99]) : new FragmentedReadStream([10, 20, 99]);
        using var bounded = new BoundedStream(source, 2);
        var buffer = new byte[8];
        var count = memoryOverload
            ? await bounded.ReadAsync(buffer.AsMemory())
            : await bounded.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);

        await Assert.That(count).IsEqualTo(2);
        await Assert.That(buffer[..count]).IsEquivalentTo(new byte[] { 10, 20 }, CollectionOrdering.Matching);
        await Assert.That(await bounded.ReadAsync(buffer.AsMemory())).IsEqualTo(0);
        await Assert.That(source.ReadByte()).IsEqualTo(99);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TruncatedInputDoesNotPretendToHaveReadTheDeclaredLength(bool seekable)
    {
        using Stream source = seekable ? new MemoryStream([10]) : new FragmentedReadStream([10]);
        using var bounded = new BoundedStream(source, 3);
        var buffer = new byte[8];

        await Assert.That(bounded.Read(buffer, 0, buffer.Length)).IsEqualTo(1);
        await Assert.That(bounded.Read(buffer, 0, buffer.Length)).IsEqualTo(0);
        await Assert.That(bounded.Remaining).IsEqualTo(2L);
        var exception = Assert.Throws<InvalidOperationException>(() => bounded.EnsureFullyRead("chunk payload"));
        await Assert.That(exception.Message).Contains("chunk payload");
    }

    [Test]
    public async Task DisposingTheViewLeavesTheSourceOpen()
    {
        using var source = new MemoryStream([10, 20]);
        var bounded = new BoundedStream(source, 1);
        bounded.Dispose();
        await Assert.That(source.ReadByte()).IsEqualTo(10);
    }

    [Test]
    public void RejectsInvalidConstructionAndWrites()
    {
        using var source = new MemoryStream();
        Assert.Throws<ArgumentNullException>(() => new BoundedStream(null!, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BoundedStream(source, -1));
        using var bounded = new BoundedStream(source, 0);
        Assert.Throws<NotSupportedException>(() => bounded.Write([1], 0, 1));
        Assert.Throws<NotSupportedException>(() => bounded.SetLength(1));
    }
}
