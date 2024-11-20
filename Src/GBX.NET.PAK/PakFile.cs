namespace GBX.NET.PAK;

public sealed class PakFile
{
    public string Name { get; }
    public string FolderPath { get; }
    public uint ClassId { get; }
    public int Offset { get; }
    public int UncompressedSize { get; }
    public int CompressedSize { get; }
    public ulong Flags { get; }

    public bool IsCompressed => (Flags & 0x7C) != 0;

    public PakFile(
        string name,
        string folderPath, 
        uint classId, 
        int offset, 
        int uncompressedSize, 
        int compressedSize, 
        ulong flags)
    {
        Name = name;
        FolderPath = folderPath;
        ClassId = classId;
        Offset = offset;
        UncompressedSize = uncompressedSize;
        CompressedSize = compressedSize;
        Flags = flags;
    }

    public override string ToString()
    {
        return $"{Name}, Folder: {FolderPath}";
    }
}