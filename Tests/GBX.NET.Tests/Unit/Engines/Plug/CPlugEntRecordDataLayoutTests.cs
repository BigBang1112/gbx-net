using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugEntRecordDataLayoutTests
{
    [Test]
    public async Task ReadWrite_EntRecordDesc_KeepsNativeOrder()
    {
        var desc = new CPlugEntRecordData.EntRecordDesc();
        var payload = Payload(w =>
        {
            w.Write(0x0A019000u);
            w.Write(16); // StateSize
            w.Write(4); // SceneUIdOffset
            w.Write(2); // StateArchiveVersion
            Data(w, [1, 2, 3]);
            w.Write(0);
        });

        await RoundTrip(payload, rw => desc.ReadWrite(rw));

        await Assert.That(desc.ClassId).IsEqualTo(0x0A019000u);
        await Assert.That(desc.StateSize).IsEqualTo(16);
        await Assert.That(desc.SceneUIdOffset).IsEqualTo(4);
        await Assert.That(desc.StateArchiveVersion).IsEqualTo(2);
        await Assert.That(desc.InitialState).IsEquivalentTo(new byte[] { 1, 2, 3 }, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_NoticeRecordDesc_ReadsClassIdFromVersion4(int version)
    {
        var desc = new CPlugEntRecordData.NoticeRecordDesc();
        var payload = Payload(w =>
        {
            w.Write(1); // NoticeArchiveVersion
            w.Write(3); // NoticeType
            if (version >= 4) w.Write(0x0A019000u);
        });

        await RoundTrip(payload, rw => desc.ReadWrite(rw, version));

        await Assert.That(desc.NoticeArchiveVersion).IsEqualTo(1);
        await Assert.That(desc.NoticeType).IsEqualTo(3);
        await Assert.That(desc.ClassId).IsEqualTo(version >= 4 ? 0x0A019000u : null);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructors()
    {
        var entDesc = new CPlugEntRecordData.EntRecordDesc();
        var noticeDesc = new CPlugEntRecordData.NoticeRecordDesc();

        await Assert.That(entDesc.SceneUIdOffset).IsEqualTo(-1);
        await Assert.That(entDesc.StateArchiveVersion).IsEqualTo(-1);
        await Assert.That(noticeDesc.NoticeArchiveVersion).IsEqualTo(-1);
        await Assert.That(noticeDesc.NoticeType).IsEqualTo(-1);
    }

    [Test]
    public async Task ReadWrite_Version10_KeepsNativeOrder()
    {
        var node = new CPlugEntRecordData();
        node.CreateChunk<CPlugEntRecordData.Chunk0911F000>().Version = 10;

        await RoundTrip(RecordDataPayload(), rw => node.ReadWrite(rw, 10));

        var ent = node.EntList.Single();
        await Assert.That(node.Start).IsEqualTo(TimeInt32.Zero);
        await Assert.That(node.End).IsEqualTo(TimeInt32.FromMilliseconds(1000));
        await Assert.That(ent.Type).IsEqualTo(0);
        await Assert.That(ent.SceneUId).IsEqualTo(0x01000005);
        await Assert.That(ent.BeginTime).IsEqualTo(TimeInt32.FromMilliseconds(100));
        await Assert.That(ent.EndTime).IsEqualTo(TimeInt32.FromMilliseconds(900));
        await Assert.That(ent.U04).IsEqualTo(7);
        await Assert.That(ent.Samples.Single().Time).IsEqualTo(TimeInt32.FromMilliseconds(100));
        await Assert.That(ent.Samples2.Single().Time).IsEqualTo(TimeInt32.FromMilliseconds(200));

        var notice = node.BulkNoticeList.Single();
        await Assert.That(notice.Type).IsEqualTo(0);
        await Assert.That(notice.Time).IsEqualTo(TimeInt32.FromMilliseconds(300));

        var deltaList = node.CustomModulesDeltaLists.Single();
        await Assert.That(deltaList.Deltas.Single().U01).IsEqualTo(5);
        await Assert.That(deltaList.ClassId).IsEqualTo(0x2F0CB000u);
        await Assert.That(deltaList.Period).IsEqualTo(50);
    }

    private static byte[] RecordDataPayload() => Payload(w =>
    {
        w.Write(0); // Start
        w.Write(1000); // End

        w.Write(1); // Entity descs
        w.Write(0x0A019000u);
        w.Write(4);
        w.Write(0);
        w.Write(2);
        Data(w, [0, 0, 0, 0]);
        w.Write(0);

        w.Write(1); // Notice descs
        w.Write(1);
        w.Write(3);
        w.Write(0x0A019000u);

        w.Write((byte)1); // Entity list
        w.Write(0); // Type
        w.Write(0x01000005); // SceneUId
        w.Write(100); // BeginTime
        w.Write(900); // EndTime
        w.Write(7); // U04
        w.Write((byte)1); // Deltas
        w.Write(100);
        Data(w, [1, 2, 3, 4]);
        w.Write((byte)0);
        w.Write((byte)0); // No next entity
        w.Write((byte)1); // Entity notices
        w.Write(0);
        w.Write(200);
        Data(w, [9]);
        w.Write((byte)0);

        w.Write((byte)1); // Bulk notices
        w.Write(0);
        w.Write(300);
        Data(w, []);
        w.Write((byte)0);

        w.Write(1); // Custom module delta lists
        w.Write((byte)1);
        w.Write(5);
        Data(w, [9]);
        Data(w, []);
        w.Write((byte)0);
        w.Write(0x2F0CB000u); // ClassId
        w.Write(50); // Period
    });

    private static void Data(BinaryWriter w, byte[] data)
    {
        w.Write(data.Length);
        w.Write(data);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
