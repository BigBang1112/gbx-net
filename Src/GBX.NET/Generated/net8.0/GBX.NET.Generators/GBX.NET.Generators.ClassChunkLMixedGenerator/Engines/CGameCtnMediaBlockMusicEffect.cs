namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A6000</remarks>
[Class(0x030A6000)]
public partial class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A6000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A6000>]
    [AppliedWithChunk<Chunk030A6001>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockMusicEffect"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockMusicEffect() { }


    /// <summary>
    /// CGameCtnMediaBlockMusicEffect 0x000 chunk
    /// </summary>
    [Chunk(0x030A6000)]
    public partial class Chunk030A6000 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A6000;


        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockMusicEffect 0x001 chunk
    /// </summary>
    [Chunk(0x030A6001)]
    public partial class Chunk030A6001 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A6001;


        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 1);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float musicVolume;
        public float MusicVolume { get => musicVolume; set => musicVolume = value; }

        private float soundVolume;
        public float SoundVolume { get => soundVolume; set => soundVolume = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref musicVolume);
            if (v >= 1)
            {
                rw.Single(ref soundVolume);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A6000 => new Chunk030A6000(),
        0x030A6001 => new Chunk030A6001(),
        _ => base.NewChunk(chunkId),
    };
}
