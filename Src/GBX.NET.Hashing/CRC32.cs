using GBX.NET.Extensions;
using System.IO.Hashing;

namespace GBX.NET.Hashing;

public sealed class CRC32 : ICrc32
{
    public uint Hash(ReadOnlySpan<byte> source)
    {
        return Crc32.HashToUInt32(source);
    }

    public uint Hash(byte[] source)
    {
        return Crc32.HashToUInt32(source);
    }
}
