using System.Text;

namespace GBX.NET;

public class EmbeddedFile
{
    public EmbeddedFileType Type { get; }
    public byte[] Data { get; }

    public EmbeddedFile(EmbeddedFileType type, byte[] data)
    {
        Type = type;
        Data = data;
    }

    public static EmbeddedFile Parse(byte[] data)
    {
        var type = EmbeddedFileType.Unknown;

        using var ms = new MemoryStream(data);
        using var r = new BinaryReader(ms);

        var buffer = r.ReadBytes(12);

        if (buffer.Take(4).SequenceEqual(riffMagic))
        {
            if (buffer.Skip(8).Take(4).SequenceEqual(webpMagic))
            {
                type = EmbeddedFileType.Webp;
            }
        }

        return new(type, data);
    }

    private static readonly byte[] riffMagic = Encoding.ASCII.GetBytes("RIFF");
    private static readonly byte[] webpMagic = Encoding.ASCII.GetBytes("WEBP");
}
