using System.Diagnostics;
using System.IO.Compression;

namespace GBX.NET;

public class CompressedStream : DeflateStream
{
    private byte[]? magic;

    public CompressionLevel? Compression { get; private set; }

    public CompressedStream(Stream stream, CompressionMode mode) : base(stream, mode, true)
    {
        
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (magic is null)
        {
            ReadMagic();
        }

        return base.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (magic is null)
        {
            WriteMagic();
        }

        base.Write(buffer, offset, count);
    }

    private void ReadMagic()
    {
        magic = new byte[2];

        BaseStream.Read(magic, 0, magic.Length); // Needed for DeflateStream to work

        if (magic[0] != 0x78)
            throw new Exception("Data isn't compressed with Deflate ZLIB");

        var log = magic[1] switch
        {
            0x01 => "Deflate ZLIB - No compression",
            0x9C => "Deflate ZLIB - Default compression",
            0xDA => "Deflate ZLIB - Best compression",
            _ => "Deflate ZLIB - Unknown compression",
        };

        Debug.WriteLine(log);

        Compression = magic[1] switch
        {
            0x01 => CompressionLevel.NoCompression,
            0x9C => CompressionLevel.DefaultCompression,
            0xDA => CompressionLevel.BestCompression,
            _ => CompressionLevel.UnknownCompression,
        };
    }

    private void WriteMagic()
    {
        byte compressionTypeByte = Compression switch
        {
            CompressionLevel.BestCompression => 0xDA,
            CompressionLevel.DefaultCompression => 0x9C,
            CompressionLevel.NoCompression => 0x01,
            _ => throw new Exception("Unknown compression can't be written."),
        };

        magic = new byte[] { 0x78, compressionTypeByte };

        BaseStream.Write(magic, 0, magic.Length);
    }
}
