using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Extensions;

public static class StringExtensions
{
    public static string HashMD5(this string value)
    {
        var lowered = value.ToLower();
        var bytes = Encoding.UTF8.GetBytes(lowered);

        using var md5 = MD5.Create();

        var hash = new byte[17];

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        if (!md5.TryComputeHash(bytes, hash, out int md5Length))
        {
            throw new Exception();
        }

        var offset = hash.Length - md5Length;

        for (var i = hash.Length - 1; i >= offset; i--)
        {
            hash[i] = hash[i - offset];
        }
#else
        var hashWithoutLength = md5.ComputeHash(bytes);

        Buffer.BlockCopy(hashWithoutLength, 0, hash, 1, hashWithoutLength.Length);
#endif

        hash[0] = (byte)bytes.Length;

        return ToHex(hash).ToString();
    }

    private static Span<char> ToHex(byte[] value)
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
        if (v < 10)
        {
            return (char)(v + 48);
        }

        return (char)(v + 55);
    }

}
