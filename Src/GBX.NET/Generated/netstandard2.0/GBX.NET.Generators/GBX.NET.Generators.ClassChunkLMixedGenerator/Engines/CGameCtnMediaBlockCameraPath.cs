namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A1000</remarks>
[Class(0x030A1000)]
public partial class CGameCtnMediaBlockCameraPath : CGameCtnMediaBlockCamera, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A1000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A1000>]
    [AppliedWithChunk<Chunk030A1001>]
    [AppliedWithChunk<Chunk030A1002>]
    [AppliedWithChunk<Chunk030A1003>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraPath"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraPath() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x000 chunk
    /// </summary>
    [Chunk(0x030A1000)]
    public partial class Chunk030A1000 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1000;


        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 0);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x001 chunk
    /// </summary>
    [Chunk(0x030A1001)]
    public partial class Chunk030A1001 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1001;


        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 1);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x002 chunk
    /// </summary>
    [Chunk(0x030A1002)]
    public partial class Chunk030A1002 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1002;


        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 2);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x003 chunk
    /// </summary>
    [Chunk(0x030A1003)]
    public partial class Chunk030A1003 : Chunk<CGameCtnMediaBlockCameraPath>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1003;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        private Vec3 pitchYawRoll;
        /// <summary>
        /// Pitch, yaw and roll in radians.
        /// </summary>
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        private float fov;
        public float Fov { get => fov; set => fov = value; }

        private float nearZ;
        public float NearZ { get => nearZ; set => nearZ = value; }

        private bool anchorRot;
        public bool AnchorRot { get => anchorRot; set => anchorRot = value; }

        private int anchor;
        public int Anchor { get => anchor; set => anchor = value; }

        private bool anchorVis;
        public bool AnchorVis { get => anchorVis; set => anchorVis = value; }

        private int target;
        public int Target { get => target; set => target = value; }

        private Vec3 targetPosition;
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }

        private float weight;
        public float Weight { get => weight; set => weight = value; }

        private Quat u01;
        /// <summary>
        /// some rotation (yaw + pitch + roll + squad)
        /// </summary>
        public Quat U01 { get => u01; set => u01 = value; }

        private int u02;
        /// <summary>
        /// 5 or -1, on v4 related to NSceneEntityIdAllocator::GetClientIdFromSceneUId
        /// </summary>
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        /// <summary>
        /// 1699124 or -1, on v4 related to NSceneEntityIdAllocator::GetClientIdFromSceneUId
        /// </summary>
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Vec3(ref position);
            rw.Vec3(ref pitchYawRoll); // Pitch, yaw and roll in radians.
            rw.Single(ref fov);
            if (v >= 3)
            {
                rw.Single(ref nearZ);
            }
            rw.Boolean(ref anchorRot);
            rw.Int32(ref anchor);
            rw.Boolean(ref anchorVis);
            rw.Int32(ref target);
            rw.Vec3(ref targetPosition);
            rw.Single(ref weight);
            rw.Quat(ref u01); // some rotation (yaw + pitch + roll + squad)
            if (v >= 4)
            {
                rw.Int32(ref u02); // 5 or -1, on v4 related to NSceneEntityIdAllocator::GetClientIdFromSceneUId
                rw.Int32(ref u03); // 1699124 or -1, on v4 related to NSceneEntityIdAllocator::GetClientIdFromSceneUId
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A1000 => new Chunk030A1000(),
        0x030A1001 => new Chunk030A1001(),
        0x030A1002 => new Chunk030A1002(),
        0x030A1003 => new Chunk030A1003(),
        _ => base.NewChunk(chunkId),
    };
}
