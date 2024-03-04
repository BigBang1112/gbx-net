namespace GBX.NET;

internal static partial class GbxCompressionUtils
{
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> CompressAsync(Stream input, Stream output, CancellationToken cancellationToken)
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
            await input.CopyToAsync(output, bufferSize: 81920, cancellationToken);

            return false;
        }

        w.Write('C');

        await CopyRestOfTheHeaderAsync(version, r, w, cancellationToken);

        var uncompressedData = await r.ReadToEndAsync(cancellationToken);
        var compressedData = Gbx.LZO.Compress(uncompressedData);

        w.Write(uncompressedData.Length);
        w.Write(compressedData.Length);
        await w.WriteAsync(compressedData, cancellationToken);

        return true;
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> DecompressAsync(Stream input, Stream output, CancellationToken cancellationToken)
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
            await input.CopyToAsync(output, bufferSize: 81920, cancellationToken);

            return false;
        }

        w.Write('U');

        await CopyRestOfTheHeaderAsync(version, r, w, cancellationToken);

        var uncompressedSize = r.ReadInt32();
        var compressedSize = r.ReadInt32();
        var compressedData = await r.ReadBytesAsync(compressedSize, cancellationToken);

        var buffer = new byte[uncompressedSize];
        Gbx.LZO.Decompress(compressedData, buffer);
        await w.WriteAsync(buffer, cancellationToken);

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

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    private static async Task CopyRestOfTheHeaderAsync(short version, GbxReader r, GbxWriter w, CancellationToken cancellationToken)
    {
        if (version >= 4)
        {
            w.Write(r.ReadByte()); // Unknown byte
        }

        w.Write(r.ReadUInt32()); // Id

        if (version >= 6)
        {
            await w.WriteDataAsync(await r.ReadDataAsync(cancellationToken), cancellationToken); // User data
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
