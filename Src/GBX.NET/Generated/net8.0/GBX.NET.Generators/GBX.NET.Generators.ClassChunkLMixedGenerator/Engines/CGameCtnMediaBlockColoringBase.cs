namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03172000</remarks>
[Class(0x03172000)]
public partial class CGameCtnMediaBlockColoringBase : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03172000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03172000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private int baseIndex;
    [AppliedWithChunk<Chunk03172000>]
    public int BaseIndex { get => baseIndex; set => baseIndex = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockColoringBase"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockColoringBase() { }


    /// <summary>
    /// CGameCtnMediaBlockColoringBase 0x000 chunk
    /// </summary>
    [Chunk(0x03172000)]
    public partial class Chunk03172000 : Chunk<CGameCtnMediaBlockColoringBase>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03172000;

        public int Version { get; set; }

        public int U01 = 2;

        public override void ReadWrite(CGameCtnMediaBlockColoringBase n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 2)
            {
                rw.Int32(ref U01);
            }
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
            if (Version >= 1)
            {
                rw.Int32(ref n.baseIndex);
            }
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float hue;
        public float Hue { get => hue; set => hue = value; }

        private float intensity;
        public float Intensity { get => intensity; set => intensity = value; }

        private short u01;
        public short U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref hue);
            if (v >= 1)
            {
                rw.Single(ref intensity);
                if (v >= 2)
                {
                    rw.Int16(ref u01);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03172000 => new Chunk03172000(),
        _ => base.NewChunk(chunkId),
    };
}
