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
}