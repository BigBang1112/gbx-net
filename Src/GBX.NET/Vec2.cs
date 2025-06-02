﻿using System.Numerics;

namespace GBX.NET;

/// <summary>
/// [GmVec2] Represents a two-dimensional vector with <see cref="float"/> components (X, Y).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
public readonly record struct Vec2(float X, float Y)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Vec2 Zero = new();

    /// <summary>
    /// Returns a string representation of the <see cref="Vec2"/> vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}>";
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public float GetMagnitude() => MathF.Sqrt(GetSqrMagnitude());
#endif

#if NETSTANDARD2_0 || NET462_OR_GREATER
    public float GetMagnitude() => (float)Math.Sqrt(GetSqrMagnitude());
#endif

    public float GetSqrMagnitude() => X * X + Y * Y;

    public Vector2 AsVector2() => new(X, Y);

    /// <summary>
    /// Implicitly converts a tuple of floats to an <see cref="Vec2"/> vector.
    /// </summary>
    /// <param name="v">The tuple containing X and Y components.</param>
    public static implicit operator Vec2((float X, float Y) v) => new(v.X, v.Y);

    /// <summary>
    /// Implicitly converts an <see cref="Vec2"/> vector to a tuple of integers.
    /// </summary>
    /// <param name="v">The <see cref="Vec2"/> vector to convert.</param>
    public static implicit operator (float X, float Y)(Vec2 v) => (v.X, v.Y);

    public static implicit operator Vec2(Int2 a) => new(a.X, a.Y);

    public static implicit operator Vector2(Vec2 a) => a.AsVector2();
    public static implicit operator Vec2(Vector2 a) => new(a.X, a.Y);

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator +(Vec2 a, Int2 b) => new(a.X + b.X, a.Y + b.Y);

    public static Vec2 operator -(Vec2 a) => new(-a.X, -a.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 a, Int2 b) => new(a.X - b.X, a.Y - b.Y);

    public static Vec2 operator *(Vec2 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(Vec2 a, Int2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(Vec2 a, int b) => new(a.X * b, a.Y * b);
    public static Vec2 operator *(Vec2 a, float b) => new(a.X * b, a.Y * b);

    public static Vec2 operator *(Int2 a, Vec2 b) => b * a;
    public static Vec2 operator *(int a, Vec2 b) => b * a;
    public static Vec2 operator *(float a, Vec2 b) => b * a;

    public static Vec2 operator /(Vec2 a, int b) => new(a.X / b, a.Y / b);
    public static Vec2 operator /(Vec2 a, float b) => new(a.X / b, a.Y / b);
}