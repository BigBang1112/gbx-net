namespace GBX.NET.Extensions;

public interface ILzo
{
    void Decompress(byte[] input, byte[] output);
    byte[] Compress(byte[] data);
}
