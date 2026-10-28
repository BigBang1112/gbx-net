using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class UnixTimeSerializationTests
{
    [Test]
    public async Task ReadUnixTime_MaxValue_ReturnsNull()
    {
        using var stream = new MemoryStream([0xFF, 0xFF, 0xFF, 0xFF]);
        using var reader = new GbxReader(stream);

        await Assert.That(reader.ReadUnixTime()).IsNull();
        await Assert.That(stream.Position).IsEqualTo(4L);
    }

    [Test]
    [Arguments(0u)]
    [Arguments(1u)]
    [Arguments(uint.MaxValue - 1)]
    public async Task UnixTime_ValidTimestamp_RoundTrips(uint seconds)
    {
        var value = DateTimeOffset.FromUnixTimeSeconds(seconds);
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.WriteUnixTime(value);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(seconds);

        stream.Position = 0;
        await Assert.That(reader.ReadUnixTime()).IsEqualTo((DateTimeOffset?)value);
    }

    [Test]
    public async Task WriteUnixTime_Null_WritesMaxValue()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.WriteUnixTime(null);

        await Assert.That(stream.ToArray().SequenceEqual(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })).IsTrue();
    }

    [Test]
    public async Task UnixTime_ReadWrite_PreservesNullSentinel()
    {
        using var input = new MemoryStream([0xFF, 0xFF, 0xFF, 0xFF]);
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var writer = new GbxWriter(output);
        using var rw = new GbxReaderWriter(reader, writer);
        DateTimeOffset? value = DateTimeOffset.FromUnixTimeSeconds(0);

        rw.UnixTime(ref value);

        await Assert.That(value).IsNull();
        await Assert.That(output.ToArray().SequenceEqual(input.ToArray())).IsTrue();
    }
}
