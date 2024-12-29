namespace GBX.NET;

public static class AdditionalMath
{
    public static float ToRadians(float degree)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return degree / 180 * MathF.PI;
#else
        return (float)(degree / 180 * Math.PI);
#endif
    }

    public static float ToRadians(Direction direction)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return (int)direction * MathF.PI / 2;
#else
        return (float)((int)direction * Math.PI / 2);
#endif
    }

    public static Vec3 ToRadians(Vec3 pitchYawRoll)
    {
        return (ToRadians(pitchYawRoll.X), ToRadians(pitchYawRoll.Y), ToRadians(pitchYawRoll.Z));
    }

    public static float ToDegrees(float radians)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return radians / MathF.PI * 180;
#else
        return (float)(radians / Math.PI * 180);
#endif
    }

    public static Vec3 ToDegrees(Vec3 pitchYawRoll)
    {
        return (ToDegrees(pitchYawRoll.X), ToDegrees(pitchYawRoll.Y), ToDegrees(pitchYawRoll.Z));
    }

    public static Vec3 RotateAroundCenter(Vec3 point, Vec3 center, float radians)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return (
            MathF.Cos(radians) * (point.X - center.X) - MathF.Sin(radians) * (point.Z - center.Z) + center.X,
            point.Y,
            MathF.Sin(radians) * (point.X - center.X) + MathF.Cos(radians) * (point.Z - center.Z) + center.Z
        );
#else
        return (
            (float)(Math.Cos(radians) * (point.X - center.X) - Math.Sin(radians) * (point.Z - center.Z) + center.X),
            point.Y,
            (float)(Math.Sin(radians) * (point.X - center.X) + Math.Cos(radians) * (point.Z - center.Z) + center.Z)
        );
#endif
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    public static Vec2 Lerp(Vec2 a, Vec2 b, float t)
    {
        return a + t * (b - a);
    }

    public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
    {
        return a + t * (b - a);
    }

    public static Quat Lerp(Quat a, Quat b, float t)
    {
        return a + t * (b - a);
    }
}