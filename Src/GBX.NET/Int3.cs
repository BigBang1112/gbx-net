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
    /// Implicitly converts a <see cref="Byte3"/> to an <see cref="Int3"/> vector.
    /// </summary>
    /// <param name="a">The <see cref="Byte3"/> to convert.</param>
    public static implicit operator Int3(Byte3 a) => new(a.X, a.Y, a.Z);

    public static explicit operator Int3(Vec3 a) => new((int)a.X, (int)a.Y, (int)a.Z);

    public static Int3 operator +(Int3 a, Int3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Int3 operator -(Int3 a) => new(-a.X, -a.Y, -a.Z);
    public static Int3 operator -(Int3 a, Int3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Int3 operator *(Int3 a, Int3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
}