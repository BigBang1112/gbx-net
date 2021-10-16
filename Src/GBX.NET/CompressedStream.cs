using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GBX.NET
{
    public class CompressedStream : DeflateStream
    {
        public CompressionLevel Compression { get; } = CompressionLevel.BestCompression;

        public CompressedStream(Stream stream, CompressionMode mode) : base(stream, mode, true)
        {
            switch (mode)
            {
                case CompressionMode.Compress:
                    stream.WriteByte(0x78);

                    switch (Compression)
                    {
                        case CompressionLevel.BestCompression:
                            stream.WriteByte(0xDA);
                            break;
                        case CompressionLevel.DefaultCompression:
                            stream.WriteByte(0x9C);
                            break;
                        case CompressionLevel.NoCompression:
                            stream.WriteByte(0x01);
                            break;
                        case CompressionLevel.UnknownCompression:
                            throw new Exception("Unknown compression can't be written.");
                    }
                    break;
                case CompressionMode.Decompress:
                    var magic = new byte[2];
                    stream.Read(magic, 0, 2); // Needed for DeflateStream to work

                    if (magic[0] != 0x78)
                        throw new Exception("Data isn't compressed with Deflate ZLIB");

                    switch (magic[1])
                    {
                        case 0x01:
                            Compression = CompressionLevel.NoCompression;
                            Debug.WriteLine("Deflate ZLIB - No compression");
                            break;
                        case 0x9C:
                            Compression = CompressionLevel.DefaultCompression;
                            Debug.WriteLine("Deflate ZLIB - Default compression");
                            break;
                        case 0xDA:
                            Compression = CompressionLevel.BestCompression;
                            Debug.WriteLine("Deflate ZLIB - Best compression");
                            break;
                        default:
                            Compression = CompressionLevel.UnknownCompression;
                            Debug.WriteLine("Deflate ZLIB - Unknown compression");
                            break;
                    }
                    break;
            }
        }
    }
}
