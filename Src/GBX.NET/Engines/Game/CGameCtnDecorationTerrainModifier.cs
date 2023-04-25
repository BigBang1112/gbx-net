namespace GBX.NET.Engines.Game;

[Node(0x0303C000)]
public class CGameCtnDecorationTerrainModifier : CMwNod
{
    private CPlugGameSkin? remapping;
    private GameBoxRefTable.File? remappingFile;
    private string? remapFolder;
    private string? idName;

    internal CGameCtnDecorationTerrainModifier()
    {

    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303C000>]
    public CPlugGameSkin? Remapping
    {
        get => remapping = GetNodeFromRefTable(remapping, remappingFile) as CPlugGameSkin;
        set => remapping = value;
    }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303C000>]
    public string? RemapFolder { get => remapFolder; set => remapFolder = value; }

    [NodeMember(ExactName = "Id")]
    [AppliedWithChunk<Chunk0303C001>]
    public string? IdName { get => idName; set => idName = value; }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnDecorationTerrainModifier 0x000 chunk
    /// </summary>
    [Chunk(0x0303C000)]
    public class Chunk0303C000 : Chunk<CGameCtnDecorationTerrainModifier>
    {
        public override void ReadWrite(CGameCtnDecorationTerrainModifier n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugGameSkin>(ref n.remapping, ref n.remappingFile);
            rw.String(ref n.remapFolder);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnDecorationTerrainModifier 0x001 chunk
    /// </summary>
    [Chunk(0x0303C001)]
    public class Chunk0303C001 : Chunk<CGameCtnDecorationTerrainModifier>
    {
        public override void ReadWrite(CGameCtnDecorationTerrainModifier n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.idName);
        }
    }

    #endregion
}
