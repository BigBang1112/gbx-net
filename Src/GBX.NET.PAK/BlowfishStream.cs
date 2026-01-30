using System.Buffers.Binary;

namespace GBX.NET.PAK;

public class BlowfishStream : Stream, IEncryptionInitializer
{
    private readonly Stream stream;
    private readonly Blowfish blowfish;

    private ulong iv;
    private ulong ivXor;
    private int bufferIndex;
    private int totalIndex;
    private readonly byte[] memoryBuffer;
    private readonly bool isPak18;

    public BlowfishStream(Stream stream, byte[] key, ulong iv, bool isPak18 = false)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(BlowfishStream.stream));
        blowfish = new Blowfish(key, isPak18);
        this.iv = iv;
        memoryBuffer = new byte[8];
        this.isPak18 = isPak18;
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

                // Async read of one block
                var read = await stream.ReadAsync(memoryBuffer.AsMemory(0, 8), cancellationToken).ConfigureAwait(false);

                if (read < 8)
                {
                    return i; // Return actual bytes read so far
                }

                var nextIV = BinaryPrimitives.ReadUInt64LittleEndian(memoryBuffer);

                // Trick #3: Switch Decipher with Encipher
                if (isPak18)
                {
                    blowfish.Encipher(memoryBuffer);
                }
                else
                {
                    blowfish.Decipher(memoryBuffer);
                }

                var block = BinaryPrimitives.ReadUInt64LittleEndian(memoryBuffer);
                block ^= iv;
                BitConverter.GetBytes(block).CopyTo(memoryBuffer, 0);

                // Trick #4: Custom nextIV logic
                if (isPak18)
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
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}