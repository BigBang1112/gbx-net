namespace GBX.NET;

/// <summary>
/// GmRectAligned.
/// </summary>
public readonly record struct Rect(float X, float Y, float X2, float Y2)
{
    public static readonly Rect Zero = new();
}
