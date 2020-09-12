using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public struct Int3
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        [IgnoreDataMember]
        public Int3 XZ => new Int3(X, 0, Z);

        public Int3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static implicit operator Int3((int X, int Y, int Z) v)
        {
            return new Int3(v.X, v.Y, v.Z);
        }

        public static implicit operator (int X, int Y, int Z)(Int3 v)
        {
            return (v.X, v.Y, v.Z);
        }

        public override bool Equals(object obj)
        {
            if(obj is Int3 int3)
                return X == int3.X && Y == int3.Y && Z == int3.Z;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public static Int3 operator +(Int3 a, Int3 b) => new Int3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Int3 operator +(Int3 a, int b) => new Int3(a.X * b, a.Y * b, a.Z * b);

        public static Int3 operator -(Int3 a, Int3 b) => new Int3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Int3 operator *(Int3 a, Int3 b) => new Int3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Int3 operator *(Int3 a, int b) => new Int3(a.X * b, a.Y * b, a.Z * b);

        public static bool operator ==(Int3 a, Int3 b) => a.Equals(b);
        public static bool operator !=(Int3 a, Int3 b) => !a.Equals(b);

        public static explicit operator Int3(Byte3 a) => new Int3(a.X, a.Y, a.Z);
        public static explicit operator Int3(Int2 a) => new Int3(a.X, 0, a.Y);

        public static implicit operator Vec3(Int3 a) => new Vec3(a.X, a.Y, a.Z);

        public static explicit operator Int3(Direction a)
        {
            switch (a)
            {
                case Direction.North:
                    return (0, 0, 1);
                case Direction.East:
                    return (-1, 0, 0);
                case Direction.South:
                    return (0, 0, -1);
                case Direction.West:
                    return (1, 0, 0);
                default:
                    return (0, 0, 0);
            }
        }
    }
}
