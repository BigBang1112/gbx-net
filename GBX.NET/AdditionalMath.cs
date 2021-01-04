using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
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
            return ( ToRadians(pitchYawRoll.X), ToRadians(pitchYawRoll.Y), ToRadians(pitchYawRoll.Z) );
        }

        public static float ToDegrees(float radians)
        {
            return (float)(radians / Math.PI * 180);
        }

        public static Vec3 ToDegrees(Vec3 pitchYawRoll)
        {
            return ( ToDegrees(pitchYawRoll.X), ToDegrees(pitchYawRoll.Y), ToDegrees(pitchYawRoll.Z) );
        }

        public static Vec3 RotateAroundCenter(Vec3 point, Vec3 center, float radians)
        {
            return (
                (float)(Math.Cos(radians) * (point.X - center.X) - Math.Sin(radians) * (point.Z - center.Z) + center.X),
                point.Y,
                (float)(Math.Sin(radians) * (point.X - center.X) + Math.Cos(radians) * (point.Z - center.Z) + center.Z)
            );
        }
    }
}
