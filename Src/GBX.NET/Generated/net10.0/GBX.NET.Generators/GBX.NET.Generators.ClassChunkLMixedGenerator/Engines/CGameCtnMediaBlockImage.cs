namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A5000</remarks>
[Class(0x030A5000)]
public partial class CGameCtnMediaBlockImage : CGameCtnMediaBlock, IClass
{
    [Hexadecimal] public static new uint Id => 0x030A5000;




    private CControlEffectSimi? effect;
    [AppliedWithChunk<Chunk030A5000>]
    public CControlEffectSimi? Effect { get => effect; set => effect = value; }

    private PackDesc? image;
    [AppliedWithChunk<Chunk030A5000>]
    public PackDesc? Image { get => image; set => image = value; }


    /// <summary>
    /// CGameCtnMediaBlockImage 0x000 chunk
    /// </summary>
    [Chunk(0x030A5000)]
    public partial class Chunk030A5000 : Chunk<CGameCtnMediaBlockImage>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A5000;


        public override void ReadWrite(CGameCtnMediaBlockImage n, GbxReaderWriter rw)
        {
            rw.NodeRef<CControlEffectSimi>(ref n.effect);
            rw.PackDesc(ref n.image);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockImage 0x001 chunk
    /// </summary>
    [Chunk(0x030A5001)]
    public partial class Chunk030A5001 : Chunk<CGameCtnMediaBlockImage>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A5001;

        public float U01 = 0.2f;

        public override void ReadWrite(CGameCtnMediaBlockImage n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A5000 => new Chunk030A5000(),
        0x030A5001 => new Chunk030A5001(),
        _ => base.NewChunk(chunkId),
    };
}
