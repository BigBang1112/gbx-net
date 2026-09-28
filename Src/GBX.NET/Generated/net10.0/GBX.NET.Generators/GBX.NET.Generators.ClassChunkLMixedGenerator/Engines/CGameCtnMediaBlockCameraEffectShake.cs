namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A4000</remarks>
[Class(0x030A4000)]
public partial class CGameCtnMediaBlockCameraEffectShake : CGameCtnMediaBlockCameraEffect, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A4000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A4000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraEffectShake"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraEffectShake() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraEffectShake 0x000 chunk
    /// </summary>
    [Chunk(0x030A4000)]
    public partial class Chunk030A4000 : Chunk<CGameCtnMediaBlockCameraEffectShake>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A4000;


        public override void ReadWrite(CGameCtnMediaBlockCameraEffectShake n, GbxReaderWriter rw)
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

        private float speed;
        public float Speed { get => speed; set => speed = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
            rw.Single(ref speed);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A4000 => new Chunk030A4000(),
        _ => base.NewChunk(chunkId),
    };
}
