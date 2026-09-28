namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E002000</remarks>
[Class(0x2E002000)]
public partial class CGameItemModel : CGameCtnCollector, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E002000;




    private EItemType itemType;
    [AppliedWithChunk<HeaderChunk2E002000>]
    public EItemType ItemType { get => itemType; set => itemType = value; }

    private CSceneMobil? vehicle;
    [AppliedWithChunk<Chunk2E002000>]
    public CSceneMobil? Vehicle { get => vehicleFile?.GetNode(ref vehicle) ?? vehicle; set => vehicle = value; }
    private Components.GbxRefTableFile? vehicleFile;
    public Components.GbxRefTableFile? VehicleFile { get => vehicleFile; set => vehicleFile = value; }
    public CSceneMobil? GetVehicle(GbxReadSettings settings = default, bool exceptions = false) => vehicleFile?.GetNode(ref vehicle, settings, exceptions) ?? vehicle;

    private CPlugGameSkin? gameSkin;
    [AppliedWithChunk<Chunk2E002001>]
    public CPlugGameSkin? GameSkin { get => gameSkin; set => gameSkin = value; }

    private CScene2d? raceInterfaceFid;
    [AppliedWithChunk<Chunk2E002002>]
    [AppliedWithChunk<Chunk2E00200C>]
    public CScene2d? RaceInterfaceFid { get => raceInterfaceFid; set => raceInterfaceFid = value; }

    private CPlugSolid? lowQualitySolid;
    [AppliedWithChunk<Chunk2E002003>]
    public CPlugSolid? LowQualitySolid { get => lowQualitySolidFile?.GetNode(ref lowQualitySolid) ?? lowQualitySolid; set => lowQualitySolid = value; }
    private Components.GbxRefTableFile? lowQualitySolidFile;
    public Components.GbxRefTableFile? LowQualitySolidFile { get => lowQualitySolidFile; set => lowQualitySolidFile = value; }
    public CPlugSolid? GetLowQualitySolid(GbxReadSettings settings = default, bool exceptions = false) => lowQualitySolidFile?.GetNode(ref lowQualitySolid, settings, exceptions) ?? lowQualitySolid;

    private int defaultCamIndex;
    [AppliedWithChunk<Chunk2E002006>]
    public int DefaultCamIndex { get => defaultCamIndex; set => defaultCamIndex = value; }

    private CPlugMaterial? materialSkin;
    [AppliedWithChunk<Chunk2E002007>]
    [AppliedWithChunk<Chunk2E00200E>]
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugMaterial? MaterialSkin { get => materialSkinFile?.GetNode(ref materialSkin) ?? materialSkin; set => materialSkin = value; }
    private Components.GbxRefTableFile? materialSkinFile;
    public Components.GbxRefTableFile? MaterialSkinFile { get => materialSkinFile; set => materialSkinFile = value; }
    public CPlugMaterial? GetMaterialSkin(GbxReadSettings settings = default, bool exceptions = false) => materialSkinFile?.GetNode(ref materialSkin, settings, exceptions) ?? materialSkin;

    private CPlugMaterial? materialGlass;
    [AppliedWithChunk<Chunk2E002007>]
    [AppliedWithChunk<Chunk2E00200E>]
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugMaterial? MaterialGlass { get => materialGlassFile?.GetNode(ref materialGlass) ?? materialGlass; set => materialGlass = value; }
    private Components.GbxRefTableFile? materialGlassFile;
    public Components.GbxRefTableFile? MaterialGlassFile { get => materialGlassFile; set => materialGlassFile = value; }
    public CPlugMaterial? GetMaterialGlass(GbxReadSettings settings = default, bool exceptions = false) => materialGlassFile?.GetNode(ref materialGlass, settings, exceptions) ?? materialGlass;

    private CPlugMaterial? materialDetails;
    [AppliedWithChunk<Chunk2E002007>]
    [AppliedWithChunk<Chunk2E00200E>]
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugMaterial? MaterialDetails { get => materialDetailsFile?.GetNode(ref materialDetails) ?? materialDetails; set => materialDetails = value; }
    private Components.GbxRefTableFile? materialDetailsFile;
    public Components.GbxRefTableFile? MaterialDetailsFile { get => materialDetailsFile; set => materialDetailsFile = value; }
    public CPlugMaterial? GetMaterialDetails(GbxReadSettings settings = default, bool exceptions = false) => materialDetailsFile?.GetNode(ref materialDetails, settings, exceptions) ?? materialDetails;

    private External<CMwNod>?[]? nadeoSkinFids;
    [AppliedWithChunk<Chunk2E002008>]
    public External<CMwNod>?[]? NadeoSkinFids { get => nadeoSkinFids; set => nadeoSkinFids = value; }

    private External<CMwNod>[]? cameras;
    [AppliedWithChunk<Chunk2E002009>]
    public External<CMwNod>[]? Cameras { get => cameras; set => cameras = value; }

    private CPlugDecoratorSolid? decoratorSolid;
    [AppliedWithChunk<Chunk2E00200A>]
    public CPlugDecoratorSolid? DecoratorSolid { get => decoratorSolidFile?.GetNode(ref decoratorSolid) ?? decoratorSolid; set => decoratorSolid = value; }
    private Components.GbxRefTableFile? decoratorSolidFile;
    public Components.GbxRefTableFile? DecoratorSolidFile { get => decoratorSolidFile; set => decoratorSolidFile = value; }
    public CPlugDecoratorSolid? GetDecoratorSolid(GbxReadSettings settings = default, bool exceptions = false) => decoratorSolidFile?.GetNode(ref decoratorSolid, settings, exceptions) ?? decoratorSolid;

    private CPlugMaterial? stemMaterial;
    [AppliedWithChunk<Chunk2E00200B>]
    public CPlugMaterial? StemMaterial { get => stemMaterialFile?.GetNode(ref stemMaterial) ?? stemMaterial; set => stemMaterial = value; }
    private Components.GbxRefTableFile? stemMaterialFile;
    public Components.GbxRefTableFile? StemMaterialFile { get => stemMaterialFile; set => stemMaterialFile = value; }
    public CPlugMaterial? GetStemMaterial(GbxReadSettings settings = default, bool exceptions = false) => stemMaterialFile?.GetNode(ref stemMaterial, settings, exceptions) ?? stemMaterial;

    private CPlugMaterial? stemBumpMaterial;
    [AppliedWithChunk<Chunk2E00200B>]
    public CPlugMaterial? StemBumpMaterial { get => stemBumpMaterialFile?.GetNode(ref stemBumpMaterial) ?? stemBumpMaterial; set => stemBumpMaterial = value; }
    private Components.GbxRefTableFile? stemBumpMaterialFile;
    public Components.GbxRefTableFile? StemBumpMaterialFile { get => stemBumpMaterialFile; set => stemBumpMaterialFile = value; }
    public CPlugMaterial? GetStemBumpMaterial(GbxReadSettings settings = default, bool exceptions = false) => stemBumpMaterialFile?.GetNode(ref stemBumpMaterial, settings, exceptions) ?? stemBumpMaterial;

    private CMwNod[]? forcedSkinsFids;
    [AppliedWithChunk<Chunk2E00200D>]
    public CMwNod[]? ForcedSkinsFids { get => forcedSkinsFids; set => forcedSkinsFids = value; }

    private CPlugMaterial? materialPilot;
    [AppliedWithChunk<Chunk2E00200E>]
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugMaterial? MaterialPilot { get => materialPilotFile?.GetNode(ref materialPilot) ?? materialPilot; set => materialPilot = value; }
    private Components.GbxRefTableFile? materialPilotFile;
    public Components.GbxRefTableFile? MaterialPilotFile { get => materialPilotFile; set => materialPilotFile = value; }
    public CPlugMaterial? GetMaterialPilot(GbxReadSettings settings = default, bool exceptions = false) => materialPilotFile?.GetNode(ref materialPilot, settings, exceptions) ?? materialPilot;

    private CPlugBitmap? bannerProfileFid;
    [AppliedWithChunk<Chunk2E002010>]
    public CPlugBitmap? BannerProfileFid { get => bannerProfileFidFile?.GetNode(ref bannerProfileFid) ?? bannerProfileFid; set => bannerProfileFid = value; }
    private Components.GbxRefTableFile? bannerProfileFidFile;
    public Components.GbxRefTableFile? BannerProfileFidFile { get => bannerProfileFidFile; set => bannerProfileFidFile = value; }
    public CPlugBitmap? GetBannerProfileFid(GbxReadSettings settings = default, bool exceptions = false) => bannerProfileFidFile?.GetNode(ref bannerProfileFid, settings, exceptions) ?? bannerProfileFid;

    private CPlugLight? frontLight;
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugLight? FrontLight { get => frontLightFile?.GetNode(ref frontLight) ?? frontLight; set => frontLight = value; }
    private Components.GbxRefTableFile? frontLightFile;
    public Components.GbxRefTableFile? FrontLightFile { get => frontLightFile; set => frontLightFile = value; }
    public CPlugLight? GetFrontLight(GbxReadSettings settings = default, bool exceptions = false) => frontLightFile?.GetNode(ref frontLight, settings, exceptions) ?? frontLight;

    private CPlugLight? frontLightSmall;
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugLight? FrontLightSmall { get => frontLightSmallFile?.GetNode(ref frontLightSmall) ?? frontLightSmall; set => frontLightSmall = value; }
    private Components.GbxRefTableFile? frontLightSmallFile;
    public Components.GbxRefTableFile? FrontLightSmallFile { get => frontLightSmallFile; set => frontLightSmallFile = value; }
    public CPlugLight? GetFrontLightSmall(GbxReadSettings settings = default, bool exceptions = false) => frontLightSmallFile?.GetNode(ref frontLightSmall, settings, exceptions) ?? frontLightSmall;

    private CPlugLight? rearLight;
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugLight? RearLight { get => rearLightFile?.GetNode(ref rearLight) ?? rearLight; set => rearLight = value; }
    private Components.GbxRefTableFile? rearLightFile;
    public Components.GbxRefTableFile? RearLightFile { get => rearLightFile; set => rearLightFile = value; }
    public CPlugLight? GetRearLight(GbxReadSettings settings = default, bool exceptions = false) => rearLightFile?.GetNode(ref rearLight, settings, exceptions) ?? rearLight;

    private CPlugLight? projShadow;
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugLight? ProjShadow { get => projShadowFile?.GetNode(ref projShadow) ?? projShadow; set => projShadow = value; }
    private Components.GbxRefTableFile? projShadowFile;
    public Components.GbxRefTableFile? ProjShadowFile { get => projShadowFile; set => projShadowFile = value; }
    public CPlugLight? GetProjShadow(GbxReadSettings settings = default, bool exceptions = false) => projShadowFile?.GetNode(ref projShadow, settings, exceptions) ?? projShadow;

    private CPlugLight? projFront;
    [AppliedWithChunk<Chunk2E002011>]
    public CPlugLight? ProjFront { get => projFrontFile?.GetNode(ref projFront) ?? projFront; set => projFront = value; }
    private Components.GbxRefTableFile? projFrontFile;
    public Components.GbxRefTableFile? ProjFrontFile { get => projFrontFile; set => projFrontFile = value; }
    public CPlugLight? GetProjFront(GbxReadSettings settings = default, bool exceptions = false) => projFrontFile?.GetNode(ref projFront, settings, exceptions) ?? projFront;

    private Vec3 groundPoint;
    [AppliedWithChunk<Chunk2E002012>]
    public Vec3 GroundPoint { get => groundPoint; set => groundPoint = value; }

    private float painterGroundMargin;
    [AppliedWithChunk<Chunk2E002012>]
    public float PainterGroundMargin { get => painterGroundMargin; set => painterGroundMargin = value; }

    private float orbitalCenterHeightFromGround;
    [AppliedWithChunk<Chunk2E002012>]
    public float OrbitalCenterHeightFromGround { get => orbitalCenterHeightFromGround; set => orbitalCenterHeightFromGround = value; }

    private float orbitalRadiusBase;
    [AppliedWithChunk<Chunk2E002012>]
    public float OrbitalRadiusBase { get => orbitalRadiusBase; set => orbitalRadiusBase = value; }

    private float orbitalPreviewAngle;
    [AppliedWithChunk<Chunk2E002012>]
    public float OrbitalPreviewAngle { get => orbitalPreviewAngle; set => orbitalPreviewAngle = value; }

    private CPlugAudioEnvironment? audioEnvironmentInCar;
    [AppliedWithChunk<Chunk2E002013>]
    public CPlugAudioEnvironment? AudioEnvironmentInCar { get => audioEnvironmentInCarFile?.GetNode(ref audioEnvironmentInCar) ?? audioEnvironmentInCar; set => audioEnvironmentInCar = value; }
    private Components.GbxRefTableFile? audioEnvironmentInCarFile;
    public Components.GbxRefTableFile? AudioEnvironmentInCarFile { get => audioEnvironmentInCarFile; set => audioEnvironmentInCarFile = value; }
    public CPlugAudioEnvironment? GetAudioEnvironmentInCar(GbxReadSettings settings = default, bool exceptions = false) => audioEnvironmentInCarFile?.GetNode(ref audioEnvironmentInCar, settings, exceptions) ?? audioEnvironmentInCar;

    private EItemType itemTypeE;
    [AppliedWithChunk<Chunk2E002015>]
    public EItemType ItemTypeE { get => itemTypeE; set => itemTypeE = value; }

    private CGameItemPlacementParam? defaultPlacement;
    [AppliedWithChunk<Chunk2E00201C>]
    public CGameItemPlacementParam? DefaultPlacement { get => defaultPlacementFile?.GetNode(ref defaultPlacement) ?? defaultPlacement; set => defaultPlacement = value; }
    private Components.GbxRefTableFile? defaultPlacementFile;
    public Components.GbxRefTableFile? DefaultPlacementFile { get => defaultPlacementFile; set => defaultPlacementFile = value; }
    public CGameItemPlacementParam? GetDefaultPlacement(GbxReadSettings settings = default, bool exceptions = false) => defaultPlacementFile?.GetNode(ref defaultPlacement, settings, exceptions) ?? defaultPlacement;

    private string? archetypeRef;
    [AppliedWithChunk<Chunk2E00201E>]
    public string? ArchetypeRef { get => archetypeRef; set => archetypeRef = value; }

    private CGameItemModel? archetypeFid;
    [AppliedWithChunk<Chunk2E00201E>]
    public CGameItemModel? ArchetypeFid { get => archetypeFid; set => archetypeFid = value; }

    private EWaypointType waypointType;
    [AppliedWithChunk<Chunk2E00201F>]
    public EWaypointType WaypointType { get => waypointType; set => waypointType = value; }

    private bool disableLightmap;
    [AppliedWithChunk<Chunk2E00201F>]
    public bool DisableLightmap { get => disableLightmap; set => disableLightmap = value; }

    private string? iconFid;
    [AppliedWithChunk<Chunk2E002020>]
    public string? IconFid { get => iconFid; set => iconFid = value; }

    private ItemGroupElement[]? itemGroupElements;
    [AppliedWithChunk<Chunk2E002021>]
    public ItemGroupElement[]? ItemGroupElements { get => itemGroupElements; set => itemGroupElements = value; }

    /// <summary>
    /// [SHeaderItemModelDesc] CGameItemModel 0x000 header chunk (item type)
    /// </summary>
    [Chunk(0x2E002000, "item type")]
    public partial class HeaderChunk2E002000 : HeaderChunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002000;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EItemType>(ref n.itemType);
        }
    }

    /// <summary>
    /// [SHeaderFileVersion] CGameItemModel 0x001 header chunk (file version)
    /// </summary>
    [Chunk(0x2E002001, "file version")]
    public partial class HeaderChunk2E002001 : HeaderChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002001;

        public int Version { get; set; }


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
        }
    }


    /// <summary>
    /// CGameItemModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E002000)]
    public partial class Chunk2E002000 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002000;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneMobil>(ref n.vehicle, ref n.vehicleFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E002001)]
    public partial class Chunk2E002001 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002001;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugGameSkin>(ref n.gameSkin);
        }
    }

    /// <summary>
    /// CGameItemModel 0x002 chunk (old race interface fid)
    /// </summary>
    [Chunk(0x2E002002, "old race interface fid")]
    public partial class Chunk2E002002 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002002;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CScene2d>(ref n.raceInterfaceFid);
        }
    }

    /// <summary>
    /// CGameItemModel 0x003 chunk (LowQualitySolid)
    /// </summary>
    [Chunk(0x2E002003, "LowQualitySolid")]
    public partial class Chunk2E002003 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002003;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSolid>(ref n.lowQualitySolid, ref n.lowQualitySolidFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x004 chunk
    /// </summary>
    [Chunk(0x2E002004)]
    public partial class Chunk2E002004 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002004;

        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x006 chunk (DefaultCamIndex)
    /// </summary>
    [Chunk(0x2E002006, "DefaultCamIndex")]
    public partial class Chunk2E002006 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002006;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.defaultCamIndex);
        }
    }

    /// <summary>
    /// CGameItemModel 0x007 chunk (old materials)
    /// </summary>
    [Chunk(0x2E002007, "old materials")]
    public partial class Chunk2E002007 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002007;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterial>(ref n.materialSkin, ref n.materialSkinFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialGlass, ref n.materialGlassFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialDetails, ref n.materialDetailsFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x008 chunk (Nadeo skin fids)
    /// </summary>
    [Chunk(0x2E002008, "Nadeo skin fids")]
    public partial class Chunk2E002008 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002008;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CMwNod>(ref n.nadeoSkinFids);
        }
    }

    /// <summary>
    /// CGameItemModel 0x009 chunk (Cameras)
    /// </summary>
    [Chunk(0x2E002009, "Cameras")]
    public partial class Chunk2E002009 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002009;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CMwNod>(ref n.cameras!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x00A chunk (DecoratorSolid)
    /// </summary>
    [Chunk(0x2E00200A, "DecoratorSolid")]
    public partial class Chunk2E00200A : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00200A;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugDecoratorSolid>(ref n.decoratorSolid, ref n.decoratorSolidFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x00B chunk (stem materials)
    /// </summary>
    [Chunk(0x2E00200B, "stem materials")]
    public partial class Chunk2E00200B : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00200B;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterial>(ref n.stemMaterial, ref n.stemMaterialFile);
            rw.NodeRef<CPlugMaterial>(ref n.stemBumpMaterial, ref n.stemBumpMaterialFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x00C chunk (race interface fid)
    /// </summary>
    [Chunk(0x2E00200C, "race interface fid")]
    public partial class Chunk2E00200C : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00200C;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CScene2d>(ref n.raceInterfaceFid);
        }
    }

    /// <summary>
    /// CGameItemModel 0x00D chunk
    /// </summary>
    [Chunk(0x2E00200D)]
    public partial class Chunk2E00200D : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00200D;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CMwNod>(ref n.forcedSkinsFids!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x00E chunk (StadiumCar materials)
    /// </summary>
    [Chunk(0x2E00200E, "StadiumCar materials")]
    public partial class Chunk2E00200E : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00200E;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterial>(ref n.materialSkin, ref n.materialSkinFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialGlass, ref n.materialGlassFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialDetails, ref n.materialDetailsFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialPilot, ref n.materialPilotFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x010 chunk
    /// </summary>
    [Chunk(0x2E002010)]
    public partial class Chunk2E002010 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002010;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.bannerProfileFid, ref n.bannerProfileFidFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x011 chunk (materials)
    /// </summary>
    [Chunk(0x2E002011, "materials")]
    public partial class Chunk2E002011 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002011;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterial>(ref n.materialSkin, ref n.materialSkinFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialGlass, ref n.materialGlassFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialDetails, ref n.materialDetailsFile);
            rw.NodeRef<CPlugMaterial>(ref n.materialPilot, ref n.materialPilotFile);
            rw.NodeRef<CPlugLight>(ref n.frontLight, ref n.frontLightFile);
            rw.NodeRef<CPlugLight>(ref n.frontLightSmall, ref n.frontLightSmallFile);
            rw.NodeRef<CPlugLight>(ref n.rearLight, ref n.rearLightFile);
            rw.NodeRef<CPlugLight>(ref n.projShadow, ref n.projShadowFile);
            rw.NodeRef<CPlugLight>(ref n.projFront, ref n.projFrontFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x012 chunk
    /// </summary>
    [Chunk(0x2E002012)]
    public partial class Chunk2E002012 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002012;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.groundPoint);
            rw.Single(ref n.painterGroundMargin);
            rw.Single(ref n.orbitalCenterHeightFromGround);
            rw.Single(ref n.orbitalRadiusBase);
            rw.Single(ref n.orbitalPreviewAngle);
        }
    }

    /// <summary>
    /// CGameItemModel 0x013 chunk
    /// </summary>
    [Chunk(0x2E002013)]
    public partial class Chunk2E002013 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002013;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvironmentInCar, ref n.audioEnvironmentInCarFile);
        }
    }

    /// <summary>
    /// CGameItemModel 0x014 chunk
    /// </summary>
    [Chunk(0x2E002014)]
    public partial class Chunk2E002014 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002014;

        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x015 chunk (ItemTypeE)
    /// </summary>
    [Chunk(0x2E002015, "ItemTypeE")]
    public partial class Chunk2E002015 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002015;


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EItemType>(ref n.itemTypeE);
        }
    }

    /// <summary>
    /// CGameItemModel 0x019 chunk (model)
    /// </summary>
    [Chunk(0x2E002019, "model")]
    public partial class Chunk2E002019 : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002019;

    }

    /// <summary>
    /// CGameItemModel 0x01A chunk
    /// </summary>
    [Chunk(0x2E00201A)]
    public partial class Chunk2E00201A : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201A;

        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x01B chunk
    /// </summary>
    [Chunk(0x2E00201B)]
    public partial class Chunk2E00201B : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201B;

        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x01C chunk (default placement)
    /// </summary>
    [Chunk(0x2E00201C, "default placement")]
    public partial class Chunk2E00201C : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201C;

        public int Version { get; set; }

        public Vec3[]? U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 5)
            {
                rw.NodeRef<CGameItemPlacementParam>(ref n.defaultPlacement, ref n.defaultPlacementFile);
                return;
            }
            rw.Array<Vec3>(ref U01!);
            if (Version >= 1)
            {
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                rw.Single(ref U07);
                if (Version >= 2)
                {
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);
                    if (Version >= 3)
                    {
                        rw.Single(ref U13);
                        rw.Single(ref U14);
                        if (Version >= 4)
                        {
                            rw.Single(ref U15);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameItemModel 0x01D chunk
    /// </summary>
    [Chunk(0x2E00201D)]
    public partial class Chunk2E00201D : Chunk<CGameItemModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201D;

        public short U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.Int16(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x01E chunk
    /// </summary>
    [Chunk(0x2E00201E)]
    public partial class Chunk2E00201E : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201E;

        public int Version { get; set; }

        /// <summary>
        /// SkinDirNameCustom
        /// </summary>
        public string? U01;
        /// <summary>
        /// -1
        /// </summary>
        public int U02;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 2)
            {
                rw.String(ref n.archetypeRef);
                if (Version >= 5)
                {
                    if (n.ArchetypeRef==null||n.ArchetypeRef=="")
                    {
                        rw.NodeRef<CGameItemModel>(ref n.archetypeFid);
                    }
                    if (Version >= 6)
                    {
                        rw.String(ref U01); // SkinDirNameCustom
                        if (Version >= 7)
                        {
                            rw.Int32(ref U02); // -1
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameItemModel 0x01F chunk
    /// </summary>
    [Chunk(0x2E00201F)]
    public partial class Chunk2E00201F : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00201F;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public int? U03;
        public int? U04;
        public short? U05;
        public Iso4 U06;
        public CMwNod? U07;
        public byte? U08;
        public int? U09;
        public int? U10;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 6)
            {
                if (Version >= 5)
                {
                    rw.String(ref U01);
                    rw.String(ref U02);
                    rw.Int32(ref U03);
                }
                if (Version >= 4)
                {
                    rw.Int32(ref U04);
                }
                if (Version <= 2)
                {
                    rw.Int16(ref U05);
                }
            }
            rw.EnumInt32<EWaypointType>(ref n.waypointType);
            if (Version <= 7)
            {
                rw.Iso4(ref U06);
            }
            if (Version >= 6)
            {
                rw.Boolean(ref n.disableLightmap);
                if (Version >= 9)
                {
                    if (Version <= 12)
                    {
                        rw.NodeRef<CMwNod>(ref U07);
                    }
                    if (Version >= 11)
                    {
                        rw.Byte(ref U08);
                        if (Version >= 12)
                        {
                            rw.Int32(ref U09);
                            rw.Int32(ref U10);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameItemModel 0x020 chunk
    /// </summary>
    [Chunk(0x2E002020)]
    public partial class Chunk2E002020 : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002020;

        public int Version { get; set; }

        public string? U01;
        /// <summary>
        /// CPlugFileImg
        /// </summary>
        public CMwNod? U02;
        /// <summary>
        /// ArticlePtr? xD
        /// </summary>
        public bool U03;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.String(ref U01);
                rw.NodeRef<CMwNod>(ref U02); // CPlugFileImg
                return;
            }
            rw.String(ref n.iconFid);
            if (Version >= 3)
            {
                rw.Boolean(ref U03, asByte: true); // ArticlePtr? xD
            }
        }
    }

    /// <summary>
    /// CGameItemModel 0x021 chunk
    /// </summary>
    [Chunk(0x2E002021)]
    public partial class Chunk2E002021 : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002021;

        public int Version { get; set; }


        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<ItemGroupElement>(ref n.itemGroupElements!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x022 skippable chunk
    /// </summary>
    [Chunk(0x2E002022)]
    public partial class Chunk2E002022 : SkippableChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002022;

        public int Version { get; set; }

        public int U01;
        public string[]? U02;
        public int U03;
        public ushort[]? U04;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.ArrayId(ref U02!);
            rw.Int32(ref U03);
            rw.Array<ushort>(ref U04!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x023 chunk
    /// </summary>
    [Chunk(0x2E002023)]
    public partial class Chunk2E002023 : Chunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002023;

        public int Version { get; set; }

        public byte U01;
        public int U02;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Byte(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CGameItemModel 0x024 skippable chunk
    /// </summary>
    [Chunk(0x2E002024)]
    public partial class Chunk2E002024 : SkippableChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002024;

        public int Version { get; set; }

        public Vec2[]? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<Vec2>(ref U01!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x025 skippable chunk
    /// </summary>
    [Chunk(0x2E002025)]
    public partial class Chunk2E002025 : SkippableChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002025;

        public int Version { get; set; }

        public bool U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CGameItemModel 0x026 skippable chunk
    /// </summary>
    [Chunk(0x2E002026)]
    public partial class Chunk2E002026 : SkippableChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002026;

        public int Version { get; set; }

        public Vec3[]? U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<Vec3>(ref U01!);
        }
    }

    /// <summary>
    /// CGameItemModel 0x027 skippable chunk
    /// </summary>
    [Chunk(0x2E002027)]
    public partial class Chunk2E002027 : SkippableChunk<CGameItemModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E002027;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
        }
    }


    public sealed partial class ItemGroupElement : IReadableWritable
    {

        private Iso4 location;
        public Iso4 Location { get => location; set => location = value; }

        private string? itemRef;
        public string? ItemRef { get => itemRef; set => itemRef = value; }

        private CGameItemModel? item;
        public CGameItemModel? Item { get => item; set => item = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Iso4(ref location);
            rw.String(ref itemRef);
            if (ItemRef==null||ItemRef=="")
            {
                rw.NodeRef<CGameItemModel>(ref item);
            }
        }
    }


    public enum EItemType
    {
        Undefined,
        /// <summary>
        /// StaticObject
        /// </summary>
        Ornament,
        /// <summary>
        /// DynaObject
        /// </summary>
        PickUp,
        Character,
        Vehicle,
        Spot,
        Cannon,
        Group,
        Decal,
        Turret,
        Wagon,
        Block,
        EntitySpawner,
    }

    public enum EWaypointType
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser,
    }

    public enum EDefaultCam
    {
        None,
        Default,
        Free,
        Spectator,
        Behind,
        Close,
        Internal,
        Helico,
        FirstPerson,
        ThirdPerson,
        ThirdPersonTop,
        Iso,
        IsoFocus,
        Dia3,
        Board,
        MonoScreen,
        Rear,
        Debug,
        _1,
        _2,
        _3,
        Alt1,
        Orbital,
        Decals,
        Snap,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E002000 => new Chunk2E002000(),
        0x2E002001 => new Chunk2E002001(),
        0x2E002002 => new Chunk2E002002(),
        0x2E002003 => new Chunk2E002003(),
        0x2E002004 => new Chunk2E002004(),
        0x2E002006 => new Chunk2E002006(),
        0x2E002007 => new Chunk2E002007(),
        0x2E002008 => new Chunk2E002008(),
        0x2E002009 => new Chunk2E002009(),
        0x2E00200A => new Chunk2E00200A(),
        0x2E00200B => new Chunk2E00200B(),
        0x2E00200C => new Chunk2E00200C(),
        0x2E00200D => new Chunk2E00200D(),
        0x2E00200E => new Chunk2E00200E(),
        0x2E002010 => new Chunk2E002010(),
        0x2E002011 => new Chunk2E002011(),
        0x2E002012 => new Chunk2E002012(),
        0x2E002013 => new Chunk2E002013(),
        0x2E002014 => new Chunk2E002014(),
        0x2E002015 => new Chunk2E002015(),
        0x2E002019 => new Chunk2E002019(),
        0x2E00201A => new Chunk2E00201A(),
        0x2E00201B => new Chunk2E00201B(),
        0x2E00201C => new Chunk2E00201C(),
        0x2E00201D => new Chunk2E00201D(),
        0x2E00201E => new Chunk2E00201E(),
        0x2E00201F => new Chunk2E00201F(),
        0x2E002020 => new Chunk2E002020(),
        0x2E002021 => new Chunk2E002021(),
        0x2E002022 => new Chunk2E002022(),
        0x2E002023 => new Chunk2E002023(),
        0x2E002024 => new Chunk2E002024(),
        0x2E002025 => new Chunk2E002025(),
        0x2E002026 => new Chunk2E002026(),
        0x2E002027 => new Chunk2E002027(),
        _ => base.NewChunk(chunkId),
    };
}
