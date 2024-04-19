namespace GBX.NET;

/// <summary>
/// [GmTransQuat]
/// </summary>
public readonly record struct TransQuat(float TX, float TY, float TZ,
                                        float X,  float Y,  float Z, float W)
{
    public static readonly TransQuat Zero = new();

    public TransQuat(Vec3 xyz, Quat quat) : this(xyz.X, xyz.Y, xyz.Z, quat.X, quat.Y, quat.Z, quat.W) { }

    public override string ToString() => $"<{TX}, {TY}, {TZ}, {X}, {Y}, {Z}, {W}>";

    public static implicit operator TransQuat((float TX, float TY, float TZ,
                                               float X,  float Y,  float Z, float W) v) => new TransQuat(v.TX, v.TY, v.TZ,
                                                                                                         v.X,  v.Y,  v.Z, v.W);
}
