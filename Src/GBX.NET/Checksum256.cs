namespace GBX.NET;

public readonly record struct Checksum256
{
    public static readonly Checksum256 Zero = new();

    private readonly Checksum128 low;
    private readonly Checksum128 high;

    internal Checksum128 Low => low;
    internal Checksum128 High => high;

    internal Checksum256(ulong a, ulong b, ulong c, ulong d)
    {
        low = new Checksum128(a, b);
        high = new Checksum128(c, d);
    }

    public Checksum256(ReadOnlySpan<byte> span)
    {
        if (span.Length < 32) throw new ArgumentException("Byte span must be at least 32 bytes.", nameof(span));

        low = new Checksum128(span.Slice(0, 16));
        high = new Checksum128(span.Slice(16, 16));
    }

    public void WriteLittleEndian(Span<byte> destination)
    {
        if (destination.Length < 32)
        {
            throw new ArgumentException("Destination span is too short.", nameof(destination));
        }

        low.WriteLittleEndian(destination);
        high.WriteLittleEndian(destination.Slice(16));
    }

    public byte[] GetBytes()
    {
        var bytes = new byte[32];
        WriteLittleEndian(bytes);
        return bytes;
    }

    public override string ToString()
    {
        return string.Concat(GetBytes().Select(b => b.ToString("x2")));
    }
}
