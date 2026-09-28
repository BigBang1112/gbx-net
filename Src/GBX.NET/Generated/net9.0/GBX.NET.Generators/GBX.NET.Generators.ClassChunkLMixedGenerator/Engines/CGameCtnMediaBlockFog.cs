namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03199000</remarks>
[Class(0x03199000)]
public partial class CGameCtnMediaBlockFog : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03199000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03199000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFog"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFog() { }


    /// <summary>
    /// CGameCtnMediaBlockFog 0x000 chunk
    /// </summary>
    [Chunk(0x03199000)]
    public partial class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03199000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockFog n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float intensity;
        public float Intensity { get => intensity; set => intensity = value; }

        private float skyIntensity;
        public float SkyIntensity { get => skyIntensity; set => skyIntensity = value; }

        private float distance;
        public float Distance { get => distance; set => distance = value; }

        private float? coefficient;
        public float? Coefficient { get => coefficient; set => coefficient = value; }

        private Vec3? color;
        public Vec3? Color { get => color; set => color = value; }

        private float? cloudsOpacity;
        public float? CloudsOpacity { get => cloudsOpacity; set => cloudsOpacity = value; }

        private float? cloudsSpeed;
        public float? CloudsSpeed { get => cloudsSpeed; set => cloudsSpeed = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
            rw.Single(ref skyIntensity);
            rw.Single(ref distance);
            if (v >= 1)
            {
                rw.Single(ref coefficient);
                rw.Vec3(ref color);
                if (v >= 2)
                {
                    rw.Single(ref cloudsOpacity);
                    rw.Single(ref cloudsSpeed);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03199000 => new Chunk03199000(),
        _ => base.NewChunk(chunkId),
    };
}
