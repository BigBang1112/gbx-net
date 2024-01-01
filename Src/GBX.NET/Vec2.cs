namespace GBX.NET;

/// <summary>
/// [GmVec2] Represents a two-dimensional vector with <see cref="float"/> components (X, Y).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
public readonly record struct Vec2(float X, float Y)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Vec2 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Vec2"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of floats to an <see cref="Vec2"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X and Y components.</param>
    public static implicit operator Vec2((float X, float Y) v) => new(v.X, v.Y);

    /// <summary>
    /// Implicitly converts an <see cref="Vec2"/> vector to a tuple of integers.
    /// </summary>
    /// <param name="v">The <see cref="Vec2"/> vector to convert.</param>
    public static implicit operator (float X, float Y)(Vec2 v) => (v.X, v.Y);
}