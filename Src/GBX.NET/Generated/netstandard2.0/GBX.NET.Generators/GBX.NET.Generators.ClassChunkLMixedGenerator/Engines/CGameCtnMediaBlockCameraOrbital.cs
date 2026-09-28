namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A0000</remarks>
[Class(0x030A0000)]
public partial class CGameCtnMediaBlockCameraOrbital : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A0000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A0000>]
    [AppliedWithChunk<Chunk030A0001>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraOrbital"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraOrbital() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraOrbital 0x000 chunk
    /// </summary>
    [Chunk(0x030A0000)]
    public partial class Chunk030A0000 : Chunk<CGameCtnMediaBlockCameraOrbital>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A0000;


        public override void ReadWrite(CGameCtnMediaBlockCameraOrbital n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: -1);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraOrbital 0x001 chunk
    /// </summary>
    [Chunk(0x030A0001)]
    public partial class Chunk030A0001 : Chunk<CGameCtnMediaBlockCameraOrbital>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A0001;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockCameraOrbital n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private float radius;
        public float Radius { get => radius; set => radius = value; }

        private float longitude;
        public float Longitude { get => longitude; set => longitude = value; }

        private float latitude;
        public float Latitude { get => latitude; set => latitude = value; }

        private Vec3 targetPosition;
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }

        private float fov;
        public float Fov { get => fov; set => fov = value; }

        private float minRenderDistance;
        public float MinRenderDistance { get => minRenderDistance; set => minRenderDistance = value; }

        private float maxRenderDistance;
        public float MaxRenderDistance { get => maxRenderDistance; set => maxRenderDistance = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Byte(ref u01);
            rw.Single(ref radius);
            rw.Single(ref longitude);
            rw.Single(ref latitude);
            rw.Vec3(ref targetPosition);
            rw.Single(ref fov);
            rw.Single(ref minRenderDistance);
            rw.Single(ref maxRenderDistance);
            if (v >= 0)
            {
                rw.Int32(ref u02);
                if (v >= 1)
                {
                    rw.Int32(ref u03);
                    rw.Int32(ref u04);
                    rw.Int32(ref u05);
                    rw.Int32(ref u06);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A0000 => new Chunk030A0000(),
        0x030A0001 => new Chunk030A0001(),
        _ => base.NewChunk(chunkId),
    };
}
