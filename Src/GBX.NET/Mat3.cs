namespace GBX.NET;

public readonly record struct Mat3(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ)
{
    public static readonly Mat3 Zero = new();
    public static readonly Mat3 Identity = new Mat3(1, 0, 0,
                                                    0, 1, 0,
                                                    0, 0, 1);

    public static implicit operator Mat3((float XX, float XY, float XZ,
                                          float YX, float YY, float YZ,
                                          float ZX, float ZY, float ZZ) v) => new Mat3(v.XX, v.XY, v.XZ,
                                                                                       v.YX, v.YY, v.YZ,
                                                                                       v.ZX, v.ZY, v.ZZ);
}