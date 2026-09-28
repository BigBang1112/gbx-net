namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Custom camera.
/// </summary>
/// <remarks>ID: 0x030A2000</remarks>
[Class(0x030A2000)]
public partial class CGameCtnMediaBlockCameraCustom : CGameCtnMediaBlockCamera, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A2000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A2001>]
    [AppliedWithChunk<Chunk030A2002>]
    [AppliedWithChunk<Chunk030A2005>]
    [AppliedWithChunk<Chunk030A2006>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraCustom"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraCustom() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x001 chunk
    /// </summary>
    [Chunk(0x030A2001)]
    public partial class Chunk030A2001 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A2001;


        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 1);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x002 chunk
    /// </summary>
    [Chunk(0x030A2002)]
    public partial class Chunk030A2002 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A2002;


        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 2);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x005 chunk (TMUF)
    /// </summary>
    [Chunk(0x030A2005, "TMUF")]
    public partial class Chunk030A2005 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A2005;


        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 5);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x006 chunk (ManiaPlanet)
    /// </summary>
    [Chunk(0x030A2006, "ManiaPlanet")]
    public partial class Chunk030A2006 : Chunk<CGameCtnMediaBlockCameraCustom>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A2006;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: 6 + Version);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private Interpolation interpolation;
        public Interpolation Interpolation { get => interpolation; set => interpolation = value; }

        private int? u01;
        public int? U01 { get => u01; set => u01 = value; }

        private int? u02;
        public int? U02 { get => u02; set => u02 = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        private Vec3 pitchYawRoll;
        /// <summary>
        /// Pitch, yaw and roll in radians.
        /// </summary>
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        private float fov;
        public float Fov { get => fov; set => fov = value; }

        private bool anchorRot;
        public bool AnchorRot { get => anchorRot; set => anchorRot = value; }

        private int anchor;
        /// <summary>
        /// -1, entity number (SGameClipEntityId) or Id if 0x004
        /// </summary>
        public int Anchor { get => anchor; set => anchor = value; }

        private bool anchorVis;
        public bool AnchorVis { get => anchorVis; set => anchorVis = value; }

        private int target;
        /// <summary>
        /// -1, entity number (SGameClipEntityId) or Id if 0x004
        /// </summary>
        public int Target { get => target; set => target = value; }

        private Vec3 targetPosition;
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }

        private float? u03;
        /// <summary>
        /// 60
        /// </summary>
        public float? U03 { get => u03; set => u03 = value; }

        private float? u04;
        /// <summary>
        /// 30
        /// </summary>
        public float? U04 { get => u04; set => u04 = value; }

        private InterpVal? leftTangent;
        public InterpVal? LeftTangent { get => leftTangent; set => leftTangent = value; }

        private InterpVal? rightTangent;
        public InterpVal? RightTangent { get => rightTangent; set => rightTangent = value; }

        private float? u05;
        public float? U05 { get => u05; set => u05 = value; }

        private Quat? u06;
        public Quat? U06 { get => u06; set => u06 = value; }

        private float? nearZ;
        public float? NearZ { get => nearZ; set => nearZ = value; }

        private int? u07;
        public int? U07 { get => u07; set => u07 = value; }

        private int? u08;
        public int? U08 { get => u08; set => u08 = value; }

        private int? u09;
        public int? U09 { get => u09; set => u09 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.EnumInt32<Interpolation>(ref interpolation);
            if (v <= 5)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Vec3(ref position);
                rw.Vec3(ref pitchYawRoll); // Pitch, yaw and roll in radians.
                rw.Single(ref fov);
            }
            rw.Boolean(ref anchorRot);
            rw.Int32(ref anchor); // -1, entity number (SGameClipEntityId) or Id if 0x004
            rw.Boolean(ref anchorVis);
            rw.Int32(ref target); // -1, entity number (SGameClipEntityId) or Id if 0x004
            if (v <= 5)
            {
                rw.Vec3(ref targetPosition);
                if (v == 1)
                {
                    rw.Single(ref u03); // 60
                    rw.Single(ref u04); // 30
                    return;
                }
                rw.ReadableWritable<InterpVal>(ref leftTangent, version: v);
                rw.ReadableWritable<InterpVal>(ref rightTangent, version: v);
                if (v == 3)
                {
                    rw.Single(ref u05);
                    rw.Quat(ref u06);
                }
            }
            if (v >= 6)
            {
                rw.Vec3(ref position);
                rw.Vec3(ref pitchYawRoll);
                rw.Single(ref fov);
                rw.Vec3(ref targetPosition);
                if (v >= 7)
                {
                    rw.Single(ref nearZ);
                }
                rw.ReadableWritable<InterpVal>(ref leftTangent, version: v);
                rw.ReadableWritable<InterpVal>(ref rightTangent, version: v);
                if (v == 8)
                {
                    rw.Int32(ref u07);
                    rw.Int32(ref u08);
                }
                if (v >= 10)
                {
                    rw.Int32(ref u09);
                }
            }
        }
    }

    public sealed partial class InterpVal : IReadableWritable
    {

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        private Vec3? pitchYawRoll;
        public Vec3? PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        private int? u01;
        public int? U01 { get => u01; set => u01 = value; }

        private float? fov;
        public float? Fov { get => fov; set => fov = value; }

        private Vec3? targetPosition;
        public Vec3? TargetPosition { get => targetPosition; set => targetPosition = value; }

        private float? nearZ;
        public float? NearZ { get => nearZ; set => nearZ = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Vec3(ref position);
            if (v <= 5)
            {
                return;
            }
            rw.Vec3(ref pitchYawRoll);
            if (v >= 10)
            {
                rw.Int32(ref u01);
            }
            rw.Single(ref fov);
            rw.Vec3(ref targetPosition);
            if (v >= 7)
            {
                rw.Single(ref nearZ);
            }
        }
    }


    public enum Interpolation
    {
        None,
        Hermite,
        Linear,
        FixedTangent,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A2001 => new Chunk030A2001(),
        0x030A2002 => new Chunk030A2002(),
        0x030A2005 => new Chunk030A2005(),
        0x030A2006 => new Chunk030A2006(),
        _ => base.NewChunk(chunkId),
    };
}
