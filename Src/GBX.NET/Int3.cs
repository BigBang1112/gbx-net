using System.Globalization;

namespace GBX.NET;

public readonly record struct Int3(int X, int Y, int Z)
{
    public Int3 GetXY() => new(X, Y, 0);
    public Int3 GetXZ() => new(X, 0, Z);
    public Int3 GetYZ() => new(0, Y, Z);

    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    public static readonly Int3 Zero = new();

    public static Int3 operator +(Int3 a, Int3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Int3 operator +(Int3 a, Int2 b) => new(a.X + b.X, a.Y + b.Y, a.Z);
    public static Int3 operator +(Int3 a, int b) => new(a.X + b, a.Y + b, a.Z + b);
    public static Vec3 operator +(Int3 a, float b) => new(a.X + b, a.Y + b, a.Z + b);

    public static Int3 operator +(Int2 a, Int3 b) => b + a;
    public static Int3 operator +(int a, Int3 b) => b + a;
    public static Vec3 operator +(float a, Int3 b) => b + a;

    public static Int3 operator -(Int3 a) => new(-a.X, -a.Y, -a.Z);
    public static Int3 operator -(Int3 a, Int3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Int3 operator -(Int3 a, Int2 b) => new(a.X - b.X, a.Y - b.Y, a.Z);
    public static Int3 operator -(Int3 a, int b) => new(a.X - b, a.Y - b, a.Z - b);
    public static Vec3 operator -(Int3 a, float b) => new(a.X - b, a.Y - b, a.Z - b);

    public static Int3 operator *(Int3 a, Int3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Int3 operator *(Int3 a, Int2 b) => new(a.X * b.X, a.Y * b.Y, a.Z);
    public static Int3 operator *(Int3 a, int b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vec3 operator *(Int3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Int3 operator *(Int2 a, Int3 b) => b * a;
    public static Int3 operator *(int a, Int3 b) => b * a;
    public static Vec3 operator *(float a, Int3 b) => b * a;

    public static Int3 operator ^(Int3 a, Int3 b) => new(a.X ^ b.X, a.Y ^ b.Y, a.Z ^ b.Z);
    public static Int3 operator ^(Int3 a, Int2 b) => new(a.X ^ b.X, a.Y ^ b.Y, a.Z);
    public static Int3 operator ^(Int3 a, int b) => new(a.X ^ b, a.Y ^ b, a.Z ^ b);

    public static Int3 operator ^(Int2 a, Int3 b) => b ^ a;
    public static Int3 operator ^(int a, Int3 b) => b ^ a;

    public static implicit operator Int3((int X, int Y, int Z) v) => new(v.X, v.Y, v.Z);
    public static implicit operator (int X, int Y, int Z)(Int3 v) => (v.X, v.Y, v.Z);

    public static explicit operator Int3(Vec3 a) => new((int)a.X, (int)a.Y, (int)a.Z);
    public static explicit operator Int3(Byte3 a) => new(a.X, a.Y, a.Z);

    public static explicit operator Int3(ReadOnlySpan<int> a) => GetInt3FromReadOnlySpan(a);
    public static explicit operator Int3(Span<int> a) => GetInt3FromReadOnlySpan(a);
    public static explicit operator Int3(int[] a) => GetInt3FromReadOnlySpan(a);

    public static explicit operator Int3(Direction a) => a switch
    {
        Direction.North => (0, 0, 1),
        Direction.East => (-1, 0, 0),
        Direction.South => (0, 0, -1),
        Direction.West => (1, 0, 0),
        _ => (0, 0, 0),
    };

    public static Int3 GetInt3FromReadOnlySpan(ReadOnlySpan<int> a) => a.Length switch
    {
        0 => default,
        1 => new(a[0], 0, 0),
        2 => new(a[0], a[1], 0),
        _ => new(a[0], a[1], a[2])
    };
}
