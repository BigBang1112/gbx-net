namespace GBX.NET.Engines.Plug;

public partial class CPlugAnimSkelEdition
{
    private int legacyData;
    private CPlugAnimClip.ClipFlags? flags;
    private float duration;
    private bool looping;
    private float[]? times;
    private int jointTrackVersion = 1;
    private Layer[] layers = [];
    private JointTrack[]? extraJointTracks;
    private Curve[]? rootMotion;
    private int legacyRootMotionType;
    private Vec3 legacyRootMotion;
    private Curve[]? firstPersonCamera;
    private Curve[]? firstPersonCameraExtra;
    private Curve[]? physicForce;
    private Curve[]? floatChannelCurves;
    private string[]? floatChannelIds;
    private uint editionFlags;

    public CPlugAnimClip.ClipFlags? Flags { get => flags; set => flags = value; }
    public float Duration { get => duration; set => duration = value; }
    public bool Looping { get => looping; set => looping = value; }
    public float[]? Times { get => times; set => times = value; }
    public int JointTrackVersion { get => jointTrackVersion; set => jointTrackVersion = value; }
    public Layer[] Layers { get => layers; set => layers = value; }
    public JointTrack[]? ExtraJointTracks { get => extraJointTracks; set => extraJointTracks = value; }
    public Curve[]? RootMotion { get => rootMotion; set => rootMotion = value; }
    public Curve[]? FirstPersonCamera { get => firstPersonCamera; set => firstPersonCamera = value; }
    public Curve[]? FirstPersonCameraExtra { get => firstPersonCameraExtra; set => firstPersonCameraExtra = value; }
    public Curve[]? PhysicForce { get => physicForce; set => physicForce = value; }
    public Curve[]? FloatChannelCurves { get => floatChannelCurves; set => floatChannelCurves = value; }
    public string[]? FloatChannelIds { get => floatChannelIds; set => floatChannelIds = value; }
    public uint EditionFlags { get => editionFlags; set => editionFlags = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (v < 0 || v > 11)
        {
            throw new ChunkVersionNotSupportedException(v);
        }
        if (v < 2)
        {
            rw.Int32(ref legacyData);
        }
        if (v >= 3)
        {
            rw.ReadableWritable(ref flags, FlagsVersion);
        }
        rw.Single(ref duration);
        rw.Boolean(ref looping);
        rw.Array(ref times);
        rw.Int32(ref jointTrackVersion);

        var layerCount = v >= 11 ? rw.Int32(layers.Length) : 1;
        if (layerCount < 0 || layerCount > 64)
        {
            throw new InvalidDataException("Invalid animation layer count.");
        }
        if (rw.Reader is not null)
        {
            layers = new Layer[layerCount];
        }
        else if (v < 11 && layers.Length != 1)
        {
            throw new InvalidDataException("Legacy animations require one layer.");
        }
        for (var i = 0; i < layerCount; i++)
        {
            rw.ReadableWritable(ref layers[i]!);
        }

        if (v >= 4)
        {
            rw.ArrayReadableWritable(ref extraJointTracks);
        }
        if (v >= 5)
        {
            rw.ArrayReadableWritable(ref rootMotion, 4);
        }
        else if (v >= 1)
        {
            rw.Int32(ref legacyRootMotionType);
            rw.Vec3(ref legacyRootMotion);
        }
        if (v >= 7)
        {
            rw.ArrayReadableWritable(ref firstPersonCamera, 4);
            rw.ArrayReadableWritable(ref firstPersonCameraExtra, 2);
        }
        if (v >= 8)
        {
            rw.ArrayReadableWritable(ref physicForce, 4);
        }
        if (v >= 10)
        {
            rw.ArrayReadableWritable(ref floatChannelCurves);
            rw.ArrayId(ref floatChannelIds);
        }
        if (v >= 6)
        {
            foreach (var layer in layers)
            {
                rw.UInt32(ref layer.Flags);
            }
            rw.UInt32(ref editionFlags);
        }
    }

    public partial class Layer
    {
        public uint Flags;
    }

    public partial class Chunk09134000
    {
        public override void ReadWrite(CPlugAnimSkelEdition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.flagsVersion);
            rw.NodeRef(ref n.skel);
            n.ReadWrite(rw, Version);
        }
    }
}
