namespace GBX.NET;

public readonly record struct Int2(int X, int Y)
{
    public static readonly Int2 Zero = new();

    public static Int2 operator +(Int2 a, Int2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Int2 operator +(Int2 a, int b) => new(a.X + b, a.Y + b);
    public static Vec2 operator +(Int2 a, float b) => new(a.X + b, a.Y + b);

    public static Int2 operator +(int a, Int2 b) => b + a;
    public static Vec2 operator +(float a, Int2 b) => b + a;

    public static Int2 operator -(Int2 a, Int2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Int2 operator -(Int2 a, int b) => new(a.X - b, a.Y - b);
    public static Vec2 operator -(Int2 a, float b) => new(a.X - b, a.Y - b);

    public static Int2 operator *(Int2 a, Int2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Int2 operator *(Int2 a, int b) => new(a.X * b, a.Y * b);
    public static Vec2 operator *(Int2 a, float b) => new(a.X * b, a.Y * b);

    public static Int2 operator *(int a, Int2 b) => b * a;
    public static Vec2 operator *(float a, Int2 b) => b * a;

    public static Int2 operator ^(Int2 a, Int2 b) => new(a.X ^ b.X, a.Y ^ b.Y);
    public static Int2 operator ^(Int2 a, int b) => new(a.X ^ b, a.Y ^ b);

    public static Int2 operator ^(int a, Int2 b) => b ^ a;

    public static implicit operator Int2((int X, int Y) v) => new(v.X, v.Y);
    public static implicit operator (int X, int Y)(Int2 v) => (v.X, v.Y);

    public static explicit operator Int2(Vec2 a) => new((int)a.X, (int)a.Y);

    public static explicit operator Int2(ReadOnlySpan<int> a) => GetInt2FromReadOnlySpan(a);
    public static explicit operator Int2(Span<int> a) => GetInt2FromReadOnlySpan(a);
    public static explicit operator Int2(int[] a) => GetInt2FromReadOnlySpan(a);

    public static Int2 GetInt2FromReadOnlySpan(ReadOnlySpan<int> a) => a.Length switch
    {
        0 => default,
        1 => new(a[0], 0),
        _ => new(a[0], a[1])
    };
}
