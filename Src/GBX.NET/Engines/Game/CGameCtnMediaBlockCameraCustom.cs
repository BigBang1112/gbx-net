namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Custom camera (0x030A2000)
/// </summary>
[Node(0x030A2000)]
public partial class CGameCtnMediaBlockCameraCustom : CGameCtnMediaBlockCamera, CGameCtnMediaBlock.IHasKeys
{
    #region Enums

    public enum Interpolation
    {
        None,
        Hermite,
        Linear,
        FixedTangent
    }

    #endregion

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

    protected CGameCtnMediaBlockCameraCustom()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x001 chunk
    /// </summary>
    [Chunk(0x030A2001)]
    public class Chunk030A2001 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 1);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x002 chunk
    /// </summary>
    [Chunk(0x030A2002)]
    public class Chunk030A2002 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 2);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x005 chunk (TMUF)
    /// </summary>
    [Chunk(0x030A2005, "TMUF")]
    public class Chunk030A2005 : Chunk<CGameCtnMediaBlockCameraCustom>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 5);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraCustom 0x006 chunk (ManiaPlanet)
    /// </summary>
    [Chunk(0x030A2006, "ManiaPlanet")]
    public class Chunk030A2006 : Chunk<CGameCtnMediaBlockCameraCustom>, IVersionable
    {
        private int version = 3;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockCameraCustom n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!, version: 6 + version);
        }
    }

    #endregion

    #endregion
}
