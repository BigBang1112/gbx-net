namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0316D000</remarks>
[Class(0x0316D000)]
public partial class CGameCtnMediaBlockFxCameraBlend : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x0316D000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk0316D000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFxCameraBlend"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFxCameraBlend() { }


    /// <summary>
    /// CGameCtnMediaBlockFxCameraBlend 0x000 chunk
    /// </summary>
    [Chunk(0x0316D000)]
    public partial class Chunk0316D000 : Chunk<CGameCtnMediaBlockFxCameraBlend>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0316D000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockFxCameraBlend n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float captureWeight;
        public float CaptureWeight { get => captureWeight; set => captureWeight = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref captureWeight);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0316D000 => new Chunk0316D000(),
        _ => base.NewChunk(chunkId),
    };
}
