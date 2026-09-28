namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0314A000</remarks>
[Class(0x0314A000)]
public partial class CGameCtnMediaBlockSkel : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x0314A000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk0314A000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockSkel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockSkel() { }


    /// <summary>
    /// CGameCtnMediaBlockSkel 0x000 chunk
    /// </summary>
    [Chunk(0x0314A000)]
    public partial class Chunk0314A000 : Chunk<CGameCtnMediaBlockSkel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0314A000;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public float U03 = 100;
        public float U04 = 1;
        public float U05 = 100;
        public float U06 = 1;
        public float U07;

        public override void ReadWrite(CGameCtnMediaBlockSkel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!);
            if (Version >= 1)
            {
                rw.Int32(ref U01);
                if (Version == 4)
                {
                    rw.Int32(ref U02);
                }
                if (Version >= 3)
                {
                    rw.Single(ref U03);
                    rw.Single(ref U04);
                    rw.Single(ref U05);
                    rw.Single(ref U06);
                    if (Version >= 4)
                    {
                        rw.Single(ref U07);
                    }
                }
            }
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private TransQuat[]? u05;
        public TransQuat[]? U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            if (v >= 2)
            {
                rw.Single(ref u01);
                rw.Single(ref u02);
                rw.Single(ref u03);
                rw.Single(ref u04);
            }
            rw.Array<TransQuat>(ref u05!);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0314A000 => new Chunk0314A000(),
        _ => base.NewChunk(chunkId),
    };
}
