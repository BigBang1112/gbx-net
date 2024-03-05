namespace GBX.NET;

/// <summary>
/// [GmBoxNat3]
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
/// <param name="X2"></param>
/// <param name="Y2"></param>
/// <param name="Z2"></param>
public readonly record struct BoxInt3(int X, int Y, int Z, int X2, int Y2, int Z2)
{
    public static readonly BoxInt3 Zero = new();
}