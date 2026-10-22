using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NullableWritableTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WriteListWritable_NullElement_WritesDefaultInstanceAndForwardsVersion(bool deprecated)
    {
        List<Value?> values = [new() { Number = 42 }, null];
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        IGbxWriter typedWriter = writer;
        if (deprecated) typedWriter.WriteListWritable_deprec(values, version: 7);
        else typedWriter.WriteListWritable(values, version: 7);
        writer.Write(0x12345678);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        if (deprecated) reader.ReadDeprecVersion();
        await Assert.That(reader.ReadInt32()).IsEqualTo(2);
        await Assert.That(reader.ReadInt32()).IsEqualTo(49);
        await Assert.That(reader.ReadInt32()).IsEqualTo(7);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    public sealed class Value : IWritable
    {
        public int Number { get; init; }

        public void Write(GbxWriter writer, int version = 0) => writer.Write(Number + version);
    }
}
