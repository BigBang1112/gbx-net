namespace GBX.NET;

public readonly record struct Mat3(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ)
{
    public static readonly Mat3 Zero = new();
    public static readonly Mat3 Identity = new(1, 0, 0,
                                                   0, 1, 0,
                                                   0, 0, 1);
}