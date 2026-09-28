namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03127000</remarks>
[Class(0x03127000)]
public partial class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03127000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03127000>]
    [AppliedWithChunk<Chunk03127001>]
    [AppliedWithChunk<Chunk03127002>]
    [AppliedWithChunk<Chunk03127003>]
    [AppliedWithChunk<Chunk03127004>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockToneMapping"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockToneMapping() { }


    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x000 chunk
    /// </summary>
    [Chunk(0x03127000)]
    public partial class Chunk03127000 : Chunk<CGameCtnMediaBlockToneMapping>
    {
        /// <inheritdoc />
        public override uint Id => 0x03127000;


        public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x001 chunk
    /// </summary>
    [Chunk(0x03127001)]
    public partial class Chunk03127001 : Chunk03127000
    {
        /// <inheritdoc />
        public override uint Id => 0x03127001;

    }

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x002 chunk
    /// </summary>
    [Chunk(0x03127002)]
    public partial class Chunk03127002 : Chunk03127000
    {
        /// <inheritdoc />
        public override uint Id => 0x03127002;

    }

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x003 chunk
    /// </summary>
    [Chunk(0x03127003)]
    public partial class Chunk03127003 : Chunk03127000
    {
        /// <inheritdoc />
        public override uint Id => 0x03127003;

    }

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x004 chunk
    /// </summary>
    [Chunk(0x03127004)]
    public partial class Chunk03127004 : Chunk03127000
    {
        /// <inheritdoc />
        public override uint Id => 0x03127004;

    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float exposure;
        public float Exposure { get => exposure; set => exposure = value; }

        private float maxHDR;
        public float MaxHDR { get => maxHDR; set => maxHDR = value; }

        private float lightTrailScale;
        public float LightTrailScale { get => lightTrailScale; set => lightTrailScale = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref exposure);
            rw.Single(ref maxHDR);
            rw.Single(ref lightTrailScale);
            rw.Int32(ref u01);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03127000 => new Chunk03127000(),
        0x03127001 => new Chunk03127001(),
        0x03127002 => new Chunk03127002(),
        0x03127003 => new Chunk03127003(),
        0x03127004 => new Chunk03127004(),
        _ => base.NewChunk(chunkId),
    };
}
