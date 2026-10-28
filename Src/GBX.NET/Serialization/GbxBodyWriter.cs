using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxBodyWriter(GbxBody body, GbxWriter writer)
{
    private readonly ILogger? logger = writer.Logger;

    internal void WriteUncompressed(CMwNod node)
    {
        writer.NodeDict[node] = 0;

        logger?.LogInformation("Writing main node...");

        using (logger?.BeginScope("{ClassName} (main)", ClassManager.GetName(node.GetType())))
        {
            using var readerWriter = new GbxReaderWriter(writer);
            node.ReadWrite(readerWriter);
        }

        logger?.LogInformation("Main node complete.");
    }

    internal void WriteRaw()
    {
        if (body.CompressedSize.HasValue)
        {
            if (logger is not null)
            {
                logger.LogInformation("Uncompressed body size: {UncompressedSize}", body.UncompressedSize);
                logger.LogInformation("Compressed body size: {CompressedSize}", body.CompressedSize.Value);
            }

            writer.Write(body.UncompressedSize);
            writer.Write(body.CompressedSize.Value);
        }
        else
        {
            logger?.LogInformation("Uncompressed body.");
        }

        logger?.LogInformation("RawBody mode: writing {Length} bytes directly...", body.RawData.Length);

        writer.Write(body.RawData.ToArray());
    }

    internal void Write(MemoryStream uncompressedInputStream, GbxCompression compression)
    {
        switch (compression)
        {
            case GbxCompression.Compressed:
                logger?.LogInformation("Compressing body...");

                var compressedData = CompressData(uncompressedInputStream.ToArray());

                if (logger is not null)
                {
                    logger.LogInformation("Uncompressed body size: {UncompressedSize}", uncompressedInputStream.Length);
                    logger.LogInformation("Compressed body size: {CompressedSize}", compressedData.Length);
                }

                writer.Write((int)uncompressedInputStream.Length);
                writer.Write(compressedData.Length);
                writer.Write(compressedData);
                break;

            case GbxCompression.Uncompressed:
                logger?.LogInformation("Uncompressed body.");
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
