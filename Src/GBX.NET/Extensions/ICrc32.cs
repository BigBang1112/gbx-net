namespace GBX.NET.Extensions;

public interface ICrc32
{
    uint Hash(byte[] source);
#if NET5_0_OR_GREATER
    uint Hash(ReadOnlySpan<byte> source);
#endif
}
