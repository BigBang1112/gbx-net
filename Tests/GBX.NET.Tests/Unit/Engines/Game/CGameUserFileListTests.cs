using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameUserFileListTests
{
    [Test]
    [Arguments(CGameUserFileList.FileType.Map, (byte)0)]
    [Arguments(CGameUserFileList.FileType.Map, (byte)0xA5)]
    [Arguments(CGameUserFileList.FileType.Ghost, (byte)0)]
    [Arguments(CGameUserFileList.FileType.Ghost, (byte)0xA5)]
    public async Task ReadWrite_FileInfo_PreservesMetadataAndTerminators(CGameUserFileList.FileType type, byte terminator)
    {
        const long fileTime = 132537600000000000;
        var payload = Payload(type, terminator, fileTime);
        var file = new CGameUserFileList.FileInfo();
        using (var stream = new MemoryStream(payload))
        using (var reader = new GbxReader(stream))
        using (var rw = new GbxReaderWriter(reader))
        {
            file.ReadWrite(rw);
            await Assert.That(stream.Position).IsEqualTo(stream.Length);
        }

        await Assert.That(file.FileWriteTime?.ToFileTimeUtc()).IsEqualTo(fileTime);
        await Assert.That(file.FileSize).IsEqualTo(456ul);
        await Assert.That(file.U01).IsEqualTo(terminator);
        await Assert.That(type == CGameUserFileList.FileType.Map ? file.MapNameTerminator : file.RecordingContextTerminator)
            .IsEqualTo(terminator);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) file.ReadWrite(rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public void ReadWrite_FileInfo_RejectsUnknownFileTypes(bool read)
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
