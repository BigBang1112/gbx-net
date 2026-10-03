using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameUserFileListTests
{
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
