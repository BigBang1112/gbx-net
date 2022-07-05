using GBX.NET.Builders.Engines.Control;

namespace GBX.NET.Engines.Control;

/// <summary>
/// An effect used on certain MediaTracker block types that tells how to generally render the component.
/// </summary>
/// <remarks>ID: 0x07005000</remarks>
[Node(0x07010000)]
[NodeExtension("ControlEffect")]
public partial class CControlEffectSimi : CControlEffect, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;
    private bool centered;
    private int colorBlendMode;
    private bool isContinousEffect;
    private bool isInterpolated;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> CGameCtnMediaBlock.IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    /// <summary>
    /// Keyframes of the effect.
    /// </summary>
    [NodeMember]
    public IList<Key> Keys { get => keys; set => keys = value; }

    /// <summary>
    /// If the effect should be centered.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    public bool Centered { get => centered; set => centered = value; }

    [NodeMember(ExactlyNamed = true)]
    public int ColorBlendMode { get => colorBlendMode; set => colorBlendMode = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsContinousEffect { get => isContinousEffect; set => isContinousEffect = value; }

    /// <summary>
    /// If the keyframes should interpolate values between each other.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    public bool IsInterpolated { get => isInterpolated; set => isInterpolated = value; }

    #endregion

    #region Constructors

    protected CControlEffectSimi()
    {
        keys = null!;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates a new builder that can output <see cref="CControlEffectSimi"/> in certain versions with certain chunks and values.
    /// </summary>
    /// <returns>A builder.</returns>
    public static CControlEffectSimiBuilder Create()
    {
        return new CControlEffectSimiBuilder();
    }

    #endregion

    #region Chunks

    #region 0x002 chunk

    /// <summary>
    /// CControlEffectSimi 0x002 chunk
    /// </summary>
    [Chunk(0x07010002)]
    public class Chunk07010002 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List<Key>(ref n.keys!, (rw, x) => x.ReadWrite(rw, version: 2));
            rw.Boolean(ref n.centered);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CControlEffectSimi 0x004 chunk
    /// </summary>
    [Chunk(0x07010004)]
    public class Chunk07010004 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List<Key>(ref n.keys!, (rw, x) => x.ReadWrite(rw, version: 4));
            rw.Boolean(ref n.centered);
            rw.Int32(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CControlEffectSimi 0x005 chunk
    /// </summary>
    [Chunk(0x07010005)]
    public class Chunk07010005 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List<Key>(ref n.keys!, (rw, x) => x.ReadWrite(rw, version: 5));
            rw.Boolean(ref n.centered);
            rw.Int32(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
            rw.Boolean(ref n.isInterpolated);
        }
    }

    #endregion

    #endregion
}
