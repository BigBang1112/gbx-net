namespace GBX.NET;

public readonly record struct TransYawPitchRollDeg(Vec3 Translation, float Yaw, float Pitch, float Roll)
{
    public static readonly TransYawPitchRollDeg Zero = default;

    public TransYawPitchRollDeg(Vec3 translation, Vec3 yawPitchRoll)
        : this(translation, yawPitchRoll.X, yawPitchRoll.Y, yawPitchRoll.Z) { }
    public Vec3 YawPitchRoll => new(Yaw, Pitch, Roll);

    public void Deconstruct(out Vec3 translation, out Vec3 yawPitchRoll)
    {
        translation = Translation;
        yawPitchRoll = YawPitchRoll;
    }

    public static implicit operator TransYawPitchRollDeg((Vec3 Translation, Vec3 YawPitchRoll) v)
        => new(v.Translation, v.YawPitchRoll);

    public override string ToString() => $"<{Translation}, {Yaw}, {Pitch}, {Roll}>";
}