namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Tone mapping
/// </summary>
/// <remarks>ID: 0x03127000</remarks>
[Node(0x03127000)]
[NodeExtension("GameCtnMediaBlockToneMapping")]
public partial class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03127000))]
    [AppliedWithChunk(typeof(Chunk03127001))]
    [AppliedWithChunk(typeof(Chunk03127002))]
    [AppliedWithChunk(typeof(Chunk03127003))]
    [AppliedWithChunk(typeof(Chunk03127004))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    protected CGameCtnMediaBlockToneMapping()
    {
        keys = Array.Empty<Key>();
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x000 chunk
    /// </summary>
    [Chunk(0x03127000)]
    public class Chunk03127000 : Chunk<CGameCtnMediaBlockToneMapping>
    {
        public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
    
    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x001 chunk
    /// </summary>
    [Chunk(0x03127001)]
    public class Chunk03127001 : Chunk03127000
    {
        
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x002 chunk
    /// </summary>
    [Chunk(0x03127002)]
    public class Chunk03127002 : Chunk03127000
    {

    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x003 chunk
    /// </summary>
    [Chunk(0x03127003)]
    public class Chunk03127003 : Chunk03127000
    {

    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnMediaBlockToneMapping 0x004 chunk
    /// </summary>
    [Chunk(0x03127004)]
    public class Chunk03127004 : Chunk03127000
    {

    }

    #endregion

    #endregion
}
