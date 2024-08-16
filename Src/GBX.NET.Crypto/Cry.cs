using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Crypto;

public static partial class Cry
{
    private const ulong Key = 0xCF08317C90460052;

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<string> DecryptAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (Gbx.LZO is null)
        {
            throw new LzoNotDefinedException();
        }

        using var r = new GbxReader(stream);
        var uncompressedSize = r.ReadInt32();
        var compressedData = await r.ReadDataAsync(cancellationToken);

        var uncompressedData = new byte[uncompressedSize];

        Gbx.LZO.Decompress(compressedData, uncompressedData);

        var shift = uncompressedSize & 0x3F;
        var rotkey = (Key << shift) | (Key >> (64 - shift));

        var rotkeyBytes = BitConverter.GetBytes(rotkey);

        for (int i = 0; i < uncompressedSize; i++)
        {
            uncompressedData[i] ^= rotkeyBytes[i & 0x7];
        }

        return Encoding.ASCII.GetString(uncompressedData);
    }

    public static async Task<string> DecryptAsync(string fileName, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return await DecryptAsync(fs, cancellationToken);
    }

    public static string Decrypt(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return Decrypt(fs);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task EncryptAsync(Stream stream, string contents, CancellationToken cancellationToken = default)
    {
        if (Gbx.LZO is null)
        {
            throw new LzoNotDefinedException();
        }

        var uncompressedData = Encoding.ASCII.GetBytes(contents);

        var shift = uncompressedData.Length & 0x3F;
        var rotkey = (Key << shift) | (Key >> (64 - shift));

        var rotkeyBytes = BitConverter.GetBytes(rotkey);

        for (int i = 0; i < uncompressedData.Length; i++)
        {
            uncompressedData[i] ^= rotkeyBytes[i & 0x7];
        }

        using var w = new GbxWriter(stream);

        w.Write(uncompressedData.Length);

        var compressedData = Gbx.LZO.Compress(uncompressedData);
        await w.WriteDataAsync(compressedData, cancellationToken);
    }

    public static async Task EncryptAsync(string fileName, string contents, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await EncryptAsync(fs, contents, cancellationToken);
    }

    public static void Encrypt(string fileName, string contents)
    {
        using var fs = File.Create(fileName);
        Encrypt(fs, contents);
    }
}
