namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Scenery (0x03188000)
/// </summary>
[Node(0x03188000)]
public partial class CGameCtnMediaBlockScenery : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;
    private CPlugDataTape? dataTape;

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

    [NodeMember]
    public CPlugDataTape? DataTape
    {
        get => dataTape;
        set => dataTape = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockScenery()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03188000)]
    public class Chunk03188000 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03188001)]
    public class Chunk03188001 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;
        private int U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Int32(ref U01);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }

    #endregion

    #endregion
}
