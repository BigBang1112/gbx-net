using GBX.NET.Engines.Input;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Input;

[Category("Unit")]
public class CInputReplayLayoutTests
{
    [Test]
    [Arguments(0, 0, 0u)]
    [Arguments(1, 0, 4096u)]
    [Arguments(0, 3, 0u)]
    [Arguments(1, 3, 4096u)]
    public async Task ReplayPreservesActionTableAndNineByteEvents(int version, int count, uint maxEventCount)
    {
        var payload = Payload(version, count, maxEventCount, valueHighBits: false);
        var node = new CInputReplay();
        var chunk = new CInputReplay.Chunk1300D000();
        using var input = WithSentinel(payload);
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(input.Position).IsEqualTo((long)payload.Length);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.ActionIds).IsEquivalentTo(new[] { "Accelerate", "Steer" }, CollectionOrdering.Matching);
        await Assert.That(node.MaxEventCount).IsEqualTo(maxEventCount);
        await Assert.That(node.Events!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            await Assert.That(node.Events[i].Time).IsEqualTo(uint.MaxValue - (uint)i);
            await Assert.That(node.Events[i].ActionIndex).IsEqualTo((byte)(i % 2));
            await Assert.That(node.Events[i].Value).IsEqualTo(0xFFFFFFu - (uint)i);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReplayMasksPackedValuesAndWritesZeroArchivePrefix()
    {
        var node = new CInputReplay();
        var chunk = new CInputReplay.Chunk1300D000();
        using var input = WithSentinel(Payload(1, 3, 0, valueHighBits: true));
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(chunk.U01).IsEqualTo(42u);
        for (var i = 0; i < 3; i++)
        {
            await Assert.That(node.Events![i].Value).IsEqualTo(0xFFFFFFu - (uint)i);
            node.Events[i].Value |= 0xAB000000;
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(Payload(1, 3, 0, valueHighBits: false), CollectionOrdering.Matching);
    }

    [Test]
    public async Task EmptyReplayWritesOnlyItsFiveHeaderWords()
    {
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) new CInputReplay.Chunk1300D000().ReadWrite(new CInputReplay(), rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, CollectionOrdering.Matching);
    }

    private static byte[] Payload(int version, int count, uint maxEventCount, bool valueHighBits)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(version);
        writer.Write(valueHighBits ? 42u : 0u);
        writer.Write(2); // Action count.
        writer.Write(3); // ID version.
        foreach (var action in new[] { "Accelerate", "Steer" })
        {
            writer.Write(0x40000000u);
            var bytes = Encoding.UTF8.GetBytes(action);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
        writer.Write(count);
        writer.Write(maxEventCount);
        for (var i = 0; i < count; i++)
        {
            writer.Write(uint.MaxValue - (uint)i);
            writer.Write((byte)(i % 2));
            writer.Write((0xFFFFFFu - (uint)i) | (valueHighBits ? 0xAB000000u : 0u));
        }
        return stream.ToArray();
    }

    private static MemoryStream WithSentinel(byte[] payload)
    {
        var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true)) writer.Write(0xDEADBEEFu);
        stream.Position = 0;
        return stream;
    }
}
