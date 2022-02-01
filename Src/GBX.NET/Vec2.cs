namespace GBX.NET;

public readonly record struct Vec2(float X, float Y) : IVec
{
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public float GetMagnitude() => MathF.Sqrt(GetSqrMagnitude());
#endif

#if NETSTANDARD2_0 || NET462_OR_GREATER
    public float GetMagnitude() => (float)Math.Sqrt(GetSqrMagnitude());
#endif

    public float GetSqrMagnitude() => X * X + Y * Y;

    public static readonly Vec2 Zero = new();
    public static float GetDotProduct(Vec2 a, Vec2 b) => a.X * b.X + a.Y * b.Y;

    public static Vec2 operator +(Vec2 a, Vec2 b) => new (a.X + b.X, a.Y + b.Y);
    public static Vec2 operator +(Vec2 a, Int2 b) => new (a.X + b.X, a.Y + b.Y);
    public static Vec2 operator +(Vec2 a, int b) => new (a.X + b, a.Y + b);
    public static Vec2 operator +(Vec2 a, float b) => new (a.X + b, a.Y + b);

    public static Vec2 operator +(Int2 a, Vec2 b) => b + a;
    public static Vec2 operator +(int a, Vec2 b) => b + a;
    public static Vec2 operator +(float a, Vec2 b) => b + a;

    public static Vec2 operator -(Vec2 a) => new (-a.X, -a.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new (a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 a, Int2 b) => new (a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 a, int b) => new (a.X - b, a.Y - b);
    public static Vec2 operator -(Vec2 a, float b) => new (a.X - b, a.Y - b);

    public static Vec2 operator *(Vec2 a, Vec2 b) => new (a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(Vec2 a, Int2 b) => new (a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(Vec2 a, int b) => new (a.X * b, a.Y * b);
    public static Vec2 operator *(Vec2 a, float b) => new (a.X * b, a.Y * b);

    public static Vec2 operator *(Int2 a, Vec2 b) => b * a;
    public static Vec2 operator *(int a, Vec2 b) => b * a;
    public static Vec2 operator *(float a, Vec2 b) => b * a;

    public static implicit operator Vec2(Int2 a) => new (a.X, a.Y);
    public static implicit operator Vec2((float X, float Y) v) => new (v.X, v.Y);
    public static implicit operator (float X, float Y)(Vec2 v) => (v.X, v.Y);

    public static explicit operator Vec2(Vec3 a) => new(a.X, a.Y);
    public static explicit operator Vec2(Vec4 a) => new(a.X, a.Y);

    public static explicit operator Vec2(ReadOnlySpan<float> a) => GetVec2FromReadOnlySpan(a);
    public static explicit operator Vec2(Span<float> a) => GetVec2FromReadOnlySpan(a);
    public static explicit operator Vec2(float[] a) => GetVec2FromReadOnlySpan(a);

    public static Vec2 GetVec2FromReadOnlySpan(ReadOnlySpan<float> a) => a.Length switch
    {
        0 => default,
        1 => new(a[0], 0),
        _ => new(a[0], a[1])
    };
}
