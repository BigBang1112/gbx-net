using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxBodyReader(GbxReaderWriter readerWriter, GbxReadSettings settings, GbxCompression compression)
{
    private readonly GbxReader reader = readerWriter.Reader ?? throw new Exception("Reader is required but not available.");

    public static GbxBody Parse(GbxReader reader, GbxCompression compression, GbxReadSettings settings)
    {
        switch (compression)
        {
            case GbxCompression.Compressed:

                var uncompressedSize = reader.ReadInt32();

                if (settings.MaxUncompressedBodySize.HasValue && uncompressedSize > settings.MaxUncompressedBodySize.Value)
                {
                    throw new Exception($"Uncompressed body (size {uncompressedSize}B) exceeds maximum allowed size {settings.MaxUncompressedBodySize}B.");
                }

                var compressedSize = reader.ReadInt32();
                var rawData = settings.ReadRawBody ? reader.ReadBytes(compressedSize) : null;

                return new GbxBody
                {
                    UncompressedSize = uncompressedSize,
                    CompressedSize = compressedSize,
                    RawData = rawData
                };

            case GbxCompression.Uncompressed:

                return new GbxBody
                {
                    RawData = settings.ReadRawBody ? reader.ReadToEnd() : null
                };

            default:
                throw new Exception($"Unknown compression type: {compression}");
        }
    }

    public GbxBody Parse(IClass node)
    {
        var body = Parse(reader, compression, settings);

        if (body.CompressedSize is null)
        {
            ReadMainNode(node, body, readerWriter);
            return body;
        }

        var decompressedData = DecompressData(body.CompressedSize.Value, body.UncompressedSize);

        using var ms = new MemoryStream(decompressedData);
        using var decompressedReader = new GbxReader(ms);
        using var decompressedReaderWriter = new GbxReaderWriter(decompressedReader);

        ReadMainNode(node, body, decompressedReaderWriter);

        return body;
    }

    public GbxBody Parse<T>(T node) where T : IClass
    {
        var body = Parse(reader, compression, settings);

        if (body.CompressedSize is null)
        {
            ReadMainNode(node, body, readerWriter);
            return body;
        }

        var decompressedData = DecompressData(body.CompressedSize.Value, body.UncompressedSize);

        using var ms = new MemoryStream(decompressedData);
        using var decompressedReader = new GbxReader(ms);
        using var decompressedReaderWriter = new GbxReaderWriter(decompressedReader);

        ReadMainNode(node, body, decompressedReaderWriter);

        return body;
    }

    private byte[] DecompressData(int compressedSize, int uncompressedSize)
    {
#if NET5_0_OR_GREATER
        Span<byte> compressedData = stackalloc byte[compressedSize];
        if (reader.Read(compressedData) != compressedSize)
        {
            throw new Exception("Failed to read compressed data");
        }
#else
        var compressedData = reader.ReadBytes(compressedSize);
#endif
        var decompressedData = new byte[uncompressedSize];

        if (Gbx.LZO is null)
        {
            throw new Exception("LZO is required but not available.");
        }

#if NET5_0_OR_GREATER
        Gbx.LZO.Decompress(in compressedData, decompressedData);
#else
        Gbx.LZO.Decompress(compressedData, decompressedData);
#endif

        return decompressedData;
    }

    private void ReadMainNode(IClass node, GbxBody body, GbxReaderWriter rw)
    {
        try
        {
            node.ReadWrite(rw);
        }
        catch (Exception ex)
        {
            body.Exception = ex;

            if (!settings.SkipExceptionsInBody)
            {
                throw;
            }
        }
    }

    private void ReadMainNode<T>(T node, GbxBody body, GbxReaderWriter rw) where T : IClass
    {
        try
        {
#if NET8_0_OR_GREATER
            T.Read(node, rw);
#else
            node.ReadWrite(rw);
#endif
        }
        catch (Exception ex)
        {
            body.Exception = ex;

            if (!settings.SkipExceptionsInBody)
            {
                throw;
            }
        }
    }
}
