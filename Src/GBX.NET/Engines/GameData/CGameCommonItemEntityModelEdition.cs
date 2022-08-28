namespace GBX.NET.Engines.GameData;

/// <summary>
/// Custom item data.
/// </summary>
/// <remarks>ID: 0x2E025000</remarks>
[Node(0x2E026000)]
public class CGameCommonItemEntityModelEdition : CMwNod
{
    #region Enums

    public enum EItemType
    {
        Undefined,
        Ornament,
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
        DeprecV,
        Procedural
    }

    #endregion

    #region Fields

    private EItemType itemType;
    private CPlugCrystal meshCrystal;
    private float? mass;
    private string? inventoryName;
    private string? inventoryDescription;
    private int inventoryItemClass;
    private int inventoryOccupation;

    #endregion

    #region Properties

    /// <summary>
    /// Type of the item.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000))]
    public EItemType ItemType { get => itemType; set => itemType = value; }

    /// <summary>
    /// Mesh of the item model.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000))]
    public CPlugCrystal MeshCrystal { get => meshCrystal; set => meshCrystal = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000), sinceVersion: 3)]
    public float? Mass { get => mass; set => mass = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000), sinceVersion: 1)]
    public string? InventoryName { get => inventoryName; set => inventoryName = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000), sinceVersion: 1)]
    public string? InventoryDescription { get => inventoryDescription; set => inventoryDescription = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000), sinceVersion: 1)]
    public int InventoryItemClass { get => inventoryItemClass; set => inventoryItemClass = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E026000), sinceVersion: 1)]
    public int InventoryOccupation { get => inventoryOccupation; set => inventoryOccupation = value; }

    #endregion

    #region Constructors

    protected CGameCommonItemEntityModelEdition()
    {
        meshCrystal = null!;
    }

    #endregion

    #region 0x000 chunk

    /// <summary>
    /// CGameCommonItemEntityModelEdition 0x000 chunk
    /// </summary>
    [Chunk(0x2E026000)]
    public class Chunk2E026000 : Chunk<CGameCommonItemEntityModelEdition>, IVersionable
    {
        private int version;

        public string? U01;
        public CMwNod? U02;
        public int U03;
        public int U04;
        public CMwNod? U05;
        public CMwNod? U06;
        public int U07;
        public string? U08;
        public string? U09;
        public string? U10;
        public string? U11;
        public string? U12;
        public string? U13;
        public string? U14;
        public Iso4 U15;
        public bool U16;
        public bool U17;
        public int? U18;
        public Iso4? U19;
        public int U20;
        public CMwNod? U21;
        public CMwNod? U22;
        public bool? U23;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCommonItemEntityModelEdition n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            
            rw.EnumInt32<EItemType>(ref n.itemType);
            rw.NodeRef<CPlugCrystal>(ref n.meshCrystal!);
            rw.String(ref U01);
            rw.NodeRef(ref U02); // if U01 is empty probably

            rw.Int32(ref U03); // CPlugFileImg array
            if (U03 > 0) throw new Exception("CPlugFileImg array not empty");
            
            rw.Int32(ref U04); // SSpriteParam array
            if (U04 > 0) throw new Exception("SSpriteParam array not empty");

            rw.NodeRef(ref U05); // DestroyParticleModel?
            rw.NodeRef(ref U06); // DestroyParticleModel?

            rw.Int32(ref U07); // SPlugLightBallStateSimple array
            if (U07 > 0) throw new Exception("SPlugLightBallStateSimple array not empty");

            rw.String(ref U08);
            rw.String(ref U09);

            rw.String(ref U10);
            rw.String(ref U11);
            rw.String(ref U12);
            rw.String(ref U13);
            rw.String(ref U14);

            rw.Iso4(ref U15);

            if (version >= 3 && n.itemType == EItemType.PickUp)
            {
                rw.Single(ref n.mass);
            }
 
            rw.Boolean(ref U16);

            if (!U16)
            {
                rw.NodeRef(ref U21);
            }

            if (n.itemType != EItemType.Ornament)
            {
                throw new NotSupportedException("Other item types than Ornament not supported");
            }

            rw.Boolean(ref U17);

            if (U17)
            {
                rw.Int32(ref U18);
                rw.Iso4(ref U19);
            }

            rw.Int32(ref U20);

            if (version >= 1)
            {
                rw.String(ref n.inventoryName);
                rw.String(ref n.inventoryDescription);
                rw.Int32(ref n.inventoryItemClass);
                rw.Int32(ref n.inventoryOccupation);

                if (version >= 6)
                {
                    rw.NodeRef(ref U22);

                    if (version >= 7 && n.itemType == EItemType.PickUp)
                    {
                        rw.Boolean(ref U23);
                    }
                }
            }
        }
    }

    #endregion
}
