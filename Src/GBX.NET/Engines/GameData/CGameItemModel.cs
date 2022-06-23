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

    #endregion

    #region Fields

    private EItemType itemType;
    private CMwNod?[]? nadeoSkinFids;
    private CMwNod?[]? cameras;
    private CMwNod? raceInterfaceFid;
    private Vec3 groundPoint;
    private float painterGroundMargin;
    private float orbitalCenterHeightFromGround;
    private float orbitalRadiusBase;
    private float orbitalPreviewAngle;
    private CMwNod? baseAttributes;
    private string? defaultWeaponName;
    private CPlugVehiclePhyModelCustom? phyModelCustom;
    private CMwNod? visModelCustom;
    private CMwNod? entityModelEdition;
    private CMwNod? entityModel;
    private int? defaultCam;
    private CGameItemPlacementParam? itemPlacement;
    private string? archetypeRef;
    private string? iconFid;

    #endregion

    #region Properties

    [NodeMember]
    public EItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }

    [NodeMember]
    public CMwNod?[]? NadeoSkinFids
    {
        get => nadeoSkinFids;
        set => nadeoSkinFids = value;
    }

    [NodeMember]
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

    [NodeMember]
    public Vec3 GroundPoint
    {
        get => groundPoint;
        set => groundPoint = value;
    }

    [NodeMember]
    public float PainterGroundMargin
    {
        get => painterGroundMargin;
        set => painterGroundMargin = value;
    }

    [NodeMember]
    public float OrbitalCenterHeightFromGround
    {
        get => orbitalCenterHeightFromGround;
        set => orbitalCenterHeightFromGround = value;
    }

    [NodeMember]
    public float OrbitalRadiusBase
    {
        get => orbitalRadiusBase;
        set => orbitalRadiusBase = value;
    }

    [NodeMember]
    public float OrbitalPreviewAngle
    {
        get => orbitalPreviewAngle;
        set => orbitalPreviewAngle = value;
    }

    [NodeMember]
    public CMwNod? BaseAttributes
    {
        get => baseAttributes;
        set => baseAttributes = value;
    }

    [NodeMember]
    public string? DefaultWeaponName
    {
        get => defaultWeaponName;
        set => defaultWeaponName = value;
    }

    [NodeMember]
    public CPlugVehiclePhyModelCustom? PhyModelCustom
    {
        get => phyModelCustom;
        set => phyModelCustom = value;
    }

    [NodeMember]
    public CMwNod? VisModelCustom
    {
        get => visModelCustom;
        set => visModelCustom = value;
    }

    [NodeMember]
    public int? DefaultCam
    {
        get => defaultCam;
        set => defaultCam = value;
    }

    /// <summary>
    /// An entity object of the item model.
    /// </summary>
    [NodeMember]
    public CMwNod? EntityModelEdition
    {
        get => entityModelEdition;
        set => entityModelEdition = value;
    }

    [NodeMember]
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
    [NodeMember]
    public CGameItemPlacementParam? ItemPlacement
    {
        get => itemPlacement;
        set => itemPlacement = value;
    }

    [NodeMember]
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

    #region 0x013 (base attributes)

    /// <summary>
    /// CGameItemModel 0x013 chunk (base attributes)
    /// </summary>
    [Chunk(0x2E002013, "base attributes")]
    public class Chunk2E002013 : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.baseAttributes);
        }
    }

    #endregion

    #region 0x015 chunk (item type)

    /// <summary>
    /// CGameItemModel 0x015 chunk (item type)
    /// </summary>
    [Chunk(0x2E002015, "item type")]
    public class Chunk2E002015 : Chunk<CGameItemModel>
    {
        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32(ref n.itemType);
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
        private int? u01;
        private CMwNod? u02;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public int? U01
        {
            get => u01;
            set => u01 = value;
        }

        public CMwNod? U02
        {
            get => u02;
            set => u02 = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version == 1)
            {

            }

            if (version >= 3)
            {
                rw.Id(ref n.defaultWeaponName); // DefaultWeaponName

                if (version >= 4)
                {
                    rw.NodeRef(ref n.phyModelCustom);

                    if (version >= 5)
                    {
                        rw.NodeRef(ref n.visModelCustom);

                        if (version >= 6)
                        {
                            rw.Int32(ref u01); // Actions

                            if (version >= 7)
                            {
                                rw.Int32(ref n.defaultCam);

                                if (version >= 8)
                                {
                                    rw.NodeRef(ref n.entityModelEdition); // CGameCommonItemEntityModelEdition, CGameBlockItem

                                    if (version >= 12)
                                    {
                                        rw.NodeRef(ref n.entityModel);

                                        if (version >= 13)
                                        {
                                            rw.NodeRef(ref u02);
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

    #region 0x01C chunk

    /// <summary>
    /// CGameItemModel 0x01C chunk
    /// </summary>
    [Chunk(0x2E00201C)]
    public class Chunk2E00201C : Chunk<CGameItemModel>
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
            rw.NodeRef(ref n.itemPlacement);
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

            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x01F chunk

    /// <summary>
    /// CGameItemModel 0x01F chunk
    /// </summary>
    [Chunk(0x2E00201F)]
    public class Chunk2E00201F : Chunk<CGameItemModel>
    {
        private int version;
        private int u01;
        private int u02;
        private int u03;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public int U01
        {
            get => u01;
            set => u01 = value;
        }

        public int U02
        {
            get => u02;
            set => u02 = value;
        }

        public int U03
        {
            get => u03;
            set => u03 = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref u01);
            rw.Int32(ref u02);

            if (version >= 10)
            {
                rw.Int32(ref u03);
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

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameItemModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
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
