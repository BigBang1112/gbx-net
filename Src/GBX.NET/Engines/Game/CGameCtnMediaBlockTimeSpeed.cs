namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Time speed.
/// </summary>
/// <remarks>ID: 0x03129000</remarks>
[Node(0x03129000)]
public partial class CGameCtnMediaBlockTimeSpeed : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk03085000>]
    [AppliedWithChunk<Chunk03129000>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockTimeSpeed()
    {
        keys = Array.Empty<Key>();
    }

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
