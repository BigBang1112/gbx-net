using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Crypto;

public sealed partial class Mux
{
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<MuxStream> DecryptAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var r = new GbxReader(stream);

        var magic = await r.ReadBytesAsync(9, cancellationToken);

        if (Encoding.ASCII.GetString(magic) != "NadeoFile")
        {
            throw new InvalidDataException("Invalid magic number.");
        }
        
        var version = r.ReadByte();
        var keySalt = r.ReadInt32();

        return new MuxStream(stream, keySalt);
    }

    public static async Task<MuxStream> DecryptAsync(string fileName, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return await DecryptAsync(fs, cancellationToken);
    }

    public static MuxStream Decrypt(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return Decrypt(fs);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<MuxStream> EncryptAsync(Stream stream, int keySalt, byte version = 1, CancellationToken cancellationToken = default)
    {
        using var w = new GbxWriter(stream);

        await w.WriteAsync(Encoding.ASCII.GetBytes("NadeoFile"), cancellationToken);
        w.Write(version);
        w.Write(keySalt);

        return new MuxStream(stream, keySalt);
    }

    public static async Task<MuxStream> EncryptAsync(string fileName, int keySalt, byte version = 1, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        return await EncryptAsync(fs, keySalt, version, cancellationToken);
    }

    public static MuxStream Encrypt(string fileName, int keySalt, byte version = 1)
    {
        using var fs = File.Create(fileName);
        return Encrypt(fs, keySalt, version);
    }
}
