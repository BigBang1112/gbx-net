using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Text.
/// </summary>
/// <remarks>ID: 0x030A8000</remarks>
[Node(0x030A8000)]
[NodeExtension("GameCtnMediaBlockText")]
public class CGameCtnMediaBlockText : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private string text;
    private CControlEffectSimi effect;
    private Vec3 color;

    #region Properties

    IEnumerable<Key> IHasKeys.Keys
    {
        get => Effect.Keys.Cast<Key>();
        set => Effect.Keys = value.Cast<CControlEffectSimi.Key>().ToList();
    }

    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk030A8001>]
    public string Text { get => text; set => text = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030A8001>]
    public CControlEffectSimi Effect { get => effect; set => effect = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030A8002>]
    public Vec3 Color { get => color; set => color = value; }

    #endregion

    #region Constructors

    internal CGameCtnMediaBlockText()
    {
        text = "";
        effect = null!;
    }

    public static CGameCtnMediaBlockTextBuilder Create(CControlEffectSimi effect) => new(effect);

    #endregion

    #region Chunks

    #region 0x001 chunk (text)

    /// <summary>
    /// CGameCtnMediaBlockText 0x001 chunk (text)
    /// </summary>
    [Chunk(0x030A8001, "text")]
    public class Chunk030A8001 : Chunk<CGameCtnMediaBlockText>
    {
        public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.text!);
            rw.NodeRef<CControlEffectSimi>(ref n.effect!);
        }
    }

    #endregion

    #region 0x002 chunk (color)

    /// <summary>
    /// CGameCtnMediaBlockText 0x002 chunk (color)
    /// </summary>
    [Chunk(0x030A8002, "color")]
    public class Chunk030A8002 : Chunk<CGameCtnMediaBlockText>
    {
        public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockText 0x003 chunk
    /// </summary>
    [Chunk(0x030A8003)]
    public class Chunk030A8003 : Chunk<CGameCtnMediaBlockText>
    {
        public float U01 = 0.2f;

        public override void ReadWrite(CGameCtnMediaBlockText n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01); // 0.2
        }
    }

    #endregion

    #endregion
}
