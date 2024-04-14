namespace GBX.NET;

public static class AdditionalMath
{
    public static float ToRadians(float degree)
    {
        return (float)(degree / 180 * Math.PI);
    }

    public static float ToRadians(Direction direction)
    {
        return (float)((int)direction * Math.PI / 2);
    }

    public static Vec3 ToRadians(Vec3 pitchYawRoll)
    {
        return (ToRadians(pitchYawRoll.X), ToRadians(pitchYawRoll.Y), ToRadians(pitchYawRoll.Z));
    }

    public static float ToDegrees(float radians)
    {
        return (float)(radians / Math.PI * 180);
    }

    public static Vec3 ToDegrees(Vec3 pitchYawRoll)
    {
        return (ToDegrees(pitchYawRoll.X), ToDegrees(pitchYawRoll.Y), ToDegrees(pitchYawRoll.Z));
    }

    public static Vec3 RotateAroundCenter(Vec3 point, Vec3 center, float radians)
    {
        return (
            (float)(Math.Cos(radians) * (point.X - center.X) - Math.Sin(radians) * (point.Z - center.Z) + center.X),
            point.Y,
            (float)(Math.Sin(radians) * (point.X - center.X) + Math.Cos(radians) * (point.Z - center.Z) + center.Z)
        );
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