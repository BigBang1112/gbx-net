namespace GBX.NET;

#if NETSTANDARD2_0 || !NET7_0_OR_GREATER

public readonly record struct UInt128
{
    public static readonly UInt128 Zero = new();
    
    private readonly ulong low;
    private readonly ulong high;
    
    internal ulong Low => low;
    internal ulong High => high;

    public UInt128(ulong high, ulong low)
    {
        this.high = high;
        this.low = low;
    }
}

#endif