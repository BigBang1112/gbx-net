namespace GBX.NET;

public readonly record struct Int4(int X, int Y, int Z, int W)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Int4 Zero = new();

    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}, {W}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of integers to an <see cref="Int4"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, Z, and W components.</param>
    public static implicit operator Int4((int X, int Y, int Z, int W) v) => new(v.X, v.Y, v.Z, v.W);

    public static explicit operator Int4(Vec4 a) => new((int)a.X, (int)a.Y, (int)a.Z, (int)a.W);
}