using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class CollectionReaderTests
{
    [Test]
    public async Task PrimitiveCollections_ReadAllBytesFromFragmentedStreams()
    {
        byte[] data = [0x22, 0x11, 0x44, 0x33, 0x66, 0x55];
        using var stream = new FragmentedStream(data);
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadArray<short>(2).SequenceEqual(new short[] { 0x1122, 0x3344 })).IsTrue();
        await Assert.That(reader.ReadList<short>(1).SequenceEqual(new short[] { 0x5566 })).IsTrue();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public void PrimitiveCollections_RejectTruncatedData()
    {
        using var stream = new MemoryStream(new byte[] { 0x22, 0x11, 0x44 });
        using var reader = new GbxReader(stream);
        Assert.Throws<EndOfStreamException>(() => reader.ReadArray<short>(2));
        stream.Position = 0;
        Assert.Throws<EndOfStreamException>(() => reader.ReadList<short>(2));
    }

    [Test]
    public async Task PrimitiveCollections_RejectInvalidLengthsBeforeReading()
    {
        using var stream = new MemoryStream();
        using var reader = new GbxReader(stream, new GbxReadSettings { MaxDataSize = int.MaxValue });
        Assert.Throws<ArgumentOutOfRangeException>(() => reader.ReadArray<int>(-0x40000000));
        Assert.Throws<ArgumentOutOfRangeException>(() => reader.ReadList<int>(-0x40000000));
        Assert.Throws<OverflowException>(() => reader.ReadArray<int>(0x40000000));
        Assert.Throws<OverflowException>(() => reader.ReadList<int>(0x40000000));
        await Assert.That(stream.Position).IsEqualTo(0);
    }

    private sealed class FragmentedStream : MemoryStream
    {
        public FragmentedStream(byte[] data) : base(data)
        {
        }

        public override int Read(Span<byte> buffer) => base.Read(buffer.Slice(0, Math.Min(buffer.Length, 1)));

        public override int Read(byte[] buffer, int offset, int count) => base.Read(buffer, offset, Math.Min(count, 1));
    }
}
