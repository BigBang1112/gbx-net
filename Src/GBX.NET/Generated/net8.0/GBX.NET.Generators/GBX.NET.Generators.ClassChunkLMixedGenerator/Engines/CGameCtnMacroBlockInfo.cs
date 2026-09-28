namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0310D000</remarks>
[Class(0x0310D000)]
public partial class CGameCtnMacroBlockInfo : CGameCtnCollector, IClass
{
    [Hexadecimal] public static new uint Id => 0x0310D000;




    private List<BlockSpawn>? blockSpawns;
    [AppliedWithChunk<Chunk0310D000>]
    public List<BlockSpawn>? BlockSpawns { get => blockSpawns; set => blockSpawns = value; }

    private List<BlockSkinSpawn>? blockSkinSpawns;
    [AppliedWithChunk<Chunk0310D001>]
    public List<BlockSkinSpawn>? BlockSkinSpawns { get => blockSkinSpawns; set => blockSkinSpawns = value; }

    private List<CardEventsSpawn>? cardEventsSpawns;
    [AppliedWithChunk<Chunk0310D002>]
    public List<CardEventsSpawn>? CardEventsSpawns { get => cardEventsSpawns; set => cardEventsSpawns = value; }

    private byte[]? sceneDecals;
    [AppliedWithChunk<Chunk0310D006>]
    public byte[]? SceneDecals { get => sceneDecals; set => sceneDecals = value; }

    private CGameCtnAutoTerrain[]? autoTerrains;
    [AppliedWithChunk<Chunk0310D008>]
    public CGameCtnAutoTerrain[]? AutoTerrains { get => autoTerrains; set => autoTerrains = value; }

    private List<ObjectSpawn>? objectSpawns;
    [AppliedWithChunk<Chunk0310D00E>]
    public List<ObjectSpawn>? ObjectSpawns { get => objectSpawns; set => objectSpawns = value; }

    private Int3 offzoneTriggerSize = (3, 1, 3);
    [AppliedWithChunk<Chunk0310D00F>]
    public Int3 OffzoneTriggerSize { get => offzoneTriggerSize; set => offzoneTriggerSize = value; }

    private BoxInt3[]? offzones;
    [AppliedWithChunk<Chunk0310D00F>]
    public BoxInt3[]? Offzones { get => offzones; set => offzones = value; }

    private int iconSize;
    [AppliedWithChunk<Chunk0310D010>]
    public int IconSize { get => iconSize; set => iconSize = value; }


    /// <summary>
    /// CGameCtnMacroBlockInfo 0x000 chunk (block spawns)
    /// </summary>
    [Chunk(0x0310D000, "block spawns")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D000 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<BlockSpawn>(ref n.blockSpawns!);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x001 chunk (block skin spawns)
    /// </summary>
    [Chunk(0x0310D001, "block skin spawns")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D001 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<BlockSkinSpawn>(ref n.blockSkinSpawns!);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x002 chunk (card events spawns)
    /// </summary>
    [Chunk(0x0310D002, "card events spawns")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D002 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;


        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<CardEventsSpawn>(ref n.cardEventsSpawns!);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x006 chunk
    /// </summary>
    [Chunk(0x0310D006)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D006 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int U01 = 2;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Data(ref n.sceneDecals);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x007 chunk
    /// </summary>
    [Chunk(0x0310D007)]
    public partial class Chunk0310D007 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D007;

        public CGameCtnMediaClipGroup? U01;
        public CGameCtnMediaClipGroup? U02;
        public CMwNod? U03;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnMediaClipGroup>(ref U01);
            rw.NodeRef<CGameCtnMediaClipGroup>(ref U02);
            rw.NodeRef<CMwNod>(ref U03);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x008 chunk
    /// </summary>
    [Chunk(0x0310D008)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D008 : Chunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnAutoTerrain>(ref n.autoTerrains!);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00B skippable chunk (script metadata)
    /// </summary>
    [Chunk(0x0310D00B, "script metadata")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D00B : SkippableChunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D00B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00C skippable chunk (splines)
    /// </summary>
    [Chunk(0x0310D00C, "splines")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 2, 2)]
    public partial class Chunk0310D00C : SkippableChunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D00C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; } = 2;

        public CMwNod[]? U01;
        public CPlugSpline3D[]? U02;
        public CMwNod[]? U03;
        public CMwNod[]? U04;
        public int U05;
        public int U06;
        public int U07;
        public CMwNod[]? U08;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.ArrayNodeRef_deprec<CMwNod>(ref U01!);
                rw.ArrayNodeRef_deprec<CPlugSpline3D>(ref U02!);
                rw.ArrayNodeRef_deprec<CMwNod>(ref U03!);
                rw.ArrayNodeRef_deprec<CMwNod>(ref U04!);
                rw.Int32(ref U05);
                rw.Int32(ref U06);
                rw.Int32(ref U07);
            }
            if (Version >= 2)
            {
                return;
            }
            rw.ArrayNodeRef_deprec<CMwNod>(ref U08!);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00D chunk
    /// </summary>
    [Chunk(0x0310D00D)]
    public partial class Chunk0310D00D : Chunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D00D;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00E chunk (object spawns)
    /// </summary>
    [Chunk(0x0310D00E, "object spawns")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020, 2, 2)]
    public partial class Chunk0310D00E : Chunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D00E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public Int2[]? U01;
        public Int4[]? U02;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<ObjectSpawn>(ref n.objectSpawns!);
            if (Version <= 2)
            {
                if (Version >= 1)
                {
                    rw.Array<Int2>(ref U01!);
                }
            }
            if (Version >= 3)
            {
                rw.Array<Int4>(ref U02!);
            }
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x00F chunk
    /// </summary>
    [Chunk(0x0310D00F)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D00F : Chunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D00F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }

        public Int3 U01;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int3(ref U01);
            rw.Int3(ref n.offzoneTriggerSize);
            rw.Array<BoxInt3>(ref n.offzones!);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x010 skippable chunk
    /// </summary>
    [Chunk(0x0310D010)]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D010 : SkippableChunk<CGameCtnMacroBlockInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.iconSize);
        }
    }

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x011 skippable chunk (mediatracker)
    /// </summary>
    [Chunk(0x0310D011, "mediatracker")]
    [ChunkGameVersion(GameVersion.MP4 | GameVersion.TM2020)]
    public partial class Chunk0310D011 : SkippableChunk<CGameCtnMacroBlockInfo>
    {
        /// <inheritdoc />
        public override uint Id => 0x0310D011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4 | GameVersion.TM2020;

    }


    public sealed partial class BlockSpawn : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private Ident? blockModel;
        public Ident? BlockModel { get => blockModel; set => blockModel = value; }

        private Int3 coord;
        public Int3 Coord { get => coord; set => coord = value; }

        private Direction direction;
        public Direction Direction { get => direction; set => direction = value; }

        private int flags;
        public int Flags { get => flags; set => flags = value; }

        private Vec3 absolutePositionInMap;
        public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }

        private Vec3 pitchYawRoll;
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        private CGameWaypointSpecialProperty? waypoint;
        public CGameWaypointSpecialProperty? Waypoint { get => waypoint; set => waypoint = value; }

        private CMwNod? u01;
        public CMwNod? U01 { get => u01; set => u01 = value; }

        private short u02;
        public short U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Ident(ref blockModel);
            if (Version<2)
            {
                rw.Int3(ref coord);
                rw.EnumInt32<Direction>(ref direction);
            }
            if (Version>=2)
            {
                if (Version<5)
                {
                    rw.Byte3(ref coord);
                    rw.EnumByte<Direction>(ref direction);
                }
                rw.Int32(ref flags);
                if (Version>=3)
                {
                    if (Version>=5)
                    {
                        if (((Flags>>26)&1)!=0)
                        {
                            rw.Vec3(ref absolutePositionInMap);
                            rw.Vec3(ref pitchYawRoll);
                        }
                        if (((Flags>>26)&1)==0)
                        {
                            rw.Byte3(ref coord);
                            rw.EnumByte<Direction>(ref direction);
                        }
                    }
                    rw.NodeRef<CGameWaypointSpecialProperty>(ref waypoint);
                    if (Version>=4)
                    {
                        if (Version<6)
                        {
                            rw.NodeRef<CMwNod>(ref u01);
                        }
                        if (Version>=8)
                        {
                            rw.Int16(ref u02);
                        }
                    }
                }
            }
        }
    }

    public sealed partial class CardEventsSpawn : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private Ident[]? u01;
        public Ident[]? U01 { get => u01; set => u01 = value; }

        private Int3 u02;
        public Int3 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.ArrayIdent(ref u01!);
            rw.Int3(ref u02);
        }
    }

    public sealed partial class ObjectSpawn : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private Ident? itemModel;
        public Ident? ItemModel { get => itemModel; set => itemModel = value; }

        private byte quarterY;
        public byte QuarterY { get => quarterY; set => quarterY = value; }

        private byte additionalDir;
        public byte AdditionalDir { get => additionalDir; set => additionalDir = value; }

        private Vec3 pitchYawRoll;
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        private Int3 blockCoord;
        public Int3 BlockCoord { get => blockCoord; set => blockCoord = value; }

        private string? anchorTreeId;
        public string? AnchorTreeId { get => anchorTreeId; set => anchorTreeId = value; }

        private Vec3 absolutePositionInMap;
        public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private short u03;
        public short U03 { get => u03; set => u03 = value; }

        private Vec3 pivotPosition;
        public Vec3 PivotPosition { get => pivotPosition; set => pivotPosition = value; }

        private CGameWaypointSpecialProperty? waypoint;
        public CGameWaypointSpecialProperty? Waypoint { get => waypoint; set => waypoint = value; }

        private float scale;
        public float Scale { get => scale; set => scale = value; }

        private Int3 u04;
        public Int3 U04 { get => u04; set => u04 = value; }

        private byte u05;
        public byte U05 { get => u05; set => u05 = value; }

        private byte u06;
        public byte U06 { get => u06; set => u06 = value; }

        private byte u07;
        public byte U07 { get => u07; set => u07 = value; }

        private bool hasPackDesc;
        public bool HasPackDesc { get => hasPackDesc; set => hasPackDesc = value; }

        private bool hasForegroundPackDesc;
        public bool HasForegroundPackDesc { get => hasForegroundPackDesc; set => hasForegroundPackDesc = value; }

        private PackDesc? packDesc;
        public PackDesc? PackDesc { get => packDesc; set => packDesc = value; }

        private PackDesc? foregroundPackDesc;
        public PackDesc? ForegroundPackDesc { get => foregroundPackDesc; set => foregroundPackDesc = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Ident(ref itemModel);
            if (Version<3)
            {
                rw.Byte(ref quarterY);
                if (Version>=1)
                {
                    rw.Byte(ref additionalDir);
                }
            }
            if (Version>=3)
            {
                rw.Vec3(ref pitchYawRoll);
            }
            rw.Int3(ref blockCoord);
            rw.Id(ref anchorTreeId);
            rw.Vec3(ref absolutePositionInMap);
            if (Version<5)
            {
                rw.Int32(ref u01);
            }
            if (Version<6)
            {
                rw.Int32(ref u02);
            }
            if (Version>=6)
            {
                rw.Int16(ref u03);
                if (Version>=7)
                {
                    rw.Vec3(ref pivotPosition);
                    if (Version>=8)
                    {
                        rw.NodeRef<CGameWaypointSpecialProperty>(ref waypoint);
                        if (Version>=9)
                        {
                            rw.Single(ref scale);
                            if (Version>=10)
                            {
                                rw.Int3(ref u04);
                                if (Version>=11)
                                {
                                    rw.Byte(ref u05);
                                    rw.Byte(ref u06);
                                    if (Version>=12)
                                    {
                                        rw.Byte(ref u07);
                                        if (Version>=13)
                                        {
                                            rw.Boolean(ref hasPackDesc, asByte: true);
                                            rw.Boolean(ref hasForegroundPackDesc, asByte: true);
                                            if (HasPackDesc)
                                            {
                                                rw.PackDesc(ref packDesc);
                                            }
                                            if (HasForegroundPackDesc)
                                            {
                                                rw.PackDesc(ref foregroundPackDesc);
                                            }
                                            if (Version>=14)
                                            {
                                                rw.Int32(ref u10);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public sealed partial class BlockSkinSpawn : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private CGameCtnBlockSkin? skin;
        public CGameCtnBlockSkin? Skin { get => skin; set => skin = value; }

        private Int3 u01;
        /// <summary>
        /// its position?
        /// </summary>
        public Int3 U01 { get => u01; set => u01 = value; }

        private int blockSpawnIndex;
        public int BlockSpawnIndex { get => blockSpawnIndex; set => blockSpawnIndex = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.NodeRef<CGameCtnBlockSkin>(ref skin);
            if (Version==0)
            {
                rw.Int3(ref u01); // its position?
            }
            rw.Int32(ref blockSpawnIndex);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0310D000 => new Chunk0310D000(),
        0x0310D001 => new Chunk0310D001(),
        0x0310D002 => new Chunk0310D002(),
        0x0310D006 => new Chunk0310D006(),
        0x0310D007 => new Chunk0310D007(),
        0x0310D008 => new Chunk0310D008(),
        0x0310D00B => new Chunk0310D00B(),
        0x0310D00C => new Chunk0310D00C(),
        0x0310D00D => new Chunk0310D00D(),
        0x0310D00E => new Chunk0310D00E(),
        0x0310D00F => new Chunk0310D00F(),
        0x0310D010 => new Chunk0310D010(),
        0x0310D011 => new Chunk0310D011(),
        _ => base.NewChunk(chunkId),
    };
}
