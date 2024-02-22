namespace GBX.NET;

internal static class GbxCompressionUtils
{
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Compress(Stream input, Stream output)
    {
        Validate(input, output);

        if (Gbx.LZO is null)
        {
            throw new LzoNotDefinedException();
        }

        using var r = new GbxReader(input);
        using var w = new GbxWriter(output);

        var version = CopyBasicInformation(r, w);

        // Body compression type
        var compressedBody = r.ReadByte();

        if (compressedBody != 'U')
        {
            input.CopyTo(output);

            return false;
        }

        w.Write('C');

        CopyRestOfTheHeader(version, r, w);

        var uncompressedData = r.ReadToEnd();
        var compressedData = Gbx.LZO.Compress(uncompressedData);

        w.Write(uncompressedData.Length);
        w.Write(compressedData.Length);
        w.Write(compressedData);

        return true;
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Decompress(Stream input, Stream output)
    {
        Validate(input, output);

        if (Gbx.LZO is null)
        {
            throw new LzoNotDefinedException();
        }

        using var r = new GbxReader(input);
        using var w = new GbxWriter(output);

        var version = CopyBasicInformation(r, w);

        // Body compression type
        var compressedBody = r.ReadByte();

        if (compressedBody != 'C')
        {
            w.Write(compressedBody);
            input.CopyTo(output);

            return false;
        }

        w.Write('U');

        CopyRestOfTheHeader(version, r, w);

        var uncompressedSize = r.ReadInt32();
        var compressedData = r.ReadData();

        var buffer = new byte[uncompressedSize];
        Gbx.LZO.Decompress(compressedData, buffer);
        w.Write(buffer);

        return true;
    }

    private static void Validate(Stream input, Stream output)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (output is null)
        {
            throw new ArgumentNullException(nameof(output));
        }

        if (input.CanRead is false)
        {
            throw new ArgumentException("Input stream must be readable.", nameof(input));
        }

        if (output.CanWrite is false)
        {
            throw new ArgumentException("Output stream must be writable.", nameof(output));
        }
    }

    private static short CopyBasicInformation(GbxReader r, GbxWriter w)
    {
        // Magic
        if (!r.ReadGbxMagic())
        {
            throw new NotAGbxException();
        }

        w.WriteGbxMagic();

        // Version
        var version = r.ReadInt16();

        if (version < 3)
        {
            throw new VersionNotSupportedException(version);
        }

        w.Write(version);

        // Format
        var format = r.ReadByte();

        if (format != 'B')
        {
            throw new TextFormatNotSupportedException();
        }

        w.Write(format);

        w.Write(r.ReadByte()); // Ref table compression

        return version;
    }

    private static void CopyRestOfTheHeader(short version, GbxReader r, GbxWriter w)
    {
        if (version >= 4)
        {
            w.Write(r.ReadByte()); // Unknown byte
        }

        w.Write(r.ReadUInt32()); // Id

        if (version >= 6)
        {
            w.WriteData(r.ReadData()); // User data
        }

        w.Write(r.ReadInt32()); // Num nodes

        var numExternalNodes = r.ReadInt32();
        w.Write(numExternalNodes);

        if (numExternalNodes <= 0)
        {
            return;
        }

        w.Write(r.ReadInt32()); // Ancestor level

        RecurseSubFolders(r, w);

        for (var i = 0; i < numExternalNodes; i++)
        {
            var flags = r.ReadInt32();
            w.Write(flags);

            if ((flags & 4) == 0)
            {
                w.Write(r.ReadString()); // File name
            }
            else
            {
                w.Write(r.ReadInt32()); // Resource index
            }

            w.Write(r.ReadInt32()); // Node index

            if (version >= 5)
            {
                w.Write(r.ReadBoolean()); // Use file
            }

            if ((flags & 4) == 0)
            {
                w.Write(r.ReadInt32()); // Folder index
            }
        }

        static void RecurseSubFolders(GbxReader r, GbxWriter w)
        {
            var numSubFolders = r.ReadInt32();
            w.Write(numSubFolders);

            for (int i = 0; i < numSubFolders; i++)
            {
                w.Write(r.ReadString()); // name
                RecurseSubFolders(r, w);
            }
        }
    }
}
