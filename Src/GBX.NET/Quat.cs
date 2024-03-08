namespace GBX.NET;

public readonly record struct Quat(float X, float Y, float Z, float W)
{
    public static readonly Quat Zero = new();

    public Quat(Vec3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}, {W}>";
    }

    public Vec3 ToPitchYawRoll()
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var pitch = MathF.Atan2(2 * X * W - 2 * Y * Z, 1 - 2 * X * X - 2 * Z * Z);
        var yaw = MathF.Atan2(2 * Y * W - 2 * X * Z, 1 - 2 * Y * Y - 2 * Z * Z);
        var roll = MathF.Asin(2 * X * Y + 2 * Z * W);
#else
        var pitch = (float)Math.Atan2(2 * X * W - 2 * Y * Z, 1 - 2 * X * X - 2 * Z * Z);
        var yaw = (float)Math.Atan2(2 * Y * W - 2 * X * Z, 1 - 2 * Y * Y - 2 * Z * Z);
        var roll = (float)Math.Asin(2 * X * Y + 2 * Z * W);
#endif

        return (pitch, yaw, roll);
    }

    public static implicit operator Quat((float X, float Y, float Z, float W) v) => new(v.X, v.Y, v.Z, v.W);
}