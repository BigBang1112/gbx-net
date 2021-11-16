using System;

namespace GBX.NET;

public struct Quaternion
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float W { get; }

    public Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Quaternion(Vec3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w)
    {

    }

    public Vec3 ToPitchYawRoll()
    {
        var pitch = (float)Math.Atan2(2 * X * W - 2 * Y * Z, 1 - 2 * X * X - 2 * Z * Z);
        var yaw = (float)Math.Atan2(2 * Y * W - 2 * X * Z, 1 - 2 * Y * Y - 2 * Z * Z);
        var roll = (float)Math.Asin(2 * X * Y + 2 * Z * W);

        return (pitch, yaw, roll);
    }

    public override string ToString() => $"({X}, {Y}, {Z}, {W})";
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
    public override bool Equals(object? obj) => obj is Quaternion a && a == this;

    public static bool operator ==(Quaternion a, Quaternion b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(Quaternion a, Quaternion b) => !(a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W);

    public static Quaternion operator +(Quaternion a, Quaternion b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Quaternion operator -(Quaternion a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Quaternion operator -(Quaternion a, Quaternion b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Quaternion operator *(Quaternion a, Quaternion b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * a.W);
    public static Quaternion operator *(Quaternion a, float b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);

    public static Quaternion operator *(float a, Quaternion b) => b * a;

    public static implicit operator Quaternion((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator (float X, float Y, float Z, float W)(Quaternion v) => (v.X, v.Y, v.Z, v.W);
}
