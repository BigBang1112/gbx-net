namespace GBX.NET;

public sealed class MacroblockInstance
{
    public int Flags { get; set; }

    public override string ToString()
    {
        return $"{nameof(MacroblockInstance)} {{ Flags = {Flags} }}";
    }
}