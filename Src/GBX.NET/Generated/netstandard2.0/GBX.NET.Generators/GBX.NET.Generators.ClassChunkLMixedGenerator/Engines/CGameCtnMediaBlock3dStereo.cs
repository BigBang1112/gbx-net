namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03024000</remarks>
[Class(0x03024000)]
public partial class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03024000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03024000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlock3dStereo"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlock3dStereo() { }


    /// <summary>
    /// CGameCtnMediaBlock3dStereo 0x000 chunk
    /// </summary>
    [Chunk(0x03024000)]
    public partial class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
    {
        /// <inheritdoc />
        public override uint Id => 0x03024000;


        public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float upToMax;
        public float UpToMax { get => upToMax; set => upToMax = value; }

        private float screenDist;
        public float ScreenDist { get => screenDist; set => screenDist = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref upToMax);
            rw.Single(ref screenDist);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03024000 => new Chunk03024000(),
        _ => base.NewChunk(chunkId),
    };
}
