namespace GBX.NET.Comparers;

// Thanks! https://stackoverflow.com/a/48655058/3923447

internal sealed class Vec3EqualityComparer : EqualityComparer<Vec3>
{
    public override bool Equals(Vec3 x, Vec3 y) => GetEquatable(x) == GetEquatable(y);

    public override int GetHashCode(Vec3 vec) => GetEquatable(vec).GetHashCode();

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static Vec3 GetEquatable(Vec3 vec) => new(
        MathF.Round(vec.X, 3), 
        MathF.Round(vec.Y, 3), 
        MathF.Round(vec.Z, 3));
#else
    private static Vec3 GetEquatable(Vec3 vec) => new(
        (float)Math.Round(vec.X, 3),
        (float)Math.Round(vec.Y, 3),
        (float)Math.Round(vec.Z, 3));
#endif
}