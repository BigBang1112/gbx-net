using System.Numerics;

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

    public Matrix4x4 AsMatrix4x4()
    {
        return new Matrix4x4(XX, XY, XZ, XW,
                             YX, YY, YZ, YW,
                             ZX, ZY, ZZ, ZW,
                             WX, WY, WZ, WW);
    }

    public static implicit operator Mat4((float XX, float XY, float XZ, float XW,
                                          float YX, float YY, float YZ, float YW,
                                          float ZX, float ZY, float ZZ, float ZW,
                                          float WX, float WY, float WZ, float WW) v) => new Mat4(v.XX, v.XY, v.XZ, v.XW,
                                                                                                 v.YX, v.YY, v.YZ, v.YW,
                                                                                                 v.ZX, v.ZY, v.ZZ, v.ZW,
                                                                                                 v.WX, v.WY, v.WZ, v.WW);

    public static implicit operator Matrix4x4(Mat4 a) => a.AsMatrix4x4();

    public static implicit operator Mat4(Matrix4x4 m) => new Mat4(m.M11, m.M12, m.M13, m.M14,
                                                                  m.M21, m.M22, m.M23, m.M24,
                                                                  m.M31, m.M32, m.M33, m.M34,
                                                                  m.M41, m.M42, m.M43, m.M44);
}