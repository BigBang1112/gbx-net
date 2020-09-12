using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public struct Vec3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static implicit operator Vec3((float x, float y, float z) v)
        {
            return new Vec3(v.x, v.y, v.z);
        }

        public static implicit operator (float x, float y, float z)(Vec3 v)
        {
            return (v.X, v.Y, v.Z);
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
}
