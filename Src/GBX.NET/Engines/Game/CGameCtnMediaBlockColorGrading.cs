namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Color grading.
/// </summary>
/// <remarks>ID: 0x03186000</remarks>
[Node(0x03186000)]
[NodeExtension("GameCtnMediaBlockColorGrading")]
public partial class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private FileRef image;
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03186000))]
    public FileRef Image { get => image; set => image = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03186001))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockColorGrading()
    {
        image = FileRef.Default;
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x000 chunk
    /// </summary>
    [Chunk(0x03186000)]
    public class Chunk03186000 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.image!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockColorGrading 0x001 chunk
    /// </summary>
    [Chunk(0x03186001)]
    public class Chunk03186001 : Chunk<CGameCtnMediaBlockColorGrading>
    {
        public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #endregion
}
