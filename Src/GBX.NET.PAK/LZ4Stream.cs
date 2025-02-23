﻿using System.Runtime.InteropServices;

namespace GBX.NET.PAK;

public class LZ4Exception : Exception
{
    public LZ4Exception() { }
    public LZ4Exception(string message) : base("[LZ4] " + message) { }
    public LZ4Exception(string message, Exception innerException) : base(message, innerException) { }
}

public class LZ4Stream : Stream
{
    private static readonly byte[] LZ4_DICTIONARY =
    [
        0x44 ,0x44 ,0x53 ,0x20 ,0x7C ,0x00 ,0x00 ,0x00 , 0x07 ,0x10 ,0x0A ,0x00 ,0x00 ,0x02 ,0x00 ,0x00
        ,0x00 ,0x02 ,0x00 ,0x00 ,0x00 ,0x00 ,0x04 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x0A ,0x00 ,0x00 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x20 ,0x00 ,0x00 ,0x00
        ,0x04 ,0x00 ,0x00 ,0x00 ,0x44 ,0x58 ,0x54 ,0x35 , 0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x10 ,0x40 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x01 ,0x00 ,0x00 ,0x00 ,0x55 ,0x55 ,0x55 ,0x55
        ,0x2F ,0x2F ,0x43 ,0x6F ,0x6E ,0x74 ,0x65 ,0x78 , 0x74 ,0x44 ,0x65 ,0x66 ,0x69 ,0x6E ,0x65 ,0x42
        ,0x65 ,0x67 ,0x69 ,0x6E ,0x0D ,0x0A ,0x2F ,0x2F , 0x20 ,0x44 ,0x54 ,0x77 ,0x6B ,0x5F ,0x51 ,0x75
        ,0x61 ,0x6C ,0x69 ,0x74 ,0x79 ,0x4C ,0x65 ,0x73 , 0x73 ,0x5F ,0x4C ,0x6F ,0x77 ,0x0D ,0x0A ,0x2F
        ,0x2F ,0x20 ,0x44 ,0x54 ,0x77 ,0x6B ,0x5F ,0x51 , 0x75 ,0x61 ,0x6C ,0x69 ,0x74 ,0x79 ,0x4C ,0x65
        ,0x73 ,0x73 ,0x5F ,0x4D ,0x65 ,0x64 ,0x69 ,0x75 , 0x6D ,0x0D ,0x0A ,0x2F ,0x2F ,0x20 ,0x44 ,0x54
        ,0x77 ,0x6B ,0x5F ,0x51 ,0x75 ,0x61 ,0x6C ,0x69 , 0x74 ,0x79 ,0x4C ,0x65 ,0x73 ,0x73 ,0x5F ,0x48
        ,0x69 ,0x67 ,0x68 ,0x0D ,0x23 ,0x69 ,0x6E ,0x63 , 0x6C ,0x75 ,0x64 ,0x65 ,0x20 ,0x3C ,0x43 ,0x6F
        ,0x6D ,0x6D ,0x6F ,0x6E ,0x2E ,0x50 ,0x68 ,0x6C , 0x73 ,0x6C ,0x2E ,0x74 ,0x78 ,0x74 ,0x3E ,0x0D
        ,0x0A ,0x73 ,0x74 ,0x72 ,0x75 ,0x63 ,0x74 ,0x20 , 0x50 ,0x53 ,0x5F ,0x49 ,0x4E ,0x50 ,0x55 ,0x54
        ,0x09 ,0x09 ,0x7B ,0x0D ,0x0A ,0x09 ,0x66 ,0x6C , 0x6F ,0x61 ,0x74 ,0x34 ,0x20 ,0x52 ,0x61 ,0x73
        ,0x74 ,0x65 ,0x72 ,0x50 ,0x23 ,0x69 ,0x6E ,0x63 , 0x6C ,0x75 ,0x64 ,0x65 ,0x20 ,0x3C ,0x43 ,0x6F
        ,0x6D ,0x6D ,0x6F ,0x6E ,0x2E ,0x56 ,0x68 ,0x6C , 0x73 ,0x6C ,0x2E ,0x74 ,0x78 ,0x74 ,0x3E ,0x0D
        ,0x23 ,0x69 ,0x6E ,0x63 ,0x6C ,0x75 ,0x64 ,0x65 , 0x20 ,0x3C ,0x43 ,0x6F ,0x6D ,0x6D ,0x6F ,0x6E
        ,0x2E ,0x56 ,0x48 ,0x6C ,0x73 ,0x6C ,0x2E ,0x74 , 0x78 ,0x74 ,0x3E ,0x0D ,0x47 ,0x42 ,0x58 ,0x06
        ,0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x50 ,0x00 , 0x09 ,0x00 ,0x00 ,0x00 ,0x00 ,0x05 ,0x00 ,0x00
        ,0x00 ,0x01 ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06 , 0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x50 ,0x00
        ,0x09 ,0x00 ,0x00 ,0x00 ,0x00 ,0x09 ,0x00 ,0x00 , 0x00 ,0x02 ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06
        ,0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x50 ,0x00 , 0x09 ,0x00 ,0x00 ,0x00 ,0x00 ,0x1F ,0x00 ,0x00
        ,0x47 ,0x42 ,0x58 ,0x06 ,0x00 ,0x42 ,0x55 ,0x55 , 0x52 ,0x00 ,0x50 ,0x00 ,0x09 ,0x00 ,0x00 ,0x00
        ,0x00 ,0x12 ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06 , 0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x50 ,0x00
        ,0x09 ,0x00 ,0x00 ,0x00 ,0x00 ,0x18 ,0x00 ,0x00 , 0x47 ,0x42 ,0x58 ,0x06 ,0x00 ,0x42 ,0x55 ,0x55
        ,0x52 ,0x00 ,0x50 ,0x00 ,0x09 ,0x00 ,0x00 ,0x00 , 0x00 ,0x0F ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06
        ,0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x60 ,0x02 , 0x09 ,0x00 ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06
        ,0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x60 ,0x01 , 0x07 ,0x00 ,0x00 ,0x00 ,0x47 ,0x42 ,0x58 ,0x06
        ,0x00 ,0x42 ,0x55 ,0x43 ,0x52 ,0x00 ,0xD0 ,0x10 , 0x47 ,0x42 ,0x58 ,0x06 ,0x00 ,0x42 ,0x55 ,0x55
        ,0x52 ,0x00 ,0x10 ,0x05 ,0x47 ,0x42 ,0x58 ,0x06 , 0x00 ,0x42 ,0x55 ,0x55 ,0x52 ,0x00 ,0x30 ,0x05
        ,0x44 ,0x44 ,0x53 ,0x20 ,0x7C ,0x00 ,0x00 ,0x00 , 0x07 ,0x10 ,0x02 ,0x00 ,0x00 ,0x00 ,0x02 ,0x00
        ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 , 0x47 ,0x42 ,0x58 ,0x06 ,0x00 ,0x42 ,0x55 ,0x43
        ,0x52 ,0x00 ,0x30 ,0x04 ,0x52 ,0x49 ,0x46 ,0x46 , 0x3C ,0x3F ,0x78 ,0x6D ,0x6C ,0x20 ,0x76 ,0x65
        ,0x72 ,0x73 ,0x69 ,0x6F ,0x6E ,0x3D ,0x22 ,0x31 , 0x2E ,0x30 ,0x22 ,0x20 ,0x65 ,0x6E ,0x63 ,0x6F
        ,0x64 ,0x69 ,0x6E ,0x67 ,0x3D ,0x22 ,0x75 ,0x74 , 0x66 ,0x2D ,0x38 ,0x22 ,0x20 ,0x3F ,0x3E ,0x0A
        ,0x3C ,0x6D ,0x61 ,0x6E ,0x69 ,0x61 ,0x6C ,0x69 , 0x6E ,0x6B ,0x20 ,0x76 ,0x65 ,0x72 ,0x73 ,0x69
        ,0x6F ,0x6E ,0x3D ,0x22 ,0x22 ,0x66 ,0x69 ,0x6C , 0x65 ,0x3A ,0x2F ,0x2F ,0x4D ,0x65 ,0x64 ,0x69
        ,0x61 ,0x2F ,0x49 ,0x6D ,0x61 ,0x67 ,0x65 ,0x73 , 0x2F ,0x22 ,0x66 ,0x69 ,0x6C ,0x65 ,0x3A ,0x2F
        ,0x2F ,0x4D ,0x65 ,0x64 ,0x69 ,0x61 ,0x2F ,0x4D , 0x61 ,0x6E ,0x69 ,0x61 ,0x6C ,0x69 ,0x6E ,0x6B
        ,0x73 ,0x2F ,0x43 ,0x6F ,0x6D ,0x6D ,0x6F ,0x6E , 0x3C ,0x66 ,0x72 ,0x61 ,0x6D ,0x65 ,0x20 ,0x69
        ,0x64 ,0x3D ,0x22 ,0x3C ,0x6C ,0x61 ,0x62 ,0x65 , 0x6C ,0x20 ,0x69 ,0x64 ,0x3D ,0x22 ,0x3C ,0x71
        ,0x75 ,0x61 ,0x64 ,0x20 ,0x69 ,0x64 ,0x3D ,0x22 , 0x3C ,0x2F ,0x66 ,0x72 ,0x61 ,0x6D ,0x65 ,0x3E
        ,0x20 ,0x2F ,0x3E ,0x0D ,0x0A ,0x23 ,0x52 ,0x65 , 0x71 ,0x75 ,0x69 ,0x72 ,0x65 ,0x43 ,0x6F ,0x6E
        ,0x74 ,0x65 ,0x78 ,0x74 ,0x20 ,0x23 ,0x49 ,0x6E , 0x63 ,0x6C ,0x75 ,0x64 ,0x65 ,0x20 ,0x22 ,0x4C
        ,0x69 ,0x62 ,0x73 ,0x2F ,0x4E ,0x61 ,0x64 ,0x65 , 0x6F ,0x2F ,0x2E ,0x53 ,0x63 ,0x72 ,0x69 ,0x70
        ,0x74 ,0x2E ,0x74 ,0x78 ,0x74 ,0x22 ,0x23 ,0x43 , 0x6F ,0x6E ,0x73 ,0x74 ,0x20 ,0x43 ,0x5F ,0x09
        ,0x64 ,0x65 ,0x63 ,0x6C ,0x61 ,0x72 ,0x65 ,0x20 , 0x54 ,0x65 ,0x78 ,0x74 ,0x20 ,0x79 ,0x69 ,0x65
        ,0x6C ,0x64 ,0x3B ,0x66 ,0x6F ,0x72 ,0x65 ,0x61 , 0x63 ,0x68 ,0x28 ,0x63 ,0x6F ,0x6E ,0x74 ,0x69
        ,0x6E ,0x75 ,0x65 ,0x3B ,0x6D ,0x61 ,0x69 ,0x6E , 0x28 ,0x29 ,0x42 ,0x6F ,0x6F ,0x6C ,0x65 ,0x61
        ,0x6E ,0x49 ,0x6E ,0x74 ,0x65 ,0x67 ,0x65 ,0x72 , 0x73 ,0x77 ,0x69 ,0x74 ,0x63 ,0x68 ,0x28 ,0x72
        ,0x65 ,0x74 ,0x75 ,0x72 ,0x6E ,0x3B ,0x00 ,0x00 , 0x80 ,0x3F ,0xFF ,0xFF ,0xFF ,0xFF
    ];

    private Stream innerStream;
    private int UncompressedSize;
    private int position;
    private IntPtr dictionary;
    private IntPtr lz4_stream;
    private IntPtr ringBuffer;
    private static int RING_BUFFER_SIZE = 73728;

    private byte[] _buffer = new byte[] { };
    private int _bufferIndex = 0;

    [DllImport("liblz4")]
    private static extern unsafe IntPtr LZ4_createStreamDecode();

    [DllImport("liblz4")]
    private static extern unsafe int LZ4_freeStreamDecode(IntPtr stream);

    [DllImport("liblz4")]
    private static extern unsafe int LZ4_setStreamDecode(IntPtr stream, IntPtr dictionary, int dictSize);

    [DllImport("liblz4")]
    private static extern unsafe int LZ4_decompress_safe_continue(IntPtr stream, byte* src, byte* dst, int srcSize, int dstCapacity);

    public unsafe LZ4Stream(Stream innerStream, int UncompressedSize)
    {
        this.innerStream = innerStream;
        this.UncompressedSize = UncompressedSize;

        dictionary = Marshal.AllocHGlobal(LZ4_DICTIONARY.Length);
        Marshal.Copy(LZ4_DICTIONARY, 0, dictionary, LZ4_DICTIONARY.Length);
        lz4_stream = LZ4_createStreamDecode();
        if (LZ4_setStreamDecode(lz4_stream, dictionary, LZ4_DICTIONARY.Length) != 1)
            throw new Exception("LZ4_setStreamDecode failed.");

        ringBuffer = Marshal.AllocHGlobal(RING_BUFFER_SIZE);
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => UncompressedSize;

    public override long Position { get { return position; } set => Seek(value, SeekOrigin.Begin); }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override unsafe int Read(byte[] buffer, int offset, int count)
    {
        int fullCount = count;
        while (count > 0)
        {
            int numBytesFromBuffer = Math.Min(count, _buffer.Length - _bufferIndex);
            for (int i = 0; i < numBytesFromBuffer; i++)
            {
                buffer[offset + i] = _buffer[_bufferIndex + i];
            }
            offset += numBytesFromBuffer;
            _bufferIndex += numBytesFromBuffer;
            position += numBytesFromBuffer;
            count -= numBytesFromBuffer;

            if (position == UncompressedSize)
                break;

            if (_bufferIndex == _buffer.Length)
            {
                uint numBytes = (uint)innerStream.ReadByte();
                uint numBytes2 = (uint)innerStream.ReadByte();
                numBytes |= numBytes2 << 8;
                // Console.WriteLine($"    numBytes={numBytes}");
                if (numBytes > 4128)
                    throw new LZ4Exception("Wrong number of bytes");

                var srcBuf = new byte[numBytes];
                int inputSize = innerStream.Read(srcBuf, 0, srcBuf.Length);
                if (inputSize != numBytes)
                    throw new LZ4Exception("Wrong number of bytes read from inner stream");

                int offsetInRingBuffer = position % RING_BUFFER_SIZE;
                int newSize;
                fixed (byte* src = srcBuf)
                {
                    newSize = LZ4_decompress_safe_continue(lz4_stream, src, (byte*)(ringBuffer + offsetInRingBuffer), srcBuf.Length, 4096);
                }
                // Console.WriteLine($"    newSize={newSize}");
                if (newSize < 0)
                {
                    throw new LZ4Exception("Failed to decompress");
                }
                _bufferIndex = 0;
                _buffer = new byte[newSize];
                Marshal.Copy(ringBuffer + offsetInRingBuffer, _buffer, 0, newSize);
            }
        }

        return fullCount - count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        offset = origin == SeekOrigin.Begin ? offset : origin == SeekOrigin.Current ? position + offset : UncompressedSize + offset;
        if (offset >= position)
        {
            byte[] dummy = new byte[offset - position];
            Read(dummy, 0, (int)(offset - position));
            return position;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (dictionary != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(dictionary);
                dictionary = IntPtr.Zero;
            }
            if (ringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ringBuffer);
                ringBuffer = IntPtr.Zero;
            }
            if (lz4_stream != IntPtr.Zero)
            {
                LZ4_freeStreamDecode(lz4_stream);
                lz4_stream = IntPtr.Zero;
            }
        }
        base.Dispose(disposing);
    }
}