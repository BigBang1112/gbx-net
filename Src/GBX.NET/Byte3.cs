using System;

namespace GBX.NET
{
    /// <summary>
    /// 3-dimensional byte coordination struct. Implicit <see cref="ValueTuple{T1, T2, T3}"/> is available.
    /// </summary>
    public struct Byte3
    {
        public byte X { get; }
        public byte Y { get; }
        public byte Z { get; }

        /// <summary>
        /// Constructs <see cref="Byte3"/> with three byte values.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public Byte3(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Converts the bytes to a string format of "({X}, {Y}, {Z})".
        /// </summary>
        /// <returns>Returns formatted <see cref="string"/>.</returns>
        public override string ToString() => $"({X}, {Y}, {Z})";
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        public override bool Equals(object? obj) => obj is Byte3 a && a == this;

        public static bool operator ==(Byte3 a, Byte3 b) => a.X == b.X && b.Y == b.Y && b.Z == b.Z;
        public static bool operator !=(Byte3 a, Byte3 b) => !(a.X == b.X && b.Y == b.Y && b.Z == b.Z);

        public static Byte3 operator +(Byte3 a, Byte3 b) => new Byte3((byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z));
        public static Byte3 operator +(Byte3 a, byte b) => new Byte3((byte)(a.X + b), (byte)(a.Y + b), (byte)(a.Z + b));

        public static Byte3 operator -(Byte3 a, Byte3 b) => new Byte3((byte)(a.X - b.X), (byte)(a.Y - b.Y), (byte)(a.Z - b.Z));
        public static Byte3 operator -(Byte3 a, byte b) => new Byte3((byte)(a.X - b), (byte)(a.Y - b), (byte)(a.Z - b));

        public static Byte3 operator *(Byte3 a, Byte3 b) => new Byte3((byte)(a.X * b.X), (byte)(a.Y * b.Y), (byte)(a.Z * b.Z));
        public static Byte3 operator *(Byte3 a, byte b) => new Byte3((byte)(a.X * b), (byte)(a.Y * b), (byte)(a.Z * b));

        public static implicit operator Byte3((byte X, byte Y, byte Z) v) => new Byte3(v.X, v.Y, v.Z);
        public static implicit operator (byte X, byte Y, byte Z)(Byte3 v) => (v.X, v.Y, v.Z);

        public static explicit operator Byte3(Int3 a) => new Byte3((byte)a.X, (byte)a.Y, (byte)a.Z);
    }
}
