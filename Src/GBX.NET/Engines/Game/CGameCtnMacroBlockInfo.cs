namespace GBX.NET.Engines.Game;

/// <summary>
/// A macroblock.
/// </summary>
/// <remarks>ID: 0x0310D000</remarks>
[Node(0x0310D000)]
[NodeExtension("Macroblock")]
public partial class CGameCtnMacroBlockInfo : CGameCtnCollector
{
    private IList<BlockSpawn>? blockSpawns;
    private IList<BlockSkinSpawn>? blockSkinSpawns;
    private IList<CardEventsSpawn>? cardEventsSpawns;
    private CGameCtnAutoTerrain?[]? autoTerrains;
    private IList<ObjectSpawn>? objectSpawns;

    [NodeMember]
    public IList<BlockSpawn>? BlockSpawns { get => blockSpawns; set => blockSpawns = value; }

    [NodeMember]
    public IList<BlockSkinSpawn>? BlockSkinSpawns { get => blockSkinSpawns; set => blockSkinSpawns = value; }

    [NodeMember]
    public IList<CardEventsSpawn>? CardEventsSpawns { get => cardEventsSpawns; set => cardEventsSpawns = value; }

    [NodeMember]
    public CGameCtnAutoTerrain?[]? AutoTerrains { get => autoTerrains; set => autoTerrains = value; }

    [NodeMember]
    public IList<ObjectSpawn>? ObjectSpawns { get => objectSpawns; set => objectSpawns = value; }

    #region Constructors

    protected CGameCtnMacroBlockInfo()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk (block spawns)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x000 chunk (block spawns)
    /// </summary>
    [Chunk(0x0310D000, "block spawns")]
    public class Chunk0310D000 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ListArchive<BlockSpawn>(ref n.blockSpawns);
        }
    }

    #endregion

    #region 0x001 chunk (block skin spawns)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x001 chunk (block skin spawns)
    /// </summary>
    [Chunk(0x0310D001, "block skin spawns")]
    public class Chunk0310D001 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ListArchive<BlockSkinSpawn>(ref n.blockSkinSpawns);
        }
    }

    #endregion

    #region 0x002 chunk (card events spawns)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x002 chunk (card events spawns)
    /// </summary>
    [Chunk(0x0310D002, "card events spawns")]
    public class Chunk0310D002 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ListArchive<CardEventsSpawn>(ref n.cardEventsSpawns);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x006 chunk
    /// </summary>
    [Chunk(0x0310D006)]
    public class Chunk0310D006 : Chunk<CGameCtnMacroBlockInfo>
    {
        public int U01;
        public byte[]? U02;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Bytes(ref U02); // SceneDecals?
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x007 chunk
    /// </summary>
    [Chunk(0x0310D007)]
    public class Chunk0310D007 : Chunk<CGameCtnMacroBlockInfo>
    {
        public CMwNod?[] U01 = new CMwNod?[] { null, null, null };

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode(ref U01!);
        }
    }

    #endregion

    #region 0x008 chunk (auto terrains)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x008 chunk (auto terrains)
    /// </summary>
    [Chunk(0x0310D008, "auto terrains")]
    public class Chunk0310D008 : Chunk<CGameCtnMacroBlockInfo>
    {
        private int listVersion = 10;

        public int U01;
        public bool U02;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnAutoTerrain>(ref n.autoTerrains);
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
        }
    }

    #endregion

    #region 0x00B skippable chunk (script metadata)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00B skippable chunk (script metadata)
    /// </summary>
    [Chunk(0x0310D00B, "script metadata"), IgnoreChunk]
    public class Chunk0310D00B : SkippableChunk<CGameCtnMacroBlockInfo>
    {
        
    }

    #endregion

    #region 0x00E chunk (object spawns)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00E chunk (object spawns)
    /// </summary>
    [Chunk(0x0310D00E, "object spawns")]
    public class Chunk0310D00E : Chunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        private int version;

        public Int2[]? U01;
        public Int4[]? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ListArchive<ObjectSpawn>(ref n.objectSpawns);

            if (version < 3)
            {
                if (version >= 1)
                {
                    rw.Array<Int2>(ref U01);
                }
            }

            if (version >= 3)
            {
                rw.Array<Int4>(ref U02);
            }
        }
    }

    #endregion

    #region 0x00F chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00F chunk
    /// </summary>
    [Chunk(0x0310D00F)]
    public class Chunk0310D00F : Chunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        private int version;

        public Int3 U01;
        public Int3 U02;
        public Int3[]? U03;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int3(ref U01);

            // SBoxesSelection
            rw.Int3(ref U02);
            rw.Array<Int3>(ref U03);
        }
    }

    #endregion
    
    #endregion
}
