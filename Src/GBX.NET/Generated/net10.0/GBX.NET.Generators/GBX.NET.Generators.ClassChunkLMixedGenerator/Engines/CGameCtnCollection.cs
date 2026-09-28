namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03033000</remarks>
[Class(0x03033000)]
public partial class CGameCtnCollection : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03033000;




    private bool needUnlock;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033009>]
    public bool NeedUnlock { get => needUnlock; set => needUnlock = value; }

    private string? iconFullName;
    [AppliedWithChunk<HeaderChunk03033001>]
    public string? IconFullName { get => iconFullName; set => iconFullName = value; }

    private string? iconSmallFullName;
    [AppliedWithChunk<HeaderChunk03033001>]
    public string? IconSmallFullName { get => iconSmallFullName; set => iconSmallFullName = value; }

    private int sortIndex;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk0303300E>]
    public int SortIndex { get => sortIndex; set => sortIndex = value; }

    private string? defaultZoneId;
    [AppliedWithChunk<HeaderChunk03033001>]
    public string? DefaultZoneId { get => defaultZoneId; set => defaultZoneId = value; }

    private Ident? vehicle;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033009>]
    public Ident? Vehicle { get => vehicle; set => vehicle = value; }

    private string? mapFullName;
    [AppliedWithChunk<HeaderChunk03033001>]
    public string? MapFullName { get => mapFullName; set => mapFullName = value; }

    private Rect mapRect;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033018>]
    [AppliedWithChunk<Chunk0303301A>]
    public Rect MapRect { get => mapRect; set => mapRect = value; }

    private Vec2 mapCoordElem;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033018>]
    [AppliedWithChunk<Chunk0303301A>]
    public Vec2 MapCoordElem { get => mapCoordElem; set => mapCoordElem = value; }

    private Vec2 mapCoordIcon;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033018>]
    [AppliedWithChunk<Chunk0303301A>]
    public Vec2 MapCoordIcon { get => mapCoordIcon; set => mapCoordIcon = value; }

    private string? loadScreen;
    [AppliedWithChunk<HeaderChunk03033001>]
    public string? LoadScreen { get => loadScreen; set => loadScreen = value; }

    private Vec2 mapCoordDesc;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk0303301A>]
    public Vec2 MapCoordDesc { get => mapCoordDesc; set => mapCoordDesc = value; }

    private string? longDesc;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk0303301A>]
    public string? LongDesc { get => longDesc; set => longDesc = value; }

    private string? displayName;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk03033021>]
    public string? DisplayName { get => displayName; set => displayName = value; }

    private bool isEditable;
    [AppliedWithChunk<HeaderChunk03033001>]
    [AppliedWithChunk<Chunk0303300C>]
    public bool IsEditable { get => isEditable; set => isEditable = value; }

    private string? folderBlockInfo;
    [AppliedWithChunk<HeaderChunk03033002>]
    [AppliedWithChunk<Chunk03033020>]
    public string? FolderBlockInfo { get => folderBlockInfo; set => folderBlockInfo = value; }

    private string? folderItem;
    [AppliedWithChunk<HeaderChunk03033002>]
    [AppliedWithChunk<Chunk03033020>]
    public string? FolderItem { get => folderItem; set => folderItem = value; }

    private string? folderDecoration;
    [AppliedWithChunk<HeaderChunk03033002>]
    [AppliedWithChunk<Chunk03033020>]
    public string? FolderDecoration { get => folderDecoration; set => folderDecoration = value; }

    private string? folderCardEventInfo;
    [AppliedWithChunk<HeaderChunk03033002>]
    public string? FolderCardEventInfo { get => folderCardEventInfo; set => folderCardEventInfo = value; }

    private string? folderMacroBlockInfo;
    [AppliedWithChunk<HeaderChunk03033002>]
    public string? FolderMacroBlockInfo { get => folderMacroBlockInfo; set => folderMacroBlockInfo = value; }

    private string? folderMacroDecals;
    [AppliedWithChunk<HeaderChunk03033002>]
    [AppliedWithChunk<Chunk03033031>]
    public string? FolderMacroDecals { get => folderMacroDecals; set => folderMacroDecals = value; }

    private string? folderMenusIcons;
    [AppliedWithChunk<HeaderChunk03033003>]
    [AppliedWithChunk<Chunk03033020>]
    public string? FolderMenusIcons { get => folderMenusIcons; set => folderMenusIcons = value; }

    private CGameCtnDecoration? defaultDecoration;
    [AppliedWithChunk<Chunk03033008>]
    [AppliedWithChunk<Chunk03033011>]
    public CGameCtnDecoration? DefaultDecoration { get => defaultDecorationFile?.GetNode(ref defaultDecoration) ?? defaultDecoration; set => defaultDecoration = value; }
    private Components.GbxRefTableFile? defaultDecorationFile;
    public Components.GbxRefTableFile? DefaultDecorationFile { get => defaultDecorationFile; set => defaultDecorationFile = value; }
    public CGameCtnDecoration? GetDefaultDecoration(GbxReadSettings settings = default, bool exceptions = false) => defaultDecorationFile?.GetNode(ref defaultDecoration, settings, exceptions) ?? defaultDecoration;

    private External<CGameCtnZone>[]? completeListZoneList;
    [AppliedWithChunk<Chunk03033009>]
    public External<CGameCtnZone>[]? CompleteListZoneList { get => completeListZoneList; set => completeListZoneList = value; }

    private CGameCtnZone? defaultZone;
    [AppliedWithChunk<Chunk03033009>]
    public CGameCtnZone? DefaultZone { get => defaultZoneFile?.GetNode(ref defaultZone) ?? defaultZone; set => defaultZone = value; }
    private Components.GbxRefTableFile? defaultZoneFile;
    public Components.GbxRefTableFile? DefaultZoneFile { get => defaultZoneFile; set => defaultZoneFile = value; }
    public CGameCtnZone? GetDefaultZone(GbxReadSettings settings = default, bool exceptions = false) => defaultZoneFile?.GetNode(ref defaultZone, settings, exceptions) ?? defaultZone;

    private float squareSize;
    [AppliedWithChunk<Chunk03033009>]
    public float SquareSize { get => squareSize; set => squareSize = value; }

    private float squareHeight;
    [AppliedWithChunk<Chunk03033009>]
    public float SquareHeight { get => squareHeight; set => squareHeight = value; }

    private int blocksShadow;
    [AppliedWithChunk<Chunk0303300B>]
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    public int BlocksShadow { get => blocksShadow; set => blocksShadow = value; }

    private ECollectionType collectionType;
    [AppliedWithChunk<Chunk0303300C>]
    public ECollectionType CollectionType { get => collectionType; set => collectionType = value; }

    private float waterTop;
    [AppliedWithChunk<Chunk03033013>]
    [AppliedWithChunk<Chunk0303301E>]
    public float WaterTop { get => waterTop; set => waterTop = value; }

    private float waterBottom;
    [AppliedWithChunk<Chunk03033013>]
    [AppliedWithChunk<Chunk0303301E>]
    public float WaterBottom { get => waterBottom; set => waterBottom = value; }

    private float cameraMinHeight;
    [AppliedWithChunk<Chunk03033013>]
    [AppliedWithChunk<Chunk0303301E>]
    [AppliedWithChunk<Chunk03033038>]
    public float CameraMinHeight { get => cameraMinHeight; set => cameraMinHeight = value; }

    private bool shadowCastBack;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    public bool ShadowCastBack { get => shadowCastBack; set => shadowCastBack = value; }

    private EBackgroundShadow backgroundShadow;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    [AppliedWithChunk<Chunk0303303A>]
    public EBackgroundShadow BackgroundShadow { get => backgroundShadow; set => backgroundShadow = value; }

    private float shadowSoftSizeInWorld;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    [AppliedWithChunk<Chunk0303303A>]
    public float ShadowSoftSizeInWorld { get => shadowSoftSizeInWorld; set => shadowSoftSizeInWorld = value; }

    private EVertexLighting vertexLighting;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    [AppliedWithChunk<Chunk0303303A>]
    public EVertexLighting VertexLighting { get => vertexLighting; set => vertexLighting = value; }

    private float colorVertexMin;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    [AppliedWithChunk<Chunk0303303A>]
    public float ColorVertexMin { get => colorVertexMin; set => colorVertexMin = value; }

    private float colorVertexMax;
    [AppliedWithChunk<Chunk03033016>]
    [AppliedWithChunk<Chunk0303301B>]
    [AppliedWithChunk<Chunk03033024>]
    [AppliedWithChunk<Chunk0303303A>]
    public float ColorVertexMax { get => colorVertexMax; set => colorVertexMax = value; }

    private CPlugBitmap? mapFid;
    [AppliedWithChunk<Chunk03033018>]
    [AppliedWithChunk<Chunk0303301A>]
    public CPlugBitmap? MapFid { get => mapFid; set => mapFid = value; }

    private CPlugBitmap? loadScreenFid;
    [AppliedWithChunk<Chunk03033019>]
    public CPlugBitmap? LoadScreenFid { get => loadScreenFidFile?.GetNode(ref loadScreenFid) ?? loadScreenFid; set => loadScreenFid = value; }
    private Components.GbxRefTableFile? loadScreenFidFile;
    public Components.GbxRefTableFile? LoadScreenFidFile { get => loadScreenFidFile; set => loadScreenFidFile = value; }
    public CPlugBitmap? GetLoadScreenFid(GbxReadSettings settings = default, bool exceptions = false) => loadScreenFidFile?.GetNode(ref loadScreenFid, settings, exceptions) ?? loadScreenFid;

    private ZoneString[]? zoneStrings;
    [AppliedWithChunk<Chunk0303301D>]
    public ZoneString[]? ZoneStrings { get => zoneStrings; set => zoneStrings = value; }

    private External<CGameCtnDecorationTerrainModifier>[]? replacementTerrainModifiers;
    [AppliedWithChunk<Chunk0303301D>]
    public External<CGameCtnDecorationTerrainModifier>[]? ReplacementTerrainModifiers { get => replacementTerrainModifiers; set => replacementTerrainModifiers = value; }

    private bool isWaterOutsidePlayField;
    [AppliedWithChunk<Chunk0303301E>]
    public bool IsWaterOutsidePlayField { get => isWaterOutsidePlayField; set => isWaterOutsidePlayField = value; }

    private int[]? particleEmitterModelsFids;
    [AppliedWithChunk<Chunk0303301F>]
    public int[]? ParticleEmitterModelsFids { get => particleEmitterModelsFids; set => particleEmitterModelsFids = value; }

    private bool isWaterMultiHeight;
    [AppliedWithChunk<Chunk03033022>]
    [AppliedWithChunk<Chunk03033038>]
    public bool IsWaterMultiHeight { get => isWaterMultiHeight; set => isWaterMultiHeight = value; }

    private bool? carCanBeDirty;
    [AppliedWithChunk<Chunk03033023>]
    public bool? CarCanBeDirty { get => carCanBeDirty; set => carCanBeDirty = value; }

    private float boardSquareHeight;
    [AppliedWithChunk<Chunk03033027>]
    public float BoardSquareHeight { get => boardSquareHeight; set => boardSquareHeight = value; }

    private float boardSquareBorder;
    [AppliedWithChunk<Chunk03033027>]
    public float BoardSquareBorder { get => boardSquareBorder; set => boardSquareBorder = value; }

    private string? folderAdditionalItem1;
    [AppliedWithChunk<Chunk03033028>]
    public string? FolderAdditionalItem1 { get => folderAdditionalItem1; set => folderAdditionalItem1 = value; }

    private string? folderAdditionalItem2;
    [AppliedWithChunk<Chunk03033029>]
    public string? FolderAdditionalItem2 { get => folderAdditionalItem2; set => folderAdditionalItem2 = value; }

    private string? folderDecalModels;
    [AppliedWithChunk<Chunk0303302A>]
    public string? FolderDecalModels { get => folderDecalModels; set => folderDecalModels = value; }

    private Vec3? tech3TunnelSpecularExpScaleMax;
    [AppliedWithChunk<Chunk0303302F>]
    public Vec3? Tech3TunnelSpecularExpScaleMax { get => tech3TunnelSpecularExpScaleMax; set => tech3TunnelSpecularExpScaleMax = value; }

    private int decalFadeCBlockFullDensity;
    [AppliedWithChunk<Chunk03033033>]
    public int DecalFadeCBlockFullDensity { get => decalFadeCBlockFullDensity; set => decalFadeCBlockFullDensity = value; }

    private CFuncShaderLayerUV? fidFuncShaderCloudsX2;
    [AppliedWithChunk<Chunk03033034>]
    public CFuncShaderLayerUV? FidFuncShaderCloudsX2 { get => fidFuncShaderCloudsX2File?.GetNode(ref fidFuncShaderCloudsX2) ?? fidFuncShaderCloudsX2; set => fidFuncShaderCloudsX2 = value; }
    private Components.GbxRefTableFile? fidFuncShaderCloudsX2File;
    public Components.GbxRefTableFile? FidFuncShaderCloudsX2File { get => fidFuncShaderCloudsX2File; set => fidFuncShaderCloudsX2File = value; }
    public CFuncShaderLayerUV? GetFidFuncShaderCloudsX2(GbxReadSettings settings = default, bool exceptions = false) => fidFuncShaderCloudsX2File?.GetNode(ref fidFuncShaderCloudsX2, settings, exceptions) ?? fidFuncShaderCloudsX2;

    private CPlugBitmap? fidPlugBitmapCloudsX2;
    [AppliedWithChunk<Chunk03033034>]
    public CPlugBitmap? FidPlugBitmapCloudsX2 { get => fidPlugBitmapCloudsX2File?.GetNode(ref fidPlugBitmapCloudsX2) ?? fidPlugBitmapCloudsX2; set => fidPlugBitmapCloudsX2 = value; }
    private Components.GbxRefTableFile? fidPlugBitmapCloudsX2File;
    public Components.GbxRefTableFile? FidPlugBitmapCloudsX2File { get => fidPlugBitmapCloudsX2File; set => fidPlugBitmapCloudsX2File = value; }
    public CPlugBitmap? GetFidPlugBitmapCloudsX2(GbxReadSettings settings = default, bool exceptions = false) => fidPlugBitmapCloudsX2File?.GetNode(ref fidPlugBitmapCloudsX2, settings, exceptions) ?? fidPlugBitmapCloudsX2;

    private CPlugBitmap? vehicleEnvLayerFidBitmap;
    [AppliedWithChunk<Chunk03033034>]
    public CPlugBitmap? VehicleEnvLayerFidBitmap { get => vehicleEnvLayerFidBitmapFile?.GetNode(ref vehicleEnvLayerFidBitmap) ?? vehicleEnvLayerFidBitmap; set => vehicleEnvLayerFidBitmap = value; }
    private Components.GbxRefTableFile? vehicleEnvLayerFidBitmapFile;
    public Components.GbxRefTableFile? VehicleEnvLayerFidBitmapFile { get => vehicleEnvLayerFidBitmapFile; set => vehicleEnvLayerFidBitmapFile = value; }
    public CPlugBitmap? GetVehicleEnvLayerFidBitmap(GbxReadSettings settings = default, bool exceptions = false) => vehicleEnvLayerFidBitmapFile?.GetNode(ref vehicleEnvLayerFidBitmap, settings, exceptions) ?? vehicleEnvLayerFidBitmap;

    private EVehicleEnvLayer vehicleEnvLayer;
    [AppliedWithChunk<Chunk03033034>]
    public EVehicleEnvLayer VehicleEnvLayer { get => vehicleEnvLayer; set => vehicleEnvLayer = value; }

    private CPlugFogMatter? offZoneFogMatter;
    [AppliedWithChunk<Chunk03033036>]
    public CPlugFogMatter? OffZoneFogMatter { get => offZoneFogMatterFile?.GetNode(ref offZoneFogMatter) ?? offZoneFogMatter; set => offZoneFogMatter = value; }
    private Components.GbxRefTableFile? offZoneFogMatterFile;
    public Components.GbxRefTableFile? OffZoneFogMatterFile { get => offZoneFogMatterFile; set => offZoneFogMatterFile = value; }
    public CPlugFogMatter? GetOffZoneFogMatter(GbxReadSettings settings = default, bool exceptions = false) => offZoneFogMatterFile?.GetNode(ref offZoneFogMatter, settings, exceptions) ?? offZoneFogMatter;

    private float terrainHeightOffset;
    [AppliedWithChunk<Chunk03033037>]
    public float TerrainHeightOffset { get => terrainHeightOffset; set => terrainHeightOffset = value; }

    private Water? water1;
    [AppliedWithChunk<Chunk03033038>]
    [AppliedWithChunk<Chunk03033038>]
    [AppliedWithChunk<Chunk03033038>]
    public Water? Water1 { get => water1; set => water1 = value; }

    private Water? water2;
    [AppliedWithChunk<Chunk03033038>]
    [AppliedWithChunk<Chunk03033038>]
    public Water? Water2 { get => water2; set => water2 = value; }

    private Water? water3;
    [AppliedWithChunk<Chunk03033038>]
    public Water? Water3 { get => water3; set => water3 = value; }

    private Water? water4;
    [AppliedWithChunk<Chunk03033038>]
    public Water? Water4 { get => water4; set => water4 = value; }

    private CPlugMaterialWaterArray? waterArray;
    [AppliedWithChunk<Chunk03033038>]
    public CPlugMaterialWaterArray? WaterArray { get => waterArray; set => waterArray = value; }

    private CPlugBitmap? waterGBitmapNormal;
    [AppliedWithChunk<Chunk03033038>]
    public CPlugBitmap? WaterGBitmapNormal { get => waterGBitmapNormalFile?.GetNode(ref waterGBitmapNormal) ?? waterGBitmapNormal; set => waterGBitmapNormal = value; }
    private Components.GbxRefTableFile? waterGBitmapNormalFile;
    public Components.GbxRefTableFile? WaterGBitmapNormalFile { get => waterGBitmapNormalFile; set => waterGBitmapNormalFile = value; }
    public CPlugBitmap? GetWaterGBitmapNormal(GbxReadSettings settings = default, bool exceptions = false) => waterGBitmapNormalFile?.GetNode(ref waterGBitmapNormal, settings, exceptions) ?? waterGBitmapNormal;

    private float waterGBumpSpeedUV;
    [AppliedWithChunk<Chunk03033038>]
    public float WaterGBumpSpeedUV { get => waterGBumpSpeedUV; set => waterGBumpSpeedUV = value; }

    private float waterGBumpScaleUV;
    [AppliedWithChunk<Chunk03033038>]
    public float WaterGBumpScaleUV { get => waterGBumpScaleUV; set => waterGBumpScaleUV = value; }

    private float waterGBumpScale;
    [AppliedWithChunk<Chunk03033038>]
    public float WaterGBumpScale { get => waterGBumpScale; set => waterGBumpScale = value; }

    private float waterGRefracPertub;
    [AppliedWithChunk<Chunk03033038>]
    public float WaterGRefracPertub { get => waterGRefracPertub; set => waterGRefracPertub = value; }

    private float waterFogClampAboveDist;
    [AppliedWithChunk<Chunk03033038>]
    public float WaterFogClampAboveDist { get => waterFogClampAboveDist; set => waterFogClampAboveDist = value; }

    private CMwNod? itemPlacementGroups;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? ItemPlacementGroups { get => itemPlacementGroupsFile?.GetNode(ref itemPlacementGroups) ?? itemPlacementGroups; set => itemPlacementGroups = value; }
    private Components.GbxRefTableFile? itemPlacementGroupsFile;
    public Components.GbxRefTableFile? ItemPlacementGroupsFile { get => itemPlacementGroupsFile; set => itemPlacementGroupsFile = value; }
    public CMwNod? GetItemPlacementGroups(GbxReadSettings settings = default, bool exceptions = false) => itemPlacementGroupsFile?.GetNode(ref itemPlacementGroups, settings, exceptions) ?? itemPlacementGroups;

    private CMwNod? adnRandomGenList;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? AdnRandomGenList { get => adnRandomGenList; set => adnRandomGenList = value; }

    private CMwNod? fidBlockInfoGroups;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? FidBlockInfoGroups { get => fidBlockInfoGroupsFile?.GetNode(ref fidBlockInfoGroups) ?? fidBlockInfoGroups; set => fidBlockInfoGroups = value; }
    private Components.GbxRefTableFile? fidBlockInfoGroupsFile;
    public Components.GbxRefTableFile? FidBlockInfoGroupsFile { get => fidBlockInfoGroupsFile; set => fidBlockInfoGroupsFile = value; }
    public CMwNod? GetFidBlockInfoGroups(GbxReadSettings settings = default, bool exceptions = false) => fidBlockInfoGroupsFile?.GetNode(ref fidBlockInfoGroups, settings, exceptions) ?? fidBlockInfoGroups;

    private CMwNod? fidBlockInfoInventory;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? FidBlockInfoInventory { get => fidBlockInfoInventoryFile?.GetNode(ref fidBlockInfoInventory) ?? fidBlockInfoInventory; set => fidBlockInfoInventory = value; }
    private Components.GbxRefTableFile? fidBlockInfoInventoryFile;
    public Components.GbxRefTableFile? FidBlockInfoInventoryFile { get => fidBlockInfoInventoryFile; set => fidBlockInfoInventoryFile = value; }
    public CMwNod? GetFidBlockInfoInventory(GbxReadSettings settings = default, bool exceptions = false) => fidBlockInfoInventoryFile?.GetNode(ref fidBlockInfoInventory, settings, exceptions) ?? fidBlockInfoInventory;

    private CMwNod? fidItemModelInventory;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? FidItemModelInventory { get => fidItemModelInventoryFile?.GetNode(ref fidItemModelInventory) ?? fidItemModelInventory; set => fidItemModelInventory = value; }
    private Components.GbxRefTableFile? fidItemModelInventoryFile;
    public Components.GbxRefTableFile? FidItemModelInventoryFile { get => fidItemModelInventoryFile; set => fidItemModelInventoryFile = value; }
    public CMwNod? GetFidItemModelInventory(GbxReadSettings settings = default, bool exceptions = false) => fidItemModelInventoryFile?.GetNode(ref fidItemModelInventory, settings, exceptions) ?? fidItemModelInventory;

    private CPlugFileImg? blockSkins_Default_FidAdvertisement1x1;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidAdvertisement1x1 { get => blockSkins_Default_FidAdvertisement1x1File?.GetNode(ref blockSkins_Default_FidAdvertisement1x1) ?? blockSkins_Default_FidAdvertisement1x1; set => blockSkins_Default_FidAdvertisement1x1 = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidAdvertisement1x1File;
    public Components.GbxRefTableFile? BlockSkins_Default_FidAdvertisement1x1File { get => blockSkins_Default_FidAdvertisement1x1File; set => blockSkins_Default_FidAdvertisement1x1File = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidAdvertisement1x1(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidAdvertisement1x1File?.GetNode(ref blockSkins_Default_FidAdvertisement1x1, settings, exceptions) ?? blockSkins_Default_FidAdvertisement1x1;

    private CPlugFileImg? blockSkins_Default_FidAdvertisement2x1;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidAdvertisement2x1 { get => blockSkins_Default_FidAdvertisement2x1File?.GetNode(ref blockSkins_Default_FidAdvertisement2x1) ?? blockSkins_Default_FidAdvertisement2x1; set => blockSkins_Default_FidAdvertisement2x1 = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidAdvertisement2x1File;
    public Components.GbxRefTableFile? BlockSkins_Default_FidAdvertisement2x1File { get => blockSkins_Default_FidAdvertisement2x1File; set => blockSkins_Default_FidAdvertisement2x1File = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidAdvertisement2x1(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidAdvertisement2x1File?.GetNode(ref blockSkins_Default_FidAdvertisement2x1, settings, exceptions) ?? blockSkins_Default_FidAdvertisement2x1;

    private CPlugFileImg? blockSkins_Default_FidAdvertisement2x3;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidAdvertisement2x3 { get => blockSkins_Default_FidAdvertisement2x3File?.GetNode(ref blockSkins_Default_FidAdvertisement2x3) ?? blockSkins_Default_FidAdvertisement2x3; set => blockSkins_Default_FidAdvertisement2x3 = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidAdvertisement2x3File;
    public Components.GbxRefTableFile? BlockSkins_Default_FidAdvertisement2x3File { get => blockSkins_Default_FidAdvertisement2x3File; set => blockSkins_Default_FidAdvertisement2x3File = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidAdvertisement2x3(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidAdvertisement2x3File?.GetNode(ref blockSkins_Default_FidAdvertisement2x3, settings, exceptions) ?? blockSkins_Default_FidAdvertisement2x3;

    private CPlugFileImg? blockSkins_Default_FidAdvertisement4x1;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidAdvertisement4x1 { get => blockSkins_Default_FidAdvertisement4x1File?.GetNode(ref blockSkins_Default_FidAdvertisement4x1) ?? blockSkins_Default_FidAdvertisement4x1; set => blockSkins_Default_FidAdvertisement4x1 = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidAdvertisement4x1File;
    public Components.GbxRefTableFile? BlockSkins_Default_FidAdvertisement4x1File { get => blockSkins_Default_FidAdvertisement4x1File; set => blockSkins_Default_FidAdvertisement4x1File = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidAdvertisement4x1(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidAdvertisement4x1File?.GetNode(ref blockSkins_Default_FidAdvertisement4x1, settings, exceptions) ?? blockSkins_Default_FidAdvertisement4x1;

    private CPlugFileImg? blockSkins_Default_FidItemFlag;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidItemFlag { get => blockSkins_Default_FidItemFlagFile?.GetNode(ref blockSkins_Default_FidItemFlag) ?? blockSkins_Default_FidItemFlag; set => blockSkins_Default_FidItemFlag = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidItemFlagFile;
    public Components.GbxRefTableFile? BlockSkins_Default_FidItemFlagFile { get => blockSkins_Default_FidItemFlagFile; set => blockSkins_Default_FidItemFlagFile = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidItemFlag(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidItemFlagFile?.GetNode(ref blockSkins_Default_FidItemFlag, settings, exceptions) ?? blockSkins_Default_FidItemFlag;

    private CPlugFileImg? blockSkins_Default_FidAdvertisement16x9;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugFileImg? BlockSkins_Default_FidAdvertisement16x9 { get => blockSkins_Default_FidAdvertisement16x9File?.GetNode(ref blockSkins_Default_FidAdvertisement16x9) ?? blockSkins_Default_FidAdvertisement16x9; set => blockSkins_Default_FidAdvertisement16x9 = value; }
    private Components.GbxRefTableFile? blockSkins_Default_FidAdvertisement16x9File;
    public Components.GbxRefTableFile? BlockSkins_Default_FidAdvertisement16x9File { get => blockSkins_Default_FidAdvertisement16x9File; set => blockSkins_Default_FidAdvertisement16x9File = value; }
    public CPlugFileImg? GetBlockSkins_Default_FidAdvertisement16x9(GbxReadSettings settings = default, bool exceptions = false) => blockSkins_Default_FidAdvertisement16x9File?.GetNode(ref blockSkins_Default_FidAdvertisement16x9, settings, exceptions) ?? blockSkins_Default_FidAdvertisement16x9;

    private CMwNod? fidMacroBlockInfoInventory;
    [AppliedWithChunk<Chunk03033039>]
    public CMwNod? FidMacroBlockInfoInventory { get => fidMacroBlockInfoInventoryFile?.GetNode(ref fidMacroBlockInfoInventory) ?? fidMacroBlockInfoInventory; set => fidMacroBlockInfoInventory = value; }
    private Components.GbxRefTableFile? fidMacroBlockInfoInventoryFile;
    public Components.GbxRefTableFile? FidMacroBlockInfoInventoryFile { get => fidMacroBlockInfoInventoryFile; set => fidMacroBlockInfoInventoryFile = value; }
    public CMwNod? GetFidMacroBlockInfoInventory(GbxReadSettings settings = default, bool exceptions = false) => fidMacroBlockInfoInventoryFile?.GetNode(ref fidMacroBlockInfoInventory, settings, exceptions) ?? fidMacroBlockInfoInventory;

    private CPlugMediaClipList? defaultSpawnClipList;
    [AppliedWithChunk<Chunk03033039>]
    public CPlugMediaClipList? DefaultSpawnClipList { get => defaultSpawnClipListFile?.GetNode(ref defaultSpawnClipList) ?? defaultSpawnClipList; set => defaultSpawnClipList = value; }
    private Components.GbxRefTableFile? defaultSpawnClipListFile;
    public Components.GbxRefTableFile? DefaultSpawnClipListFile { get => defaultSpawnClipListFile; set => defaultSpawnClipListFile = value; }
    public CPlugMediaClipList? GetDefaultSpawnClipList(GbxReadSettings settings = default, bool exceptions = false) => defaultSpawnClipListFile?.GetNode(ref defaultSpawnClipList, settings, exceptions) ?? defaultSpawnClipList;

    private Ident? vehicleTransform_CarSnow;
    [AppliedWithChunk<Chunk03033039>]
    public Ident? VehicleTransform_CarSnow { get => vehicleTransform_CarSnow; set => vehicleTransform_CarSnow = value; }

    private Ident? vehicleTransform_CarRally;
    [AppliedWithChunk<Chunk03033039>]
    public Ident? VehicleTransform_CarRally { get => vehicleTransform_CarRally; set => vehicleTransform_CarRally = value; }

    private Ident? vehicleTransform_CarDesert;
    [AppliedWithChunk<Chunk03033039>]
    public Ident? VehicleTransform_CarDesert { get => vehicleTransform_CarDesert; set => vehicleTransform_CarDesert = value; }

    private float? visMeshLodDistScale;
    [AppliedWithChunk<Chunk0303303A>]
    public float? VisMeshLodDistScale { get => visMeshLodDistScale; set => visMeshLodDistScale = value; }

    private uint? turboColorRoulette1;
    [AppliedWithChunk<Chunk0303303B>]
    public uint? TurboColorRoulette1 { get => turboColorRoulette1; set => turboColorRoulette1 = value; }

    private uint? turboColorRoulette2;
    [AppliedWithChunk<Chunk0303303B>]
    public uint? TurboColorRoulette2 { get => turboColorRoulette2; set => turboColorRoulette2 = value; }

    private uint? turboColorRoulette3;
    [AppliedWithChunk<Chunk0303303B>]
    public uint? TurboColorRoulette3 { get => turboColorRoulette3; set => turboColorRoulette3 = value; }

    private uint? turboColorTurbo;
    [AppliedWithChunk<Chunk0303303B>]
    public uint? TurboColorTurbo { get => turboColorTurbo; set => turboColorTurbo = value; }

    private uint? turboColorTurbo2;
    [AppliedWithChunk<Chunk0303303B>]
    public uint? TurboColorTurbo2 { get => turboColorTurbo2; set => turboColorTurbo2 = value; }

    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram16x9;
    [AppliedWithChunk<Chunk0303303D>]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram16x9 { get => bitmapDisplayControlDefaultTVProgram16x9File?.GetNode(ref bitmapDisplayControlDefaultTVProgram16x9) ?? bitmapDisplayControlDefaultTVProgram16x9; set => bitmapDisplayControlDefaultTVProgram16x9 = value; }
    private Components.GbxRefTableFile? bitmapDisplayControlDefaultTVProgram16x9File;
    public Components.GbxRefTableFile? BitmapDisplayControlDefaultTVProgram16x9File { get => bitmapDisplayControlDefaultTVProgram16x9File; set => bitmapDisplayControlDefaultTVProgram16x9File = value; }
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram16x9(GbxReadSettings settings = default, bool exceptions = false) => bitmapDisplayControlDefaultTVProgram16x9File?.GetNode(ref bitmapDisplayControlDefaultTVProgram16x9, settings, exceptions) ?? bitmapDisplayControlDefaultTVProgram16x9;

    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10A;
    [AppliedWithChunk<Chunk0303303D>]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10A { get => bitmapDisplayControlDefaultTVProgram64x10AFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10A) ?? bitmapDisplayControlDefaultTVProgram64x10A; set => bitmapDisplayControlDefaultTVProgram64x10A = value; }
    private Components.GbxRefTableFile? bitmapDisplayControlDefaultTVProgram64x10AFile;
    public Components.GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10AFile { get => bitmapDisplayControlDefaultTVProgram64x10AFile; set => bitmapDisplayControlDefaultTVProgram64x10AFile = value; }
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10A(GbxReadSettings settings = default, bool exceptions = false) => bitmapDisplayControlDefaultTVProgram64x10AFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10A, settings, exceptions) ?? bitmapDisplayControlDefaultTVProgram64x10A;

    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10B;
    [AppliedWithChunk<Chunk0303303D>]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10B { get => bitmapDisplayControlDefaultTVProgram64x10BFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10B) ?? bitmapDisplayControlDefaultTVProgram64x10B; set => bitmapDisplayControlDefaultTVProgram64x10B = value; }
    private Components.GbxRefTableFile? bitmapDisplayControlDefaultTVProgram64x10BFile;
    public Components.GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10BFile { get => bitmapDisplayControlDefaultTVProgram64x10BFile; set => bitmapDisplayControlDefaultTVProgram64x10BFile = value; }
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10B(GbxReadSettings settings = default, bool exceptions = false) => bitmapDisplayControlDefaultTVProgram64x10BFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10B, settings, exceptions) ?? bitmapDisplayControlDefaultTVProgram64x10B;

    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram64x10C;
    [AppliedWithChunk<Chunk0303303D>]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram64x10C { get => bitmapDisplayControlDefaultTVProgram64x10CFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10C) ?? bitmapDisplayControlDefaultTVProgram64x10C; set => bitmapDisplayControlDefaultTVProgram64x10C = value; }
    private Components.GbxRefTableFile? bitmapDisplayControlDefaultTVProgram64x10CFile;
    public Components.GbxRefTableFile? BitmapDisplayControlDefaultTVProgram64x10CFile { get => bitmapDisplayControlDefaultTVProgram64x10CFile; set => bitmapDisplayControlDefaultTVProgram64x10CFile = value; }
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram64x10C(GbxReadSettings settings = default, bool exceptions = false) => bitmapDisplayControlDefaultTVProgram64x10CFile?.GetNode(ref bitmapDisplayControlDefaultTVProgram64x10C, settings, exceptions) ?? bitmapDisplayControlDefaultTVProgram64x10C;

    private CPlugBitmap? bitmapDisplayControlDefaultTVProgram2x3;
    [AppliedWithChunk<Chunk0303303D>]
    public CPlugBitmap? BitmapDisplayControlDefaultTVProgram2x3 { get => bitmapDisplayControlDefaultTVProgram2x3File?.GetNode(ref bitmapDisplayControlDefaultTVProgram2x3) ?? bitmapDisplayControlDefaultTVProgram2x3; set => bitmapDisplayControlDefaultTVProgram2x3 = value; }
    private Components.GbxRefTableFile? bitmapDisplayControlDefaultTVProgram2x3File;
    public Components.GbxRefTableFile? BitmapDisplayControlDefaultTVProgram2x3File { get => bitmapDisplayControlDefaultTVProgram2x3File; set => bitmapDisplayControlDefaultTVProgram2x3File = value; }
    public CPlugBitmap? GetBitmapDisplayControlDefaultTVProgram2x3(GbxReadSettings settings = default, bool exceptions = false) => bitmapDisplayControlDefaultTVProgram2x3File?.GetNode(ref bitmapDisplayControlDefaultTVProgram2x3, settings, exceptions) ?? bitmapDisplayControlDefaultTVProgram2x3;

    private CPlugGameSkinAndFolder? colorBlindnessModifier;
    [AppliedWithChunk<Chunk03033040>]
    public CPlugGameSkinAndFolder? ColorBlindnessModifier { get => colorBlindnessModifierFile?.GetNode(ref colorBlindnessModifier) ?? colorBlindnessModifier; set => colorBlindnessModifier = value; }
    private Components.GbxRefTableFile? colorBlindnessModifierFile;
    public Components.GbxRefTableFile? ColorBlindnessModifierFile { get => colorBlindnessModifierFile; set => colorBlindnessModifierFile = value; }
    public CPlugGameSkinAndFolder? GetColorBlindnessModifier(GbxReadSettings settings = default, bool exceptions = false) => colorBlindnessModifierFile?.GetNode(ref colorBlindnessModifier, settings, exceptions) ?? colorBlindnessModifier;

    private CPlugGameSkinAndFolder? globalMaterialModifier;
    [AppliedWithChunk<Chunk03033044>]
    public CPlugGameSkinAndFolder? GlobalMaterialModifier { get => globalMaterialModifierFile?.GetNode(ref globalMaterialModifier) ?? globalMaterialModifier; set => globalMaterialModifier = value; }
    private Components.GbxRefTableFile? globalMaterialModifierFile;
    public Components.GbxRefTableFile? GlobalMaterialModifierFile { get => globalMaterialModifierFile; set => globalMaterialModifierFile = value; }
    public CPlugGameSkinAndFolder? GetGlobalMaterialModifier(GbxReadSettings settings = default, bool exceptions = false) => globalMaterialModifierFile?.GetNode(ref globalMaterialModifier, settings, exceptions) ?? globalMaterialModifier;

    /// <summary>
    /// [SOldHeaderDesc] CGameCtnCollection 0x000 header chunk (old header)
    /// </summary>
    [Chunk(0x03033000, "old header")]
    public partial class HeaderChunk03033000 : HeaderChunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033000;

        public string? U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// [SHeaderDesc] CGameCtnCollection 0x001 header chunk (desc)
    /// </summary>
    [Chunk(0x03033001, "desc")]
    public partial class HeaderChunk03033001 : HeaderChunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033001;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.Id(ref n.collection);
            rw.Boolean(ref n.needUnlock);
            if (Version >= 1)
            {
                rw.String(ref n.iconFullName);
                rw.String(ref n.iconSmallFullName);
                if (Version >= 2)
                {
                    rw.Int32(ref n.sortIndex);
                    if (Version >= 3)
                    {
                        rw.Id(ref n.defaultZoneId);
                        if (Version >= 4)
                        {
                            rw.Ident(ref n.vehicle);
                            if (Version >= 5)
                            {
                                rw.String(ref n.mapFullName);
                                rw.Rect(ref n.mapRect);
                                if (Version <= 7)
                                {
                                    rw.Vec2(ref n.mapCoordElem);
                                    if (Version >= 6)
                                    {
                                        rw.Vec2(ref n.mapCoordIcon);
                                    }
                                }
                                if (Version >= 7)
                                {
                                    rw.String(ref n.loadScreen);
                                    if (Version >= 8)
                                    {
                                        rw.Vec2(ref n.mapCoordElem);
                                        rw.Vec2(ref n.mapCoordIcon);
                                        rw.Vec2(ref n.mapCoordDesc);
                                        rw.String(ref n.longDesc);
                                        if (Version >= 9)
                                        {
                                            rw.String(ref n.displayName);
                                            if (Version >= 10)
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

    /// <summary>
    /// [SHeaderCollectorFolders] CGameCtnCollection 0x002 header chunk
    /// </summary>
    [Chunk(0x03033002)]
    public partial class HeaderChunk03033002 : HeaderChunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033002;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public string? U03;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.String(ref n.folderBlockInfo);
            rw.String(ref n.folderItem);
            rw.String(ref n.folderDecoration);
            if (Version >= 1)
            {
                if (Version <= 2)
                {
                    rw.String(ref U01);
                }
                if (Version >= 2)
                {
                    rw.String(ref n.folderCardEventInfo);
                    if (Version >= 3)
                    {
                        rw.String(ref n.folderMacroBlockInfo);
                        if (Version >= 4)
                        {
                            rw.String(ref n.folderMacroDecals);
                            if (Version >= 5)
                            {
                                rw.String(ref U02);
                                if (Version >= 6)
                                {
                                    rw.String(ref U03);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// [SHeaderMenuIconsFolders] CGameCtnCollection 0x003 header chunk
    /// </summary>
    [Chunk(0x03033003)]
    public partial class HeaderChunk03033003 : HeaderChunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033003;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.String(ref n.folderMenusIcons);
        }
    }


    /// <summary>
    /// CGameCtnCollection 0x008 chunk (DefaultDecoration)
    /// </summary>
    [Chunk(0x03033008, "DefaultDecoration")]
    public partial class Chunk03033008 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033008;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecoration>(ref n.defaultDecoration, ref n.defaultDecorationFile);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x009 chunk
    /// </summary>
    [Chunk(0x03033009)]
    public partial class Chunk03033009 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033009;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Id(ref n.collection);
            rw.ArrayNodeRef_deprec<CGameCtnZone>(ref n.completeListZoneList!);
            rw.NodeRef<CGameCtnZone>(ref n.defaultZone, ref n.defaultZoneFile);
            rw.Boolean(ref n.needUnlock);
            rw.Single(ref n.squareSize);
            rw.Single(ref n.squareHeight);
            rw.Ident(ref n.vehicle);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x00B chunk
    /// </summary>
    [Chunk(0x0303300B)]
    public partial class Chunk0303300B : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303300B;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.blocksShadow);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x00C chunk
    /// </summary>
    [Chunk(0x0303300C)]
    public partial class Chunk0303300C : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303300C;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isEditable);
            rw.EnumInt32<ECollectionType>(ref n.collectionType);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x00D chunk (IconFid, IconSmallFid)
    /// </summary>
    [Chunk(0x0303300D, "IconFid, IconSmallFid")]
    public partial class Chunk0303300D : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303300D;

    }

    /// <summary>
    /// CGameCtnCollection 0x00E chunk
    /// </summary>
    [Chunk(0x0303300E)]
    public partial class Chunk0303300E : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303300E;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.sortIndex);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x011 chunk
    /// </summary>
    [Chunk(0x03033011)]
    public partial class Chunk03033011 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033011;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecoration>(ref n.defaultDecoration, ref n.defaultDecorationFile);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x013 chunk
    /// </summary>
    [Chunk(0x03033013)]
    public partial class Chunk03033013 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033013;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterTop);
            rw.Single(ref n.waterBottom);
            rw.Single(ref n.cameraMinHeight);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x016 chunk
    /// </summary>
    [Chunk(0x03033016)]
    public partial class Chunk03033016 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033016;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.blocksShadow);
            rw.Boolean(ref n.shadowCastBack);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x018 chunk
    /// </summary>
    [Chunk(0x03033018)]
    public partial class Chunk03033018 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033018;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.mapFid);
            rw.Rect(ref n.mapRect);
            rw.Vec2(ref n.mapCoordElem);
            rw.Vec2(ref n.mapCoordIcon);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x019 chunk (LoadScreenFid)
    /// </summary>
    [Chunk(0x03033019, "LoadScreenFid")]
    public partial class Chunk03033019 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033019;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.loadScreenFid, ref n.loadScreenFidFile);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x01A chunk
    /// </summary>
    [Chunk(0x0303301A)]
    public partial class Chunk0303301A : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303301A;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.mapFid);
            rw.Rect(ref n.mapRect);
            rw.Vec2(ref n.mapCoordElem);
            rw.Vec2(ref n.mapCoordIcon);
            rw.Vec2(ref n.mapCoordDesc);
            rw.String(ref n.longDesc);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x01B chunk
    /// </summary>
    [Chunk(0x0303301B)]
    public partial class Chunk0303301B : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303301B;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.blocksShadow);
            rw.Boolean(ref n.shadowCastBack);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x01D chunk
    /// </summary>
    [Chunk(0x0303301D)]
    public partial class Chunk0303301D : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303301D;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<ZoneString>(ref n.zoneStrings!);
            rw.ArrayNodeRef_deprec<CGameCtnDecorationTerrainModifier>(ref n.replacementTerrainModifiers!);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x01E chunk (camera)
    /// </summary>
    [Chunk(0x0303301E, "camera")]
    public partial class Chunk0303301E : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303301E;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Single(ref n.waterTop);
            rw.Single(ref n.waterBottom);
            rw.Single(ref n.cameraMinHeight);
            rw.Boolean(ref n.isWaterOutsidePlayField);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x01F chunk (ParticleEmitterModelsFids)
    /// </summary>
    [Chunk(0x0303301F, "ParticleEmitterModelsFids")]
    public partial class Chunk0303301F : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303301F;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref n.particleEmitterModelsFids!);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x020 chunk (folders)
    /// </summary>
    [Chunk(0x03033020, "folders")]
    public partial class Chunk03033020 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033020;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.folderBlockInfo);
            rw.String(ref n.folderItem);
            rw.String(ref n.folderDecoration);
            rw.String(ref n.folderMenusIcons);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x021 chunk (display name)
    /// </summary>
    [Chunk(0x03033021, "display name")]
    public partial class Chunk03033021 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033021;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.displayName);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x022 chunk (is water multi-height)
    /// </summary>
    [Chunk(0x03033022, "is water multi-height")]
    public partial class Chunk03033022 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033022;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isWaterMultiHeight);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x023 chunk (CarCanBeDirty)
    /// </summary>
    [Chunk(0x03033023, "CarCanBeDirty")]
    public partial class Chunk03033023 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033023;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.carCanBeDirty);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x024 chunk
    /// </summary>
    [Chunk(0x03033024)]
    public partial class Chunk03033024 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033024;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.blocksShadow);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Boolean(ref n.shadowCastBack);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x027 chunk (board square)
    /// </summary>
    [Chunk(0x03033027, "board square")]
    public partial class Chunk03033027 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033027;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Single(ref n.boardSquareHeight);
            rw.Single(ref n.boardSquareBorder);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x028 chunk (FolderAdditionalItem1)
    /// </summary>
    [Chunk(0x03033028, "FolderAdditionalItem1")]
    public partial class Chunk03033028 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033028;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.folderAdditionalItem1);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x029 chunk (FolderAdditionalItem2)
    /// </summary>
    [Chunk(0x03033029, "FolderAdditionalItem2")]
    public partial class Chunk03033029 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033029;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.folderAdditionalItem2);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x02A chunk (FolderDecalModels)
    /// </summary>
    [Chunk(0x0303302A, "FolderDecalModels")]
    public partial class Chunk0303302A : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303302A;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.folderDecalModels);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x02C chunk
    /// </summary>
    [Chunk(0x0303302C)]
    public partial class Chunk0303302C : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303302C;

        public Int128 U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int128(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x02F chunk (Tech3TunnelSpecularExpScaleMax)
    /// </summary>
    [Chunk(0x0303302F, "Tech3TunnelSpecularExpScaleMax")]
    public partial class Chunk0303302F : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303302F;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.tech3TunnelSpecularExpScaleMax);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x030 chunk
    /// </summary>
    [Chunk(0x03033030)]
    public partial class Chunk03033030 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033030;

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x031 chunk (FolderMacroDecals)
    /// </summary>
    [Chunk(0x03033031, "FolderMacroDecals")]
    public partial class Chunk03033031 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033031;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref n.folderMacroDecals);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x033 chunk
    /// </summary>
    [Chunk(0x03033033)]
    public partial class Chunk03033033 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033033;

        public int Version { get; set; }

        public string[]? U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayId(ref U01!);
            if (Version >= 1)
            {
                rw.Int32(ref n.decalFadeCBlockFullDensity);
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x034 chunk
    /// </summary>
    [Chunk(0x03033034)]
    public partial class Chunk03033034 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033034;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CFuncShaderLayerUV>(ref n.fidFuncShaderCloudsX2, ref n.fidFuncShaderCloudsX2File);
            rw.NodeRef<CPlugBitmap>(ref n.fidPlugBitmapCloudsX2, ref n.fidPlugBitmapCloudsX2File);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugBitmap>(ref n.vehicleEnvLayerFidBitmap, ref n.vehicleEnvLayerFidBitmapFile);
                rw.EnumInt32<EVehicleEnvLayer>(ref n.vehicleEnvLayer);
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x036 chunk (OffZone_FogMatter)
    /// </summary>
    [Chunk(0x03033036, "OffZone_FogMatter")]
    public partial class Chunk03033036 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033036;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugFogMatter>(ref n.offZoneFogMatter, ref n.offZoneFogMatterFile);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x037 chunk (TerrainHeightOffset)
    /// </summary>
    [Chunk(0x03033037, "TerrainHeightOffset")]
    public partial class Chunk03033037 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033037;


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Single(ref n.terrainHeightOffset);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x038 chunk
    /// </summary>
    [Chunk(0x03033038)]
    public partial class Chunk03033038 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033038;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public int U04;
        /// <summary>
        /// not seen in code
        /// </summary>
        public int U05;
        /// <summary>
        /// CSystemFidsFolder something?
        /// </summary>
        public bool U06;
        public float U07;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 7)
            {
                if (Version <= 3)
                {
                    rw.Single(ref U01);
                    if (Version == 0)
                    {
                        rw.Single(ref U02);
                        rw.Single(ref U03);
                    }
                    if (Version >= 1)
                    {
                        rw.ReadableWritable<Water>(ref n.water1, version: Version);
                        rw.ReadableWritable<Water>(ref n.water2, version: Version);
                    }
                }
                if (Version >= 4)
                {
                    rw.ReadableWritable<Water>(ref n.water1, version: Version);
                    rw.ReadableWritable<Water>(ref n.water2, version: Version);
                    rw.ReadableWritable<Water>(ref n.water3, version: Version);
                    rw.ReadableWritable<Water>(ref n.water4, version: Version);
                }
            }
            if (Version >= 8)
            {
                rw.NodeRef<CPlugMaterialWaterArray>(ref n.waterArray);
                if (n.WaterArray==null)
                {
                    rw.Int32(ref U04);
                    rw.ReadableWritable<Water>(ref n.water1, version: Version);
                }
            }
            if (Version >= 5)
            {
                rw.NodeRef<CPlugBitmap>(ref n.waterGBitmapNormal, ref n.waterGBitmapNormalFile);
                rw.Single(ref n.waterGBumpSpeedUV);
                rw.Single(ref n.waterGBumpScaleUV);
                rw.Single(ref n.waterGBumpScale);
                rw.Single(ref n.waterGRefracPertub);
                if (Version >= 7)
                {
                    rw.Int32(ref U05); // not seen in code
                }
            }
            rw.Single(ref n.cameraMinHeight);
            if (Version <= 3)
            {
                rw.Boolean(ref U06); // CSystemFidsFolder something?
            }
            rw.Boolean(ref n.isWaterMultiHeight);
            if (Version <= 2)
            {
                rw.Single(ref U07);
            }
            rw.Single(ref n.waterFogClampAboveDist);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x039 chunk
    /// </summary>
    [Chunk(0x03033039)]
    public partial class Chunk03033039 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033039;

        public int Version { get; set; }

        public float? U01;
        public float? U02;
        public float? U03;
        /// <summary>
        /// VehicleStyles?
        /// </summary>
        public int? U04;
        public CMwNod? U05;
        public CMwNod? U06;
        public CMwNod? U07;
        public CPlugBitmap? U08;
        public Components.GbxRefTableFile? U08File;
        public CPlugBitmap? U09;
        public Components.GbxRefTableFile? U09File;
        public CPlugBitmap? U10;
        public Components.GbxRefTableFile? U10File;
        public CPlugBitmap? U11;
        public Components.GbxRefTableFile? U11File;
        public CPlugBitmap? U12;
        public Components.GbxRefTableFile? U12File;
        public CMwNod? U13;
        public Components.GbxRefTableFile? U13File;
        public CMwNod? U14;
        public CMwNod? U15;
        public CMwNod? U16;
        public CMwNod? U17;
        public CMwNod? U18;
        public int U19;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.Single(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
            }
            if (Version >= 1)
            {
                rw.Int32(ref U04); // VehicleStyles?
                if (Version >= 2)
                {
                    rw.NodeRef<CMwNod>(ref n.itemPlacementGroups, ref n.itemPlacementGroupsFile);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CMwNod>(ref n.adnRandomGenList);
                        if (Version >= 4)
                        {
                            rw.NodeRef<CMwNod>(ref n.fidBlockInfoGroups, ref n.fidBlockInfoGroupsFile);
                            if (Version <= 10)
                            {
                                rw.NodeRef<CMwNod>(ref U05);
                                if (Version >= 8)
                                {
                                    rw.NodeRef<CMwNod>(ref U06);
                                }
                            }
                            if (Version >= 6)
                            {
                                rw.NodeRef<CMwNod>(ref n.fidBlockInfoInventory, ref n.fidBlockInfoInventoryFile);
                            }
                            if (Version <= 8)
                            {
                                rw.NodeRef<CMwNod>(ref U07);
                            }
                            if (Version >= 10)
                            {
                                rw.NodeRef<CMwNod>(ref n.fidItemModelInventory, ref n.fidItemModelInventoryFile);
                                if (Version >= 12)
                                {
                                    rw.NodeRef<CPlugBitmap>(ref U08, ref U08File);
                                }
                                if (Version >= 11)
                                {
                                    rw.NodeRef<CPlugBitmap>(ref U09, ref U09File);
                                    rw.NodeRef<CPlugBitmap>(ref U10, ref U10File);
                                    rw.NodeRef<CPlugBitmap>(ref U11, ref U11File);
                                    rw.NodeRef<CPlugBitmap>(ref U12, ref U12File);
                                    rw.NodeRef<CMwNod>(ref U13, ref U13File);
                                    if (Version >= 20)
                                    {
                                        rw.NodeRef<CMwNod>(ref U14);
                                        if (Version >= 21)
                                        {
                                            rw.NodeRef<CMwNod>(ref U15);
                                            rw.NodeRef<CMwNod>(ref U16);
                                            rw.NodeRef<CMwNod>(ref U17);
                                            rw.NodeRef<CMwNod>(ref U18);
                                            if (Version >= 22)
                                            {
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidAdvertisement1x1, ref n.blockSkins_Default_FidAdvertisement1x1File);
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidAdvertisement2x1, ref n.blockSkins_Default_FidAdvertisement2x1File);
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidAdvertisement2x3, ref n.blockSkins_Default_FidAdvertisement2x3File);
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidAdvertisement4x1, ref n.blockSkins_Default_FidAdvertisement4x1File);
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidItemFlag, ref n.blockSkins_Default_FidItemFlagFile);
                                                rw.NodeRef<CPlugFileImg>(ref n.blockSkins_Default_FidAdvertisement16x9, ref n.blockSkins_Default_FidAdvertisement16x9File);
                                            }
                                        }
                                    }
                                    if (Version >= 13)
                                    {
                                        rw.NodeRef<CMwNod>(ref n.fidMacroBlockInfoInventory, ref n.fidMacroBlockInfoInventoryFile);
                                        if (Version >= 14)
                                        {
                                            rw.NodeRef<CPlugMediaClipList>(ref n.defaultSpawnClipList, ref n.defaultSpawnClipListFile);
                                            if (Version >= 16)
                                            {
                                                rw.Ident(ref n.vehicleTransform_CarSnow);
                                                if (Version >= 17)
                                                {
                                                    rw.Ident(ref n.vehicleTransform_CarRally);
                                                    if (Version >= 18)
                                                    {
                                                        rw.Ident(ref n.vehicleTransform_CarDesert);
                                                        if (Version >= 19)
                                                        {
                                                            rw.Int32(ref U19);
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
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03A chunk
    /// </summary>
    [Chunk(0x0303303A)]
    public partial class Chunk0303303A : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303A;

        public int Version { get; set; }

        public int U01;
        public bool U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.EnumInt32<EBackgroundShadow>(ref n.backgroundShadow);
            rw.Boolean(ref U02);
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.EnumInt32<EVertexLighting>(ref n.vertexLighting);
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
            rw.Int32(ref U03);
            rw.Single(ref n.visMeshLodDistScale);
            if (Version == 1)
            {
                rw.Int32(ref U04);
            }
            if (Version >= 3)
            {
                rw.Int32(ref U05);
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03B chunk (turbo color)
    /// </summary>
    [Chunk(0x0303303B, "turbo color")]
    public partial class Chunk0303303B : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303B;

        public int Version { get; set; }

        public int? U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref n.turboColorRoulette1);
            rw.UInt32(ref n.turboColorRoulette2);
            rw.UInt32(ref n.turboColorRoulette3);
            if (Version >= 1)
            {
                rw.UInt32(ref n.turboColorTurbo);
                rw.UInt32(ref n.turboColorTurbo2);
                if (Version >= 2)
                {
                    rw.Int32(ref U01);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03C chunk
    /// </summary>
    [Chunk(0x0303303C)]
    public partial class Chunk0303303C : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303C;

        public int Version { get; set; }

        public string? U01;
        public int? U02;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            if (Version >= 1)
            {
                rw.Int32(ref U02);
            }
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03D chunk (BitmapDisplayControlDefaultTVProgram)
    /// </summary>
    [Chunk(0x0303303D, "BitmapDisplayControlDefaultTVProgram")]
    public partial class Chunk0303303D : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303D;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram16x9, ref n.bitmapDisplayControlDefaultTVProgram16x9File);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10A, ref n.bitmapDisplayControlDefaultTVProgram64x10AFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10B, ref n.bitmapDisplayControlDefaultTVProgram64x10BFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram64x10C, ref n.bitmapDisplayControlDefaultTVProgram64x10CFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapDisplayControlDefaultTVProgram2x3, ref n.bitmapDisplayControlDefaultTVProgram2x3File);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03E chunk
    /// </summary>
    [Chunk(0x0303303E)]
    public partial class Chunk0303303E : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303E;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x03F chunk
    /// </summary>
    [Chunk(0x0303303F)]
    public partial class Chunk0303303F : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303303F;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x040 chunk (ColorBlindnessModifier)
    /// </summary>
    [Chunk(0x03033040, "ColorBlindnessModifier")]
    public partial class Chunk03033040 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033040;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugGameSkinAndFolder>(ref n.colorBlindnessModifier, ref n.colorBlindnessModifierFile);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x041 chunk
    /// </summary>
    [Chunk(0x03033041)]
    public partial class Chunk03033041 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033041;

        public string? U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x042 chunk
    /// </summary>
    [Chunk(0x03033042)]
    public partial class Chunk03033042 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033042;

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x043 chunk
    /// </summary>
    [Chunk(0x03033043)]
    public partial class Chunk03033043 : Chunk<CGameCtnCollection>
    {
        /// <inheritdoc />
        public override uint Id => 0x03033043;

        public string? U01;

        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnCollection 0x044 chunk (GlobalMaterialModifier)
    /// </summary>
    [Chunk(0x03033044, "GlobalMaterialModifier")]
    public partial class Chunk03033044 : Chunk<CGameCtnCollection>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03033044;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnCollection n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugGameSkinAndFolder>(ref n.globalMaterialModifier, ref n.globalMaterialModifierFile);
        }
    }


    public sealed partial class ZoneString : IReadableWritable
    {

        private string? @base;
        public string? Base { get => @base; set => @base = value; }

        private string? replacement;
        public string? Replacement { get => replacement; set => replacement = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref @base);
            rw.Id(ref replacement);
        }
    }

    public sealed partial class Water : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float? u04;
        public float? U04 { get => u04; set => u04 = value; }

        private CMwNod? u05;
        public CMwNod? U05 { get => u05File?.GetNode(ref u05) ?? u05; set => u05 = value; }
        private Components.GbxRefTableFile? u05File;
        public Components.GbxRefTableFile? U05File { get => u05File; set => u05File = value; }
        public CMwNod? GetU05(GbxReadSettings settings = default, bool exceptions = false) => u05File?.GetNode(ref u05, settings, exceptions) ?? u05;

        private int? u06;
        /// <summary>
        /// not seen in code
        /// </summary>
        public int? U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            if (v >= 3)
            {
                rw.Single(ref u04);
            }
            if (v >= 2)
            {
                rw.NodeRef<CMwNod>(ref u05, ref u05File);
                if (v >= 7)
                {
                    rw.Int32(ref u06); // not seen in code
                }
            }
        }
    }


    public enum EVertexLighting
    {
        None,
        Sunrise,
        Nations,
    }

    public enum ECollectionType
    {
        Environment,
        Car,
    }

    public enum EVehicleEnvLayer
    {
        Dirt,
        Mud,
    }

    public enum EBackgroundShadow
    {
        None,
        Receive,
        CastAndReceive,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03033008 => new Chunk03033008(),
        0x03033009 => new Chunk03033009(),
        0x0303300B => new Chunk0303300B(),
        0x0303300C => new Chunk0303300C(),
        0x0303300D => new Chunk0303300D(),
        0x0303300E => new Chunk0303300E(),
        0x03033011 => new Chunk03033011(),
        0x03033013 => new Chunk03033013(),
        0x03033016 => new Chunk03033016(),
        0x03033018 => new Chunk03033018(),
        0x03033019 => new Chunk03033019(),
        0x0303301A => new Chunk0303301A(),
        0x0303301B => new Chunk0303301B(),
        0x0303301D => new Chunk0303301D(),
        0x0303301E => new Chunk0303301E(),
        0x0303301F => new Chunk0303301F(),
        0x03033020 => new Chunk03033020(),
        0x03033021 => new Chunk03033021(),
        0x03033022 => new Chunk03033022(),
        0x03033023 => new Chunk03033023(),
        0x03033024 => new Chunk03033024(),
        0x03033027 => new Chunk03033027(),
        0x03033028 => new Chunk03033028(),
        0x03033029 => new Chunk03033029(),
        0x0303302A => new Chunk0303302A(),
        0x0303302C => new Chunk0303302C(),
        0x0303302F => new Chunk0303302F(),
        0x03033030 => new Chunk03033030(),
        0x03033031 => new Chunk03033031(),
        0x03033033 => new Chunk03033033(),
        0x03033034 => new Chunk03033034(),
        0x03033036 => new Chunk03033036(),
        0x03033037 => new Chunk03033037(),
        0x03033038 => new Chunk03033038(),
        0x03033039 => new Chunk03033039(),
        0x0303303A => new Chunk0303303A(),
        0x0303303B => new Chunk0303303B(),
        0x0303303C => new Chunk0303303C(),
        0x0303303D => new Chunk0303303D(),
        0x0303303E => new Chunk0303303E(),
        0x0303303F => new Chunk0303303F(),
        0x03033040 => new Chunk03033040(),
        0x03033041 => new Chunk03033041(),
        0x03033042 => new Chunk03033042(),
        0x03033043 => new Chunk03033043(),
        0x03033044 => new Chunk03033044(),
        _ => base.NewChunk(chunkId),
    };
}
