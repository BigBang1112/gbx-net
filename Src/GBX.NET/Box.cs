namespace GBX.NET;

/// <summary>
/// GmBoxAligned.
/// </summary>
public readonly record struct Box(float X, float Y, float Z, float X2, float Y2, float Z2)
{
    public static readonly Box Zero = new();
}
