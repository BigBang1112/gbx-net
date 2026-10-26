namespace GBX.NET.PAK;

public sealed class PakFile
{
    public string Name { get; }
    public string FolderPath { get; }
    public uint ClassId { get; }
    
    /// <summary>
    /// Offset of the file within the Pak archive. This can be <see cref="uint.MaxValue"/> if the file data is not available anywhere.
    /// </summary>
    public uint Offset { get; }

    public int UncompressedSize { get; }
    public int CompressedSize { get; }

    /// <summary>
    /// Stored size including alignment padding. Available from Pak version 17.
    /// </summary>
    public int? Size { get; }

    public UInt128? Checksum { get; }
    public ulong Flags { get; }

    /// <summary>
    /// Unidentified unsigned field serialized immediately before the file sizes.
    /// </summary>
    public uint U01 { get; }

    public bool DontUseDummyWrite => (Flags & 0x100000000) != 0;
    public bool PublicFile => (Flags & 0x2000000000000) != 0;
    public bool ForceNoCrypt => (Flags & 0x4000000000000) != 0;
    public bool IsCompressed => (Flags & 0x3C) != 0;

    /// <summary>
    /// Whether encrypted data uses the Blowfish CTR cipher instead of the chained cipher. Applies from Pak version 11.
    /// </summary>
    public bool UsesCounterMode => (Flags & 0x40) != 0;
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
        ulong flags) : this(name, folderPath, classId, offset, uncompressedSize, compressedSize, size, checksum, flags, 0)
    {
    }

    public PakFile(
        string name,
        string folderPath,
        uint classId,
        uint offset,
        int uncompressedSize,
        int compressedSize,
        int? size,
        UInt128? checksum,
        ulong flags,
        uint u01)
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
        U01 = u01;
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(FolderPath))
        {
            return Name;
        }

        return $"{Name}, Folder: {FolderPath}";
    }
}
