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

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public float GetMagnitude() => MathF.Sqrt(GetSqrMagnitude());
#else
    public float GetMagnitude() => (float)Math.Sqrt(GetSqrMagnitude());
#endif

    public float GetSqrMagnitude() => X * X + Y * Y + Z * Z;

    public Vec3 GetNormalized()
    {
        var magnitude = GetMagnitude();
        return magnitude == 0 ? this : new(X / magnitude, Y / magnitude, Z / magnitude);
    }

    public static float GetDotProduct(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static Vec3 GetCrossProduct(Vec3 a, Vec3 b) => new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

    /// <summary>
    /// Implicitly converts a tuple of floats to an <see cref="Vec3"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, and Z components.</param>
    public static implicit operator Vec3((float X, float Y, float Z) v) => new(v.X, v.Y, v.Z);

    public static implicit operator Vec3(Int3 a) => new(a.X, a.Y, a.Z);
}