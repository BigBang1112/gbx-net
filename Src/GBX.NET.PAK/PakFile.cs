namespace GBX.NET.PAK;

public sealed class PakFile
{
    public string Name { get; }
    public string FolderPath { get; }
    public uint ClassId { get; }
    public uint Offset { get; }
    public int UncompressedSize { get; }
    public int CompressedSize { get; }
    public int? Size { get; }
    public UInt128? Checksum { get; }
    public ulong Flags { get; }
    public bool DontUseDummyWrite => (Flags & 0x100000000) != 0;
    public bool PublicFile => (Flags & 0x2000000000000) != 0;
    public bool ForceNoCrypt => (Flags & 0x4000000000000) != 0;
    public bool IsCompressed => (Flags & 0x3C) != 0;
    public bool IsEncrypted => !ForceNoCrypt && !PublicFile;

    public PakFile(
        string name,
        string folderPath, 
        uint classId, 
        uint offset, 
        int uncompressedSize, 
        int compressedSize,
        int? size,
        UInt128? checksum,
        ulong flags)
    {
        Name = name;
        FolderPath = folderPath;
        ClassId = classId;
        Offset = offset;
        UncompressedSize = uncompressedSize;
        CompressedSize = compressedSize;
        Size = size;
        Checksum = checksum;
        Flags = flags;
    }

    public override string ToString()
    {
        return $"{Name}, Folder: {FolderPath}";
    }
}