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
        // Version 0.1.0.0 without a commit or a race start, as written by the Always Official encoder
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)carried);
            w.Write([0x00, 0x01, 0x00]);
            w.Write(new byte[20]);
            w.Write((byte)0);
        });

        var ghost = await RoundTrip(payload);
        var data = ghost.AlwaysOfficialData!;
        await Assert.That(data.ModVersion).IsEqualTo(new Version(0, 1, 0));
        await Assert.That(data.GitCommit).IsNull();
        await Assert.That(data.CarriedAccelerate).IsEqualTo(accelerate);
        await Assert.That(data.CarriedBrake).IsEqualTo(brake);
        await Assert.That(data.IsOfficial).IsFalse();
        await Assert.That(data.RaceStartTimestamp).IsNull();
        await Assert.That(data.RaceFinishTimestamp).IsNull();
        await Assert.That(data.RaceDuration).IsNull();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task ReadWrite_RaceTimestamps_ReadOfficialFlagAndOptionalFinish(bool finished, bool official)
    {
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)((finished ? 8 : 0) | (official ? 16 : 0)));
            w.Write([0x00, 0x01, 0x00]);
            w.Write(new byte[20]);
            WriteUnsigned(w, Timestamp);
            if (finished) WriteUnsigned(w, Timestamp + 1000);
        });

        var data = (await RoundTrip(payload)).AlwaysOfficialData!;
        await Assert.That(data.IsOfficial).IsEqualTo(official);
        await Assert.That(data.RaceStartTimestamp).IsEqualTo(DateTimeOffset.FromUnixTimeMilliseconds(Timestamp));
        await Assert.That(data.RaceFinishTimestamp.HasValue).IsEqualTo(finished);
        await Assert.That(data.RaceDuration).IsEqualTo(finished ? TimeSpan.FromSeconds(1) : null);
    }

    [Test]
    public async Task ReadWrite_FullPayload_ReadsRevisionAndCommit()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)31);
            WriteUnsigned(w, 127);
            WriteUnsigned(w, 128);
            WriteUnsigned(w, 16384);
            WriteUnsigned(w, 65535);
            w.Write(Convert.FromHexString(Commit));
            WriteUnsigned(w, Timestamp);
            WriteUnsigned(w, Timestamp + 1000);
        });

        var data = (await RoundTrip(payload)).AlwaysOfficialData!;
        await Assert.That(data.ModVersion).IsEqualTo(new Version(127, 128, 16384, 65535));
        await Assert.That(data.GitCommit).IsEqualTo(Commit);
        await Assert.That(data.CarriedAccelerate).IsTrue();
        await Assert.That(data.CarriedBrake).IsTrue();
        await Assert.That(data.IsOfficial).IsTrue();
    }

    [Test]
    public async Task Read_UnknownFlag_Throws()
    {
        using var stream = new MemoryStream([0, 32]);
        using var reader = new GbxReader(stream);
        var chunk = new CGameCtnGhost.ChunkBB111201();
        await Assert.That(() => chunk.Read(new CGameCtnGhost(), reader)).Throws<NotSupportedException>();
    }

    [Test]
    public async Task ReadNode_SkippableChunk_PreservesDataAndClones()
    {
        var payload = Payload(w =>
        {
            w.Write((byte)0);
            w.Write((byte)(2 | 8 | 16));
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
        await Assert.That(restored.AlwaysOfficialData.RaceFinishTimestamp).IsEqualTo(DateTimeOffset.FromUnixTimeMilliseconds(Timestamp + 5000));

        var clone = (CGameCtnGhost)restored.DeepClone();
        await Assert.That(clone.AlwaysOfficialData).IsNotSameReferenceAs(restored.AlwaysOfficialData);
        await Assert.That(clone.AlwaysOfficialData!.GitCommit).IsEqualTo(Commit);
        await Assert.That(clone.AlwaysOfficialData.CarriedBrake).IsTrue();
        await Assert.That(clone.AlwaysOfficialData.IsOfficial).IsTrue();

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
