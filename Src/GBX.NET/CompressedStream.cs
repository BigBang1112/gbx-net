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
                    WriteMagic(stream);
                    break;
                case CompressionMode.Decompress:
                    Compression = ReadMagic(stream);
                    break;
            }
        }

        private CompressionLevel ReadMagic(Stream stream)
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
        }
    }
}
