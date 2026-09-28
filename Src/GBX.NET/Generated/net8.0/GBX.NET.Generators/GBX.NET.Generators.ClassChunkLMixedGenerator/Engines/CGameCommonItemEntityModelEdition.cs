namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E026000</remarks>
[Class(0x2E026000)]
public partial class CGameCommonItemEntityModelEdition : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E026000;




    private EItemType itemType;
    [AppliedWithChunk<Chunk2E026000>]
    public EItemType ItemType { get => itemType; set => itemType = value; }

    private CPlugCrystal? meshCrystal;
    [AppliedWithChunk<Chunk2E026000>]
    public CPlugCrystal? MeshCrystal { get => meshCrystal; set => meshCrystal = value; }

    private SpriteParam[]? spriteParams;
    [AppliedWithChunk<Chunk2E026000>]
    public SpriteParam[]? SpriteParams { get => spriteParams; set => spriteParams = value; }

    private float mass;
    [AppliedWithChunk<Chunk2E026000>]
    public float Mass { get => mass; set => mass = value; }

    private string? inventoryName;
    [AppliedWithChunk<Chunk2E026000>]
    public string? InventoryName { get => inventoryName; set => inventoryName = value; }

    private string? inventoryDescription;
    [AppliedWithChunk<Chunk2E026000>]
    public string? InventoryDescription { get => inventoryDescription; set => inventoryDescription = value; }

    private int inventoryItemClass;
    [AppliedWithChunk<Chunk2E026000>]
    public int InventoryItemClass { get => inventoryItemClass; set => inventoryItemClass = value; }

    private int inventoryOccupation;
    [AppliedWithChunk<Chunk2E026000>]
    public int InventoryOccupation { get => inventoryOccupation; set => inventoryOccupation = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCommonItemEntityModelEdition"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCommonItemEntityModelEdition() { }


    /// <summary>
    /// CGameCommonItemEntityModelEdition 0x000 chunk
    /// </summary>
    [Chunk(0x2E026000)]
    public partial class Chunk2E026000 : Chunk<CGameCommonItemEntityModelEdition>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E026000;

        public int Version { get; set; }

        public string? U01;
        /// <summary>
        /// if U01 is empty probably
        /// </summary>
        public CPlugSolid? U02;
        public CPlugFileImg[]? U03;
        public CPlugParticleEmitterModel? U04;
        public CPlugAnimLocSimple? U05;
        public LightBallStateSimple[]? U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public Iso4 U14;
        public bool U15;
        public CPlugCrystal? U16;
        public bool U17;
        public int? U18;
        public Iso4? U19;
        public int U20;
        public CMwNod? U21;
        public bool U22;

        public override void ReadWrite(CGameCommonItemEntityModelEdition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumInt32<EItemType>(ref n.itemType);
            rw.NodeRef<CPlugCrystal>(ref n.meshCrystal);
            rw.String(ref U01);
            rw.NodeRef<CPlugSolid>(ref U02); // if U01 is empty probably
            rw.ArrayNodeRef<CPlugFileImg>(ref U03!);
            rw.ArrayReadableWritable<SpriteParam>(ref n.spriteParams!);
            rw.NodeRef<CPlugParticleEmitterModel>(ref U04);
            rw.NodeRef<CPlugAnimLocSimple>(ref U05);
            rw.ArrayReadableWritable<LightBallStateSimple>(ref U06!);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Iso4(ref U14);
            if (Version >= 3)
            {
                if (n.ItemType==EItemType.PickUp)
                {
                    rw.Single(ref n.mass);
                }
            }
            rw.Boolean(ref U15);
            if (!U15)
            {
                rw.NodeRef<CPlugCrystal>(ref U16);
            }
            if (n.ItemType!=EItemType.Ornament)
            {
                throw new ("");
            }
            rw.Boolean(ref U17);
            if (U17)
            {
                rw.Int32(ref U18);
                rw.Iso4(ref U19);
            }
            rw.Int32(ref U20);
            if (Version >= 1)
            {
                rw.String(ref n.inventoryName);
                rw.String(ref n.inventoryDescription);
                rw.Int32(ref n.inventoryItemClass);
                rw.Int32(ref n.inventoryOccupation);
                if (Version >= 6)
                {
                    if (Version <= 7)
                    {
                        rw.NodeRef<CMwNod>(ref U21);
                    }
                    if (Version >= 7)
                    {
                        if (n.ItemType==EItemType.PickUp)
                        {
                            rw.Boolean(ref U22);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCommonItemEntityModelEdition 0x001 skippable chunk
    /// </summary>
    [Chunk(0x2E026001)]
    public partial class Chunk2E026001 : SkippableChunk<CGameCommonItemEntityModelEdition>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E026001;

        /// <inheritdoc />
        public override bool Ignore => true;

    }


    public sealed partial class SpriteParam : IReadableWritable
    {

        private Vec3 u01;
        public Vec3 U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Vec3(ref u01);
            rw.Boolean(ref u02);
            rw.Single(ref u03);
        }
    }

    public sealed partial class LightBallStateSimple : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
        }
    }


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
        Procedural,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E026000 => new Chunk2E026000(),
        0x2E026001 => new Chunk2E026001(),
        _ => base.NewChunk(chunkId),
    };
}
