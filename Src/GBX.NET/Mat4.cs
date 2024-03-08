namespace GBX.NET;

public readonly record struct Mat4(float XX, float XY, float XZ, float XW,
                                   float YX, float YY, float YZ, float YW,
                                   float ZX, float ZY, float ZZ, float ZW,
                                   float WX, float WY, float WZ, float WW)
{
    public static readonly Mat4 Zero = new();
    public static readonly Mat4 Identity = new Mat4(1, 0, 0, 0,
                                                    0, 1, 0, 0,
                                                    0, 0, 1, 0,
                                                    0, 0, 0, 1);

    public static implicit operator Mat4((float XX, float XY, float XZ, float XW,
                                          float YX, float YY, float YZ, float YW,
                                          float ZX, float ZY, float ZZ, float ZW,
                                          float WX, float WY, float WZ, float WW) v) => new Mat4(v.XX, v.XY, v.XZ, v.XW,
                                                                                                 v.YX, v.YY, v.YZ, v.YW,
                                                                                                 v.ZX, v.ZY, v.ZZ, v.ZW,
                                                                                                 v.WX, v.WY, v.WZ, v.WW);
}