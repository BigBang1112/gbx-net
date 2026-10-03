using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameCtnGhostTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task RecordContext_ReadsScopeAndGameMode(int scope)
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(scope);
            w.Write("SeasonId");
            w.Write("TrackMania/TM_Race");
            w.Write("CustomData");
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        new CGameCtnGhost.Chunk03092029().ReadWrite(ghost, rw);

        await Assert.That((int)ghost.Validate_ScopeType).IsEqualTo(scope);
        await Assert.That(ghost.Validate_ScopeId).IsEqualTo("SeasonId");
        await Assert.That(ghost.Validate_GameMode).IsEqualTo("TrackMania/TM_Race");
        await Assert.That(ghost.Validate_GameModeCustomData).IsEqualTo("CustomData");
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(120100)]
    public async Task ValidationSettings_ReadsPackedRulesAndSimulationTime(int startTime)
    {
        var payload = Payload(w =>
        {
            w.Write(0xFEDCBA98u);
            w.Write(startTime);
        });
        using var stream = new MemoryStream(payload);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        new CGameCtnGhost.Chunk0309202A().ReadWrite(ghost, rw);

        await Assert.That(ghost.Validate_GameRules).IsEqualTo(0xFEDCBA98u);
        await Assert.That(ghost.Validate_RaceStartTime?.TotalMilliseconds).IsEqualTo(startTime == -1 ? null : (int?)startTime);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            new CGameCtnGhost.Chunk0309202A().ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();

        var validationPayload = Payload(w =>
        {
            w.Write(0); // no input store
            w.Write(""); // executable version
            w.Write(0u); // executable checksum
            w.Write(0); // OS kind
            w.Write(0); // CPU kind
            w.Write(-1); // walltime start
            w.Write(-1); // walltime end
            w.Write(""); // title ID
            w.Write(new byte[32]); // title checksum
            w.Write(0xFEDCBA98u);
            w.Write(startTime);
            w.Write(-1); // validation seed
            w.Write(0); // simulation flags
            w.Write(""); // race settings
        });
        using var validationStream = new MemoryStream(validationPayload);
        using var validationReader = new GbxReader(validationStream);
        using var validationRw = new GbxReaderWriter(validationReader);
        var validationGhost = new CGameCtnGhost();
        var validationChunk = new CGameCtnGhost.Chunk0309202D();
        validationChunk.ReadWrite(validationGhost, validationRw);
        await Assert.That(validationGhost.Validate_GameRules).IsEqualTo(ghost.Validate_GameRules);
        await Assert.That(validationGhost.Validate_RaceStartTime).IsEqualTo(ghost.Validate_RaceStartTime);
        await Assert.That(validationStream.Position).IsEqualTo((long)validationPayload.Length);
        var validationRewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            validationChunk.ReadWrite(validationGhost, writerWriter);
        });
        await Assert.That(validationRewritten.SequenceEqual(validationPayload)).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task RaceResult_ReadsNativeFormatAndPreservesUnknowns(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(12345); // time
            w.Write(0); // stunts score
            w.Write(2); // respawns
            w.Write(0x12345678); // extra result field
            w.Write(2); // checkpoint count
            w.Write(4567);
            w.Write(37); // checkpoint identifier
            w.Write(12345);
            w.Write(-1); // missing identifier
            w.Write(0x76543210); // trailing ghost field
        });
        using var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new GbxWriter(stream)) writer.Write(0x11223344);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var ghost = new CGameCtnGhost();
        var chunk = new CGameCtnGhost.Chunk0309202B();
        chunk.ReadWrite(ghost, rw);

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(ghost.RaceTime?.TotalMilliseconds).IsEqualTo(12345);
        await Assert.That(ghost.StuntScore).IsEqualTo(0);
        await Assert.That(ghost.Respawns).IsEqualTo(2);
        await Assert.That(chunk.U01).IsEqualTo(0x12345678);
        await Assert.That(chunk.U02).IsEqualTo(0x76543210);
        await Assert.That(ghost.Checkpoints!.Length).IsEqualTo(2);
        await Assert.That(ghost.Checkpoints[0].Time?.TotalMilliseconds).IsEqualTo(4567);
        await Assert.That(ghost.Checkpoints[0].CheckpointId).IsEqualTo(37);
        await Assert.That(ghost.Checkpoints[1].Time?.TotalMilliseconds).IsEqualTo(12345);
        await Assert.That(ghost.Checkpoints[1].CheckpointId).IsEqualTo(-1);
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);

        var rewritten = Payload(w =>
        {
            using var writerWriter = new GbxReaderWriter(w);
            chunk.ReadWrite(ghost, writerWriter);
        });
        await Assert.That(rewritten.SequenceEqual(payload)).IsTrue();
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) write(writer);
        return stream.ToArray();
    }
}
