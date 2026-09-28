namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0316C000</remarks>
[Class(0x0316C000)]
public partial class CGameCtnMediaBlockColoringCapturable : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x0316C000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk0316C000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private int capturableIndex;
    [AppliedWithChunk<Chunk0316C000>]
    public int CapturableIndex { get => capturableIndex; set => capturableIndex = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockColoringCapturable"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockColoringCapturable() { }


    /// <summary>
    /// CGameCtnMediaBlockColoringCapturable 0x000 chunk
    /// </summary>
    [Chunk(0x0316C000)]
    public partial class Chunk0316C000 : Chunk<CGameCtnMediaBlockColoringCapturable>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0316C000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnMediaBlockColoringCapturable n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ListReadableWritable<Key>(ref n.keys!);
            if (Version >= 1)
            {
                rw.Int32(ref n.capturableIndex);
            }
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float hue;
        public float Hue { get => hue; set => hue = value; }

        private float gauge;
        public float Gauge { get => gauge; set => gauge = value; }

        private int emblem;
        public int Emblem { get => emblem; set => emblem = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref hue);
            rw.Single(ref gauge);
            rw.Int32(ref emblem);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0316C000 => new Chunk0316C000(),
        _ => base.NewChunk(chunkId),
    };
}
