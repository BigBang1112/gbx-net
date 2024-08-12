using System.Text;

namespace GBX.NET.Crypto;

public class MuxStream : Stream
{
    private readonly Stream stream;
    private readonly byte[] key;

    private long position;

    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;
    public override long Length => stream.Length;

    public override long Position
    {
        get => position;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            stream.Position = value;
            position = value;
        }
    }

    public MuxStream(Stream stream, int keySalt)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));

        if (!BitConverter.IsLittleEndian)
        {
            throw new PlatformNotSupportedException("Only little-endian systems are supported.");
        }

        key = MD5.Compute(BitConverter.GetBytes(keySalt).Concat(Encoding.ASCII.GetBytes("Hello,hack3r!")).ToArray());
    }

    public override void Flush() => stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
        var bytesRead = stream.Read(buffer, offset, count);
        Xor(key, buffer, offset, bytesRead);
        position += bytesRead;
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        var newPosition = stream.Seek(offset, origin);
        position = newPosition;
        return position;
    }

    public override void SetLength(long value) => stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
        Xor(key, buffer, offset, count);
        stream.Write(buffer, offset, count);
        position += count;
    }

    private void Xor(byte[] key, byte[] data, int offset, int length)
    {
        for (int i = 0; i < length; i++)
        {
            data[offset + i] ^= GetKeyStreamByte(key, (int)position + i);
        }
    }

    private static byte GetKeyStreamByte(byte[] key, int pos)
    {
        return Rol(key[pos % 16], (pos / 17) % 8);
    }

    private static byte Rol(byte input, int amount)
    {
        return (byte)((input << amount) | (input >> (8 - amount)));
    }

    // Dispose pattern to properly clean up the base stream.
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            stream?.Dispose();
        }

        base.Dispose(disposing);
    }
}
