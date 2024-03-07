namespace GBX.NET;

#if NETSTANDARD2_0 || !NET7_0_OR_GREATER

public readonly record struct Int128
{
    public static readonly Int128 Zero = new();

    private readonly ulong high;
    private readonly ulong low;

    internal ulong High => high;
    internal ulong Low => low;

    public Int128(ulong high, ulong low)
    {
        this.high = high;
        this.low = low;
    }
}

#endif
