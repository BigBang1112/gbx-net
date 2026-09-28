namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03165000</remarks>
[Class(0x03165000)]
public partial class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03165000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03165000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockDirtyLens"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockDirtyLens() { }


    /// <summary>
    /// CGameCtnMediaBlockDirtyLens 0x000 chunk
    /// </summary>
    [Chunk(0x03165000)]
    public partial class Chunk03165000 : Chunk<CGameCtnMediaBlockDirtyLens>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03165000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockDirtyLens n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float intensity;
        public float Intensity { get => intensity; set => intensity = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03165000 => new Chunk03165000(),
        _ => base.NewChunk(chunkId),
    };
}
