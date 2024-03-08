namespace GBX.NET;

/// <summary>
/// [GmInt2] Represents a two-dimensional vector with <see cref="int"/> components (X, Y).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
public readonly record struct Int2(int X, int Y)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Int2 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Int2"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of integers to an <see cref="Int2"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X and Y components.</param>
    public static implicit operator Int2((int X, int Y) v) => new(v.X, v.Y);

    public static explicit operator Int2(Vec2 v) => new((int)v.X, (int)v.Y);
}