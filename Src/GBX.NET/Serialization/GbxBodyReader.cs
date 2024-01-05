using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxBodyReader(GbxReaderWriter readerWriter, GbxReadSettings settings, GbxCompression compression)
{
    private readonly GbxReader reader = readerWriter.Reader ?? throw new Exception("Reader is required but not available.");

    public static GbxBody Parse(GbxReader reader, GbxCompression compression, bool readRawBody)
    {
        switch (compression)
        {
            case GbxCompression.Compressed:

                var uncompressedSize = reader.ReadInt32();
                var compressedSize = reader.ReadInt32();
                var rawData = readRawBody ? reader.ReadBytes(compressedSize) : null;

                return new GbxBody
                {
                    UncompressedSize = uncompressedSize,
                    CompressedSize = compressedSize,
                    RawData = rawData
                };

            case GbxCompression.Uncompressed:

                return new GbxBody
                {
                    RawData = readRawBody ? reader.ReadToEnd() : null
                };

            default:
                throw new Exception($"Unknown compression type: {compression}");
        }
    }

    public GbxBody Parse(IClass node)
    {
        var body = Parse(reader, compression, readRawBody: false);

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
        var body = Parse(reader, compression, readRawBody: false);

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
        var compressedData = reader.ReadBytes(compressedSize);
        var decompressedData = new byte[uncompressedSize];

        if (Gbx.LZO is null)
        {
            throw new Exception("LZO is required but not available.");
        }

        Gbx.LZO.Decompress(compressedData, decompressedData);

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
            if (!settings.AvoidExceptionsInBody)
            {
                throw;
            }

            body.Exception = ex;
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
            if (!settings.AvoidExceptionsInBody)
            {
                throw;
            }

            body.Exception = ex;
        }
    }
}
