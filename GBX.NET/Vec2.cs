using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET
{
    public struct Vec2
    {
        public float X { get; }
        public float Y { get; }

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static implicit operator Vec2((float x, float y) v)
        {
            return new Vec2(v.x, v.y);
        }

        public static implicit operator (float x, float y)(Vec2 v)
        {
            return (v.X, v.Y);
        }
    }
}
