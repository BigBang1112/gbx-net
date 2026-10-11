namespace GBX.NET.Engines.Plug;

public partial class CPlugSkel
{
    [Obsolete("Use SkelSetup instead.")]
    public bool U01
    {
        get => SkelSetup is not null;
        set => SkelSetup = value ? SkelSetup ?? new Setup() : null;
    }

    [Obsolete("Use AdditionalJoints instead.")]
    public bool U02
    {
        get => AdditionalJoints is not null;
        set => AdditionalJoints = value ? AdditionalJoints ?? new ExtraJoints() : null;
    }

    [Obsolete("Use AnimLodCount instead.")]
    public byte? U05
    {
        get => AnimLodCount;
        set => AnimLodCount = value ?? 1;
    }

    [Obsolete("Use AnimLodDistances instead.")]
    public int? U06
    {
        get => AnimLodDistances?.Length;
        set => AnimLodDistances = value is int count ? new float[count] : null;
    }

    public partial class Joint
    {
        [Obsolete("Use LocalRotation instead.")]
        public Quat GlobalJoint { get => LocalRotation; set => LocalRotation = value; }

        [Obsolete("Use LocalTranslation instead.")]
        public Vec3 U01 { get => LocalTranslation; set => LocalTranslation = value; }

        [Obsolete("Use GlobalTransform instead.")]
        public Iso4 U02 { get => GlobalTransform; set => GlobalTransform = value; }
    }

    public partial class Socket
    {
        [Obsolete("Use JointIndex instead.")]
        public short U01 { get => JointIndex; set => JointIndex = value; }

        [Obsolete("Use LocalTransform instead.")]
        public Iso4 U02 { get => LocalTransform; set => LocalTransform = value; }
    }

    public partial class JointInfo
    {
        [Obsolete("Use Flags instead.")]
        public byte[]? U04
        {
            get => BitConverter.GetBytes(Flags);
            set => Flags = value is null ? 0 : BitConverter.ToUInt32(value, 0);
        }
    }

    public partial class Chunk090BA000
    {
        public override void ReadWrite(CPlugSkel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.ReadWrite(rw, Version);
        }
    }
}
