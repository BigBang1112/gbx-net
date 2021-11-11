using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GBX.NET;

public class CompressedStream : DeflateStream
{
    public CompressionLevel Compression { get; } = CompressionLevel.BestCompression;

    public CompressedStream(Stream stream, CompressionMode mode) : base(stream, mode, true)
    {
        switch (mode)
        {
            case CompressionMode.Compress:
                WriteMagic(stream);
                break;
            case CompressionMode.Decompress:
                Compression = CompressedStream.ReadMagic(stream);
                break;
        }
    }

    private static CompressionLevel ReadMagic(Stream stream)
    {
        var magic = new byte[2];
        stream.Read(magic, 0, 2); // Needed for DeflateStream to work

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

        return magic[1] switch
        {
            0x01 => CompressionLevel.NoCompression,
            0x9C => CompressionLevel.DefaultCompression,
            0xDA => CompressionLevel.BestCompression,
            _ => CompressionLevel.UnknownCompression,
        };
    }

    private void WriteMagic(Stream stream)
    {
        stream.WriteByte(0x78);

        byte compressionTypeByte = Compression switch
        {
            CompressionLevel.BestCompression => 0xDA,
            CompressionLevel.DefaultCompression => 0x9C,
            CompressionLevel.NoCompression => 0x01,
            _ => throw new Exception("Unknown compression can't be written."),
        };

        stream.WriteByte(compressionTypeByte);
    }
}
