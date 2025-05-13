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

    /// <summary>
    /// Returns 4x4 matrix representation of this 3x3 matrix.
    /// </summary>
    /// <returns>The <see cref="Matrix4x4"/> from the System.Numerics namespace.</returns>
    public Matrix4x4 ToMatrix4x4()
    {
        return new Matrix4x4(XX, XY, XZ, 0,
                             YX, YY, YZ, 0,
                             ZX, ZY, ZZ, 0,
                             0, 0, 0, 1);
    }

    public static implicit operator Mat3((float XX, float XY, float XZ,
                                          float YX, float YY, float YZ,
                                          float ZX, float ZY, float ZZ) v) => new Mat3(v.XX, v.XY, v.XZ,
                                                                                       v.YX, v.YY, v.YZ,
                                                                                       v.ZX, v.ZY, v.ZZ);

    // not entirely correct but screw it
    public static implicit operator Matrix4x4(Mat3 a) => a.ToMatrix4x4();

    public static explicit operator Mat3(Matrix4x4 a) => new Mat3(a.M11, a.M12, a.M13,
                                                                  a.M21, a.M22, a.M23,
                                                                  a.M31, a.M32, a.M33);

    public static explicit operator Mat3(Mat4 a) => new Mat3(a.XX, a.XY, a.XZ,
                                                             a.YX, a.YY, a.YZ,
                                                             a.ZX, a.ZY, a.ZZ);
}