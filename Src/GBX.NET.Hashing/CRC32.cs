using GBX.NET.Extensions;
using System.IO.Hashing;

namespace GBX.NET.Hashing;

/// <summary>
/// CRC32 implementation using <see cref="Crc32"/> behind the scenes.
/// </summary>
public sealed class CRC32 : ICrc32
{
    /// <summary>
    /// Hashes the source to a CRC32 hash.
    /// </summary>
    /// <param name="source">Source to hash.</param>
    /// <returns>A CRC32 hash.</returns>
    public uint Hash(ReadOnlySpan<byte> source)
    {
        return Crc32.HashToUInt32(source);
    }

    /// <summary>
    /// Hashes the source to a CRC32 hash.
    /// </summary>
    /// <param name="source">Source to hash.</param>
    /// <returns>A CRC32 hash.</returns>
    public uint Hash(byte[] source)
    {
        return Crc32.HashToUInt32(source);
    }
}
