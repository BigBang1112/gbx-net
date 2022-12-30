namespace GBX.NET;

public readonly record struct Mat3(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ)
{
    public static readonly Mat3 Zero = new();
}
