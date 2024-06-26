﻿namespace GBX.NET;

/// <summary>
/// [GmRectAligned]
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="X2"></param>
/// <param name="Y2"></param>
public readonly record struct Rect(float X, float Y, float X2, float Y2)
{
    public static readonly Rect Zero = new();

    public static implicit operator Rect((float X, float Y, float X2, float Y2) v) => new(v.X, v.Y, v.X2, v.Y2);

    public static explicit operator Rect(Vec4 v) => new(v.X, v.Y, v.Z, v.W);
}