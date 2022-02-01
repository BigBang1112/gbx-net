namespace GBX.NET;

/// <summary>
/// 3-dimensional byte coordination struct. Implicit <see cref="ValueTuple{T1, T2, T3}"/> is available.
/// </summary>
/// <param name="X">X value.</param>
/// <param name="Y">Y value.</param>
/// <param name="Z">Z value.</param>
public readonly record struct Byte3(byte X, byte Y, byte Z)
{
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    public static Byte3 operator +(Byte3 a, Byte3 b) => new((byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z));
    public static Byte3 operator +(Byte3 a, byte b) => new((byte)(a.X + b), (byte)(a.Y + b), (byte)(a.Z + b));

    public static Byte3 operator -(Byte3 a, Byte3 b) => new((byte)(a.X - b.X), (byte)(a.Y - b.Y), (byte)(a.Z - b.Z));
    public static Byte3 operator -(Byte3 a, byte b) => new((byte)(a.X - b), (byte)(a.Y - b), (byte)(a.Z - b));

    public static Byte3 operator *(Byte3 a, Byte3 b) => new((byte)(a.X * b.X), (byte)(a.Y * b.Y), (byte)(a.Z * b.Z));
    public static Byte3 operator *(Byte3 a, byte b) => new((byte)(a.X * b), (byte)(a.Y * b), (byte)(a.Z * b));

    public static implicit operator Byte3((byte X, byte Y, byte Z) v) => new(v.X, v.Y, v.Z);
    public static implicit operator (byte X, byte Y, byte Z)(Byte3 v) => (v.X, v.Y, v.Z);

    public static explicit operator Byte3(Int3 a) => new((byte)a.X, (byte)a.Y, (byte)a.Z);
}
