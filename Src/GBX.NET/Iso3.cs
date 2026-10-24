namespace GBX.NET;

/// <summary>
/// A two-dimensional affine transform stored as six floats.
/// </summary>
public readonly record struct Iso3(float XX, float XY,
                                   float YX, float YY,
                                   float TX, float TY)
{
    public static readonly Iso3 Zero = new();
    public static readonly Iso3 Identity = new(1, 0,
                                               0, 1,
                                               0, 0);

    public Vec2 Translation => new(TX, TY);

    public static implicit operator Iso3((float XX, float XY,
                                          float YX, float YY,
                                          float TX, float TY) v) => new(v.XX, v.XY,
                                                                       v.YX, v.YY,
                                                                       v.TX, v.TY);
}
