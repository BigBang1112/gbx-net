namespace GBX.NET;

internal static class GbxCompressor
{
    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">GBX stream to decompress.</param>
    /// <param name="output">Output GBX stream in the decompressed form.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static void Decompress(Stream input, Stream output)
    {
        using var r = new GameBoxReader(input);
        using var w = new GameBoxWriter(output);
        var rw = new GameBoxReaderWriter(r, w);

        var version = CopyBasicInformation(r, w);

        // Body compression type
        var compressedBody = r.ReadByte();

        if (compressedBody != 'C')
        {
            w.Write(compressedBody);
            input.CopyTo(output);
            return;
        }

        w.Write('U');

        CopyRestOfTheHeader(version, r, w);

        var uncompressedSize = r.ReadInt32();
        var compressedData = r.ReadBytes();

        var buffer = new byte[uncompressedSize];
        Lzo.Decompress(compressedData, buffer);
        w.Write(buffer);
    }

    public static void Compress(Stream input, Stream output)
    {
        using var r = new GameBoxReader(input);
        using var w = new GameBoxWriter(output);

        var version = CopyBasicInformation(r, w);

        // Body compression type
        var compressedBody = r.ReadByte();

        if (compressedBody != 'U')
        {
            input.CopyTo(output);
            return;
        }

        w.Write('C');

        CopyRestOfTheHeader(version, r, w);

        var uncompressedData = r.ReadToEnd();
        var compressedData = Lzo.Compress(uncompressedData);

        w.Write(uncompressedData.Length);
        w.Write(compressedData.Length);
        w.Write(compressedData);
    }

    private static void CopyRestOfTheHeader(short version, GameBoxReader r, GameBoxWriter w)
    {
        var rw = new GameBoxReaderWriter(r, w);

        if (version >= 4)
        {
            rw.Byte(); // Unknown byte
        }

        rw.Int32(); // Id

        if (version >= 6)
        {
            rw.Bytes(); // User data
        }

        rw.Int32(); // Num nodes

        var numExternalNodes = rw.Int32();

        if (numExternalNodes <= 0)
        {
            return;
        }
        
        rw.Int32(); // Ancestor level

        RecurseSubFolders(rw);

        for (var i = 0; i < numExternalNodes; i++)
        {
            var flags = rw.Int32();

            if ((flags & 4) == 0)
            {
                rw.String(); // File name
            }
            else
            {
                rw.Int32(); // Resource index
            }

            rw.Int32(); // Node index

            if (version >= 5)
            {
                rw.Boolean(); // Use file
            }

            if ((flags & 4) == 0)
            {
                rw.Int32(); // Folder index
            }
        }
    }

    private static short CopyBasicInformation(GameBoxReader r, GameBoxWriter w)
    {
        // Magic
        if (!r.HasMagic(GameBox.Magic))
        {
            throw new NotAGbxException();
        }

        w.Write(GameBox.Magic, StringLengthPrefix.None);

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

    private static void RecurseSubFolders(GameBoxReaderWriter rw)
    {
        var numSubFolders = rw.Int32();

        for (int i = 0; i < numSubFolders; i++)
        {
            rw.String(); // name
            RecurseSubFolders(rw);
        }
    }
}
