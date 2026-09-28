namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030EB000</remarks>
[Class(0x030EB000)]
public partial class CGameCtnMediaBlockSpectators : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030EB000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030EB000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockSpectators"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockSpectators() { }


    /// <summary>
    /// CGameCtnMediaBlockSpectators 0x000 chunk
    /// </summary>
    [Chunk(0x030EB000)]
    public partial class Chunk030EB000 : Chunk<CGameCtnMediaBlockSpectators>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030EB000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockSpectators n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float spectators;
        public float Spectators { get => spectators; set => spectators = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref spectators);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030EB000 => new Chunk030EB000(),
        _ => base.NewChunk(chunkId),
    };
}
