using System.Globalization;

namespace GBX.NET;

public readonly record struct Vec3(float X, float Y, float Z) : IVec
{
    public Vec3 GetXY() => new(X, Y, 0);
    public Vec3 GetXZ() => new(X, 0, Z);
    public Vec3 GetYZ() => new(0, Y, Z);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public float GetMagnitude() => MathF.Sqrt(GetSqrMagnitude());
#else
    public float GetMagnitude() => (float)Math.Sqrt(GetSqrMagnitude());
#endif

    public float GetSqrMagnitude() => X * X + Y * Y + Z * Z;

    public Vec3 GetNormalized()
    {
        var magnitude = GetMagnitude();
        return magnitude == 0 ? this : this / magnitude;
    }

    public override string ToString()
    {
        var c = CultureInfo.InvariantCulture;

        var x = X.ToString(c);
        var y = Y.ToString(c);
        var z = Z.ToString(c);

        return $"<{x}, {y}, {z}>";
    }

    public static readonly Vec3 Zero = new();
    public static float GetDotProduct(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static Vec3 GetCrossProduct(Vec3 a, Vec3 b) => new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

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
    
    public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);

    public static implicit operator Vec3(Int3 a) => new(a.X, a.Y, a.Z);
    public static implicit operator Vec3((float X, float Y, float Z) v) => new(v.X, v.Y, v.Z);
    public static implicit operator (float X, float Y, float Z)(Vec3 v) => (v.X, v.Y, v.Z);

    public static explicit operator Vec3(Byte3 a) => new(a.X, a.Y, a.Z);
    public static explicit operator Vec3(Int2 a) => new(a.X, 0, a.Y);
    public static explicit operator Vec3(Vec2 a) => new(a.X, a.Y, 0);
    public static explicit operator Vec3(Vec4 a) => new(a.X, a.Y, a.Z);

    public static explicit operator Vec3(ReadOnlySpan<float> a) => GetVec3FromReadOnlySpan(a);
    public static explicit operator Vec3(Span<float> a) => GetVec3FromReadOnlySpan(a);
    public static explicit operator Vec3(float[] a) => GetVec3FromReadOnlySpan(a);

    public static Vec3 GetVec3FromReadOnlySpan(ReadOnlySpan<float> a, int offset = 0)
    {
        return GetVec3FromReadOnlySpan(in a, offset);
    }

    public static Vec3 GetVec3FromReadOnlySpan(in ReadOnlySpan<float> a, int offset = 0)
    {
        var l = a.Length;

        if (offset > l)
        {
            return new();
        }

        var bI = offset + 1;
        var cI = offset + 2;

        return new(a[offset], bI > l ? 0 : a[bI], cI > l ? 0 : a[cI]);
    }
}
