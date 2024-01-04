namespace GBX.NET;

/// <summary>
/// [GmVec3] Represents a three-dimensional vector with <see cref="float"/> components (X, Y, Z).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
/// <param name="Z">The Z-component of the vector.</param>
public readonly record struct Vec3(float X, float Y, float Z)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Vec3 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Vec3"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y, Z&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of floats to an <see cref="Vec3"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, and Z components.</param>
    public static implicit operator Vec3((float X, float Y, float Z) v) => new(v.X, v.Y, v.Z);

    /// <summary>
    /// Implicitly converts an <see cref="Vec3"/> vector to a tuple of floats.
    /// </summary>
    /// <param name="v">The <see cref="Vec3"/> vector to convert.</param>
    public static implicit operator (float X, float Y, float Z)(Vec3 v) => (v.X, v.Y, v.Z);

    public static implicit operator Vec3(Int3 a) => new(a.X, a.Y, a.Z);
}