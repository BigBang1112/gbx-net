namespace GBX.NET.Components;

/// <summary>
/// The most basic Gbx header data that does not have any class specifics.
/// </summary>
/// <param name="Version">Version of Gbx.</param>
/// <param name="Format">Format of Gbx.</param>
/// <param name="CompressionOfRefTable">Compression of reference table.</param>
/// <param name="CompressionOfBody">Compression of body.</param>
/// <param name="Mode">Editor ('E') or release ('R') serialization mode. Headers before version 4 omit this byte and use editor mode.</param>
public readonly record struct GbxHeaderBasic(
    ushort Version,
    GbxFormat Format,
    GbxCompression CompressionOfRefTable,
    GbxCompression CompressionOfBody,
    GbxMode Mode)
{
    public static readonly GbxHeaderBasic Default = Create();

    public static GbxHeaderBasic Create(
        ushort version = 6,
        GbxFormat format = GbxFormat.Binary,
        GbxCompression compressionOfRefTable = GbxCompression.Uncompressed,
        GbxCompression compressionOfBody = GbxCompression.Compressed,
        GbxMode mode = GbxMode.Release)
    {
        return new GbxHeaderBasic(version, format, compressionOfRefTable, compressionOfBody, mode);
    }

    internal static GbxHeaderBasic Parse(GbxReader reader)
    {
        var r = reader;

        // GBX magic
        if (!r.ReadGbxMagic())
        {
            throw new NotAGbxException();
        }

        var version = r.ReadUInt16();
        var format = r.ReadFormatByte();
        var compressionOfRefTable = (GbxCompression)r.ReadByte();
        var compressionOfBody = (GbxCompression)r.ReadByte();
        var mode = GbxMode.Editor;

        if (version >= 4)
        {
            mode = (GbxMode)r.ReadByte();

            if (mode is not GbxMode.Editor and not GbxMode.Release)
            {
                throw new InvalidDataException($"Invalid Gbx serialization mode byte: 0x{(byte)mode:X2}.");
            }
        }

        r.IsRelease = mode == GbxMode.Release;

        return new GbxHeaderBasic(version, format, compressionOfRefTable, compressionOfBody, mode);
    }

    /// <summary>
    /// Parses a basic header from the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the data to parse.</param>
    /// <param name="settings">The settings to use when reading the data.</param>
    /// <returns>A <see cref="GbxHeaderBasic"/> instance parsed from the input <paramref name="stream"/>.</returns>
    public static GbxHeaderBasic Parse(Stream stream, GbxReadSettings settings = default)
    {
        using var r = new GbxReader(stream, settings);
        return Parse(r);
    }

    /// <summary>
    /// Parses a basic header from the specified file.
    /// </summary>
    /// <param name="fileName">The name of the file to parse.</param>
    /// <param name="settings">The settings to use when reading the data.</param>
    /// <returns>A <see cref="GbxHeaderBasic"/> instance parsed from the specified file.</returns>
    public static GbxHeaderBasic Parse(string fileName, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, settings);
    }

    internal bool Write(GbxWriter writer)
    {
        var w = writer;

        if (Version >= 4 && Mode is not GbxMode.Editor and not GbxMode.Release)
        {
            throw new InvalidDataException($"Invalid Gbx serialization mode byte: 0x{(byte)Mode:X2}.");
        }

        w.IsRelease = Version >= 4 && Mode == GbxMode.Release;

        // GBX magic
        w.WriteGbxMagic();

        w.Write(Version);
        w.WriteFormat(Format);
        w.Write((byte)CompressionOfRefTable);
        w.Write((byte)CompressionOfBody);

        if (Version >= 4)
        {
            w.Write((byte)Mode);
        }

        return true;
    }
}
