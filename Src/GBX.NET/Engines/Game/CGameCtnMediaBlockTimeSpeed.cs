namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Time speed.
/// </summary>
/// <remarks>ID: 0x03129000</remarks>
[Node(0x03129000)]
public partial class CGameCtnMediaBlockTimeSpeed : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockTimeSpeed()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    [Chunk(0x03085000)]
    public class Chunk03085000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    [Chunk(0x03129000)]
    public class Chunk03129000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
}
