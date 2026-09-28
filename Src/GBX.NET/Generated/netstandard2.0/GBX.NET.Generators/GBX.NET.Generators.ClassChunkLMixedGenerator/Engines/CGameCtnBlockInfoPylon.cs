namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03055000</remarks>
[Class(0x03055000)]
public partial class CGameCtnBlockInfoPylon : CGameCtnBlockInfo, IClass
{
    [Hexadecimal] public static new uint Id => 0x03055000;




    private float pylonOffset;
    [AppliedWithChunk<Chunk03055002>]
    public float PylonOffset { get => pylonOffset; set => pylonOffset = value; }

    private EPylonAmount pylonAmount;
    [AppliedWithChunk<Chunk03055002>]
    public EPylonAmount PylonAmount { get => pylonAmount; set => pylonAmount = value; }

    private EPylonPlacement pylonPlacement;
    [AppliedWithChunk<Chunk03055002>]
    public EPylonPlacement PylonPlacement { get => pylonPlacement; set => pylonPlacement = value; }

    private int blockHeightOffset;
    [AppliedWithChunk<Chunk03055002>]
    public int BlockHeightOffset { get => blockHeightOffset; set => blockHeightOffset = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoPylon"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoPylon() { }


    /// <summary>
    /// CGameCtnBlockInfoPylon 0x000 chunk
    /// </summary>
    [Chunk(0x03055000)]
    public partial class Chunk03055000 : Chunk<CGameCtnBlockInfoPylon>
    {
        /// <inheritdoc />
        public override uint Id => 0x03055000;

        public CMwNod? U01;
        public CMwNod? U02;
        public CMwNod? U03;

        public override void ReadWrite(CGameCtnBlockInfoPylon n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CMwNod>(ref U02);
            rw.NodeRef<CMwNod>(ref U03);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoPylon 0x002 chunk
    /// </summary>
    [Chunk(0x03055002)]
    public partial class Chunk03055002 : Chunk<CGameCtnBlockInfoPylon>
    {
        /// <inheritdoc />
        public override uint Id => 0x03055002;


        public override void ReadWrite(CGameCtnBlockInfoPylon n, GbxReaderWriter rw)
        {
            rw.Single(ref n.pylonOffset);
            rw.EnumInt32<EPylonAmount>(ref n.pylonAmount);
            rw.EnumInt32<EPylonPlacement>(ref n.pylonPlacement);
            rw.Int32(ref n.blockHeightOffset);
        }
    }



    public enum EPylonPlacement
    {
        SustainRoadCore,
        SustainRoadExterior,
    }

    public enum EPylonAmount
    {
        PylonsX4,
        PylonsX8,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03055000 => new Chunk03055000(),
        0x03055002 => new Chunk03055002(),
        _ => base.NewChunk(chunkId),
    };
}
