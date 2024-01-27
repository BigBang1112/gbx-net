namespace GBX.NET.Extensions;

public interface ILzo
{
    void Decompress(in Span<byte> input, byte[] output);
    byte[] Compress(byte[] data);
}
