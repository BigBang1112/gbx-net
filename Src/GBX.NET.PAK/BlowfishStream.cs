using GBX.NET.Crypto;
using System.Buffers.Binary;

namespace GBX.NET.PAK;

public partial class BlowfishStream : Stream, IEncryptionInitializer
{
    private readonly Stream stream;
    private readonly Blowfish blowfish;

    private ulong iv;
    private ulong ivXor;
    private int bufferIndex;
    private int totalIndex;
    private readonly byte[] memoryBuffer;
    private readonly int version;

    public BlowfishStream(Stream stream, byte[] key, ulong iv, int version)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        blowfish = new Blowfish(key, version >= 18 ? BlowfishTrick.LittleEndianPak18 : BlowfishTrick.LittleEndian);
        this.iv = iv;
        memoryBuffer = new byte[8];
        this.version = version;
    }

    public override bool CanRead => stream.CanRead;
    public override bool CanWrite => false;
    public override bool CanSeek => false;
    public override long Length => stream.Length;
    public override long Position { get => totalIndex; set => throw new NotSupportedException(); }

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

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
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

        for (int i = 0; i < buffer.Length; i++)
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

                // A single ReadAsync is not guaranteed to fill the buffer
                // so keep reading until the block is complete or EOF
                var read = 0;
                while (read < 8)
                {
                    var r = await stream.ReadAsync(memoryBuffer.AsMemory(read, 8 - read), cancellationToken).ConfigureAwait(false);

                    if (r == 0)
                    {
                        break; // True end of stream
                    }

                    read += r;
                }

                if (read < 8)
                {
                    return i; // Return actual bytes read so far
                }

                var nextIV = BinaryPrimitives.ReadUInt64LittleEndian(memoryBuffer);

                // Trick #3: Switch Decrypt with Encrypt
                if (version >= 18)
                {
                    blowfish.Encrypt(memoryBuffer.AsSpan());
                }
                else
                {
                    blowfish.Decrypt(memoryBuffer.AsSpan());
                }

                var block = BinaryPrimitives.ReadUInt64LittleEndian(memoryBuffer);
                block ^= iv;
                BitConverter.GetBytes(block).CopyTo(memoryBuffer, 0);

                // Trick #4: Custom nextIV logic
                // (not tested below version 12)
                if (version >= 12)
                {
                    iv = (iv >> 0x2f) ^ (iv * 9) ^ nextIV;
                }
                else
                {
                    iv = nextIV;
                }
            }

            buffer.Span[i] = memoryBuffer[bufferIndex & 7];
            bufferIndex++;
            totalIndex++;
        }

        return buffer.Length;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return Read(buffer.AsSpan(offset, count));
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}