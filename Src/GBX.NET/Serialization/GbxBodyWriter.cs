using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxBodyWriter(GbxBody body, GbxWriter writer, GbxWriteSettings settings)
{
    internal void WriteUncompressed(IClass? node)
    {
        using var readerWriter = new GbxReaderWriter(writer, leaveOpen: true);

        (node ?? throw new Exception("Node cannot be null for a known header, as it contains the header chunk instances."))
            .ReadWrite(readerWriter);
    }

    internal void WriteRaw()
    {
        if (body.CompressedSize.HasValue)
        {
            writer.Write(body.UncompressedSize);
            writer.Write(body.CompressedSize.Value);
        }

        writer.Write(body.RawData.ToArray());
    }

    internal void Write(MemoryStream uncompressedInputStream, GbxCompression compression)
    {
        switch (compression)
        {
            case GbxCompression.Compressed:
                var compressedData = CompressData(uncompressedInputStream.ToArray());
                writer.Write((int)uncompressedInputStream.Length);
                writer.Write(compressedData.Length);
                writer.Write(compressedData);
                break;

            case GbxCompression.Uncompressed:
                uncompressedInputStream.CopyTo(writer.BaseStream);
                break;

            default:
                throw new ArgumentException("Unknown compression type.", nameof(compression));
        }
    }

    private static byte[] CompressData(byte[] inputData)
    {
        if (Gbx.LZO is null)
        {
            throw new LzoNotDefinedException();
        }

        return Gbx.LZO.Compress(inputData);
    }
}