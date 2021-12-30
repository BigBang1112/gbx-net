namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Blur depth effect (0x03081000)
/// </summary>
[Node(0x03081000)]
public class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockFxBlurDepth()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x03081001)]
    public class Chunk03081001 : Chunk<CGameCtnMediaBlockFxBlurDepth>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxBlurDepth n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                LensSize = r1.ReadSingle(),
                ForceFocus = r1.ReadBoolean(),
                FocusZ = r1.ReadSingle(),
            },
            (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.LensSize);
                w1.Write(x.ForceFocus);
                w1.Write(x.FocusZ);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float LensSize { get; set; }
        public bool ForceFocus { get; set; }
        public float FocusZ { get; set; }
    }

    #endregion
}
