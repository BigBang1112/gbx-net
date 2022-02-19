using System.IO.Compression;

namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public class SHmsLightMapCacheMapping : IReadableWritable, IVersionable
    {
        public Int3 U01;
        public Vec3 U02;
        public Vec3 U03;
        public int U04;
        public int Count;
        public float[]? U05;
        public (short, short, short, short)[]? U06;
        public (byte, byte, byte, byte)[]? U07;
        public (short, short)[]? U08;
        public int U09;
        public uint[]? U10;

        public int ColorCount;
        public int FaceCount;
        public byte[]? ColorData;

        public int Version { get; set; }

        private int uncompressedSize1;
        private byte[]? compressedData1;
        private int uncompressedSize2;
        private byte[]? compressedData2;
        private int uncompressedSize3;
        private byte[]? compressedData3;
        private int uncompressedSize4;
        private byte[]? compressedData4;

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            Version = rw.Int32(Version);
            rw.Int3(ref U01); // 3 ints
            rw.Vec3(ref U02); // 6 floats
            rw.Vec3(ref U03);
            rw.Int32(ref U04);

            rw.Int32(ref Count);

            rw.Int32(ref uncompressedSize1);
            rw.Bytes(ref compressedData1);
            rw.Int32(ref uncompressedSize2);
            rw.Bytes(ref compressedData2);
            rw.Int32(ref uncompressedSize3);
            rw.Bytes(ref compressedData3);
            rw.Int32(ref uncompressedSize4);
            rw.Bytes(ref compressedData4);

            /*ReadWriteCompressedArray<float>(ref U05, rw, Count);
            ReadWriteCompressedArray<(short, short, short, short)>(ref U06, rw, Count, sizeOfStruct: 8);
            ReadWriteCompressedArray<(byte, byte, byte, byte)>(ref U07, rw, Count, sizeOfStruct: 4);
            ReadWriteCompressedArray<(short, short)>(ref U08, rw, Count, sizeOfStruct: 4);*/

            rw.Int32(ref U09);

            if (rw.Reader is not null)
            {
                var uncompressedSize = rw.Reader.ReadInt32();
                var compressedData = rw.Reader.ReadBytes();

                using var ms = new MemoryStream(compressedData);
#if NET6_0_OR_GREATER
                using var zlib = new ZLibStream(ms, CompressionMode.Decompress);
#else
                using var zlib = new CompressedStream(ms, CompressionMode.Decompress);
#endif
                using var r = new GameBoxReader(zlib);

                ColorCount = r.ReadInt32();
                FaceCount = r.ReadInt32();
                ColorData = r.ReadBytes(FaceCount * ColorCount + 8);
            }
            else if (rw.Writer is not null)
            {
                using var ms = new MemoryStream();
                using var w = new GameBoxWriter(ms);

                w.Write(ColorCount);
                w.Write(FaceCount);
                w.WriteBytes(ColorData);

                ms.Position = 0;

                using var compressedMs = new MemoryStream();

#if NET6_0_OR_GREATER
                using (var zlib = new ZLibStream(compressedMs, global::System.IO.Compression.CompressionLevel.SmallestSize, true))
                {
                    ms.CopyTo(zlib);
                }
#else
                using (var zlib = new CompressedStream(ms, CompressionMode.Decompress))
                {
                    ms.CopyTo(zlib);
                }
#endif

                rw.Writer.Write((int)ms.Length);
                rw.Writer.Write((int)compressedMs.Length);
                rw.Writer.WriteBytes(compressedMs.ToArray());
            }

            //ReadWriteCompressedSpan<uint>(ref U10, rw, Count);
        }

        public static void ReadWriteCompressed<T>(ref T? variable,
                                                  GameBoxReaderWriter rw,
                                                  int readCount,
                                                  Func<GameBoxReader, T> read,
                                                  Action<GameBoxWriter, T> write)
        {
            if (rw.Reader is not null)
            {
                variable = ReadCompressed(rw.Reader, readCount, read);
                return;
            }

            if (rw.Writer is not null && variable is not null)
            {
                WriteCompressed(rw.Writer, variable, write);
            }

            return;
        }

        public static void ReadWriteCompressedArray<T>(ref T[]? array,
                                                       GameBoxReaderWriter rw,
                                                       int readCount,
                                                       int? sizeOfStruct = null) where T : struct
        {
            if (rw.Reader is not null)
            {
                array = ReadCompressedArray<T>(rw.Reader, readCount, sizeOfStruct);
                return;
            }

            if (rw.Writer is not null && array is not null)
            {
                WriteCompressedArray(rw.Writer, array);
            }

            return;
        }

        public static T ReadCompressed<T>(GameBoxReader r, int count, Func<GameBoxReader, T> func)
        {
            var uncompressedSize = r.ReadInt32();
            var compressedBuffer = r.ReadBytes()!;

            using var ms = new MemoryStream(compressedBuffer);
#if NET6_0_OR_GREATER
            using var zlib = new ZLibStream(ms, CompressionMode.Decompress);
#else
            using var zlib = new CompressedStream(ms, CompressionMode.Decompress);
#endif
            using var zlibR = new GameBoxReader(zlib);

            return func(zlibR);
        }

        public static T[] ReadCompressedArray<T>(GameBoxReader r, int count, int? sizeOfStruct = null) where T : struct
        {
            var spanLength = count * (sizeOfStruct ?? 1);
            var lengthInBytes = sizeOfStruct.HasValue;
            return ReadCompressed(r, count, r => r.ReadSpan<T>(spanLength, lengthInBytes).ToArray());
        }

        public static void WriteCompressed<T>(GameBoxWriter w, T value, Action<GameBoxWriter, T> action)
        {
            using var uncompressedStream = new MemoryStream();
            using var uncompressedStreamWriter = new GameBoxWriter(uncompressedStream);

            action(uncompressedStreamWriter, value);

            w.Write((int)uncompressedStream.Length);

            uncompressedStream.Position = 0;

            using var output = new MemoryStream();

#if NET6_0_OR_GREATER
            using (var zlib = new ZLibStream(output, global::System.IO.Compression.CompressionLevel.SmallestSize, true))
            {
                uncompressedStream.CopyTo(zlib);
            }
#else
            using (var deflate = new CompressedStream(output, CompressionMode.Compress))
            {
                uncompressedStream.CopyTo(deflate);
            }
#endif

            w.Write((int)output.Length);
            w.WriteBytes(output.ToArray());
        }

        public static void WriteCompressedArray<T>(GameBoxWriter w, T[] value) where T : struct
        {
            WriteCompressed(w, value, (w, x) => w.WriteSpan<T>(value));
        }
    }
}
