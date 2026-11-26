using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockSquareCardEventIdsTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task BlockArchive_LegacyVersions_ReadImpliedEventsAndDecalsWithClearFlags(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("Block");
            writer.Write((byte)Direction.West);
            writer.Write(new Byte3(1, 2, 3));
            writer.Write(0); // These versions imply payload presence without a stored flag.
            writer.Write(0); // Empty card events.
            if (version >= 3)
            {
                writer.WriteIdAsString("Decal");
                writer.Write(2);
                writer.Write(3);
            }
            writer.Write(Sentinel);
        }

        var block = new CGameCtnBlock();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        block.Read(reader, version);

        await Assert.That(block.SquareCardEventIds!.Length).IsEqualTo(0);
        await Assert.That(block.Flags).IsEqualTo(0);
        if (version >= 3)
        {
            await Assert.That(block.DecalId).IsEqualTo("Decal");
            await Assert.That(block.DecalIntensity).IsEqualTo(2);
            await Assert.That(block.DecalVariant).IsEqualTo(3);
        }
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            block.Write(writer, version);
            writer.Write(Sentinel);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 2)]
    [Arguments(3, 2)]
    public async Task ReadWrite_PreservesVersionBlockUnitAndCardEvents(int version, int count)
    {
        var events = Enumerable.Range(0, count).Select(i => new Ident($"Event{i}", "Stadium", "Author")).ToArray();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(7);
            writer.Write(count);
            foreach (var cardEvent in events) writer.Write(cardEvent);
            writer.Write(Sentinel);
        }

        var entry = new CGameCtnBlock.SSquareCardEventIds();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) entry.ReadWrite(rw, v: 6);
        await Assert.That(entry.Version).IsEqualTo(version);
        await Assert.That(entry.BlockUnitIndex).IsEqualTo(7);
        await Assert.That(entry.CardEventIds!.SequenceEqual(events)).IsTrue();
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            using (var rw = new GbxReaderWriter(writer)) entry.ReadWrite(rw, v: 6);
            writer.Write(Sentinel);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
    }

    [Test]
    public async Task BlockArchive_PreservesSquareCardEvents()
    {
        using var payload = new MemoryStream();
        var expectedEvent = new Ident("Event", "Stadium", "Author");
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("Block");
            writer.Write((byte)Direction.West);
            writer.Write(new Byte3(1, 2, 3));
            writer.Write(1 << 18);
            writer.Write(1);
            writer.Write(0);
            writer.Write(5);
            writer.Write(1);
            writer.Write(expectedEvent);
            writer.Write(Sentinel);
        }

        var block = new CGameCtnBlock();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        block.Read(reader, v: 6);
        await Assert.That(block.SquareCardEventIds!.Length).IsEqualTo(1);
        var entry = block.SquareCardEventIds[0];
        await Assert.That(entry.Version).IsEqualTo(0);
        await Assert.That(entry.BlockUnitIndex).IsEqualTo(5);
        await Assert.That(entry.CardEventIds!.Single()).IsEqualTo(expectedEvent);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            block.Write(writer, v: 6);
            writer.Write(Sentinel);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
    }
}
