namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03083000</remarks>
[Class(0x03083000)]
public partial class CGameCtnMediaBlockFxBloom : CGameCtnMediaBlockFx, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03083000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03083001>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFxBloom"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFxBloom() { }


    /// <summary>
    /// CGameCtnMediaBlockFxBloom 0x001 chunk
    /// </summary>
    [Chunk(0x03083001)]
    public partial class Chunk03083001 : Chunk<CGameCtnMediaBlockFxBloom>
    {
        /// <inheritdoc />
        public override uint Id => 0x03083001;


        public override void ReadWrite(CGameCtnMediaBlockFxBloom n, GbxReaderWriter rw)
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

        private float sensitivity;
        public float Sensitivity { get => sensitivity; set => sensitivity = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
            rw.Single(ref sensitivity);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03083001 => new Chunk03083001(),
        _ => base.NewChunk(chunkId),
    };
}
