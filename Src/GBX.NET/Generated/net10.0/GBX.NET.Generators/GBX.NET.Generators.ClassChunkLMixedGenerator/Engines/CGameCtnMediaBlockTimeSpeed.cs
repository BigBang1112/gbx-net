namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03129000</remarks>
[Class(0x03129000)]
public partial class CGameCtnMediaBlockTimeSpeed : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03129000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03085000>]
    [AppliedWithChunk<Chunk03129000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockTimeSpeed"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockTimeSpeed() { }


    /// <summary>
    /// CGameCtnMediaBlockTimeSpeed 0x000 chunk
    /// </summary>
    [Chunk(0x03085000)]
    public partial class Chunk03085000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        /// <inheritdoc />
        public override uint Id => 0x03085000;


        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockTimeSpeed 0x000 chunk
    /// </summary>
    [Chunk(0x03129000)]
    public partial class Chunk03129000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        /// <inheritdoc />
        public override uint Id => 0x03129000;


        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float speed;
        public float Speed { get => speed; set => speed = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref speed);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03085000 => new Chunk03085000(),
        0x03129000 => new Chunk03129000(),
        _ => base.NewChunk(chunkId),
    };
}
