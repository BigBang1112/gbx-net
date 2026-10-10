using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameSaveLaunchedCheckpointsLayoutTests
{
    [Test]
    public async Task CheckpointMetadataAndModelsFollowBothStateArrays()
    {
        var payload = Payload(w =>
        {
            w.Write(7);
            w.Write(33);
            w.Write(2);
            WriteCheckpoint(w, 5);
            WriteCheckpoint(w, 13);
            w.Write(2);
            WriteSnapshot(w, 0xFEDCBA98u, 1000);
            WriteSnapshot(w, 0x87654321u, 2000);
            w.Write(1u);
            w.Write(1u);
            w.Write(2);
            w.Write(new Ident("CarSnow", 26, "Nadeo"));
            w.Write(3);
            w.Write(new Ident("CarDesert", 26, "Nadeo"));
        });
        var node = new CGameSaveLaunchedCheckpoints();
        var chunk = new CGameSaveLaunchedCheckpoints.Chunk03262000();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Checkpoints![0].State!.CheckpointIndex).IsEqualTo(5u);
        await Assert.That(node.Checkpoints[1].State!.CheckpointIndex).IsEqualTo(13u);
        await Assert.That(node.Checkpoints[1].State!.CaptureTime).IsEqualTo(0x87654321u);
        await Assert.That(node.Checkpoints[1].State!.ClassId).IsEqualTo(0x0A020000u);
        await Assert.That(node.Checkpoints[0].SnapshotCount).IsEqualTo(1u);
        await Assert.That(node.Checkpoints[1].ModelIndex).IsEqualTo(3);
        await Assert.That(node.Checkpoints[1].Model).IsEqualTo(new Ident("CarDesert", 26, "Nadeo"));
        await Assert.That(node.Snapshots![0].PackedVehicleState).IsEqualTo(0xFEDCBA98u);
        await Assert.That(node.Snapshots[1].PackedVehicleState).IsEqualTo(0x87654321u);
        await Assert.That(node.Snapshots[1].Time.TotalMilliseconds).IsEqualTo(2000);
        await Assert.That(new CGameSaveLaunchedCheckpoints.Checkpoint().Model).IsEqualTo(Ident.Empty);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await Assert.That((object)chunk is GBX.NET.Serialization.Chunking.ISkippableChunk).IsFalse();
    }

    [Test]
    [Arguments(1u, 0u, 0)]
    [Arguments(0u, 1u, 0)]
    [Arguments(1u, 1u, 1)]
    [Arguments(0xFFFFFFFFu, 2u, 0)]
    public void SnapshotCountsCannotExceedTheAvailableArray(uint firstCount, uint secondCount, int snapshotCount)
    {
        var payload = Payload(w =>
        {
            w.Write(5);
            w.Write(33);
            w.Write(2);
            WriteCheckpoint(w, 5);
            WriteCheckpoint(w, 13);
            w.Write(snapshotCount);
            for (var i = 0; i < snapshotCount; i++)
            {
                WriteSnapshot(w, 0, 1000);
            }
            w.Write(firstCount);
            w.Write(secondCount);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);

        Assert.Throws<InvalidDataException>(() => new CGameSaveLaunchedCheckpoints.Chunk03262000()
            .ReadWrite(new CGameSaveLaunchedCheckpoints(), rw));
    }

    private static void WriteCheckpoint(GbxWriter w, uint index)
    {
        w.Write(index);
        w.Write(123);
        w.Write(0x87654321u);
        w.Write(1);
        w.Write(2);
        w.Write(3);
        w.Write(0x0A020000u);
        w.WriteData(new byte[578], 578); // Vehicle state in versions 4+.
        w.Write(10);
        w.Write(11);
        w.Write(12);
        w.Write((byte)0xFE);
        w.Write(15);
        w.WriteData(new byte[40], 40); // Two events.
    }

    private static void WriteSnapshot(GbxWriter w, uint packedState, int time)
    {
        w.WriteData(new byte[85], 85);
        w.Write(17);
        w.Write(packedState);
        w.WriteData(new byte[10], 10); // Four packed wheels and two packed values.
        w.Write(18);
        w.Write((byte)0xFE);
        w.Write(-100);
        w.Write(19);
        w.Write(time);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        serialize(rw);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writerRw = new GbxReaderWriter(writer);
        serialize(writerRw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
