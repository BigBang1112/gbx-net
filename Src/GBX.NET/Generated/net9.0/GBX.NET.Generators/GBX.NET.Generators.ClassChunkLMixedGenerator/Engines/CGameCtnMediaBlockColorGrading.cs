namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03186000</remarks>
[Class(0x03186000)]
public partial class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03186000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private PackDesc? image;
    [AppliedWithChunk<Chunk03186000>]
    public PackDesc? Image { get => image; set => image = value; }

    private List<Key>? keys;
    [AppliedWithChunk<Chunk03186001>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockColorGrading"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockColorGrading() { }


    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x000 chunk
    /// </summary>
    [Chunk(0x03186000)]
    public partial class Chunk03186000 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        /// <inheritdoc />
        public override uint Id => 0x03186000;


        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GbxReaderWriter rw)
        {
            rw.PackDesc(ref n.image);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x001 chunk
    /// </summary>
    [Chunk(0x03186001)]
    public partial class Chunk03186001 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        /// <inheritdoc />
        public override uint Id => 0x03186001;


        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GbxReaderWriter rw)
        {
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
        0x03186000 => new Chunk03186000(),
        0x03186001 => new Chunk03186001(),
        _ => base.NewChunk(chunkId),
    };
}
