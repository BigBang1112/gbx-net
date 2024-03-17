namespace GBX.NET;

public readonly record struct UInt256
{
    public static readonly UInt256 Zero = new();

    private readonly UInt128 low;
    private readonly UInt128 high;

    internal UInt128 Low => low;
    internal UInt128 High => high;

    public UInt256(UInt128 high, UInt128 low)
    {
        this.high = high;
        this.low = low;
    }

    /// <summary>
    /// Little endian.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    public UInt256(ulong a, ulong b, ulong c, ulong d)
    {
        low = new UInt128(b, a);
        high = new UInt128(d, c);
    }

    public void WriteLittleEndian(Span<byte> destination)
    {
        if (destination.Length < 32)
        {
            throw new ArgumentException("Destination span is too short.", nameof(destination));
        }

        Low.WriteLittleEndian(destination);
        High.WriteLittleEndian(destination.Slice(16));
    }

    public byte[] GetBytes()
    {
        var bytes = new byte[32];
        WriteLittleEndian(bytes);
        return bytes;
    }

    public override string ToString()
    {
        return $"UInt256 {{ {string.Join(", ", GetBytes())} }}";
    }
}