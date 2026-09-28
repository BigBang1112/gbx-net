namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03085000</remarks>
[Class(0x03085000)]
public partial class CGameCtnMediaBlockTime : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03085000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03085000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockTime"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockTime() { }


    /// <summary>
    /// CGameCtnMediaBlockTime 0x000 chunk
    /// </summary>
    [Chunk(0x03085000)]
    public partial class Chunk03085000 : Chunk<CGameCtnMediaBlockTime>
    {
        /// <inheritdoc />
        public override uint Id => 0x03085000;


        public override void ReadWrite(CGameCtnMediaBlockTime n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float timeValue;
        public float TimeValue { get => timeValue; set => timeValue = value; }

        private float tangent;
        public float Tangent { get => tangent; set => tangent = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref timeValue);
            rw.Single(ref tangent);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03085000 => new Chunk03085000(),
        _ => base.NewChunk(chunkId),
    };
}
