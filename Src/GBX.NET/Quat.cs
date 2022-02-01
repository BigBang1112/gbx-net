using System.Globalization;

namespace GBX.NET;

public readonly record struct Quat(float X, float Y, float Z, float W)
{
    public Quat(Vec3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w)
    {

    }

    public Vec3 ToPitchYawRoll()
    {
        var pitch = (float)Math.Atan2(2 * X * W - 2 * Y * Z, 1 - 2 * X * X - 2 * Z * Z);
        var yaw = (float)Math.Atan2(2 * Y * W - 2 * X * Z, 1 - 2 * Y * Y - 2 * Z * Z);
        var roll = (float)Math.Asin(2 * X * Y + 2 * Z * W);

        return (pitch, yaw, roll);
    }

    public override string ToString()
    {
        var c = CultureInfo.InvariantCulture;

        var x = X.ToString(c);
        var y = Y.ToString(c);
        var z = Z.ToString(c);
        var w = W.ToString(c);

        return $"<{x}, {y}, {z}, {w}>";
    }

    public static readonly Quat Zero = new();

    public static Quat operator +(Quat a, Quat b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Quat operator -(Quat a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Quat operator -(Quat a, Quat b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Quat operator *(Quat a, Quat b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * a.W);
    public static Quat operator *(Quat a, float b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);

    public static Quat operator *(float a, Quat b) => b * a;

    public static implicit operator Quat((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator (float X, float Y, float Z, float W)(Quat v) => (v.X, v.Y, v.Z, v.W);
}
