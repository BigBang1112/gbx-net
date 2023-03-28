namespace GBX.NET.Engines.GameData;

[Node(0x2E01C000)]
public class CGameVehicleModel : CMwNod
{
    private CPlugVehiclePhyModel? phyModel;
    private GameBoxRefTable.File? phyModelFile;
    private CPlugVehicleVisModel? visModel;
    private GameBoxRefTable.File? visModelFile;
    private ItemOccupantSlotModel[] itemOccupantSlotModels = Array.Empty<ItemOccupantSlotModel>();

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01C000>]
    public CPlugVehiclePhyModel? PhyModel
    {
        get => phyModel = GetNodeFromRefTable(phyModel, phyModelFile) as CPlugVehiclePhyModel;
        set => phyModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01C000>]
    public CPlugVehicleVisModel? VisModel
    {
        get => visModel = GetNodeFromRefTable(visModel, visModelFile) as CPlugVehicleVisModel;
        set => visModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E01C000>]
    public ItemOccupantSlotModel[] ItemOccupantSlotModels { get => itemOccupantSlotModels; set => itemOccupantSlotModels = value; }

    internal CGameVehicleModel()
    {
        
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameVehicleModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E01C000)]
    public class Chunk2E01C000 : Chunk<CGameVehicleModel>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameVehicleModel n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.NodeRef<CPlugVehiclePhyModel>(ref n.phyModel, ref n.phyModelFile);
            rw.NodeRef<CPlugVehicleVisModel>(ref n.visModel, ref n.visModelFile);

            rw.ArrayArchive<ItemOccupantSlotModel>(ref n.itemOccupantSlotModels!);
        }
    }

    #endregion

    public class ItemOccupantSlotModel : IReadableWritable
    {
        private string id = "";
        private CGameActionModel?[] actions = Array.Empty<CGameActionModel>();

        public string Id { get => id; set => id = value; }
        public CGameActionModel?[] Actions { get => actions; set => actions = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref id!);
            rw.ArrayNode<CGameActionModel>(ref actions!);
        }
    }
}
