using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public struct Byte3
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }

        public Byte3(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static implicit operator Byte3((byte, byte, byte) v)
        {
            return new Byte3(v.Item1, v.Item2, v.Item3);
        }

        public static implicit operator (byte, byte, byte)(Byte3 v)
        {
            return (v.X, v.Y, v.Z);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Byte3)) return false;
            return X == ((Byte3)obj).X && Y == ((Byte3)obj).Y && Z == ((Byte3)obj).Z;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public static Byte3 operator +(Byte3 a, Byte3 b) => new Byte3((byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z));
        public static Byte3 operator -(Byte3 a, Byte3 b) => new Byte3((byte)(a.X - b.X), (byte)(a.Y - b.Y), (byte)(a.Z - b.Z));
        public static bool operator ==(Byte3 a, Byte3 b) => a.Equals(b);
        public static bool operator !=(Byte3 a, Byte3 b) => !a.Equals(b);

        public static Byte3 operator +(Byte3 a, (int, int, int) b) => new Byte3((byte)(a.X + b.Item1), (byte)(a.Y + b.Item2), (byte)(a.Z + b.Item3));
        public static Byte3 operator -(Byte3 a, (int, int, int) b) => new Byte3((byte)(a.X - b.Item1), (byte)(a.Y - b.Item2), (byte)(a.Z - b.Item3));

        public static explicit operator Byte3(Int3 a) => new Byte3((byte)a.X, (byte)a.Y, (byte)a.Z);
    }
}
