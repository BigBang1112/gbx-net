using System.Text;

namespace GBX.NET.PAK;

public static class Cry
{
    public static unsafe string Parse(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        var uncompressedSize = r.ReadInt32();
        var compressedData = r.ReadBytes();

        var uncompressedData = new byte[uncompressedSize];

        Lzo.Decompress(compressedData, uncompressedData);
        
        var key = 0xCF08317C90460052;
        var shift = uncompressedSize & 0x3F;
        var rotkey = (key << shift) | (key >> (64 - shift));
        
        for (int i = 0; i < uncompressedSize; i++)
        {
            uncompressedData[i] ^= *((byte*)&rotkey + (i & 0x7));
        }

        return Encoding.ASCII.GetString(uncompressedData);
    }

    public static string Parse(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs);
    }
}
