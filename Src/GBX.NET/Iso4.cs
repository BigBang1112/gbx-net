namespace GBX.NET;

public readonly record struct Iso4(float XX, float XY, float XZ,
                                   float YX, float YY, float YZ,
                                   float ZX, float ZY, float ZZ,
                                   float TX, float TY, float TZ)
{
    public static readonly Iso4 Zero = new();

    public float GetPitch(bool inDegrees = false)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(-ZY, ZZ) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(-ZY, ZZ) * ConversionToDegrees(inDegrees));
#endif
    }

    public float GetYaw(bool inDegrees = false)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(ZX, MathF.Sqrt(ZY * ZY + ZZ * ZZ)) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(ZX, Math.Sqrt(ZY * ZY + ZZ * ZZ)) * ConversionToDegrees(inDegrees));
#endif
    }

    public float GetRoll(bool inDegrees = false)
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Atan2(-YX, XX) * ConversionToDegrees(inDegrees);
#else
        return (float)(Math.Atan2(-YX, XX) * ConversionToDegrees(inDegrees));
#endif
    }

    public Vec3 GetPitchYawRoll(bool inDegrees = false)
    {
        return new Vec3(GetPitch(inDegrees), GetYaw(inDegrees), GetRoll(inDegrees));
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
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

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
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
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XX * XX + YX * YX + ZX * ZX);
#else
        return (float)Math.Sqrt(XX * XX + YX * YX + ZX * ZX);
#endif
    }

    public float GetScaleY()
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XY * XY + YY * YY + ZY * ZY);
#else
        return (float)Math.Sqrt(XY * XY + YY * YY + ZY * ZY);
#endif
    }

    public float GetScaleZ()
    {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return MathF.Sqrt(XZ * XZ + YZ * YZ + ZZ * ZZ);
#else
        return (float)Math.Sqrt(XZ * XZ + YZ * YZ + ZZ * ZZ);
#endif
    }

    public Vec3 GetScale()
    {
        return new Vec3(GetScaleX(), GetScaleY(), GetScaleZ());
    }
}
