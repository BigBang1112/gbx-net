using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnGhostAlwaysOfficialTests
{
    private const long Timestamp = 1794142800123;
    private const string Commit = "0123456789abcdef0123456789abcdef01234567";

    [Test]
    [Arguments(0, false, false)]
    [Arguments(1, true, false)]
    [Arguments(2, false, true)]
    [Arguments(3, true, true)]
    public async Task ReadWrite_CommonPayload_MatchesModEncoder(int carried, bool accelerate, bool brake)
    {
        // Version 0.1.0.0 without a commit, as written by the Always Official encoder
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)carried);
            w.Write([0x00, 0x01, 0x00]);
            w.Write(new byte[20]);
            WriteUnsigned(w, Timestamp);
        });

        var ghost = await RoundTrip(payload);
        var data = ghost.AlwaysOfficialData!;
        await Assert.That(data.ModVersion).IsEqualTo(new Version(0, 1, 0));
        await Assert.That(data.GitCommit).IsNull();
        await Assert.That(data.CarriedAccelerate).IsEqualTo(accelerate);
        await Assert.That(data.CarriedBrake).IsEqualTo(brake);
        await Assert.That(data.GhostStartTimestamp).IsEqualTo(DateTimeOffset.FromUnixTimeMilliseconds(Timestamp));
        await Assert.That(data.GhostEndTimestamp).IsNull();
        await Assert.That(data.OfficialStartTimestamp).IsNull();
        await Assert.That(data.OfficialEndTimestamp).IsNull();
    }

    [Test]
    public async Task ReadWrite_FullPayload_ReadsRevisionCommitAndTimestamps()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)63);
            WriteUnsigned(w, 127);
            WriteUnsigned(w, 128);
            WriteUnsigned(w, 16384);
            WriteUnsigned(w, 65535);
            w.Write(Convert.FromHexString(Commit));
            WriteUnsigned(w, Timestamp);
            WriteUnsigned(w, Timestamp + 1000);
            WriteUnsigned(w, Timestamp + 200);
            WriteUnsigned(w, Timestamp + 900);
        });

        var data = (await RoundTrip(payload)).AlwaysOfficialData!;
        await Assert.That(data.ModVersion).IsEqualTo(new Version(127, 128, 16384, 65535));
        await Assert.That(data.GitCommit).IsEqualTo(Commit);
        await Assert.That(data.CarriedAccelerate).IsTrue();
        await Assert.That(data.CarriedBrake).IsTrue();
        await Assert.That(data.GhostDuration).IsEqualTo(TimeSpan.FromSeconds(1));
        await Assert.That(data.OfficialStartTimestamp).IsEqualTo(DateTimeOffset.FromUnixTimeMilliseconds(Timestamp + 200));
        await Assert.That(data.OfficialDuration).IsEqualTo(TimeSpan.FromMilliseconds(700));
    }

    [Test]
    public async Task ReadNode_SkippableChunk_PreservesDataAndClones()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)(2 | 32));
            w.Write([0x01, 0x02, 0x03]);
            w.Write(Convert.FromHexString(Commit));
            WriteUnsigned(w, Timestamp);
            WriteUnsigned(w, Timestamp + 5000);
        });
        var node = Payload(w =>
        {
            w.Write(0xBB111201);
            w.Write(0x534B4950);
            w.Write(payload.Length);
            w.Write(payload);
            w.Write(0xFACADE01);
        });

        using var input = new MemoryStream(node);
        using var reader = new GbxReader(input);
        var restored = reader.ReadNode<CGameCtnGhost>()!;
        await Assert.That(input.Position).IsEqualTo(input.Length);
        await Assert.That(restored.AlwaysOfficialData!.ModVersion).IsEqualTo(new Version(1, 2, 3));
        await Assert.That(restored.AlwaysOfficialData.OfficialEndTimestamp).IsEqualTo(DateTimeOffset.FromUnixTimeMilliseconds(Timestamp + 5000));

        var clone = (CGameCtnGhost)restored.DeepClone();
        await Assert.That(clone.AlwaysOfficialData).IsNotSameReferenceAs(restored.AlwaysOfficialData);
        await Assert.That(clone.AlwaysOfficialData!.GitCommit).IsEqualTo(Commit);
        await Assert.That(clone.AlwaysOfficialData.CarriedBrake).IsTrue();

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) writer.WriteNode(clone);
        await Assert.That(output.ToArray()).IsEquivalentTo(node, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Read_UnknownVersion_Throws()
    {
        using var stream = new MemoryStream([1, 0]);
        using var reader = new GbxReader(stream);
        var chunk = new CGameCtnGhost.ChunkBB111201();
        await Assert.That(() => chunk.Read(new CGameCtnGhost(), reader)).Throws<VersionNotSupportedException>();
    }

    private static async Task<CGameCtnGhost> RoundTrip(byte[] payload)
    {
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        var ghost = new CGameCtnGhost();
        var chunk = new CGameCtnGhost.ChunkBB111201();
        chunk.Read(ghost, reader);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);

        var rewritten = Payload(w => chunk.Write(ghost, w));
        await Assert.That(rewritten).IsEquivalentTo(payload, CollectionOrdering.Matching);
        return ghost;
    }

    private static void WriteUnsigned(GbxWriter w, long value)
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream)) writer.Write7BitEncodedInt64(value);
        w.Write(stream.ToArray());
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) write(writer);
        return stream.ToArray();
    }
}
