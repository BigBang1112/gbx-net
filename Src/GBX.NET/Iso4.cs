namespace GBX.NET;

public readonly record struct Iso4(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ,
                                   float TX, float TY, float TZ)
{
    public static readonly Iso4 Zero = new();
}
