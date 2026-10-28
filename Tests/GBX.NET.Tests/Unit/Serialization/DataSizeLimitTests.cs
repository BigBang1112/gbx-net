using GBX.NET.Engines.MwFoundations;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class DataSizeLimitTests
{
    [Test]
    public async Task WriteString_ConfiguredLimit_CountsUtf8Bytes()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream, new GbxWriteSettings { MaxDataSize = 2 });

        Assert.Throws<LengthLimitException>(() => writer.Write("\u00e9a"));
        await Assert.That(stream.Length).IsEqualTo(0);

        writer.Write("\u00e9");
        stream.Position = 0;
        using var reader = new GbxReader(stream, new GbxReadSettings { MaxDataSize = 2 });
        await Assert.That(reader.ReadString()).IsEqualTo("\u00e9");
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWriteCollections_ConfiguredLimit_ThrowsBeforeSerializing(bool writing)
    {
        using var stream = new MemoryStream();
        using var reader = new GbxReader(stream, new GbxReadSettings { MaxDataSize = 2 });
        using var writer = new GbxWriter(stream, new GbxWriteSettings { MaxDataSize = 2 });
        using var rw = writing ? new GbxReaderWriter(writer) : new GbxReaderWriter(reader);

        Assert.Throws<LengthLimitException>(() => rw.ArrayReadableWritable<Value>(null, 3));
        Assert.Throws<LengthLimitException>(() => rw.ListReadableWritable<Value>(null, 3));
        Assert.Throws<LengthLimitException>(() => rw.ArrayReadableWritable<Value, CMwNod>(null, new CMwNod(), 3));
        await Assert.That(stream.Position).IsEqualTo(0);
        await Assert.That(stream.Length).IsEqualTo(0);
    }

    [Test]
    public async Task ReadWriteCollections_AtConfiguredLimit_RoundTrips()
    {
        using var stream = new MemoryStream();
        var node = new CMwNod();
        using (var writer = new GbxWriter(stream, new GbxWriteSettings { MaxDataSize = 1 }))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.ArrayReadableWritable<Value>([new() { Number = 42 }]);
            rw.ListReadableWritable<Value>([new() { Number = 43 }]);
            rw.ArrayReadableWritable<Value, CMwNod>([new() { Number = 44 }], node);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream, new GbxReadSettings { MaxDataSize = 1 });
        using var readWrite = new GbxReaderWriter(reader);
        await Assert.That(readWrite.ArrayReadableWritable<Value>()![0].Number).IsEqualTo(42);
        await Assert.That(readWrite.ListReadableWritable<Value>()![0].Number).IsEqualTo(43);
        await Assert.That(readWrite.ArrayReadableWritable<Value, CMwNod>(null, node)![0].Number).IsEqualTo(44);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    public sealed class Value : IReadableWritable, IReadable<CMwNod>, IWritable<CMwNod>
    {
        public int Number;

        public void ReadWrite(GbxReaderWriter rw, int v = 0) => rw.Int32(ref Number);

        public void Read(GbxReader r, CMwNod n, int v = 0) => Number = r.ReadInt32();

        public void Write(GbxWriter w, CMwNod n, int v = 0) => w.Write(Number);
    }
}
