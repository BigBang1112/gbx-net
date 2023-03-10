namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Image.
/// </summary>
/// <remarks>ID: 0x030A5000</remarks>
[Node(0x030A5000)]
[NodeExtension("GameCtnMediaBlockImage")]
public class CGameCtnMediaBlockImage : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private CControlEffectSimi effect;
    private FileRef image;

    IEnumerable<Key> IHasKeys.Keys
    {
        get => effect?.Keys.Cast<Key>() ?? Enumerable.Empty<Key>();
        set
        {
            if (effect is not null)
            {
                effect.Keys = value.Cast<CControlEffectSimi.Key>().ToList();
            }
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk030A5000>]
    public CControlEffectSimi Effect { get => effect; set => effect = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030A5000>]
    public FileRef Image { get => image; set => image = value; }

    internal CGameCtnMediaBlockImage()
    {
        effect = null!;
        image = FileRef.Default;
    }

    public static CGameCtnMediaBlockImage Create(CControlEffectSimi effect, FileRef image)
    {
        var block = new CGameCtnMediaBlockImage
        {
            effect = effect,
            image = image
        };

        block.CreateChunk<Chunk030A5000>();

        return block;
    }

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x030A5000)]
    public class Chunk030A5000 : Chunk<CGameCtnMediaBlockImage>
    {
        public override void ReadWrite(CGameCtnMediaBlockImage n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CControlEffectSimi>(ref n.effect!);
            rw.FileRef(ref n.image!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x030A5001)]
    public class Chunk030A5001 : Chunk<CGameCtnMediaBlockImage>
    {
        public float U01 = 0.2f;

        public override void ReadWrite(CGameCtnMediaBlockImage n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01); // 0.2
        }
    }

    #endregion

    #endregion
}
