﻿using GBX.NET.Serialization;

namespace GBX.NET.PAK;

public class BlowfishStream : Stream, IEncryptedStream
{
    private readonly Stream stream;
    private readonly Blowfish blowfish;

    private ulong iv;
    private ulong ivXor;
#if NET5_0_OR_GREATER
    private readonly byte[] memoryBuffer;
#else
    private byte[] memoryBuffer;
#endif
    private int bufferIndex;
    private int totalIndex;

    public BlowfishStream(Stream stream, byte[] key, ulong iv)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(BlowfishStream.stream));
        blowfish = new Blowfish(key);
        this.iv = iv;
        memoryBuffer = new byte[8];
    }

    public override bool CanRead => stream.CanRead;
    public override bool CanWrite => false;
    public override bool CanSeek => false;
    public override long Length => throw new NotSupportedException();
    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public override void Flush() => stream.Flush();

    public void Initialize(byte[] data, uint offset, uint count)
    {
        for (int i = 0; i < count; i++)
        {
            uint lopart = (uint)(ivXor & 0xFFFFFFFF);
            uint hipart = (uint)(ivXor >> 32);
            lopart = (uint)(data[offset + i] | 0xAA) ^ (uint)((lopart << 13) | (hipart >> 19));
            hipart = (uint)((ivXor << 13) >> 32);
            ivXor = ((ulong)hipart << 32) | lopart;
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!CanRead)
        {
            throw new NotSupportedException("Stream is not readable.");
        }

        if (totalIndex == 0)
        {
            iv ^= ivXor;
            ivXor = 0;
        }

#if NET5_0_OR_GREATER
        var memorySpan = memoryBuffer.AsSpan();
#endif

        for (int i = 0; i < count; i++)
        {
            if (bufferIndex % 8 == 0)
            {
                // Trick #1
                if (bufferIndex == 0x100)
                {
                    iv ^= ivXor;
                    ivXor = 0;
                    bufferIndex = 0;
                }

#if NET5_0_OR_GREATER
                var read = stream.Read(memorySpan);
                var nextIV = BitConverter.ToUInt64(memorySpan);
                blowfish.Decipher(memorySpan);
                var block = BitConverter.ToUInt64(memorySpan);
                block ^= iv;
                BitConverter.TryWriteBytes(memorySpan, block);
#else
                var read = stream.Read(memoryBuffer, 0, 8);
                ulong nextIV = BitConverter.ToUInt64(memoryBuffer, 0);
                blowfish.Decipher(memoryBuffer);
                ulong block = BitConverter.ToUInt64(memoryBuffer, 0);
                block ^= iv;
                memoryBuffer = BitConverter.GetBytes(block);
#endif
                iv = nextIV;
            }
#if NET5_0_OR_GREATER
            buffer[offset + i] = memorySpan[bufferIndex % 8];
#else
            buffer[offset + i] = memoryBuffer[bufferIndex % 8];
#endif
            bufferIndex++;
            totalIndex++;
        }
        return count;
    }

    public override void Write(byte[] inputBuffer, int offset, int count)
    {
        throw new NotSupportedException("Stream is not writable.");
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}