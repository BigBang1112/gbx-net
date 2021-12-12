using System.Globalization;

namespace GBX.NET;

public struct Vec4 : IVec
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float W { get; }

    public Vec4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public override string ToString()
    {
        var x = X.ToString(CultureInfo.InvariantCulture);
        var y = Y.ToString(CultureInfo.InvariantCulture);
        var z = Z.ToString(CultureInfo.InvariantCulture);
        var w = W.ToString(CultureInfo.InvariantCulture);

        return $"<{x}, {y}, {z}, {w}>";
    }

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
    public override bool Equals(object? obj) => obj is Vec4 a && a == this;

    public static bool operator ==(Vec4 a, Vec4 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(Vec4 a, Vec4 b) => !(a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W);
    public static Vec4 operator +(Vec4 a, Vec4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Vec4 operator -(Vec4 a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Vec4 operator *(Vec4 a, Vec4 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * a.W);

    public static implicit operator Vec4((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator (float X, float Y, float Z, float W)(Vec4 v) => (v.X, v.Y, v.Z, v.W);

    public static explicit operator Vec4(Vec2 a) => new(a.X, a.Y, 0, 0);
    public static explicit operator Vec4(Vec3 a) => new(a.X, a.Y, a.Z, 0);
    public static explicit operator Vec4(float[] a) => a == null ? new Vec4() : a.Length >= 4 ? new Vec4(a[0], a[1], a[2], a[3]) : throw new Exception();
}
