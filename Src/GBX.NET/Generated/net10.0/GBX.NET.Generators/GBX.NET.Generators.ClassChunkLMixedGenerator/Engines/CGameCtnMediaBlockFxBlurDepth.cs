namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03081000</remarks>
[Class(0x03081000)]
public partial class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03081000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03081001>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFxBlurDepth"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFxBlurDepth() { }


    /// <summary>
    /// CGameCtnMediaBlockFxBlurDepth 0x001 chunk
    /// </summary>
    [Chunk(0x03081001)]
    public partial class Chunk03081001 : Chunk<CGameCtnMediaBlockFxBlurDepth>
    {
        /// <inheritdoc />
        public override uint Id => 0x03081001;


        public override void ReadWrite(CGameCtnMediaBlockFxBlurDepth n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float lensSize;
        public float LensSize { get => lensSize; set => lensSize = value; }

        private bool forceFocus;
        public bool ForceFocus { get => forceFocus; set => forceFocus = value; }

        private float focusZ;
        public float FocusZ { get => focusZ; set => focusZ = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref lensSize);
            rw.Boolean(ref forceFocus);
            rw.Single(ref focusZ);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03081001 => new Chunk03081001(),
        _ => base.NewChunk(chunkId),
    };
}
