using GBX.NET.Engines.Function;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Function;

[Category("Unit")]
public class CFuncSegmentTests
{
    [Test]
    [Arguments(0)]
    [Arguments(2)]
    public async Task ReadWrite_Keys_RoundTripTwentyByteRecordsWithoutAnArchiveVersion(int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(count);
            for (var i = 0; i < count; i++)
            {
                writer.Write(i + 0.25f);
                writer.Write(i + 1.5f);
                writer.Write(-i - 2.75f);
                writer.Write(i + 3.125f);
                writer.Write(-i - 4.5f);
            }
            writer.Write(0xDEADBEEFu);
        }
        payload.Position = 0;
        var segment = new CFuncSegment();
        var chunk = new CFuncSegment.Chunk05037000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(segment, rw);
            await Assert.That(payload.Position).IsEqualTo(4L + count * 20);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        }
        await Assert.That(segment.Keys!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            var key = segment.Keys[i];
            await Assert.That(key.X).IsEqualTo(i + 0.25f);
            await Assert.That(key.ValueIn).IsEqualTo(i + 1.5f);
            await Assert.That(key.TangentIn).IsEqualTo(-i - 2.75f);
            await Assert.That(key.ValueOut).IsEqualTo(i + 3.125f);
            await Assert.That(key.TangentOut).IsEqualTo(-i - 4.5f);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(segment, rw);
            writer.Write(0xDEADBEEFu);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
