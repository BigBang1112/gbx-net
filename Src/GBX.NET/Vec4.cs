using System.Globalization;

namespace GBX.NET;

public readonly record struct Vec4(float X, float Y, float Z, float W) : IVec
{
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public float GetMagnitude() => MathF.Sqrt(GetSqrMagnitude());
#endif

#if NETSTANDARD2_0 || NET462_OR_GREATER
    public float GetMagnitude() => (float)Math.Sqrt(GetSqrMagnitude());
#endif

    public float GetSqrMagnitude() => X * X + Y * Y + Z * Z + W * W;

    public override string ToString()
    {
        var c = CultureInfo.InvariantCulture;

        var x = X.ToString(c);
        var y = Y.ToString(c);
        var z = Z.ToString(c);
        var w = W.ToString(c);

        return $"<{x}, {y}, {z}, {w}>";
    }

    public static readonly Vec4 Zero = new();
    public static float GetDotProduct(Vec4 a, Vec4 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

    public static Vec4 operator +(Vec4 a, Vec4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

    public static Vec4 operator -(Vec4 a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Vec4 operator -(Vec4 a, Vec4 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

    public static Vec4 operator *(Vec4 a, Vec4 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * a.W);

    public static implicit operator Vec4((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator (float X, float Y, float Z, float W)(Vec4 v) => (v.X, v.Y, v.Z, v.W);

    public static explicit operator Vec4(Vec2 a) => new(a.X, a.Y, 0, 0);
    public static explicit operator Vec4(Vec3 a) => new(a.X, a.Y, a.Z, 0);

    public static explicit operator Vec4(ReadOnlySpan<float> a) => GetVec4FromReadOnlySpan(a);
    public static explicit operator Vec4(Span<float> a) => GetVec4FromReadOnlySpan(a);
    public static explicit operator Vec4(float[] a) => GetVec4FromReadOnlySpan(a);

    public static Vec4 GetVec4FromReadOnlySpan(ReadOnlySpan<float> a) => a.Length switch
    {
        0 => default,
        1 => new(a[0], 0, 0, 0),
        2 => new(a[0], a[1], 0, 0),
        3 => new(a[0], a[1], a[2], 0),
        _ => new(a[0], a[1], a[2], a[3])
    };
}
