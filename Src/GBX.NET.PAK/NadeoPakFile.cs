using GBX.NET.Engines.MwFoundations;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace GBX.NET.PAK;

public class NadeoPakFile
{
    private readonly NadeoPak owner;

    private byte[]? data;

    public int U01;

    public string Name { get; set; }
    public int UncompressedSize { get; }
    public int CompressedSize { get; }
    public int Offset { get; }
    public uint ClassID { get; }
    public ulong Flags { get; }
    public NadeoPakFolder? Folder { get; internal set; }

    public bool IsCompressed => (Flags & 0x7C) != 0;

    public bool IsGbx { get; }

    public bool IsHashed => Regex.IsMatch(Name, "^[0-9a-fA-F]{34}$", RegexOptions.Compiled);
    public byte? HashedNameLength
    {
        get
        {
            if (IsHashed)
            {
                return Convert.ToByte(new string(Name.Substring(0, 2).ToCharArray().Reverse().ToArray()), 16);
            }

            return null;
        }
    }

    public byte[]? Data => GetData();
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

        IsGbx = !NodeManager.TryGetExtension(ClassID, out _);
    }

    public string? GetClassName()
    {
        if (NodeManager.TryGetName(ClassID, out string? className))
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
        
        return new Arc.TrackMania.Compression.ZlibDeflateStream(blowfish, compressing: false);
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
        catch (Exception)
        {
            
        }

        return data;
    }

    public GameBox? ParseGbx(IProgress<GameBoxReadProgress>? progress = null, ILogger? logger = null)
    {
        if (!IsGbx)
        {
            return null;
        }

        using var ms = Open();

        var gbx = GameBox.Parse(ms, progress, readUncompressedBodyDirectly: true, logger);

        var fileNameBuilder = new StringBuilder(Name);

        var folder = Folder;

        while (folder is not null)
        {
            fileNameBuilder.Insert(0, folder.Name);

            folder = folder.Parent;
        }

        gbx.PakFileName = fileNameBuilder.ToString();
        gbx.ExternalGameData = owner;

        return gbx;
    }

    public GameBox? ParseGbxHeader(IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false, ILogger? logger = null)
    {
        if (!IsGbx)
        {
            return null;
        }

        using var ms = Open();

        return GameBox.ParseHeader(ms, progress, readRawBody, logger);
    }

    public Node? GetNode()
    {
        using var ms = Open();
        return GameBox.ParseNodeHeader(ms);
    }

    public string GetFullFileName()
    {
        return Path.Combine(GetDirectoryName(), Name);
    }

    /// <summary>
    /// Gets the directory name (relative to the PAK file), also including directories in <see cref="Name"/>.
    /// </summary>
    /// <returns>A full directory name.</returns>
    public string GetFullDirectoryName()
    {
        return Path.GetDirectoryName(GetFullFileName()) ?? "";
    }

    /// <summary>
    /// Gets the directory name (relative to the PAK file), not part of the <see cref="Name"/> itself.
    /// </summary>
    /// <returns></returns>
    public string GetDirectoryName()
    {
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        return string.Join(Path.DirectorySeparatorChar, GetParentDirectories().Reverse());
#else
        return Path.Combine(GetParentDirectories().Reverse().ToArray());
#endif
    }

    private IEnumerable<string> GetParentDirectories()
    {
        var currentParent = Folder;
        
        while (currentParent is not null)
        {
            yield return currentParent.Name;
            currentParent = currentParent.Parent;
        }
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
