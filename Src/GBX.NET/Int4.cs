namespace GBX.NET;

public readonly record struct Int4(int X, int Y, int Z, int W)
{
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}, {W}>";
    }

    public static readonly Int4 Zero = new();
}
