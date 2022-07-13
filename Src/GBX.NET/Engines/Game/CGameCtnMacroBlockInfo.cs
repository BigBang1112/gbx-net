namespace GBX.NET.Engines.Game;

/// <summary>
/// A macroblock.
/// </summary>
/// <remarks>ID: 0x0310D000</remarks>
[Node(0x0310D000), WritingNotSupported]
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

    #region 0x000 chunk (blocks)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x000 chunk (blocks)
    /// </summary>
    [Chunk(0x0310D000, "blocks")]
    public class Chunk0310D000 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ListArchive<BlockSpawn>(ref n.blockSpawns);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x001 chunk
    /// </summary>
    [Chunk(0x0310D001)]
    public class Chunk0310D001 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ListArchive<BlockSkinSpawn>(ref n.blockSkinSpawns);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x002 chunk
    /// </summary>
    [Chunk(0x0310D002)]
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

    #region 0x008 chunk

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x008 chunk
    /// </summary>
    [Chunk(0x0310D008)]
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

    #region 0x00E chunk (items)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00E chunk (items)
    /// </summary>
    [Chunk(0x0310D00E)]
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

    public class ObjectSpawn : IReadableWritable, IVersionable
    {
        private int ver;
        private Ident itemModel = Ident.Empty;
        private byte? quarterY;
        private byte? additionalDir;
        private Vec3 pitchYawRoll;
        private Int3 blockCoord;
        private string? anchorTreeId;
        private Vec3 absolutePositionInMap;
        private Vec3 pivotPosition;
        private float scale;

        public int U01;
        public int U02;
        public short U03;
        public CMwNod? U04;
        public Int3 U05;

        public int Version { get => ver; set => ver = value; }
        public Ident ItemModel { get => itemModel; set => itemModel = value; }
        public byte? QuarterY { get => quarterY; set => quarterY = value; }
        public byte? AdditionalDir { get => additionalDir; set => additionalDir = value; }
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }
        public Int3 BlockCoord { get => blockCoord; set => blockCoord = value; }
        public string? AnchorTreeId { get => anchorTreeId; set => anchorTreeId = value; }
        public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }
        public Vec3 PivotPosition { get => pivotPosition; set => pivotPosition = value; }
        public float Scale { get => scale; set => scale = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref ver);
            rw.Ident(ref itemModel!);

            if (ver < 3)
            {
                rw.Byte(ref quarterY);

                if (ver >= 1)
                {
                    rw.Byte(ref additionalDir);
                }
            }
            else
            {
                rw.Vec3(ref pitchYawRoll);
            }

            rw.Int3(ref blockCoord);
            rw.Id(ref anchorTreeId);
            rw.Vec3(ref absolutePositionInMap);

            if (ver < 5)
            {
                rw.Int32(ref U01);
            }

            if (ver < 6)
            {
                rw.Int32(ref U02);
            }

            if (ver >= 6)
            {
                rw.Int16(ref U03); // 0

                if (ver >= 7)
                {
                    rw.Vec3(ref pivotPosition);

                    if (ver >= 8)
                    {
                        rw.NodeRef(ref U04); // probably waypoint

                        if (ver >= 9)
                        {
                            rw.Single(ref scale);

                            if (ver >= 10)
                            {
                                rw.Int3(ref U05); // 0 1 -1
                            }
                        }
                    }
                }
            }
        }
    }
}
