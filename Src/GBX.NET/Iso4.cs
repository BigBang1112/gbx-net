namespace GBX.NET;

public readonly record struct Iso4(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ,
                                   float TX, float TY, float TZ)
{
    public static readonly Iso4 Zero = new();
    public static readonly Iso4 Identity = new Iso4(1, 0, 0,
                                                    0, 1, 0,
                                                    0, 0, 1,
                                                    0, 0, 0);

    public Mat3 Rotation => new Mat3(XX, XY, XZ,
                                     YX, YY, YZ,
                                     ZX, ZY, ZZ);

    public Vec3 Translation => new(TX, TY, TZ);

    public float GetPitch(bool inDegrees = false)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(-ZY, ZZ) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(-ZY, ZZ) * ConversionToDegrees(inDegrees));
#endif
    }

    public float GetYaw(bool inDegrees = false)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(ZX, MathF.Sqrt(ZY * ZY + ZZ * ZZ)) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(ZX, Math.Sqrt(ZY * ZY + ZZ * ZZ)) * ConversionToDegrees(inDegrees));
#endif
    }

    public float GetRoll(bool inDegrees = false)
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(-YX, XX) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(-YX, XX) * ConversionToDegrees(inDegrees));
#endif
    }

    public Vec3 GetPitchYawRoll(bool inDegrees = false)
    {
        return new Vec3(GetPitch(inDegrees), GetYaw(inDegrees), GetRoll(inDegrees));
    }

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static float ConversionToDegrees(bool inDegrees)
    {
        return inDegrees ? 180f / MathF.PI : 1;
    }
#else
    private static double ConversionToDegrees(bool inDegrees)
    {
        return inDegrees ? 180 / Math.PI : 1;
    }
#endif

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static float ConversionToRadians(bool inDegrees)
    {
        return inDegrees ? MathF.PI / 180f : 1;
    }
#else
    private static double ConversionToRadians(bool inDegrees)
    {
        return inDegrees ? Math.PI / 180f : 1;
    }
#endif

    public float GetScaleX()
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XX * XX + YX * YX + ZX * ZX);
#else
        return (float)Math.Sqrt(XX * XX + YX * YX + ZX * ZX);
#endif
    }

    public float GetScaleY()
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XY * XY + YY * YY + ZY * ZY);
#else
        return (float)Math.Sqrt(XY * XY + YY * YY + ZY * ZY);
#endif
    }

    public float GetScaleZ()
    {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XZ * XZ + YZ * YZ + ZZ * ZZ);
#else
        return (float)Math.Sqrt(XZ * XZ + YZ * YZ + ZZ * ZZ);
#endif
    }

    public Vec3 GetScale()
    {
        return new Vec3(GetScaleX(), GetScaleY(), GetScaleZ());
    }

    public Vec3 GetPosition()
    {
        return new Vec3(TX, TY, TZ);
    }

    public Iso4 ScaleX(float value)
    {
        return new Iso4(XX * value, XY, XZ,
                        YX * value, YY, YZ,
                        ZX * value, ZY, ZZ,
                        TX, TY, TZ);
    }

    public Iso4 ScaleY(float value)
    {
        return new Iso4(XX, XY * value, XZ,
                        YX, YY * value, YZ,
                        ZX, ZY * value, ZZ,
                        TX, TY, TZ);
    }

    public Iso4 ScaleZ(float value)
    {
        return new Iso4(XX, XY, XZ * value,
                        YX, YY, YZ * value,
                        ZX, ZY, ZZ * value,
                        TX, TY, TZ);
    }

    public Iso4 Scale(float value)
    {
        return new Iso4(XX * value, XY * value, XZ * value,
                        YX * value, YY * value, YZ * value,
                        ZX * value, ZY * value, ZZ * value,
                        TX, TY, TZ);
    }

    public Iso4 Translate(Vec3 translation)
    {
        return new Iso4(XX, XY, XZ,
                        YX, YY, YZ,
                        ZX, ZY, ZZ,
                        TX + translation.X, TY + translation.Y, TZ + translation.Z);
    }

    public Iso4 Translate(float x, float y, float z)
    {
        return new Iso4(XX, XY, XZ,
                        YX, YY, YZ,
                        ZX, ZY, ZZ,
                        TX + x, TY + y, TZ + z);
    }

    public Iso4 SetScaleX(float value)
    {
        var normalizedColumnX = new Vec3(XX, YX, ZX).GetNormalized();

        if (normalizedColumnX == Vec3.Zero)
        {
            normalizedColumnX = new(1, 0, 0);
        }

        return new Iso4(normalizedColumnX.X, XY, XZ,
                        normalizedColumnX.Y, YY, YZ,
                        normalizedColumnX.Z, ZY, ZZ,
                        TX, TY, TZ).ScaleX(value);
    }

    public Iso4 SetScaleY(float value)
    {
        var normalizedColumnY = new Vec3(XY, YY, ZY).GetNormalized();

        if (normalizedColumnY == Vec3.Zero)
        {
            normalizedColumnY = new(0, 1, 0);
        }

        return new Iso4(XX, normalizedColumnY.X, XZ,
                        YX, normalizedColumnY.Y, YZ,
                        ZX, normalizedColumnY.Z, ZZ,
                        TX, TY, TZ).ScaleY(value);
    }

    public Iso4 SetScaleZ(float value)
    {
        var normalizedColumnZ = new Vec3(XZ, YZ, ZZ).GetNormalized();

        if (normalizedColumnZ == Vec3.Zero)
        {
            normalizedColumnZ = new(0, 0, 1);
        }

        return new Iso4(XX, XY, normalizedColumnZ.X,
                        YX, YY, normalizedColumnZ.Y,
                        ZX, ZY, normalizedColumnZ.Z,
                        TX, TY, TZ).ScaleZ(value);
    }

    public Iso4 SetScale(float value)
    {
        return ScaleX(value).ScaleY(value).ScaleZ(value);
    }

    public void Deconstruct(out Mat3 rot, out Vec3 xyz)
    {
        rot = new Mat3(XX, XY, XZ,
                       YX, YY, YZ,
                       ZX, ZY, ZZ);
        xyz = new Vec3(TX, TY, TZ);
    }

    public static implicit operator Iso4((float XX, float XY, float XZ,
                                          float YX, float YY, float YZ,
                                          float ZX, float ZY, float ZZ,
                                          float TX, float TY, float TZ) v) => new Iso4(v.XX, v.XY, v.XZ,
                                                                                       v.YX, v.YY, v.YZ,
                                                                                       v.ZX, v.ZY, v.ZZ,
                                                                                       v.TX, v.TY, v.TZ);
}