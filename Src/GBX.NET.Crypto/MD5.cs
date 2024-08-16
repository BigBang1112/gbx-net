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
}
