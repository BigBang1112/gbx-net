namespace GBX.NET;

public struct Int2
{
    public int X { get; }
    public int Y { get; }

    public Int2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"<{X}, {Y}>";
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    public override bool Equals(object? obj) => obj is Int2 a && a == this;

    public static bool operator ==(Int2 a, Int2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Int2 a, Int2 b) => !(a.X == b.X && a.Y == b.Y);

    public static Int2 operator +(Int2 a, Int2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Int2 operator +(Int2 a, int b) => new(a.X + b, a.Y + b);

    public static Int2 operator +(int a, Int2 b) => b + a;

    public static Int2 operator -(Int2 a, Int2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Int2 operator -(Int2 a, int b) => new(a.X - b, a.Y - b);

    public static Int2 operator *(Int2 a, Int2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Int2 operator *(Int2 a, int b) => new(a.X * b, a.Y * b);

    public static Int2 operator *(int a, Int2 b) => b * a;

    public static Int2 operator ^(Int2 a, Int2 b) => new(a.X ^ b.X, a.Y ^ b.Y);
    public static Int2 operator ^(Int2 a, int b) => new(a.X ^ b, a.Y ^ b);

    public static Int2 operator ^(int a, Int2 b) => b ^ a;

    public static implicit operator Int2((int X, int Y) v) => new(v.X, v.Y);
    public static implicit operator (int X, int Y)(Int2 v) => (v.X, v.Y);
}
