namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030AB000</remarks>
[Class(0x030AB000)]
public partial class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlockTransition, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030AB000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk030AB000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private Vec3 color;
    [AppliedWithChunk<Chunk030AB000>]
    public Vec3 Color { get => color; set => color = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockTransitionFade"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockTransitionFade() { }


    /// <summary>
    /// CGameCtnMediaBlockTransitionFade 0x000 chunk
    /// </summary>
    [Chunk(0x030AB000)]
    public partial class Chunk030AB000 : Chunk<CGameCtnMediaBlockTransitionFade>
    {
        /// <inheritdoc />
        public override uint Id => 0x030AB000;

        public float U01;

        public override void ReadWrite(CGameCtnMediaBlockTransitionFade n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
            rw.Vec3(ref n.color);
            rw.Single(ref U01);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float opacity;
        public float Opacity { get => opacity; set => opacity = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref opacity);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030AB000 => new Chunk030AB000(),
        _ => base.NewChunk(chunkId),
    };
}
