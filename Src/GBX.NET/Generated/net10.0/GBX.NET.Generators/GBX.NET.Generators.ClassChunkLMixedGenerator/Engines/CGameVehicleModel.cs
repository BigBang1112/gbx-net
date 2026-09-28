namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E01C000</remarks>
[Class(0x2E01C000)]
public partial class CGameVehicleModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E01C000;




    private CPlugVehiclePhyModel? phyModel;
    [AppliedWithChunk<Chunk2E01C000>]
    public CPlugVehiclePhyModel? PhyModel { get => phyModelFile?.GetNode(ref phyModel) ?? phyModel; set => phyModel = value; }
    private Components.GbxRefTableFile? phyModelFile;
    public Components.GbxRefTableFile? PhyModelFile { get => phyModelFile; set => phyModelFile = value; }
    public CPlugVehiclePhyModel? GetPhyModel(GbxReadSettings settings = default, bool exceptions = false) => phyModelFile?.GetNode(ref phyModel, settings, exceptions) ?? phyModel;

    private CPlugVehicleVisModel? visModel;
    [AppliedWithChunk<Chunk2E01C000>]
    public CPlugVehicleVisModel? VisModel { get => visModelFile?.GetNode(ref visModel) ?? visModel; set => visModel = value; }
    private Components.GbxRefTableFile? visModelFile;
    public Components.GbxRefTableFile? VisModelFile { get => visModelFile; set => visModelFile = value; }
    public CPlugVehicleVisModel? GetVisModel(GbxReadSettings settings = default, bool exceptions = false) => visModelFile?.GetNode(ref visModel, settings, exceptions) ?? visModel;

    private ItemOccupantSlotModel[]? itemOccupantSlotModels;
    [AppliedWithChunk<Chunk2E01C000>]
    public ItemOccupantSlotModel[]? ItemOccupantSlotModels { get => itemOccupantSlotModels; set => itemOccupantSlotModels = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameVehicleModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameVehicleModel() { }


    /// <summary>
    /// CGameVehicleModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E01C000)]
    public partial class Chunk2E01C000 : Chunk<CGameVehicleModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E01C000;

        public int Version { get; set; }

        public CGameObjectModel[]? U01;
        public CMwNod? U02;

        public override void ReadWrite(CGameVehicleModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugVehiclePhyModel>(ref n.phyModel, ref n.phyModelFile);
            rw.NodeRef<CPlugVehicleVisModel>(ref n.visModel, ref n.visModelFile);
            rw.ArrayReadableWritable<ItemOccupantSlotModel>(ref n.itemOccupantSlotModels!);
            if (Version >= 1)
            {
                rw.ArrayNodeRef<CGameObjectModel>(ref U01!);
                if (Version == 2)
                {
                    rw.NodeRef<CMwNod>(ref U02);
                }
            }
        }
    }


    public sealed partial class ItemOccupantSlotModel : IReadableWritable
    {

        private string? id;
        public string? Id { get => id; set => id = value; }

        private CGameActionModel[]? actions;
        public CGameActionModel[]? Actions { get => actions; set => actions = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref id);
            rw.ArrayNodeRef<CGameActionModel>(ref actions!);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E01C000 => new Chunk2E01C000(),
        _ => base.NewChunk(chunkId),
    };
}
