namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03128000</remarks>
[Class(0x03128000)]
public partial class CGameCtnMediaBlockBloomHdr : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03128000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03128000>]
    [AppliedWithChunk<Chunk03128001>]
    [AppliedWithChunk<Chunk03128002>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockBloomHdr"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockBloomHdr() { }


    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x000 chunk
    /// </summary>
    [Chunk(0x03128000)]
    public partial class Chunk03128000 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        /// <inheritdoc />
        public override uint Id => 0x03128000;


        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x001 chunk
    /// </summary>
    [Chunk(0x03128001)]
    public partial class Chunk03128001 : Chunk03128000
    {
        /// <inheritdoc />
        public override uint Id => 0x03128001;

    }

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x002 chunk
    /// </summary>
    [Chunk(0x03128002)]
    public partial class Chunk03128002 : Chunk03128000
    {
        /// <inheritdoc />
        public override uint Id => 0x03128002;

    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float intensity;
        public float Intensity { get => intensity; set => intensity = value; }

        private float streaksIntensity;
        public float StreaksIntensity { get => streaksIntensity; set => streaksIntensity = value; }

        private float streaksAttenuation;
        public float StreaksAttenuation { get => streaksAttenuation; set => streaksAttenuation = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
            rw.Single(ref streaksIntensity);
            rw.Single(ref streaksAttenuation);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03128000 => new Chunk03128000(),
        0x03128001 => new Chunk03128001(),
        0x03128002 => new Chunk03128002(),
        _ => base.NewChunk(chunkId),
    };
}
