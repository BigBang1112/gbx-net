namespace GBX.NET;

/// <summary>
/// [GmInt3] Represents a three-dimensional vector with <see cref="int"/> components (X, Y, Z).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
/// <param name="Z">The Z-component of the vector.</param>
public readonly record struct Int3(int X, int Y, int Z)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Int3 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Int3"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y, Z&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of integers to an <see cref="Int3"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, and Z components.</param>
    public static implicit operator Int3((int X, int Y, int Z) v) => new(v.X, v.Y, v.Z);

    /// <summary>
    /// Implicitly converts an <see cref="Int3"/> vector to a tuple of integers.
    /// </summary>
    /// <param name="v">The <see cref="Int3"/> vector to convert.</param>
    public static implicit operator (int X, int Y, int Z)(Int3 v) => (v.X, v.Y, v.Z);
}