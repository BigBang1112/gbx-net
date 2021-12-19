using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace GBX.NET;

public struct Vec3 : IVec
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    [IgnoreDataMember]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Vec3 XZ => new(X, 0, Z);

    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Deconstruct(out float x, out float y, out float z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public float GetMagnitude() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    public float GetSqrMagnitude() => X * X + Y * Y + Z * Z;

    public override string ToString()
    {
        var x = X.ToString(CultureInfo.InvariantCulture);
        var y = Y.ToString(CultureInfo.InvariantCulture);
        var z = Z.ToString(CultureInfo.InvariantCulture);

        return $"<{x}, {y}, {z}>";
    }

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    public override bool Equals(object? obj) => obj is Vec3 a && a == this;

    public static bool operator ==(Vec3 a, Vec3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vec3 a, Vec3 b) => !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator +(Vec3 a, Int3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator +(Vec3 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y, a.Z);
    public static Vec3 operator +(Vec3 a, Int2 b) => new(a.X + b.X, a.Y + b.Y, a.Z);
    public static Vec3 operator +(Vec3 a, int b) => new(a.X + b, a.Y + b, a.Z + b);
    public static Vec3 operator +(Vec3 a, float b) => new(a.X + b, a.Y + b, a.Z + b);

    public static Vec3 operator +(Int3 a, Vec3 b) => b + a;
    public static Vec3 operator +(Vec2 a, Vec3 b) => b + a;
    public static Vec3 operator +(Int2 a, Vec3 b) => b + a;
    public static Vec3 operator +(int a, Vec3 b) => b + a;
    public static Vec3 operator +(float a, Vec3 b) => b + a;

    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator -(Vec3 a, Int3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator -(Vec3 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y, a.Z);
    public static Vec3 operator -(Vec3 a, Int2 b) => new(a.X - b.X, a.Y - b.Y, a.Z);
    public static Vec3 operator -(Vec3 a, int b) => new(a.X - b, a.Y - b, a.Z - b);
    public static Vec3 operator -(Vec3 a, float b) => new(a.X - b, a.Y - b, a.Z - b);

    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator *(Vec3 a, Int3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator *(Vec3 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y, a.Z);
    public static Vec3 operator *(Vec3 a, Int2 b) => new(a.X * b.X, a.Y * b.Y, a.Z);
    public static Vec3 operator *(Vec3 a, int b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vec3 operator *(Vec3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Vec3 operator *(Int3 a, Vec3 b) => b * a;
    public static Vec3 operator *(Vec2 a, Vec3 b) => b * a;
    public static Vec3 operator *(Int2 a, Vec3 b) => b * a;
    public static Vec3 operator *(int a, Vec3 b) => b * a;
    public static Vec3 operator *(float a, Vec3 b) => b * a;

    public static implicit operator Vec3(Int3 a) => new(a.X, a.Y, a.Z);
    public static implicit operator Vec3((float X, float Y, float Z) v) => new(v.X, v.Y, v.Z);
    public static implicit operator (float X, float Y, float Z)(Vec3 v) => (v.X, v.Y, v.Z);

    public static explicit operator Vec3(Byte3 a) => new(a.X, a.Y, a.Z);
    public static explicit operator Vec3(Int2 a) => new(a.X, 0, a.Y);
    public static explicit operator Vec3(Vec2 a) => new(a.X, a.Y, 0);
    public static explicit operator Vec3(Vec4 a) => new(a.X, a.Y, a.Z);
    public static explicit operator Vec3(float[] a) => a == null ? new Vec3() : a.Length >= 3 ? new Vec3(a[0], a[1], a[2]) : throw new Exception();
}
