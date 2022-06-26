namespace GBX.NET.Engines.GameData;

/// <summary>
/// CGameItemModel (0x2E002000)
/// </summary>
/// <remarks>A custom item, or possibly a custom block.</remarks>
[Node(0x2E002000), WritingNotSupported]
[NodeExtension("Item")]
[NodeExtension("Block")] // CGameCustomBlockModel but it's here instead
public class CGameItemModel : CGameCtnCollector
{
    #region Enums

    public enum EItemType : int
    {
        Undefined = 0,
        /// <summary>
        /// Formerly StaticObject
        /// </summary>
        Ornament = 1,
        /// <summary>
        /// Formerly DynaObject
        /// </summary>
        PickUp = 2,
        Character = 3,
        Vehicle = 4,
        Spot = 5,
        Cannon = 6,
        Group = 7,
        Decal = 8,
        Turret = 9,
        Wagon = 10,
        Block = 11,
        EntitySpawner = 12
    }

    public enum EWaypointType : int
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser
    }

    public enum EDefaultCam : int
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
        Snap
    }

    #endregion

    #region Fields

    private EItemType itemType;
    private EItemType itemTypeE;
    private CMwNod?[]? nadeoSkinFids;
    private CMwNod?[]? cameras;
    private CMwNod? raceInterfaceFid;
    private Vec3 groundPoint;
    private float painterGroundMargin;
    private float orbitalCenterHeightFromGround;
    private float orbitalRadiusBase;
    private float orbitalPreviewAngle;
    private string? defaultWeaponName;
    private CMwNod? phyModelCustom;
    private CMwNod? visModelCustom;
    private CMwNod? entityModelEdition;
    private CMwNod? entityModel;
    private EDefaultCam? defaultCam;
    private CGameItemPlacementParam? defaultPlacement;
    private string? archetypeRef;
    private string? iconFid;
    private EWaypointType waypointType;
    private bool disableLightmap;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    public EItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod?[]? NadeoSkinFids
    {
        get => nadeoSkinFids;
        set => nadeoSkinFids = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod?[]? Cameras
    {
        get => cameras;
        set => cameras = value;
    }

    [NodeMember]
    public CMwNod? RaceInterfaceFid
    {
        get => raceInterfaceFid;
        set => raceInterfaceFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public Vec3 GroundPoint
    {
        get => groundPoint;
        set => groundPoint = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public float PainterGroundMargin
    {
        get => painterGroundMargin;
        set => painterGroundMargin = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public float OrbitalCenterHeightFromGround
    {
        get => orbitalCenterHeightFromGround;
        set => orbitalCenterHeightFromGround = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public float OrbitalRadiusBase
    {
        get => orbitalRadiusBase;
        set => orbitalRadiusBase = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public float OrbitalPreviewAngle
    {
        get => orbitalPreviewAngle;
        set => orbitalPreviewAngle = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public EItemType ItemTypeE
    {
        get => itemTypeE;
        set => itemTypeE = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public string? DefaultWeaponName
    {
        get => defaultWeaponName;
        set => defaultWeaponName = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? PhyModelCustom
    {
        get => phyModelCustom;
        set => phyModelCustom = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? VisModelCustom
    {
        get => visModelCustom;
        set => visModelCustom = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public EDefaultCam? DefaultCam
    {
        get => defaultCam;
        set => defaultCam = value;
    }

    /// <summary>
    /// An entity object of the item model.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    public CMwNod? EntityModelEdition
    {
        get => entityModelEdition;
        set => entityModelEdition = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? EntityModel
    {
        get => entityModel;
        set => entityModel = value;
    }

    /// <summary>
    /// An item model if <see cref="EntityModelEdition"/> is <see cref="CGameCommonItemEntityModelEdition"/>, otherwise null.
    /// </summary>
    public CGameCommonItemEntityModelEdition? ItemModel
    {
        get => EntityModelEdition as CGameCommonItemEntityModelEdition;
        set => EntityModelEdition = value;
    }

    /// <summary>
    /// A block model if <see cref="EntityModelEdition"/> is <see cref="CGameBlockItem"/>, otherwise null.
    /// </summary>
    public CGameBlockItem? BlockModel
    {
        get => EntityModelEdition as CGameBlockItem;
        set => EntityModelEdition = value;
    }

    /// <summary>
    /// Placement information when the item model is used in the editor.
    /// </summary>
    [NodeMember(ExactName = "DefaultPlacementParam_Dbg")]
    public CGameItemPlacementParam? DefaultPlacement
    {
        get => defaultPlacement;
        set => defaultPlacement = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public string? ArchetypeRef
    {
        get => archetypeRef;
        set => archetypeRef = value;
    }

    [NodeMember]
    public new string? IconFid
    {
        get => iconFid;
        set => iconFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public EWaypointType WaypointType { get => waypointType; set => waypointType = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool DisableLightmap { get => disableLightmap; set => disableLightmap = value; }

    #endregion

    #region Constructors

    protected CGameItemModel()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 header chunk (item type)

    /// <summary>
    /// CGameItemModel 0x000 header chunk (item type)
    /// </summary>
    [Chunk(0x2E002000, "item type")]
    public class Chunk2E002000 : HeaderChunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32(ref n.itemType);
        }
    }

    #endregion

    #region 0x001 header chunk (item type)

    /// <summary>
    /// CGameItemModel 0x001 header chunk (file version)
    /// </summary>
    [Chunk(0x2E002001, "file version")]
    public class Chunk2E002001 : HeaderChunk<CGameItemModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameItemModel 0x006 chunk
    /// </summary>
    [Chunk(0x2E002006)]
    public class Chunk2E002006 : Chunk<CGameItemModel>
    {
        public int U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // some cam thingie?
        }
    }

    #endregion

    #region 0x008 chunk (Nadeo skin fids)

    /// <summary>
    /// CGameItemModel 0x008 chunk (Nadeo skin fids)
    /// </summary>
    [Chunk(0x2E002008, "Nadeo skin fids")]
    public class Chunk2E002008 : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode(ref n.nadeoSkinFids);
        }
    }

    #endregion

    #region 0x009 chunk (cameras)

    /// <summary>
    /// CGameItemModel 0x009 chunk (cameras)
    /// </summary>
    [Chunk(0x2E002009, "cameras")]
    public class Chunk2E002009 : Chunk<CGameItemModel>
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ArrayNode(ref n.cameras);
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CGameItemModel 0x00A chunk
    /// </summary>
    [Chunk(0x2E00200A)]
    public class Chunk2E00200A : Chunk<CGameItemModel>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x00C chunk (race interface fid)

    /// <summary>
    /// CGameItemModel 0x00C chunk (race interface fid)
    /// </summary>
    [Chunk(0x2E00200C, "race interface fid")]
    public class Chunk2E00200C : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.raceInterfaceFid);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CGameItemModel 0x010 chunk
    /// </summary>
    [Chunk(0x2E002010)]
    public class Chunk2E002010 : Chunk<CGameItemModel>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CGameItemModel 0x012 chunk
    /// </summary>
    [Chunk(0x2E002012)]
    public class Chunk2E002012 : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.groundPoint);
            rw.Single(ref n.painterGroundMargin);
            rw.Single(ref n.orbitalCenterHeightFromGround);
            rw.Single(ref n.orbitalRadiusBase);
            rw.Single(ref n.orbitalPreviewAngle);
        }
    }

    #endregion

    #region 0x013

    /// <summary>
    /// CGameItemModel 0x013 chunk
    /// </summary>
    [Chunk(0x2E002013)]
    public class Chunk2E002013 : Chunk<CGameItemModel>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x014 chunk

    /// <summary>
    /// CGameItemModel 0x014 chunk
    /// </summary>
    [Chunk(0x2E002014)]
    public class Chunk2E002014 : Chunk<CGameItemModel>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x015 chunk (item type e)

    /// <summary>
    /// CGameItemModel 0x015 chunk (item type e)
    /// </summary>
    [Chunk(0x2E002015, "item type e")]
    public class Chunk2E002015 : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32(ref n.itemTypeE);
        }
    }

    #endregion

    #region 0x019 chunk (model)

    /// <summary>
    /// CGameItemModel 0x019 chunk (model)
    /// </summary>
    [Chunk(0x2E002019, "model")]
    public class Chunk2E002019 : Chunk<CGameItemModel>, IVersionable
    {
        private int version;

        public int? U01;
        public CMwNod? U02;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if ((n.ItemTypeE == EItemType.Ornament && version < 9) || (n.ItemTypeE == EItemType.Vehicle && version < 10))
            {
                rw.NodeRef<CMwNod>(ref n.phyModelCustom);
                rw.NodeRef<CMwNod>(ref n.visModelCustom);
            }

            if (version >= 3)
            {
                rw.Id(ref n.defaultWeaponName); // DefaultWeaponName

                if (version >= 4)
                {
                    rw.NodeRef<CMwNod>(ref n.phyModelCustom, disallowOverride: true);

                    if (version >= 5)
                    {
                        rw.NodeRef<CMwNod>(ref n.visModelCustom, disallowOverride: true);

                        if (version >= 6)
                        {
                            rw.Int32(ref U01); // Actions

                            if (version >= 7)
                            {
                                rw.EnumInt32<EDefaultCam>(ref n.defaultCam);

                                if (version >= 8)
                                {
                                    rw.NodeRef(ref n.entityModelEdition); // CGameCommonItemEntityModelEdition, CGameBlockItem

                                    //if (version >= 12)
                                    //{
                                        rw.NodeRef(ref n.entityModel);

                                        if (version >= 13)
                                        {
                                            rw.NodeRef(ref U02);
                                        }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CGameItemModel 0x01A chunk
    /// </summary>
    [Chunk(0x2E00201A)]
    public class Chunk2E00201A : Chunk<CGameItemModel>
    {
        private CMwNod? u01;

        public CMwNod? U01
        {
            get => u01;
            set => u01 = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref u01);
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CGameItemModel 0x01B chunk
    /// </summary>
    [Chunk(0x2E00201B)]
    public class Chunk2E00201B : Chunk<CGameItemModel>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x01C chunk

    /// <summary>
    /// CGameItemModel 0x01C chunk
    /// </summary>
    [Chunk(0x2E00201C)]
    public class Chunk2E00201C : Chunk<CGameItemModel>, IVersionable
    {
        private int version;

        public int? U01;
        public float? U02;
        public float? U03;
        public float? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public float? U15;
        public Vec3[]? U16;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 5)
            {
                rw.NodeRef<CGameItemPlacementParam>(ref n.defaultPlacement);
                return;
            }

            rw.Int32(ref U01);

            if (version >= 1)
            {
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                rw.Single(ref U07);

                if (version >= 2)
                {
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);

                    if (version >= 3)
                    {
                        rw.Single(ref U13);
                        rw.Single(ref U14);

                        rw.Array<Vec3>(ref U16, U01.GetValueOrDefault()); // Hack

                        if (version >= 4)
                        {
                            rw.Single(ref U15);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x01D chunk

    /// <summary>
    /// CGameItemModel 0x01D chunk
    /// </summary>
    [Chunk(0x2E00201D)]
    public class Chunk2E00201D : Chunk<CGameItemModel>
    {
        public short U01;

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
        }
    }

    #endregion

    #region 0x01E chunk

    /// <summary>
    /// CGameItemModel 0x01E chunk
    /// </summary>
    [Chunk(0x2E00201E)]
    public class Chunk2E00201E : Chunk<CGameItemModel>
    {
        private int version;
        private int U01;
        private int U02;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref n.archetypeRef!);

            if (n.archetypeRef.Length == 0)
            {
                rw.Int32(ref U01);
            }

            if (version >= 6)
            {
                rw.Int32(ref U02); // SkinDirNameCustom
            }
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CGameItemModel 0x01F chunk
    /// </summary>
    [Chunk(0x2E00201F)]
    public class Chunk2E00201F : Chunk<CGameItemModel>, IVersionable
    {
        private int version;
        
        public Iso4 U01;
        public int U02;
        public string? U03;
        public string? U04;
        public int? U05;
        public int? U06;
        public short? U07;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 7)
            {
                if (version >= 5)
                {
                    rw.String(ref U03);
                    rw.String(ref U04);
                    rw.Int32(ref U05);
                }

                if (version >= 4)
                {
                    rw.Int32(ref U06);
                }

                if (version < 3)
                {
                    rw.Int16(ref U07);
                }
            }

            rw.EnumInt32<EWaypointType>(ref n.waypointType);

            if (version < 8)
            {
                rw.Iso4(ref U01);
            }

            if (version >= 6)
            {
                rw.Boolean(ref n.disableLightmap);
                
                if (version >= 10)
                {
                    rw.Int32(ref U02);
                }
            }
        }
    }

    #endregion

    #region 0x020 chunk

    /// <summary>
    /// CGameItemModel 0x020 chunk
    /// </summary>
    [Chunk(0x2E002020)]
    public class Chunk2E002020 : Chunk<CGameItemModel>, IVersionable
    {
        private int version;
        
        public bool U01;
        public string? U02;
        public CMwNod? U03;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 2)
            {
                rw.String(ref U02);
                rw.NodeRef(ref U03);

                return;
            }

            rw.String(ref n.iconFid);

            if (version >= 3)
            {
                rw.Boolean(ref U01, asByte: true); // ArticlePtr? xD
            }
        }
    }

    #endregion

    #region 0x021 chunk

    /// <summary>
    /// CGameItemModel 0x021 chunk
    /// </summary>
    [Chunk(0x2E002021)]
    public class Chunk2E002021 : Chunk<CGameItemModel>, IVersionable
    {
        private int version;
        public int U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U02); // SItemGroupElement array with Iso4 and CMwNodDataRef
        }
    }

    #endregion

    #region 0x023 chunk

    /// <summary>
    /// CGameItemModel 0x023 chunk
    /// </summary>
    [Chunk(0x2E002023)]
    public class Chunk2E002023 : Chunk<CGameItemModel>, IVersionable
    {
        private int version;

        public byte U01;
        public int U03;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Byte(ref U01);
            rw.Int32(ref U03); // SItemGroupElement array with Iso4 and CMwNodDataRef
        }
    }

    #endregion

    #region 0x024 skippable chunk

    /// <summary>
    /// CGameItemModel 0x024 skippable chunk
    /// </summary>
    [Chunk(0x2E002024)]
    public class Chunk2E002024 : SkippableChunk<CGameItemModel>, IVersionable
    {
        private int version;

        public Vec2[]? U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Array<Vec2>(ref U01);
        }
    }

    #endregion

    #endregion
}
