namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Tone mapping (0x03127000)
/// </summary>
[Node(0x03127000)]
[NodeExtension("GameCtnMediaBlockToneMapping")]
public partial class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockToneMapping()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x004 chunk

    [Chunk(0x03127004)]
    public class Chunk03127004 : Chunk<CGameCtnMediaBlockToneMapping>
    {
        public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
