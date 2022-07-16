namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - 3D stereo.
/// </summary>
/// <remarks>ID: 0x03024000</remarks>
[Node(0x03024000)]
[NodeExtension("GameCtnMediaBlock3dStereo")]
public partial class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlock3dStereo()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlock3dStereo 0x000 chunk
    /// </summary>
    [Chunk(0x03024000)]
    public class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
    {
        public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
