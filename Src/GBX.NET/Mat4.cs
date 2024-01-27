namespace GBX.NET;

public readonly record struct Mat4(float XX, float XY, float XZ, float XW,
                                   float YX, float YY, float YZ, float YW,
                                   float ZX, float ZY, float ZZ, float ZW,
                                   float WX, float WY, float WZ, float WW)
{
    public static readonly Mat4 Zero = new();
    public static readonly Mat4 Identity = new(1, 0, 0, 0,
                                                   0, 1, 0, 0,
                                                   0, 0, 1, 0,
                                                   0, 0, 0, 1);
}