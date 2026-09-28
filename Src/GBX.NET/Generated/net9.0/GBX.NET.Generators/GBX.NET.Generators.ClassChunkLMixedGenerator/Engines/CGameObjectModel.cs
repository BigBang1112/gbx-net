namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E01D000</remarks>
[Class(0x2E01D000)]
public partial class CGameObjectModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E01D000;




    private CGameObjectPhyModel? phy;
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectPhyModel? Phy { get => phy; set => phy = value; }

    private CGameObjectVisModel? vis;
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectVisModel? Vis { get => visFile?.GetNode(ref vis) ?? vis; set => vis = value; }
    private Components.GbxRefTableFile? visFile;
    public Components.GbxRefTableFile? VisFile { get => visFile; set => visFile = value; }
    public CGameObjectVisModel? GetVis(GbxReadSettings settings = default, bool exceptions = false) => visFile?.GetNode(ref vis, settings, exceptions) ?? vis;

    private string? inventoryName;
    [AppliedWithChunk<Chunk2E01D000>]
    public string? InventoryName { get => inventoryName; set => inventoryName = value; }

    private string? inventoryDescription;
    [AppliedWithChunk<Chunk2E01D000>]
    public string? InventoryDescription { get => inventoryDescription; set => inventoryDescription = value; }

    private EGameInventoryItemClass inventoryItemClass;
    [AppliedWithChunk<Chunk2E01D000>]
    public EGameInventoryItemClass InventoryItemClass { get => inventoryItemClass; set => inventoryItemClass = value; }

    private int? inventoryOccupation;
    [AppliedWithChunk<Chunk2E01D000>]
    public int? InventoryOccupation { get => inventoryOccupation; set => inventoryOccupation = value; }

    private CGameObjectModel? slaveHealDome;
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectModel? SlaveHealDome { get => slaveHealDome; set => slaveHealDome = value; }

    private CGameObjectModel? slaveShieldDome;
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectModel? SlaveShieldDome { get => slaveShieldDome; set => slaveShieldDome = value; }

    private string? scriptId;
    [AppliedWithChunk<Chunk2E01D000>]
    public string? ScriptId { get => scriptId; set => scriptId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameObjectModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameObjectModel() { }


    /// <summary>
    /// CGameObjectModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E01D000)]
    public partial class Chunk2E01D000 : Chunk<CGameObjectModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E01D000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameObjectModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameObjectPhyModel>(ref n.phy);
            rw.NodeRef<CGameObjectVisModel>(ref n.vis, ref n.visFile);
            if (Version >= 1)
            {
                rw.Int32(ref U01);
                rw.String(ref n.inventoryName);
                rw.String(ref n.inventoryDescription);
                rw.EnumInt32<EGameInventoryItemClass>(ref n.inventoryItemClass);
                rw.Int32(ref n.inventoryOccupation);
                if (Version >= 2)
                {
                    rw.NodeRef<CGameObjectModel>(ref n.slaveHealDome);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CGameObjectModel>(ref n.slaveShieldDome);
                        if (Version >= 4)
                        {
                            rw.String(ref n.scriptId);
                        }
                    }
                }
            }
        }
    }



    public enum EGameInventoryItemClass
    {
        Weapon,
        Movement,
        Consumable,
        Armor,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E01D000 => new Chunk2E01D000(),
        _ => base.NewChunk(chunkId),
    };
}
