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
    private CScriptTraitsMetadata? scriptMetadata;
    private IList<ObjectSpawn>? objectSpawns;
    private CGameCtnMediaClipGroup? clipGroupInGame;
    private CGameCtnMediaClipGroup? clipGroupEndRace;

    [NodeMember]
    [AppliedWithChunk<Chunk0310D000>]
    public IList<BlockSpawn>? BlockSpawns { get => blockSpawns; set => blockSpawns = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0310D001>]
    public IList<BlockSkinSpawn>? BlockSkinSpawns { get => blockSkinSpawns; set => blockSkinSpawns = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0310D002>]
    public IList<CardEventsSpawn>? CardEventsSpawns { get => cardEventsSpawns; set => cardEventsSpawns = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0310D008>]
    public CGameCtnAutoTerrain?[]? AutoTerrains { get => autoTerrains; set => autoTerrains = value; }
    
    /// <summary>
    /// Metadata written into the macroblock.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0310D00B>]
    public CScriptTraitsMetadata? ScriptMetadata
    {
        get
        {
            DiscoverChunk<Chunk0310D00B>();
            return scriptMetadata;
        }
        set
        {
            DiscoverChunk<Chunk0310D00B>();
            scriptMetadata = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0310D00E>]
    public IList<ObjectSpawn>? ObjectSpawns { get => objectSpawns; set => objectSpawns = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupInGame
    {
        get
        {
            DiscoverChunk<Chunk0310D011>();
            return clipGroupInGame;
        }
        set
        {
            DiscoverChunk<Chunk0310D011>();
            clipGroupInGame = value;
        }
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupEndRace
    {
        get
        {
            DiscoverChunk<Chunk0310D011>();
            return clipGroupEndRace;
        }
        set
        {
            DiscoverChunk<Chunk0310D011>();
            clipGroupEndRace = value;
        }
    }

    #region Constructors

    internal CGameCtnMacroBlockInfo()
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
    [Chunk(0x0310D00B, "script metadata")]
    public class Chunk0310D00B : SkippableChunk<CGameCtnMacroBlockInfo>
    {
        public int EncapsulationVersion { get; set; }

        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            EncapsulationVersion = r.ReadInt32();
            var size = r.ReadInt32();

            n.scriptMetadata = r.ReadNode<CScriptTraitsMetadata>(expectedClassId: 0x11002000);
        }

        public override void Write(CGameCtnMacroBlockInfo n, GameBoxWriter w)
        {
            w.Write(EncapsulationVersion);

            using var ms = new MemoryStream();
            using var wm = new GameBoxWriter(ms);

            n.scriptMetadata?.Write(wm);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    #endregion

    #region 0x00C skippable chunk (splines)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00C skippable chunk (splines)
    /// </summary>
    [Chunk(0x0310D00C, "splines"), IgnoreChunk]
    public class Chunk0310D00C : SkippableChunk<CGameCtnMacroBlockInfo>
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

    #region 0x010 skippable chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x010 skippable chunk
    /// </summary>
    [Chunk(0x0310D010)]
    public class Chunk0310D010 : SkippableChunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        private int version;
        
        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x011 skippable chunk (clips)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x011 skippable chunk (clips)
    /// </summary>
    [Chunk(0x0310D011, "clips")]
    public class Chunk0310D011 : SkippableChunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public Int3 U01;
        public Int3 U02;

        public int EncapsulationVersion { get; set; }

        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            Version = r.ReadInt32();
            EncapsulationVersion = r.ReadInt32();
            var size = r.ReadInt32();

            r = new GameBoxReader(r, encapsulated: true);

            U01 = r.ReadInt3();
            U02 = r.ReadInt3();

            n.clipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
            n.clipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();

            r.Dispose();
        }

        public override void Write(CGameCtnMacroBlockInfo n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(EncapsulationVersion);

            using var ms = new MemoryStream();
            using var wm = new GameBoxWriter(ms, w, encapsulated: true);

            wm.Write(U01);
            wm.Write(U02);
            
            wm.Write(n.clipGroupInGame);
            wm.Write(n.clipGroupEndRace);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    #endregion

    #endregion
}
