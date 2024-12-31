using System.Text;

namespace GBX.NET.Crypto;

public static partial class MD5
{
    public static byte[] Compute(byte[] data)
    {
#if NET6_0_OR_GREATER
        return System.Security.Cryptography.MD5.HashData(data);
#else
        using var md5 = System.Security.Cryptography.MD5.Create();
        return md5.ComputeHash(data);
#endif
    }

#if NET6_0_OR_GREATER
    public static byte[] Compute(Span<byte> data)
    {
        return System.Security.Cryptography.MD5.HashData(data);
    }

    public static int Compute(Span<byte> data, Span<byte> destination)
    {
        return System.Security.Cryptography.MD5.HashData(data, destination);
    }
#endif

    public static byte[] Compute(string data)
    {
        return Compute(Encoding.ASCII.GetBytes(data));
    }

#if NET8_0_OR_GREATER
    public static async ValueTask<byte[]> ComputeAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream(data);
        return await System.Security.Cryptography.MD5.HashDataAsync(ms, cancellationToken);
    }
#elif NET6_0_OR_GREATER || NETSTANDARD2_0
    public static async Task<byte[]> ComputeAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
#if NET6_0_OR_GREATER
        await using var ms = new MemoryStream(data);
        return await md5.ComputeHashAsync(ms, cancellationToken);
#else
        return await Task.FromResult(md5.ComputeHash(data));
#endif
    }
#endif

    public static async Task<byte[]> ComputeAsync(string data, CancellationToken cancellationToken = default)
    {
        return await ComputeAsync(Encoding.ASCII.GetBytes(data), cancellationToken);
    }

    public static string Compute136(string text)
    {
#if NET6_0_OR_GREATER
        var charSpan = text.AsSpan();
        Span<char> loweredCharSpan = stackalloc char[text.Length];
        if (charSpan.ToLowerInvariant(loweredCharSpan) != text.Length)
        {
            loweredCharSpan = text.ToLowerInvariant().ToArray();
        }
        Span<byte> bytes = stackalloc byte[text.Length * 2];
        if (Encoding.UTF8.GetBytes(loweredCharSpan, bytes) != bytes.Length)
        {
            bytes = Encoding.UTF8.GetBytes(text.ToLowerInvariant());
        }
        Span<byte> hash = stackalloc byte[17];
        if (Compute(bytes, hash.Slice(1)) != 16)
        {
            throw new InvalidOperationException();
        }
#else
        var lowered = text.ToLower();
        var bytes = Encoding.UTF8.GetBytes(lowered);
        var hashWithoutLength = Compute(bytes);
        var hash = new byte[17];
        Buffer.BlockCopy(hashWithoutLength, 0, hash, 1, hashWithoutLength.Length);
#endif
        hash[0] = (byte)bytes.Length;

        return ToHex(hash).ToString();
    }

    private static Span<char> ToHex(Span<byte> value)
    {
        var str = new char[value.Length * 2];

        for (var i = 0; i < value.Length; i++)
        {
            var hex1 = HexIntToChar((byte)(value[i] % 16));
            var hex2 = HexIntToChar((byte)(value[i] / 16));

            str[i * 2] = hex1;
            str[i * 2 + 1] = hex2;
        }

        return str;
    }

    private static char HexIntToChar(byte v)
    {
        return v < 10 ? (char)(v + 48) : (char)(v + 55);
    }
}
