namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E01D000</remarks>
[Node(0x2E01D000)]
public class CGameObjectModel : CMwNod
{
    public enum EGameInventoryItemClass
    {
        Weapon,
        Movement,
        Consumable,
        Armor
    }

    private CGameObjectPhyModel? phy;
    private CGameObjectVisModel? vis;
    private string? inventoryName;
    private string? inventoryDescription;
    private EGameInventoryItemClass? inventoryItemClass;
    private int? inventoryOccupation;
    private CGameObjectModel? slaveHealDome;
    private CGameObjectModel? slaveShieldDome;
    private string? scriptId;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectPhyModel? Phy { get => phy; set => phy = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01D000>]
    public CGameObjectVisModel? Vis { get => vis; set => vis = value; }

    [NodeMember(ExactName = "m_InventoryParams_InventoryName")]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 1)]
    public string? InventoryName { get => inventoryName; set => inventoryName = value; }
    
    [NodeMember(ExactName = "m_InventoryParams_InventoryDescription")]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 1)]
    public string? InventoryDescription { get => inventoryDescription; set => inventoryDescription = value; }

    [NodeMember(ExactName = "m_InventoryParams_InventoryItemClass")]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 1)]
    public EGameInventoryItemClass? InventoryItemClass { get => inventoryItemClass; set => inventoryItemClass = value; }

    [NodeMember(ExactName = "m_InventoryParams_InventoryOccupation")]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 1)]
    public int? InventoryOccupation { get => inventoryOccupation; set => inventoryOccupation = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 2)]
    public CGameObjectModel? SlaveHealDome { get => slaveHealDome; set => slaveHealDome = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 3)]
    public CGameObjectModel? SlaveShieldDome { get => slaveShieldDome; set => slaveShieldDome = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01D000>(sinceVersion: 4)]
    public string? ScriptId { get => scriptId; set => scriptId = value; }

    internal CGameObjectModel()
    {

    }

    /// <summary>
    /// CGameObjectModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E01D000)]
    public class Chunk2E01D000 : Chunk<CGameObjectModel>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameObjectModel n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameObjectPhyModel>(ref n.phy);
            rw.NodeRef<CGameObjectVisModel>(ref n.vis);

            if (Version >= 1)
            {
                // CGameObjectModel::SInventoryParams::Archive
                rw.Int32(0);
                rw.String(ref n.inventoryName);
                rw.String(ref n.inventoryDescription);
                rw.EnumInt32<EGameInventoryItemClass>(ref n.inventoryItemClass);
                rw.Int32(ref n.inventoryOccupation);
                //

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
}
