using System.Numerics;

namespace GBX.NET;

/// <summary>
/// [GmVec4] Represents a three-dimensional vector with <see cref="float"/> components (X, Y, Z, W).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
/// <param name="Z">The Z-component of the vector.</param>
/// <param name="W">The W-component of the vector.</param>
public readonly record struct Vec4(float X, float Y, float Z, float W)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Vec4 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Vec4"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y, Z, W&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}, {W}>";
    }

    public Vector4 AsVector4() => new(X, Y, Z, W);

    /// <summary>
    /// Implicitly converts a tuple of floats to an <see cref="Vec4"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, Z, and W components.</param>
    public static implicit operator Vec4((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);

    /// <summary>
    /// Implicitly converts an <see cref="Vec4"/> vector to a tuple of floats.
    /// </summary>
    /// <param name="v">The <see cref="Vec4"/> vector to convert.</param>
    public static implicit operator (float X, float Y, float Z, float W)(Vec4 v) => (v.X, v.Y, v.Z, v.W);

    public static implicit operator Vec4(Int4 a) => new(a.X, a.Y, a.Z, a.W);

    public static implicit operator Vector4(Vec4 a) => a.AsVector4();
    public static implicit operator Vec4(Vector4 a) => new(a.X, a.Y, a.Z, a.W);

    public static explicit operator Vec4(Rect a) => new(a.X, a.Y, a.X2, a.Y2);
    public static explicit operator Vec4(Quat a) => new(a.X, a.Y, a.Z, a.W);
}