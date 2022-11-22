using System.Text;

namespace GBX.NET.PAK;

public static class Cry
{
    private const ulong Key = 0xCF08317C90460052;

    public static unsafe string Decrypt(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        var uncompressedSize = r.ReadInt32();
        var compressedData = r.ReadBytes();

        var uncompressedData = new byte[uncompressedSize];

        Lzo.Decompress(compressedData, uncompressedData);
        
        var shift = uncompressedSize & 0x3F;
        var rotkey = (Key << shift) | (Key >> (64 - shift));
        
        for (int i = 0; i < uncompressedSize; i++)
        {
            uncompressedData[i] ^= *((byte*)&rotkey + (i & 0x7));
        }

        return Encoding.ASCII.GetString(uncompressedData);
    }

    public static string Decrypt(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return Decrypt(fs);
    }
    
    public static unsafe void Encrypt(Stream stream, string contents)
    {
        var uncompressedData = Encoding.ASCII.GetBytes(contents);

        var shift = uncompressedData.Length & 0x3F;
        var rotkey = (Key << shift) | (Key >> (64 - shift));

        for (int i = 0; i < uncompressedData.Length; i++)
        {
            uncompressedData[i] ^= *((byte*)&rotkey + (i & 0x7));
        }

        using var w = new GameBoxWriter(stream);
        
        w.Write(uncompressedData.Length);
        w.WriteByteArray(Lzo.Compress(uncompressedData));
    }

    public static void Encrypt(string fileName, string contents)
    {
        using var fs = File.Create(fileName);
        Encrypt(fs, contents);
    }
}
