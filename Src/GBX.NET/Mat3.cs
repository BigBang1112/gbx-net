using System.Numerics;

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

    public static implicit operator Matrix4x4(Mat3 a) => new Matrix4x4(a.XX, a.XY, a.XZ, 0,
                                                                       a.YX, a.YY, a.YZ, 0,
                                                                       a.ZX, a.ZY, a.ZZ, 0,
                                                                       0, 0, 0, 1);

    public static explicit operator Mat3(Matrix4x4 a) => new Mat3(a.M11, a.M12, a.M13,
                                                                  a.M21, a.M22, a.M23,
                                                                  a.M31, a.M32, a.M33);

    public static explicit operator Mat3(Mat4 a) => new Mat3(a.XX, a.XY, a.XZ,
                                                             a.YX, a.YY, a.YZ,
                                                             a.ZX, a.ZY, a.ZZ);
}