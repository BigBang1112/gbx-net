namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera path (0x030A1000)
/// </summary>
[Node(0x030A1000)]
[NodeExtension("CtnMediaBlockCamPath.Gbx")]
public partial class CGameCtnMediaBlockCameraPath : CGameCtnMediaBlockCamera, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockCameraPath()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x000 chunk
    /// </summary>
    [Chunk(0x030A1000)]
    public class Chunk030A1000 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x001 chunk
    /// </summary>
    [Chunk(0x030A1001)]
    public class Chunk030A1001 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 1);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x002 chunk
    /// </summary>
    [Chunk(0x030A1002)]
    public class Chunk030A1002 : Chunk<CGameCtnMediaBlockCameraPath>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!, version: 2);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraPath 0x003 chunk
    /// </summary>
    [Chunk(0x030A1003)]
    public class Chunk030A1003 : Chunk<CGameCtnMediaBlockCameraPath>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockCameraPath n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!, version);
        }
    }

    #endregion

    #endregion
}
