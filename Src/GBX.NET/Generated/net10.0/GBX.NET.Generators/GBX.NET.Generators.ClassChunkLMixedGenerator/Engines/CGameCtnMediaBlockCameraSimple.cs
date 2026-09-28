namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0309F000</remarks>
[Class(0x0309F000)]
public partial class CGameCtnMediaBlockCameraSimple : CGameCtnMediaBlockCamera, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x0309F000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A1000>]
    [AppliedWithChunk<Chunk030A1002>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraSimple"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraSimple() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraSimple 0x000 chunk
    /// </summary>
    [Chunk(0x030A1000)]
    public partial class Chunk030A1000 : Chunk<CGameCtnMediaBlockCameraSimple>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1000;


        public override void ReadWrite(CGameCtnMediaBlockCameraSimple n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraSimple 0x001 chunk
    /// </summary>
    [Chunk(0x030A1001)]
    public partial class Chunk030A1001 : Chunk<CGameCtnMediaBlockCameraSimple>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1001;

        public int U01;

        public override void ReadWrite(CGameCtnMediaBlockCameraSimple n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockCameraSimple 0x002 chunk
    /// </summary>
    [Chunk(0x030A1002)]
    public partial class Chunk030A1002 : Chunk<CGameCtnMediaBlockCameraSimple>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A1002;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public bool U03;

        public override void ReadWrite(CGameCtnMediaBlockCameraSimple n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            if (Version >= 2)
            {
                rw.Boolean(ref U03);
            }
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Byte(ref u01);
            rw.Iso4(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A1000 => new Chunk030A1000(),
        0x030A1001 => new Chunk030A1001(),
        0x030A1002 => new Chunk030A1002(),
        _ => base.NewChunk(chunkId),
    };
}
