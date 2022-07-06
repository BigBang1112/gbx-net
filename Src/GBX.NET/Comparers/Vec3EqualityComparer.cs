namespace GBX.NET.Comparers;

// Thanks! https://stackoverflow.com/a/48655058/3923447

internal sealed class Vec3EqualityComparer : EqualityComparer<Vec3>
{
    private readonly int digitThreshold;

    public Vec3EqualityComparer(int digitThreshold = 4)
    {
        this.digitThreshold = digitThreshold;
    }

    public override bool Equals(Vec3 x, Vec3 y) => GetEquatable(x) == GetEquatable(y);

    public override int GetHashCode(Vec3 vec) => GetEquatable(vec).GetHashCode();

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private Vec3 GetEquatable(Vec3 vec) => new(
        MathF.Round(vec.X, digitThreshold), 
        MathF.Round(vec.Y, digitThreshold), 
        MathF.Round(vec.Z, digitThreshold));
#else
    private Vec3 GetEquatable(Vec3 vec) => new(
        (float)Math.Round(vec.X, digitThreshold),
        (float)Math.Round(vec.Y, digitThreshold),
        (float)Math.Round(vec.Z, digitThreshold));
#endif
}