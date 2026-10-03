using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameUserFileListTests
{
    [Test]
    [Arguments(CGameUserFileList.FileType.Map, 0, 123L, 789)]
    [Arguments(CGameUserFileList.FileType.Ghost, 0, 123L, 789)]
    [Arguments(CGameUserFileList.FileType.Map, 127, 123L, 789)]
    [Arguments(CGameUserFileList.FileType.Ghost, 127, 123L, 789)]
    [Arguments(CGameUserFileList.FileType.Map, 0, 0L, -1)]
    [Arguments(CGameUserFileList.FileType.Ghost, 0, 0L, -1)]
    [Arguments(CGameUserFileList.FileType.Ghost, 0, 134354592000000000L, 0)]
    public async Task FileInfo_ReadsNativeLayoutAndPreservesTerminators(CGameUserFileList.FileType type, int terminator, long fileTime, int raceTime)
    {
        var payload = Payload(type, (byte)terminator, fileTime, raceTime);
        using var stream = new MemoryStream();
        stream.Write(payload);
        using (var writer = new GbxWriter(stream)) writer.Write(0x12345678);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        var file = new CGameUserFileList.FileInfo();
        file.ReadWrite(rw, 7);

        await Assert.That(file.Name).IsEqualTo("Example.Gbx");
        await Assert.That(file.ToString()).IsEqualTo("Example.Gbx");
        await Assert.That(file.FileWriteTime?.ToFileTimeUtc()).IsEqualTo(fileTime == 0 ? null : (long?)fileTime);
        await Assert.That(file.FileSize).IsEqualTo(456ul);
        await Assert.That(file.Type).IsEqualTo(type);
        await Assert.That(file.MapUid).IsEqualTo("MapUid");
        if (type == CGameUserFileList.FileType.Map)
        {
            await Assert.That(file.MapName).IsEqualTo("Map Name");
            await Assert.That(file.RecordingContext).IsNull();
            await Assert.That(file.RaceTime).IsNull();
        }
        else
        {
            await Assert.That(file.RecordingContext).IsEqualTo("Race");
            await Assert.That(file.MapName).IsNull();
            await Assert.That(file.RaceTime?.TotalMilliseconds).IsEqualTo(raceTime == -1 ? null : (int?)raceTime);
        }
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            file.ReadWrite(writerWriter, 7);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public void FileInfo_RejectsUnknownFileTypes(bool read)
    {
        using var stream = read ? new MemoryStream(Payload((CGameUserFileList.FileType)2, 0)) : new MemoryStream();
        using var reader = new GbxReader(stream);
        using var writer = new GbxWriter(stream);
        using var rw = read ? new GbxReaderWriter(reader) : new GbxReaderWriter(writer);
        var file = new CGameUserFileList.FileInfo { Type = (CGameUserFileList.FileType)2 };
        Assert.Throws<ThisShouldNotHappenException>(() => file.ReadWrite(rw));
    }

    private static byte[] Payload(CGameUserFileList.FileType type, byte terminator, long fileTime = 123, int raceTime = 789)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write("Example.Gbx");
        writer.Write(terminator);
        writer.Write(fileTime);
        writer.Write(456ul);
        writer.Write((int)type);
        if (type == CGameUserFileList.FileType.Map)
        {
            writer.WriteIdAsString("MapUid");
            writer.Write("Map Name");
            writer.Write(terminator);
        }
        else
        {
            writer.Write("Race");
            writer.Write(terminator);
            writer.WriteIdAsString("MapUid");
            writer.Write(raceTime);
        }
        return stream.ToArray();
    }
}
