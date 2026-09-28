namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03188000</remarks>
[Class(0x03188000)]
public partial class CGameCtnMediaBlockScenery : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03188000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03188000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private CPlugDataTape? dataTape;
    [AppliedWithChunk<Chunk03188001>]
    public CPlugDataTape? DataTape { get => dataTape; set => dataTape = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockScenery"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockScenery() { }


    /// <summary>
    /// CGameCtnMediaBlockScenery 0x000 chunk
    /// </summary>
    [Chunk(0x03188000)]
    public partial class Chunk03188000 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03188000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockScenery n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockScenery 0x001 chunk
    /// </summary>
    [Chunk(0x03188001)]
    public partial class Chunk03188001 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03188001;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
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

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03188000 => new Chunk03188000(),
        0x03188001 => new Chunk03188001(),
        _ => base.NewChunk(chunkId),
    };
}
