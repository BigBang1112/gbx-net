using GBX.NET.Extensions;

namespace GBX.NET;

public sealed class CompressedData(int uncompressedSize, byte[] data)
{
    public int UncompressedSize { get; } = uncompressedSize;
    public byte[] Data { get; } = data;

    internal MemoryStream OpenDecompressedMemoryStream(IZLib zlib)
    {
        using var compressedMs = new MemoryStream(Data);
        var uncompressedMs = new MemoryStream(UncompressedSize);

        zlib.Decompress(compressedMs, uncompressedMs);

        uncompressedMs.Position = 0;
        return uncompressedMs;
    }

    internal MemoryStream OpenDecompressedMemoryStream()
    {
        if (Gbx.ZLib is null)
        {
            throw new ZLibNotDefinedException();
        }

        return OpenDecompressedMemoryStream(Gbx.ZLib);
    }
}
