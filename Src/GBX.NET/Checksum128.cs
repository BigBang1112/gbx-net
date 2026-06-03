using System.Buffers.Binary;

namespace GBX.NET;

public readonly record struct Checksum128
{
    public static readonly Checksum128 Zero = new();

    private readonly ulong low;
    private readonly ulong high;

    internal ulong Low => low;
    internal ulong High => high;

    internal Checksum128(ulong low, ulong high)
    {
        this.high = high;
        this.low = low;
    }

    public Checksum128(ReadOnlySpan<byte> span)
    {
        if (span.Length < 16) throw new ArgumentException("Byte span must be at least 16 bytes.", nameof(span));

        low = BinaryPrimitives.ReadUInt64LittleEndian(span);
        high = BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8));
    }

    public void WriteLittleEndian(Span<byte> destination)
    {
        if (destination.Length < 16)
        {
            throw new ArgumentException("Destination span is too short.", nameof(destination));
        }

        BinaryPrimitives.WriteUInt64LittleEndian(destination, low);
        BinaryPrimitives.WriteUInt64LittleEndian(destination.Slice(8), high);
    }

    public byte[] GetBytes()
    {
        var bytes = new byte[16];
        WriteLittleEndian(bytes);
        return bytes;
    }

    public override string ToString()
    {
#if NET8_0_OR_GREATER
        Span<byte> span = stackalloc byte[16];
        WriteLittleEndian(span);
        return Convert.ToHexString(span);
#else
        return string.Concat(GetBytes().Select(b => b.ToString("X2")));
#endif
    }
}
