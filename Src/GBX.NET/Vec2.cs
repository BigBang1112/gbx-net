namespace GBX.NET;

public struct Vec2 : IVec
{
    public float X { get; }
    public float Y { get; }

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float GetMagnitude() => (float)Math.Sqrt(X * X + Y * Y);
    public float GetSqrMagnitude() => X * X + Y * Y;

    public override string ToString() => $"({X}, {Y})";
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    public override bool Equals(object? obj) => obj is Vec2 a && a == this;

    public static bool operator ==(Vec2 a, Vec2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) => !(a.X == b.X && a.Y == b.Y);

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

    public static explicit operator Vec2(Vec3 a) => new (a.X, a.Y);
    public static explicit operator Vec2(Vec4 a) => new (a.X, a.Y);
    public static explicit operator Vec2(float[] a) => a == null ? new Vec2() : a.Length >= 2 ? new Vec2(a[0], a[1]) : throw new Exception();
}
