namespace GBX.NET;

public sealed class CompressedData(int uncompressedSize, byte[] data)
{
    public int UncompressedSize { get; } = uncompressedSize;
    public byte[] Data { get; } = data;
}
