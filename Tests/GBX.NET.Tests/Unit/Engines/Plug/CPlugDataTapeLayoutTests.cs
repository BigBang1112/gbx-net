using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDataTapeLayoutTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task StateAndNoticePayloadsPreserveIndependentCounts(bool legacy, bool empty)
    {
        int[] times = empty ? [] : [100, 250];
        int[] offsets = empty ? [] : [0, 4, -1];
        byte[] states = empty ? [] : [1, 2, 3, 4, 5, 6, 7];
        byte[] notices = empty ? [] : [8, 9, 10, 11, 12];
        var noticeCount = empty ? 0 : 3;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (!legacy) writer.Write(0); // Chunk version.
            writer.Write(-123); // Unidentified tape header.
            writer.Write(times.Length);
            foreach (var time in times) writer.Write(time);
            writer.Write(offsets.Length);
            foreach (var offset in offsets) writer.Write(offset);
            writer.Write(states.Length);
            writer.Write(states);
            if (!legacy)
            {
                writer.Write(noticeCount);
                writer.Write(notices.Length);
                writer.Write(notices);
            }
        }

        byte[] previousNotices = [255];
        var node = new CPlugDataTape { NoticeCount = 77, NoticeData = previousNotices };
        Chunk<CPlugDataTape> chunk = legacy
            ? new CPlugDataTape.Chunk090CE000()
            : new CPlugDataTape.Chunk090CE001();
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);

        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Header!.U01).IsEqualTo(-123);
        await Assert.That(node.StateTimes).IsEquivalentTo(times, CollectionOrdering.Matching);
        await Assert.That(node.StateOffsets).IsEquivalentTo(offsets, CollectionOrdering.Matching);
        await Assert.That(node.StateData).IsEquivalentTo(states, CollectionOrdering.Matching);
        await Assert.That(node.NoticeCount).IsEqualTo(legacy ? 0 : noticeCount);
        await Assert.That(node.NoticeData).IsEquivalentTo(legacy ? previousNotices : notices, CollectionOrdering.Matching);

        if (legacy) node.NoticeCount = 99;
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
        await Assert.That(node.NoticeCount).IsEqualTo(legacy ? 0 : noticeCount);
    }

    [Test]
    public async Task LegacyChunkIsRegisteredForEmbeddedNodes()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x090CE000u);
            writer.Write(42); // Tape header, without a chunk version.
            writer.Write(1);
            writer.Write(300); // State time.
            writer.Write(1);
            writer.Write(0); // State offset.
            writer.Write(4);
            writer.Write(123); // Raw state data.
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugDataTape>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.GetChunk<CPlugDataTape.Chunk090CE000>()).IsNotNull();
        await Assert.That(node.Header!.U01).IsEqualTo(42);
        await Assert.That(node.StateTimes.Single()).IsEqualTo(300);
        await Assert.That(node.StateOffsets.Single()).IsEqualTo(0);
        await Assert.That(node.StateData).IsEquivalentTo(new byte[] { 123, 0, 0, 0 }, CollectionOrdering.Matching);
        await Assert.That(node.NoticeCount).IsEqualTo(0);
        await Assert.That(node.NoticeData).IsEmpty();
    }
}
