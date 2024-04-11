namespace GBX.NET.Extensions;

public interface IZLib
{
    byte[] Compress(ReadOnlySpan<byte> input);
    void Decompress(ReadOnlySpan<byte> input, Span<byte> output);
}
