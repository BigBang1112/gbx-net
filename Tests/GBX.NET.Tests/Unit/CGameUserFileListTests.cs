using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit;

public class CGameUserFileListTests
{
    [Test]
    [Arguments(CGameUserFileList.FileType.Map, 0)]
    [Arguments(CGameUserFileList.FileType.Ghost, 0)]
    [Arguments(CGameUserFileList.FileType.Map, 127)]
    [Arguments(CGameUserFileList.FileType.Ghost, 127)]
    public async Task FileInfo_ReadsNativeLayoutAndWritesZeroTerminators(CGameUserFileList.FileType type, int terminator)
    {
        var payload = Payload(type, (byte)terminator);
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
        await Assert.That(file.U01).IsEqualTo(123ul);
        await Assert.That(file.U02).IsEqualTo(456ul);
        await Assert.That(file.Type).IsEqualTo(type);
        await Assert.That(file.MapUid).IsEqualTo("MapUid");
        if (type == CGameUserFileList.FileType.Map)
        {
            await Assert.That(file.MapName).IsEqualTo("Map Name");
            await Assert.That(file.GhostKind).IsNull();
            await Assert.That(file.U04).IsNull();
        }
        else
        {
            await Assert.That(file.GhostKind).IsEqualTo("Race");
            await Assert.That(file.MapName).IsNull();
            await Assert.That(file.U04).IsEqualTo(789);
        }
        await Assert.That(stream.Position).IsEqualTo((long)payload.Length);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var rewritten = new MemoryStream();
        using (var writer = new GbxWriter(rewritten))
        using (var writerWriter = new GbxReaderWriter(writer))
        {
            file.ReadWrite(writerWriter, 7);
        }
        await Assert.That(rewritten.ToArray().SequenceEqual(Payload(type, 0))).IsTrue();
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

    private static byte[] Payload(CGameUserFileList.FileType type, byte terminator)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write("Example.Gbx");
        writer.Write(terminator);
        writer.Write(123ul);
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
            writer.Write(789);
        }
        return stream.ToArray();
    }
}
