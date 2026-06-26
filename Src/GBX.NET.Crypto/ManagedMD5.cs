namespace GBX.NET.Crypto;

/// <summary>
/// A barebone, fully managed implementation of the MD5 hashing algorithm (RFC 1321).
/// Used in environments (such as the browser/WASM runtime) where the synchronous
/// <see cref="System.Security.Cryptography.MD5"/> APIs are not available.
/// </summary>
internal static class ManagedMD5
{
    // Per-round shift amounts.
    private static readonly int[] S = [
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
    ];

    // Precomputed constants: floor(2^32 * abs(sin(i + 1))).
    private static readonly uint[] K = [
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
        0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
        0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
        0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
        0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
        0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
        0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
        0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
        0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
        0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
        0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
        0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
        0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
    ];

    /// <summary>
    /// Computes the MD5 hash of <paramref name="data"/> and returns the resulting 16-byte digest.
    /// </summary>
    public static byte[] Compute(ReadOnlySpan<byte> data)
    {
        var result = new byte[16];
        Compute(data, result);
        return result;
    }

    /// <summary>
    /// Computes the MD5 hash of <paramref name="data"/> into <paramref name="destination"/>.
    /// </summary>
    /// <returns>The number of bytes written (always 16).</returns>
    public static int Compute(ReadOnlySpan<byte> data, Span<byte> destination)
    {
        if (destination.Length < 16)
        {
            throw new ArgumentException("Destination must be at least 16 bytes long.", nameof(destination));
        }

        uint a0 = 0x67452301;
        uint b0 = 0xefcdab89;
        uint c0 = 0x98badcfe;
        uint d0 = 0x10325476;

        Span<uint> m = stackalloc uint[16];

        var fullBlocks = data.Length / 64;

        for (var blk = 0; blk < fullBlocks; blk++)
        {
            LoadBlock(data.Slice(blk * 64, 64), m);
            ProcessBlock(m, ref a0, ref b0, ref c0, ref d0);
        }

        // Final block(s) with padding (0x80 followed by zeros, then the 64-bit length).
        var remaining = data.Length - fullBlocks * 64; // 0..63
        Span<byte> tail = stackalloc byte[128];
        tail.Clear();
        data.Slice(fullBlocks * 64).CopyTo(tail);
        tail[remaining] = 0x80;

        var tailBlocks = remaining >= 56 ? 2 : 1;
        var totalTailLength = tailBlocks * 64;

        var bitLength = (ulong)data.Length * 8;
        for (var i = 0; i < 8; i++)
        {
            tail[totalTailLength - 8 + i] = (byte)(bitLength >> (8 * i));
        }

        for (var blk = 0; blk < tailBlocks; blk++)
        {
            LoadBlock(tail.Slice(blk * 64, 64), m);
            ProcessBlock(m, ref a0, ref b0, ref c0, ref d0);
        }

        WriteUInt32LittleEndian(destination, 0, a0);
        WriteUInt32LittleEndian(destination, 4, b0);
        WriteUInt32LittleEndian(destination, 8, c0);
        WriteUInt32LittleEndian(destination, 12, d0);

        return 16;
    }

    private static void LoadBlock(ReadOnlySpan<byte> block, Span<uint> m)
    {
        for (var i = 0; i < 16; i++)
        {
            m[i] = (uint)(block[i * 4]
                | block[i * 4 + 1] << 8
                | block[i * 4 + 2] << 16
                | block[i * 4 + 3] << 24);
        }
    }

    private static void ProcessBlock(ReadOnlySpan<uint> m, ref uint a0, ref uint b0, ref uint c0, ref uint d0)
    {
        var a = a0;
        var b = b0;
        var c = c0;
        var d = d0;

        for (var i = 0; i < 64; i++)
        {
            uint f;
            int g;

            if (i < 16)
            {
                f = (b & c) | (~b & d);
                g = i;
            }
            else if (i < 32)
            {
                f = (d & b) | (~d & c);
                g = (5 * i + 1) % 16;
            }
            else if (i < 48)
            {
                f = b ^ c ^ d;
                g = (3 * i + 5) % 16;
            }
            else
            {
                f = c ^ (b | ~d);
                g = (7 * i) % 16;
            }

            f = f + a + K[i] + m[g];
            a = d;
            d = c;
            c = b;
            b += LeftRotate(f, S[i]);
        }

        a0 += a;
        b0 += b;
        c0 += c;
        d0 += d;
    }

    private static uint LeftRotate(uint value, int count)
    {
        return (value << count) | (value >> (32 - count));
    }

    private static void WriteUInt32LittleEndian(Span<byte> destination, int offset, uint value)
    {
        destination[offset] = (byte)value;
        destination[offset + 1] = (byte)(value >> 8);
        destination[offset + 2] = (byte)(value >> 16);
        destination[offset + 3] = (byte)(value >> 24);
    }
}
