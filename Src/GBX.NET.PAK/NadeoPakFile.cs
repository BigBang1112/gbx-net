using GBX.NET.Engines.MwFoundations;
using GBX.NET.Managers;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace GBX.NET.PAK;

public class NadeoPakFile
{
    private readonly NadeoPak owner;

    private byte[]? data;

    public int U01;

    public string Name { get; }
    public int UncompressedSize { get; }
    public int CompressedSize { get; }
    public int Offset { get; }
    public uint ClassID { get; }
    public ulong Flags { get; }
    public NadeoPakFolder? Folder { get; }

    public bool IsCompressed => (Flags & 0x7C) != 0;

    public bool IsHashed => Regex.IsMatch(Name, "^[0-9a-fA-F]{34}$", RegexOptions.Compiled);
    public byte? HashedNameLength
    {
        get
        {
            if (IsHashed)
                return Convert.ToByte(new string(Name.Substring(0, 2).ToCharArray().Reverse().ToArray()), 16);
            return null;
        }
    }

    public byte[]? Data => GetData();
    public GameBox GBX => GetGBX();
    public Node? Node => GetNode();

    public NadeoPakFile(NadeoPak owner, NadeoPakFolder? folder, string name, int uncompressedSize, int compressedSize, int offset, uint classID, ulong flags)
    {
        this.owner = owner;
        Folder = folder;
        Name = name;
        UncompressedSize = uncompressedSize;
        CompressedSize = compressedSize;
        Offset = offset;
        ClassID = classID;
        Flags = flags;
    }

    public string? GetClassName()
    {
        if (NodeCacheManager.Names.TryGetValue(ClassID, out string? className))
            return className;
        return null;
    }

    public string? GetClassNameWithoutNamespace()
    {
        var className = GetClassName();
        if (className == null) return null;
        return className.Split(new string[] { "::" }, StringSplitOptions.None)[1];
    }

    public override string ToString()
    {
        var className = GetClassName();
        if (className != null)
            return $"{Name} ({className})";
        return Name;
    }

    public Stream Open()
    {
        var mainStream = owner.Stream;

        mainStream.Position = owner.DataStart + Offset;

        var roundedDataSize = 8 + CompressedSize;

        if ((roundedDataSize & 7) != 0)
        {
            roundedDataSize = (roundedDataSize & ~7) + 8;
        }

        var buffer = new byte[roundedDataSize];

        mainStream.Read(buffer, 0, buffer.Length);

        var ms = new MemoryStream(buffer);
        var r = new GameBoxReader(ms);

        var iv = r.ReadUInt64();

        var blowfish = new BlowfishCBCStream(ms, owner.Key, iv, ignore256ivXorReset: false);

        if (!IsCompressed)
        {
            return blowfish;
        }

#if NET6_0_OR_GREATER
        return new ZLibStream(blowfish, CompressionMode.Decompress);
#else
        return new CompressedStream(blowfish, CompressionMode.Decompress);
#endif
    }

    public byte[]? GetData()
    {
        if (data is not null)
        {
            return data;
        }

        using var stream = Open();
        using var r = new BinaryReader(stream);

        try
        {
            data = r.ReadBytes(UncompressedSize);
        }
        catch (InvalidDataException)
        {

        }

        return data;
    }

    public GameBox GetGBX()
    {
        using var ms = Open();
        return GameBox.ParseHeader(ms);
    }

    public Node? GetNode()
    {
        using var ms = Open();
        return GameBox.ParseNodeHeader(ms);
    }

    public string GetFullFileName()
    {
        if (Folder == null) return Name;

        var currentParent = Folder;
        var folders = new List<string>
            {
                Name
            };

        while (currentParent != null)
        {
            folders.Insert(0, currentParent.Name);
            currentParent = currentParent.Parent;
        }

        return Path.Combine(folders.ToArray());
    }

    public string? GetFullDirectoryName()
    {
        return Path.GetDirectoryName(GetFullFileName());
    }

    public string GetFileName()
    {
        return Path.GetFileName(Name);
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(new string(hex.Substring(x, 2).ToCharArray().Reverse().ToArray()), 16))
                         .ToArray();
    }
}
