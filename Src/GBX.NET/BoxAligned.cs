namespace GBX.NET;

/// <summary>
/// [GmBoxAligned]
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
/// <param name="X2"></param>
/// <param name="Y2"></param>
/// <param name="Z2"></param>
public readonly record struct BoxAligned(float X, float Y, float Z, float X2, float Y2, float Z2)
{
    public static readonly BoxAligned Zero = new();

    /// <summary>
    /// Implicitly converts a tuple of floats to a <see cref="BoxAligned"/> bounding box.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, Z, X2, Y2, and Z2 components.</param>
    public static implicit operator BoxAligned((float X, float Y, float Z, float X2, float Y2, float Z2) v)
        => new(v.X, v.Y, v.Z, v.X2, v.Y2, v.Z2);
}
