namespace GBX.NET;

public readonly record struct UInt256
{
    public static readonly UInt256 Zero = new();

    private readonly UInt128 high;
    private readonly UInt128 low;

    internal UInt128 High => high;
    internal UInt128 Low => low;

    public UInt256(UInt128 high, UInt128 low)
    {
        this.high = high;
        this.low = low;
    }

    public UInt256(ulong a, ulong b, ulong c, ulong d)
    {
        high = new UInt128(a, b);
        low = new UInt128(c, d);
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
}