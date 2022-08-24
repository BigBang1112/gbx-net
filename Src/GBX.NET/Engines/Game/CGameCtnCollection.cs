using System.Numerics;

namespace GBX.NET.Engines.Game;

/// <summary>
/// Information about an environment. Does not include list of possible blocks.
/// </summary>
/// <remarks>ID: 0x03033000</remarks>
[Node(0x03033000)]
[NodeExtension("TMElementColl")]
[NodeExtension("TMCollection")]
[NodeExtension("Collection")]
public partial class CGameCtnCollection : CMwNod, CGameCtnCollection.IHeader
{
    #region Enums

    public enum EBackgroundShadow
    {
        None,
        Receive,
        CastAndReceive
    }

    public enum EVertexLighting
    {
        None,
        Sunrise,
        Nations
    }

    public enum EVehicleEnvLayer
    {
        Dirt,
        Mud
    }

    #endregion

    #region Fields

    private string? collection;
    private bool needUnlock;
    private string? iconEnv;
    private string? iconCollection;
    private int sortIndex;
    private string? defaultZone;
    private Ident? vehicle;
    private string? mapFid;
    private Vec2? mapCoordElem;
    private Vec2? mapCoordIcon;
    private string? loadScreen;
    private Vec2? mapCoordDesc;
    private string? longDesc;
    private string? displayName;
    private bool? isEditable;
    private float squareSize;
    private float squareHeight;
    private float cameraMinHeight;
    private float shadowSoftSizeInWorld;
    private float colorVertexMin;
    private float colorVertexMax;
    private bool hasIconFid;
    private CPlugBitmap? iconFid;
    private GameBoxRefTable.File? iconFidFile;
    private bool hasCollectionIconFid;
    private CPlugBitmap? collectionIconFid;
    private GameBoxRefTable.File? collectionIconFidFile;
    private CPlugBitmap? loadScreenFid;
    private GameBoxRefTable.File? loadScreenFidFile;
    private CGameCtnDecoration? defaultDecoration;
    private GameBoxRefTable.File? defaultDecorationFile;
    private ExternalNode<CGameCtnZone>[]? completeZoneList;
    private ZoneString[]? zoneStrings;
    private CGameCtnDecorationTerrainModifier?[]? replacementTerrainModifiers;
    private string? folderBlockInfo;
    private string? folderItem;
    private string? folderDecoration;
    private string? folderMenusIcons;
    private bool isWaterMultiHeight;
    private EBackgroundShadow backgroundShadow;
    private EVertexLighting vertexLighting;
    private float boardSquareHeight;
    private float boardSquareBorder;
    private string? folderDecalModels;
    private Vec3? tech3TunnelSpecularExpScaleMax;
    private string? folderMacroDecals;
    private string? folderAdditionalItem1;
    private string? folderAdditionalItem2;
    private CFuncShaderLayerUV? fidFuncShaderCloudsX2;
    private CPlugBitmap? fidPlugBitmapCloudsX2;
    private CPlugBitmap? vehicleEnvLayerFidBitmap;
    private EVehicleEnvLayer vehicleEnvLayer;
    private CPlugFogMatter? offZoneFogMatter;
    private float terrainHeightOffset;
    private CPlugBitmap? waterGBitmapNormal;
    private float? waterGBumpSpeedUV;
    private float? waterGBumpScaleUV;
    private float? waterGBumpScale;
    private float? waterGRefracPertub;
    private float? waterFogClampAboveDist;
    private float? visMeshLodDistScale;
    private int decalFadeCBlockFullDensity;
    private CMwNod? itemPlacementGroups;
    private CMwNod? adnRandomGenList;
    private CMwNod? fidBlockInfoGroups;
    private uint? turboColorRoulette1;
    private uint? turboColorRoulette2;
    private uint? turboColorRoulette3;
    private uint? turboColorTurbo;
    private uint? turboColorTurbo2;
    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram16x9;
    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10A;
    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10B;
    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10C;
    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram2x3;

    #endregion

    #region Properties

    public HeaderChunkSet HeaderChunks { get; } = new();

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public string? Collection { get => collection; set => collection = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public bool NeedUnlock { get => needUnlock; set => needUnlock = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? IconEnv { get => iconEnv; set => iconEnv = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? IconCollection { get => iconCollection; set => iconCollection = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public int SortIndex { get => sortIndex; set => sortIndex = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? DefaultZone { get => defaultZone; set => defaultZone = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public Ident? Vehicle { get => vehicle; set => vehicle = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? MapFid { get => mapFid; set => mapFid = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public Vec2? MapCoordElem { get => mapCoordElem; set => mapCoordElem = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public Vec2? MapCoordIcon { get => mapCoordIcon; set => mapCoordIcon = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? LoadScreen { get => loadScreen; set => loadScreen = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public Vec2? MapCoordDesc { get => mapCoordDesc; set => mapCoordDesc = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    public string? LongDesc { get => longDesc; set => longDesc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    [AppliedWithChunk(typeof(Chunk03033021))]
    public string? DisplayName { get => displayName; set => displayName = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033001H))]
    [AppliedWithChunk(typeof(Chunk0303300C))]
    public bool? IsEditable { get => isEditable; set => isEditable = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public float SquareSize { get => squareSize; set => squareSize = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public float SquareHeight { get => squareHeight; set => squareHeight = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033013))]
    [AppliedWithChunk(typeof(Chunk0303301E))]
    [AppliedWithChunk(typeof(Chunk03033038))]
    public float CameraMinHeight { get => cameraMinHeight; set => cameraMinHeight = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033016))]
    [AppliedWithChunk(typeof(Chunk03033024))]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public float ShadowSoftSizeInWorld { get => shadowSoftSizeInWorld; set => shadowSoftSizeInWorld = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033016))]
    [AppliedWithChunk(typeof(Chunk03033024))]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public float ColorVertexMin { get => colorVertexMin; set => colorVertexMin = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033016))]
    [AppliedWithChunk(typeof(Chunk03033024))]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public float ColorVertexMax { get => colorVertexMax; set => colorVertexMax = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0303300D))]
    public bool HasIconFid { get => hasIconFid; set => hasIconFid = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0303300D))]
    public CPlugBitmap? IconFid
    {
        get => iconFid = GetNodeFromRefTable(iconFid, iconFidFile) as CPlugBitmap;
        set => iconFid = value;
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0303300D))]
    public bool HasCollectionIconFid { get => hasCollectionIconFid; set => hasCollectionIconFid = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0303300D))]
    public CPlugBitmap? CollectionIconFid
    {
        get => collectionIconFid = GetNodeFromRefTable(collectionIconFid, collectionIconFidFile) as CPlugBitmap;
        set => collectionIconFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CPlugBitmap? LoadScreenFid
    {
        get => loadScreenFid = GetNodeFromRefTable(loadScreenFid, loadScreenFidFile) as CPlugBitmap;
        set => loadScreenFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033008))]
    public CGameCtnDecoration? DefaultDecoration
    {
        get => defaultDecoration = GetNodeFromRefTable(defaultDecoration, defaultDecorationFile) as CGameCtnDecoration;
        set => defaultDecoration = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033009))]
    public ExternalNode<CGameCtnZone>[]? CompleteZoneList { get => completeZoneList; set => completeZoneList = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0303301D))]
    public ZoneString[]? ZoneStrings { get => zoneStrings; set => zoneStrings = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0303301D))]
    public CGameCtnDecorationTerrainModifier?[]? ReplacementTerrainModifiers { get => replacementTerrainModifiers; set => replacementTerrainModifiers = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033020))]
    public string? FolderBlockInfo { get => folderBlockInfo; set => folderBlockInfo = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033020))]
    public string? FolderItem { get => folderItem; set => folderItem = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033020))]
    public string? FolderDecoration { get => folderDecoration; set => folderDecoration = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033020))]
    public string? FolderMenusIcons { get => folderMenusIcons; set => folderMenusIcons = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033022))]
    [AppliedWithChunk(typeof(Chunk03033038))]
    public bool IsWaterMultiHeight { get => isWaterMultiHeight; set => isWaterMultiHeight = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033016))]
    [AppliedWithChunk(typeof(Chunk03033024))]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public EBackgroundShadow BackgroundShadow { get => backgroundShadow; set => backgroundShadow = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033016))]
    [AppliedWithChunk(typeof(Chunk03033024))]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public EVertexLighting VertexLighting { get => vertexLighting; set => vertexLighting = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033027))]
    public float BoardSquareHeight { get => boardSquareHeight; set => boardSquareHeight = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033027))]
    public float BoardSquareBorder { get => boardSquareBorder; set => boardSquareBorder = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0303302A))]
    public string? FolderDecalModels { get => folderDecalModels; set => folderDecalModels = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0303302F))]
    public Vec3? Tech3TunnelSpecularExpScaleMax { get => tech3TunnelSpecularExpScaleMax; set => tech3TunnelSpecularExpScaleMax = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033031))]
    public string? FolderMacroDecals { get => folderMacroDecals; set => folderMacroDecals = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033028))]
    public string? FolderAdditionalItem1 { get => folderAdditionalItem1; set => folderAdditionalItem1 = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033029))]
    public string? FolderAdditionalItem2 { get => folderAdditionalItem2; set => folderAdditionalItem2 = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033034))]
    public CFuncShaderLayerUV? FidFuncShaderCloudsX2 { get => fidFuncShaderCloudsX2; set => fidFuncShaderCloudsX2 = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033034))]
    public CPlugBitmap? FidPlugBitmapCloudsX2 { get => fidPlugBitmapCloudsX2; set => fidPlugBitmapCloudsX2 = value; }

    [NodeMember(ExactName = "VehicleEnvLayer_FidBitmap")]
    [AppliedWithChunk(typeof(Chunk03033034), sinceVersion: 1)]
    public CPlugBitmap? VehicleEnvLayerFidBitmap { get => vehicleEnvLayerFidBitmap; set => vehicleEnvLayerFidBitmap = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033034), sinceVersion: 1)]
    public EVehicleEnvLayer VehicleEnvLayer { get => vehicleEnvLayer; set => vehicleEnvLayer = value; }

    [NodeMember(ExactName = "OffZone_FogMatter")]
    [AppliedWithChunk(typeof(Chunk03033036))]
    public CPlugFogMatter? OffZoneFogMatter { get => offZoneFogMatter; set => offZoneFogMatter = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033037))]
    public float TerrainHeightOffset { get => terrainHeightOffset; set => terrainHeightOffset = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033038), sinceVersion: 5)]
    public CPlugBitmap? WaterGBitmapNormal { get => waterGBitmapNormal; set => waterGBitmapNormal = value; }

    [NodeMember(ExactName = "WaterG_BumpSpeedUV")]
    [AppliedWithChunk(typeof(Chunk03033038), sinceVersion: 5)]
    public float? WaterGBumpSpeedUV { get => waterGBumpSpeedUV; set => waterGBumpSpeedUV = value; }

    [NodeMember(ExactName = "WaterG_BumpScaleUV")]
    [AppliedWithChunk(typeof(Chunk03033038), sinceVersion: 5)]
    public float? WaterGBumpScaleUV { get => waterGBumpScaleUV; set => waterGBumpScaleUV = value; }

    [NodeMember(ExactName = "WaterG_BumpScale")]
    [AppliedWithChunk(typeof(Chunk03033038), sinceVersion: 5)]
    public float? WaterGBumpScale { get => waterGBumpScale; set => waterGBumpScale = value; }

    [NodeMember(ExactName = "WaterG_RefracPertub")]
    [AppliedWithChunk(typeof(Chunk03033038), sinceVersion: 5)]
    public float? WaterGRefracPertub { get => waterGRefracPertub; set => waterGRefracPertub = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033038))]
    public float? WaterFogClampAboveDist { get => waterFogClampAboveDist; set => waterFogClampAboveDist = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033039), sinceVersion: 2)]
    public CMwNod? ItemPlacementGroups { get => itemPlacementGroups; set => itemPlacementGroups = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033039), sinceVersion: 3)]
    public CMwNod? AdnRandomGenList { get => adnRandomGenList; set => adnRandomGenList = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03033039), sinceVersion: 4)]
    public CMwNod? FidBlockInfoGroups { get => fidBlockInfoGroups; set => fidBlockInfoGroups = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0303303A))]
    public float? VisMeshLodDistScale { get => visMeshLodDistScale; set => visMeshLodDistScale = value; }
    
    [NodeMember(ExactName = "DecalFade_cBlock_FullDensity")]
    [AppliedWithChunk(typeof(Chunk03033033), sinceVersion: 1)]
    public int DecalFadeCBlockFullDensity { get => decalFadeCBlockFullDensity; set => decalFadeCBlockFullDensity = value; }

    [NodeMember(ExactName = "TurboColor_Roulette1")]
    [AppliedWithChunk(typeof(Chunk0303303B))]
    public uint? TurboColorRoulette1 { get => turboColorRoulette1; set => turboColorRoulette1 = value; }

    [NodeMember(ExactName = "TurboColor_Roulette2")]
    [AppliedWithChunk(typeof(Chunk0303303B))]
    public uint? TurboColorRoulette2 { get => turboColorRoulette2; set => turboColorRoulette2 = value; }

    [NodeMember(ExactName = "TurboColor_Roulette3")]
    [AppliedWithChunk(typeof(Chunk0303303B))]
    public uint? TurboColorRoulette3 { get => turboColorRoulette3; set => turboColorRoulette3 = value; }

    [NodeMember(ExactName = "TurboColor_Turbo")]
    [AppliedWithChunk(typeof(Chunk0303303B), sinceVersion: 1)]
    public uint? TurboColorTurbo { get => turboColorTurbo; set => turboColorTurbo = value; }

    [NodeMember(ExactName = "TurboColor_Turbo2")]
    [AppliedWithChunk(typeof(Chunk0303303B), sinceVersion: 1)]
    public uint? TurboColorTurbo2 { get => turboColorTurbo2; set => turboColorTurbo2 = value; }

    [NodeMember(ExactName = "BitmapDisplayControlDefaultTVProgram_16x9")]
    [AppliedWithChunk(typeof(Chunk0303303D))]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram16x9 { get => bitmapDisplayControlDefaultTVProgram16x9; set => bitmapDisplayControlDefaultTVProgram16x9 = value; }

    [NodeMember(ExactName = "BitmapDisplayControlDefaultTVProgram_64x10A")]
    [AppliedWithChunk(typeof(Chunk0303303D))]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10A { get => bitmapDisplayControlDefaultTVProgram64x10A; set => bitmapDisplayControlDefaultTVProgram64x10A = value; }

    [NodeMember(ExactName = "BitmapDisplayControlDefaultTVProgram_64x10B")]
    [AppliedWithChunk(typeof(Chunk0303303D))]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10B { get => bitmapDisplayControlDefaultTVProgram64x10B; set => bitmapDisplayControlDefaultTVProgram64x10B = value; }

    [NodeMember(ExactName = "BitmapDisplayControlDefaultTVProgram_64x10C")]
    [AppliedWithChunk(typeof(Chunk0303303D))]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10C { get => bitmapDisplayControlDefaultTVProgram64x10C; set => bitmapDisplayControlDefaultTVProgram64x10C = value; }

    [NodeMember(ExactName = "BitmapDisplayControlDefaultTVProgram_2x3")]
    [AppliedWithChunk(typeof(Chunk0303303D))]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram2x3 { get => bitmapDisplayControlDefaultTVProgram2x3; set => bitmapDisplayControlDefaultTVProgram2x3 = value; }

    #endregion

    #region Constructors

    protected CGameCtnCollection()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 header chunk (old header)

    [Chunk(0x03033000, "old header")]
    public class Chunk03033000 : HeaderChunk<CGameCtnCollection>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x001 header chunk (desc)

    [Chunk(0x03033001, "desc")]
    public class Chunk03033001H : HeaderChunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public Vec2? U01;
        public Vec2? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);
            rw.Id(ref n.collection);
            rw.Boolean(ref n.needUnlock);

            if (version >= 1)
            {
                rw.String(ref n.iconEnv);
                rw.String(ref n.iconCollection);

                if (version >= 2)
                {
                    rw.Int32(ref n.sortIndex);

                    if (version >= 3)
                    {
                        rw.Id(ref n.defaultZone);

                        if (version >= 4)
                        {
                            rw.Ident(ref n.vehicle);

                            if (version >= 5)
                            {
                                rw.String(ref n.mapFid);
                                rw.Vec2(ref U01);
                                rw.Vec2(ref U02);

                                if (version < 8)
                                {
                                    rw.Vec2(ref n.mapCoordElem);

                                    if (version >= 6)
                                    {
                                        rw.Vec2(ref n.mapCoordIcon);
                                    }
                                }

                                if (version >= 7)
                                {
                                    rw.String(ref n.loadScreen);

                                    if (version >= 8)
                                    {
                                        rw.Vec2(ref n.mapCoordElem);
                                        rw.Vec2(ref n.mapCoordIcon);
                                        rw.Vec2(ref n.mapCoordDesc);
                                        rw.String(ref n.longDesc);

                                        if (version >= 9)
                                        {
                                            rw.String(ref n.displayName);

                                            if (version >= 10)
                                            {
                                                rw.Boolean(ref n.isEditable);
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

    #endregion

    #region 0x008 chunk (DefaultDecoration)

    /// <summary>
    /// CGameCtnCollection 0x008 chunk (DefaultDecoration)
    /// </summary>
    [Chunk(0x03033008, "DefaultDecoration")]
    public class Chunk03033008 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecoration>(ref n.defaultDecoration, ref n.defaultDecorationFile);
        }
    }

    #endregion

    #region 0x009 chunk

    [Chunk(0x03033009)]
    public class Chunk03033009 : Chunk<CGameCtnCollection>
    {
        private int listVersion = 10;

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.collection);
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnZone>(ref n.completeZoneList);
            rw.Int32(ref U01);
            rw.Boolean(ref n.needUnlock);
            rw.Single(ref n.squareSize);
            rw.Single(ref n.squareHeight);
            rw.Ident(ref n.vehicle);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CGameCtnCollection 0x00B chunk
    /// </summary>
    [Chunk(0x0303300B)]
    public class Chunk0303300B : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00C chunk

    [Chunk(0x0303300C)]
    public class Chunk0303300C : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isEditable);
            rw.Int32(ref U01); // This one just disappeared later in the code xd
        }
    }

    #endregion

    #region 0x00D chunk

    [Chunk(0x0303300D)]
    public class Chunk0303300D : Chunk<CGameCtnCollection>
    {
        public int U02;
        public bool U03;
        public int U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.hasIconFid);

            if (n.hasIconFid)
            {
                rw.NodeRef<CPlugBitmap>(ref n.iconFid, ref n.iconFidFile);
            }

            rw.Boolean(ref n.hasCollectionIconFid);

            if (n.hasCollectionIconFid)
            {
                rw.NodeRef<CPlugBitmap>(ref n.collectionIconFid, ref n.collectionIconFidFile);
            }
        }
    }

    #endregion

    #region 0x00E chunk

    [Chunk(0x0303300E)]
    public class Chunk0303300E : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x011 chunk

    [Chunk(0x03033011)]
    public class Chunk03033011 : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // Base.TMDecoration.Gbx?
        }
    }

    #endregion

    #region 0x013 chunk

    /// <summary>
    /// CGameCtnCollection 0x013 chunk
    /// </summary>
    [Chunk(0x03033013)]
    public class Chunk03033013 : Chunk<CGameCtnCollection>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref n.cameraMinHeight);
        }
    }

    #endregion

    #region 0x016 chunk

    /// <summary>
    /// CGameCtnCollection 0x016 chunk
    /// </summary>
    [Chunk(0x03033016)]
    public class Chunk03033016 : Chunk<CGameCtnCollection>
    {
        public int U01;
        public bool U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow); // Boolean in the code, may bring wrong values
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting); // Boolean in the code, may bring wrong values
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    #endregion

    #region 0x018 chunk

    /// <summary>
    /// CGameCtnCollection 0x018 chunk
    /// </summary>
    [Chunk(0x03033018)]
    public class Chunk03033018 : Chunk<CGameCtnCollection>
    {
        public int U01;
        public Rect U02;
        public Vec2 U03;
        public Vec2 U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Rect(ref U02);
            rw.Vec2(ref U03);
            rw.Vec2(ref U04);
        }
    }

    #endregion

    #region 0x019 chunk (LoadScreenFid)

    /// <summary>
    /// CGameCtnCollection 0x019 chunk (LoadScreenFid)
    /// </summary>
    [Chunk(0x03033019, "LoadScreenFid")]
    public class Chunk03033019 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.loadScreenFid, ref n.loadScreenFidFile);
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CGameCtnCollection 0x01A chunk
    /// </summary>
    [Chunk(0x0303301A)]
    public class Chunk0303301A : Chunk<CGameCtnCollection>
    {
        public CMwNod? U01;
        public Rect U02;
        public Vec2 U03;
        public Vec2 U04;
        public Vec2 U05;
        public string? U06;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
            rw.Rect(ref U02);
            rw.Vec2(ref U03);
            rw.Vec2(ref U04);
            rw.Vec2(ref U05);
            rw.String(ref U06);
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CGameCtnCollection 0x01B chunk
    /// </summary>
    [Chunk(0x0303301B)]
    public class Chunk0303301B : Chunk<CGameCtnCollection>
    {
        public int U01;
        public bool U02;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow); // Boolean in the code, may bring wrong values
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(n.colorVertexMin);
            rw.Single(n.colorVertexMax);
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CGameCtnCollection 0x01D chunk
    /// </summary>
    [Chunk(0x0303301D)]
    public class Chunk0303301D : Chunk<CGameCtnCollection>
    {
        private int listVersion = 10;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.zoneStrings, r => new(r.ReadId(), r.ReadId()), 
               (x, w) =>
               {
                   w.WriteId(x.Base);
                   w.WriteId(x.Replacement);
               });
            
            rw.Int32(ref listVersion);
            rw.ArrayNode(ref n.replacementTerrainModifiers);
        }
    }

    #endregion

    #region 0x01E chunk (camera)

    /// <summary>
    /// CGameCtnCollection 0x01E chunk (camera)
    /// </summary>
    [Chunk(0x0303301E, "camera")]
    public class Chunk0303301E : Chunk<CGameCtnCollection>
    {
        public float U01;
        public float U02;
        public bool U03;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref n.cameraMinHeight);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CGameCtnCollection 0x01F chunk
    /// </summary>
    [Chunk(0x0303301F)]
    public class Chunk0303301F : Chunk<CGameCtnCollection>
    {
        private int[]? U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Array<int>(ref U01); // ParticleEmitterModelsFids?
        }
    }

    #endregion

    #region 0x020 chunk (folders)

    /// <summary>
    /// CGameCtnCollection 0x020 chunk (folders)
    /// </summary>
    [Chunk(0x03033020, "folders")]
    public class Chunk03033020 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.folderBlockInfo);
            rw.String(ref n.folderItem);
            rw.String(ref n.folderDecoration);
            rw.String(ref n.folderMenusIcons);
        }
    }

    #endregion

    #region 0x021 chunk (display name)

    /// <summary>
    /// CGameCtnCollection 0x021 chunk (display name)
    /// </summary>
    [Chunk(0x03033021, "display name")]
    public class Chunk03033021 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.displayName);
        }
    }

    #endregion

    #region 0x022 chunk (is water multi-height)

    /// <summary>
    /// CGameCtnCollection 0x022 chunk (is water multi-height)
    /// </summary>
    [Chunk(0x03033022, "is water multi-height")]
    public class Chunk03033022 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isWaterMultiHeight);
        }
    }

    #endregion

    #region 0x023 chunk

    /// <summary>
    /// CGameCtnCollection 0x023 chunk
    /// </summary>
    [Chunk(0x03033023)]
    public class Chunk03033023 : Chunk<CGameCtnCollection>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion

    #region 0x024 chunk

    /// <summary>
    /// CGameCtnCollection 0x024 chunk
    /// </summary>
    [Chunk(0x03033024)]
    public class Chunk03033024 : Chunk<CGameCtnCollection>
    {
        public int U01;
        public bool U02;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Boolean(ref U02);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    #endregion

    #region 0x027 chunk (board square)

    /// <summary>
    /// CGameCtnCollection 0x027 chunk (board square)
    /// </summary>
    [Chunk(0x03033027, "board square")]
    public class Chunk03033027 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.boardSquareHeight);
            rw.Single(ref n.boardSquareBorder);
        }
    }

    #endregion

    #region 0x028 chunk (FolderAdditionalItem1)

    /// <summary>
    /// CGameCtnCollection 0x028 chunk (FolderAdditionalItem1)
    /// </summary>
    [Chunk(0x03033028, "FolderAdditionalItem1")]
    public class Chunk03033028 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.folderAdditionalItem1);
        }
    }

    #endregion

    #region 0x029 chunk (FolderAdditionalItem2)

    /// <summary>
    /// CGameCtnCollection 0x029 chunk (FolderAdditionalItem2)
    /// </summary>
    [Chunk(0x03033029, "FolderAdditionalItem2")]
    public class Chunk03033029 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.folderAdditionalItem2);
        }
    }

    #endregion

    #region 0x02A chunk (FolderDecalModels)

    /// <summary>
    /// CGameCtnCollection 0x02A chunk (FolderDecalModels)
    /// </summary>
    [Chunk(0x0303302A, "FolderDecalModels")]
    public class Chunk0303302A : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.folderDecalModels);
        }
    }

    #endregion

    #region 0x02C chunk

    /// <summary>
    /// CGameCtnCollection 0x02C chunk
    /// </summary>
    [Chunk(0x0303302C)]
    public class Chunk0303302C : Chunk<CGameCtnCollection>
    {
        private BigInteger U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int128(ref U01);
        }
    }

    #endregion

    #region 0x02F chunk (Tech3TunnelSpecularExpScaleMax)

    /// <summary>
    /// CGameCtnCollection 0x02F chunk (Tech3TunnelSpecularExpScaleMax)
    /// </summary>
    [Chunk(0x0303302F, "Tech3TunnelSpecularExpScaleMax")]
    public class Chunk0303302F : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.tech3TunnelSpecularExpScaleMax);
        }
    }

    #endregion

    #region 0x030 chunk

    /// <summary>
    /// CGameCtnCollection 0x030 chunk
    /// </summary>
    [Chunk(0x03033030)]
    public class Chunk03033030 : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // MarksModel?
        }
    }

    #endregion

    #region 0x031 chunk (FolderMacroDecals)

    /// <summary>
    /// CGameCtnCollection 0x031 chunk (FolderMacroDecals)
    /// </summary>
    [Chunk(0x03033031, "FolderMacroDecals")]
    public class Chunk03033031 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.folderMacroDecals);
        }
    }

    #endregion

    #region 0x033 chunk

    /// <summary>
    /// CGameCtnCollection 0x033 chunk
    /// </summary>
    [Chunk(0x03033033)]
    public class Chunk03033033 : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public string[]? U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ArrayId(ref U01);

            if (version >= 1)
            {
                rw.Int32(ref n.decalFadeCBlockFullDensity);
            }
        }
    }

    #endregion

    #region 0x034 chunk

    /// <summary>
    /// CGameCtnCollection 0x034 chunk
    /// </summary>
    [Chunk(0x03033034)]
    public class Chunk03033034 : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CFuncShaderLayerUV>(ref n.fidFuncShaderCloudsX2);
            rw.NodeRef<CPlugBitmap>(ref n.fidPlugBitmapCloudsX2);

            if (version >= 1)
            {
                rw.NodeRef<CPlugBitmap>(ref n.vehicleEnvLayerFidBitmap);
                rw.EnumInt32<EVehicleEnvLayer>(ref n.vehicleEnvLayer);
            }
        }
    }

    #endregion

    #region 0x036 chunk (OffZone_FogMatter)

    /// <summary>
    /// CGameCtnCollection 0x036 chunk (OffZone_FogMatter)
    /// </summary>
    [Chunk(0x03033036, "OffZone_FogMatter")]
    public class Chunk03033036 : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugFogMatter>(ref n.offZoneFogMatter);
        }
    }

    #endregion

    #region 0x037 chunk (TerrainHeightOffset)

    /// <summary>
    /// CGameCtnCollection 0x037 chunk (TerrainHeightOffset)
    /// </summary>
    [Chunk(0x03033037, "TerrainHeightOffset")]
    public class Chunk03033037 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.terrainHeightOffset);
        }
    }

    #endregion

    #region 0x038 chunk

    /// <summary>
    /// CGameCtnCollection 0x038 chunk
    /// </summary>
    [Chunk(0x03033038)]
    public class Chunk03033038 : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public float? U01;
        public float? U02;
        public UnknownClass1[]? U03;
        public bool? U04;
        public float? U05;
        public int? U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 4)
            {
                rw.Single(ref U01);

                if (version == 0)
                {
                    rw.Single(ref U01);
                    rw.Single(ref U02);
                }
                
                if (version >= 1)
                {
                    if (rw.Reader is GameBoxReader r)
                    {
                        U03 = r.ReadArray(2, r =>
                        {
                            var u01 = r.ReadId();
                            var u02 = r.ReadSingle();
                            var u03 = r.ReadSingle();
                            var u04 = default(float?);
                            var u05 = default(int?);

                            if (version >= 3)
                            {
                                u04 = r.ReadSingle();
                            }

                            if (version >= 2)
                            {
                                u05 = r.ReadInt32(); // NodeRef?
                            }

                            return new UnknownClass1
                            {
                                U01 = u01,
                                U02 = u02,
                                U03 = u03,
                                U04 = u04,
                                U05 = u05
                            };
                        });
                    }

                    if (rw.Writer is GameBoxWriter w)
                    {
                        if (U03?.Length != 2)
                        {
                            throw new Exception("U03 is missing or U03.Length != 2");
                        }

                        foreach (var u in U03)
                        {
                            w.WriteId(u.U01);
                            w.Write(u.U02);
                            w.Write(u.U03);

                            if (version >= 3)
                            {
                                w.Write(u.U04.GetValueOrDefault());
                            }

                            if (version >= 2)
                            {
                                w.Write(u.U05.GetValueOrDefault()); // NodeRef?
                            }
                        }
                    }
                }
            }
            
            if (version >= 4)
            {
                if (rw.Reader is GameBoxReader r)
                {
                    U03 = r.ReadArray(4, r =>
                    {
                        var u01 = r.ReadId();
                        var u02 = r.ReadSingle();
                        var u03 = r.ReadSingle();
                        var u04 = r.ReadSingle();
                        var u05 = r.ReadInt32(); // NodeRef?
                        var u06 = default(int?);

                        if (version >= 7) // Not seen in code
                        {
                            u06 = r.ReadInt32();
                        }

                        return new UnknownClass1
                        {
                            U01 = u01,
                            U02 = u02,
                            U03 = u03,
                            U04 = u04,
                            U05 = u05,
                            U06 = u06
                        };
                    });
                }

                if (rw.Writer is GameBoxWriter w)
                {
                    if (U03?.Length != 4)
                    {
                        throw new Exception("U03 is missing or U03.Length != 4");
                    }

                    foreach (var u in U03)
                    {
                        w.WriteId(u.U01);
                        w.Write(u.U02);
                        w.Write(u.U03);
                        w.Write(u.U04.GetValueOrDefault());
                        w.Write(u.U05.GetValueOrDefault()); // NodeRef?

                        if (version >= 7) // Not seen in code
                        {
                            w.Write(u.U06.GetValueOrDefault());
                        }
                    }
                }
            }

            if (version >= 5)
            {
                rw.NodeRef<CPlugBitmap>(ref n.waterGBitmapNormal);
                rw.Single(ref n.waterGBumpSpeedUV);
                rw.Single(ref n.waterGBumpScaleUV);
                rw.Single(ref n.waterGBumpScale);
                rw.Single(ref n.waterGRefracPertub);
            }

            if (version >= 7) // Not seen in code
            {
                rw.Int32(ref U06);
            }

            rw.Single(ref n.cameraMinHeight);
            
            if (version < 4)
            {
                rw.Boolean(ref U04); // CSystemFidsFolder something?
            }

            rw.Boolean(ref n.isWaterMultiHeight);
            
            if (version < 3)
            {
                rw.Single(ref U05);
            }

            rw.Single(ref n.waterFogClampAboveDist);
        }
    }

    #endregion

    #region 0x039 chunk

    /// <summary>
    /// CGameCtnCollection 0x039 chunk
    /// </summary>
    [Chunk(0x03033039)]
    public class Chunk03033039 : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public float? U01;
        public float? U02;
        public float? U03;
        public int? U04;
        public CMwNod? U05;
        public CMwNod? U06;
        public CMwNod? U07;
        public CMwNod? U08;
        public CMwNod? U09;
        public CMwNod? U10;
        public CMwNod? U11;
        public CMwNod? U12;
        public CMwNod? U13;
        public CMwNod? U14;
        public CMwNod? U15;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version == 0)
            {
                rw.Single(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
            }

            if (version >= 1)
            {
                rw.Int32(ref U04); // VehicleStyles?

                if (version >= 2)
                {
                    rw.NodeRef(ref n.itemPlacementGroups);

                    if (version >= 3)
                    {
                        rw.NodeRef(ref n.adnRandomGenList);

                        if (version >= 4)
                        {
                            rw.NodeRef(ref n.fidBlockInfoGroups);

                            if (version < 11)
                            {
                                rw.NodeRef(ref U05);

                                if (version >= 8)
                                {
                                    rw.NodeRef(ref U06);
                                }
                            }

                            if (version >= 6)
                            {
                                rw.NodeRef(ref U07);
                            }

                            if (version < 9)
                            {
                                rw.NodeRef(ref U08);
                            }

                            if (version >= 10)
                            {
                                rw.NodeRef(ref U09);
                                
                                if (version >= 12)
                                {
                                    rw.NodeRef(ref U10);
                                }

                                if (version >= 11)
                                {
                                    rw.NodeRef(ref U11);
                                    rw.NodeRef(ref U12);
                                    rw.NodeRef(ref U13);
                                    rw.NodeRef(ref U14);
                                    rw.NodeRef(ref U15);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x03A chunk

    /// <summary>
    /// CGameCtnCollection 0x03A chunk
    /// </summary>
    [Chunk(0x0303303A)]
    public class Chunk0303303A : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public int U01;
        public bool U02;
        public int U03;
        public int U04;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Boolean(ref U02);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
            rw.Int32(ref U03);
            rw.Single(ref n.visMeshLodDistScale);

            if (version == 1)
            {
                rw.Int32(ref U04); // NodeRef?
            }
        }
    }

    #endregion

    #region 0x03B chunk (turbo color)

    /// <summary>
    /// CGameCtnCollection 0x03B chunk (turbo color)
    /// </summary>
    [Chunk(0x0303303B, "turbo color")]
    public class Chunk0303303B : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.UInt32(ref n.turboColorRoulette1);
            rw.UInt32(ref n.turboColorRoulette2);
            rw.UInt32(ref n.turboColorRoulette3);

            if (version >= 1)
            {
                rw.UInt32(ref n.turboColorTurbo);
                rw.UInt32(ref n.turboColorTurbo2);
            }
        }
    }

    #endregion

    #region 0x03C chunk

    /// <summary>
    /// CGameCtnCollection 0x03C chunk
    /// </summary>
    [Chunk(0x0303303C)]
    public class Chunk0303303C : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version = 1;

        public string? U01;
        public int? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref U01);
            
            if (version >= 1)
            {
                rw.Int32(ref U02);
            }
        }
    }

    #endregion

    #region 0x03D chunk (BitmapDisplayControlDefaultTVProgram)

    /// <summary>
    /// CGameCtnCollection 0x03D chunk (BitmapDisplayControlDefaultTVProgram)
    /// </summary>
    [Chunk(0x0303303D, "BitmapDisplayControlDefaultTVProgram")]
    public class Chunk0303303D : Chunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram16x9);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10A);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10B);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10C);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram2x3);
        }
    }

    #endregion

    #endregion

    public class UnknownClass1
    {
        public Id U01 { get; set; }
        public float U02 { get; set; }
        public float U03 { get; set; }
        public float? U04 { get; set; }
        public int? U05 { get; set; }
        public int? U06 { get; internal set; }
    }
}
