namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A8000</remarks>
[Class(0x030A8000)]
public partial class CGameCtnMediaBlockText : CGameCtnMediaBlock, IClass
{
    [Hexadecimal] public static new uint Id => 0x030A8000;




    private string? text;
    [AppliedWithChunk<Chunk030A8001>]
    public string? Text { get => text; set => text = value; }

    private CControlEffectSimi? effect;
    [AppliedWithChunk<Chunk030A8001>]
    public CControlEffectSimi? Effect { get => effect; set => effect = value; }

    private Vec3 color;
    [AppliedWithChunk<Chunk030A8002>]
    public Vec3 Color { get => color; set => color = value; }


    /// <summary>
    /// CGameCtnMediaBlockText 0x001 chunk (text)
    /// </summary>
    [Chunk(0x030A8001, "text")]
    public partial class Chunk030A8001 : Chunk<CGameCtnMediaBlockText>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A8001;


        public override void ReadWrite(CGameCtnMediaBlockText n, GbxReaderWriter rw)
        {
            rw.String(ref n.text);
            rw.NodeRef<CControlEffectSimi>(ref n.effect);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockText 0x002 chunk (color)
    /// </summary>
    [Chunk(0x030A8002, "color")]
    public partial class Chunk030A8002 : Chunk<CGameCtnMediaBlockText>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A8002;


        public override void ReadWrite(CGameCtnMediaBlockText n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockText 0x003 chunk
    /// </summary>
    [Chunk(0x030A8003)]
    public partial class Chunk030A8003 : Chunk<CGameCtnMediaBlockText>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A8003;

        public float U01 = 0.2f;

        public override void ReadWrite(CGameCtnMediaBlockText n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A8001 => new Chunk030A8001(),
        0x030A8002 => new Chunk030A8002(),
        0x030A8003 => new Chunk030A8003(),
        _ => base.NewChunk(chunkId),
    };
}
