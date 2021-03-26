using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GBX.NET
{
    public class CompressedStream : DeflateStream
    {
        public CompressionLevel Compression { get; }

        public CompressedStream(Stream stream, CompressionMode mode) : base(stream, mode, true)
        {
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
        }
    }
}
