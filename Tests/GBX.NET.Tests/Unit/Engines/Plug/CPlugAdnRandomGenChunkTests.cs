using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugAdnRandomGenChunkTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task Chunk09140000_ReadsNativeTagSetPasses(int version)
    {
        var count = version < 3 ? 1 : 2;
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(version);
            if (version >= 1) writer.Write(123);
            if (version >= 3) writer.Write(count);
            for (var i = 0; i < count; i++)
            {
                WriteTags(writer, version, i);
                WriteTags(writer, version, i + 10);
            }
            if (version >= 2) writer.Write(7);
            if (version >= 4)
            {
                for (var i = 0; i < count; i++) WriteTags(writer, version, i + 20);
            }
            if (version >= 5) writer.WriteIdAsString("Pedestrians");
            writer.Write(0x12345678);
        }

        var payloadLength = stream.Length - sizeof(int);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var node = new CPlugAdnRandomGen();
        var chunk = new CPlugAdnRandomGen.Chunk09140000();
        chunk.ReadWrite(node, rw);

        await Assert.That(stream.Position).IsEqualTo(payloadLength);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(node.Sets.Length).IsEqualTo(count);
        await Assert.That(node.cModel).IsEqualTo(version >= 2 ? 7 : 1);
        await Assert.That(node.RandSeed).IsEqualTo(version >= 1 ? 123 : 0);
        if (version >= 5) await Assert.That(node.RandomGenId).IsEqualTo("Pedestrians");
        if (version == 6)
        {
            await Assert.That(node.Sets[1].RequiredTags?[0].Type).IsEqualTo("Type1");
            await Assert.That(node.Sets[1].ExcludedTags?[0].Value).IsEqualTo("Value11");
            await Assert.That(node.Sets[1].AnimRequiredTags?[0].Value).IsEqualTo("Value21");
        }
        else
        {
            await Assert.That(node.Sets[0].RequiredTags?[0].LegacyType).IsEqualTo(0);
            await Assert.That(node.Sets[0].ExcludedTags?[0].LegacyValue).IsEqualTo(110);
            if (version >= 4) await Assert.That(node.Sets[1].AnimRequiredTags?[0].LegacyValue).IsEqualTo(121);
        }

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, writerWriter);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(stream.ToArray().Take((int)payloadLength))).IsTrue();
    }

    private static void WriteTags(GbxWriter writer, int version, int index)
    {
        writer.Write(1);
        if (version >= 6)
        {
            writer.Write($"Type{index}");
            writer.Write($"Value{index}");
        }
        else
        {
            writer.Write(index);
            writer.Write(index + 100);
        }
    }
}
