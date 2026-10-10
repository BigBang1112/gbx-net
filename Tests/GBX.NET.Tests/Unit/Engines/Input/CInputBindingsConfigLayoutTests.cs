using GBX.NET.Engines.Input;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Input;

[Category("Unit")]
public class CInputBindingsConfigLayoutTests
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 2)]
    [Arguments(1, 0)]
    [Arguments(1, 2)]
    [Arguments(2, 0)]
    [Arguments(2, 2)]
    [Arguments(3, 0)]
    [Arguments(3, 2)]
    public async Task BindingPayloadsPreserveNativeFieldOrder(int format, int count)
    {
        var payload = BindingPayload(format, count);
        var node = new CInputBindingsConfig();
        var chunk = format switch
        {
            0 => (GBX.NET.Serialization.Chunking.Chunk<CInputBindingsConfig>)new CInputBindingsConfig.Chunk13006000(),
            1 or 2 => new CInputBindingsConfig.Chunk13006003(),
            _ => new CInputBindingsConfig.Chunk13006005()
        };
        using var input = WithSentinel(payload);
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(input.Position).IsEqualTo((long)payload.Length);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        }

        await Assert.That(node.Bindings!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            var binding = node.Bindings[i];
            await Assert.That(binding.ObjectIndex).IsEqualTo(100 + i);
            await Assert.That(binding.SubDeviceIndex).IsEqualTo(format == 0 ? 0 : 10 + i);
            await Assert.That(binding.ActionType).IsEqualTo(3 + i);
            await Assert.That(binding.PlayerNumber).IsEqualTo(2 + i);
            await Assert.That(binding.Name).IsEqualTo($"Action{i}");
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    public async Task SkippableIdPayloadsReuseEarlierIdsAndRestoreDictionaryCount(int chunkId)
    {
        using var expected = new MemoryStream();
        using (var writer = new BinaryWriter(expected, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(3); // One ID version for the entire stream.
            WriteNewId(writer, "Existing");
            if (chunkId == 4)
            {
                writer.Write(2);
                writer.Write(0x40000001u); // Reference the ID from before the chunk.
                WriteNewId(writer, "Temporary");
            }
            else
            {
                writer.Write(1);
                writer.Write(0x40000001u);
                WriteNewId(writer, "Temporary");
                writer.Write(2); // Subdevice.
                writer.Write(35); // Object.
                writer.Write(4); // Action type.
                writer.Write(1); // Player.
                WriteString(writer, "Action");
            }
            WriteNewId(writer, "After"); // Reuses dictionary index 2 after rollback.
            writer.Write(0x40000002u);
            WriteNewId(writer, "Temporary"); // Must be introduced again outside the chunk.
            writer.Write(0x40000003u);
        }

        var node = new CInputBindingsConfig();
        var chunk4 = new CInputBindingsConfig.Chunk13006004();
        var chunk5 = new CInputBindingsConfig.Chunk13006005();
        using var input = WithSentinel(expected.ToArray());
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            await Assert.That(reader.ReadIdAsString()).IsEqualTo("Existing");
            if (chunkId == 4) chunk4.ReadWrite(node, rw);
            else chunk5.ReadWrite(node, rw);
            await Assert.That(reader.ReadIdAsString()).IsEqualTo("After");
            await Assert.That(reader.ReadIdAsString()).IsEqualTo("After");
            await Assert.That(reader.ReadIdAsString()).IsEqualTo("Temporary");
            await Assert.That(reader.ReadIdAsString()).IsEqualTo("Temporary");
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            writer.WriteIdAsString("Existing");
            if (chunkId == 4) chunk4.ReadWrite(node, rw);
            else chunk5.ReadWrite(node, rw);
            writer.WriteIdAsString("After");
            writer.WriteIdAsString("After");
            writer.WriteIdAsString("Temporary");
            writer.WriteIdAsString("Temporary");
        }
        await Assert.That(output.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ConfigNameIsSharedByLegacyAndMetadataChunks()
    {
        var node = new CInputBindingsConfig();
        await Assert.That(node.Name).IsEqualTo("");
        var chunk = new CInputBindingsConfig.Chunk13006002();
        using var payload = new MemoryStream();
        using (var writer = new BinaryWriter(payload, Encoding.UTF8, leaveOpen: true))
        {
            WriteString(writer, "Default");
            writer.Write(42);
        }
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(node.Name).IsEqualTo("Default");
        await Assert.That(node.Version).IsEqualTo(42);
    }

    [Test]
    public async Task AdditionalSettingsWriteZeroPadding()
    {
        var chunk = new CInputBindingsConfig.Chunk13006006 { U01 = 17, U02 = 42, U03 = 43 };
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(new CInputBindingsConfig(), rw);
        output.Position = 0;
        using var reader = new BinaryReader(output);
        await Assert.That(reader.ReadInt32()).IsEqualTo(17);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        await Assert.That(output.Length).IsEqualTo(12L);
    }

    private static byte[] BindingPayload(int format, int count)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        if (format == 0) WriteString(writer, "Default");
        if (format is 1 or 2) writer.Write(format - 1);
        writer.Write(count);
        for (var i = 0; i < count; i++)
        {
            if (format == 0) writer.Write(100 + i);
            if (i == 0) writer.Write(3); // ID version.
            writer.Write(uint.MaxValue); // Empty device ID.
            if (format == 3) writer.Write(uint.MaxValue); // Additional device ID.
            if (format != 0)
            {
                writer.Write(10 + i);
                writer.Write(100 + i);
            }
            writer.Write(3 + i);
            writer.Write(2 + i);
            WriteString(writer, $"Action{i}");
        }
        return stream.ToArray();
    }

    private static void WriteNewId(BinaryWriter writer, string value)
    {
        writer.Write(0x40000000u);
        WriteString(writer, value);
    }

    private static void WriteString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
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
