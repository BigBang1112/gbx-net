namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A060000</remarks>
[Class(0x0A060000)]
public partial class CSceneVehicle : CSceneMobil, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A060000;




    private CPlugVehiclePhyTunings? vehicleTunings;
    [AppliedWithChunk<Chunk0A060000>]
    public CPlugVehiclePhyTunings? VehicleTunings { get => vehicleTuningsFile?.GetNode(ref vehicleTunings) ?? vehicleTunings; set => vehicleTunings = value; }
    private Components.GbxRefTableFile? vehicleTuningsFile;
    public Components.GbxRefTableFile? VehicleTuningsFile { get => vehicleTuningsFile; set => vehicleTuningsFile = value; }
    public CPlugVehiclePhyTunings? GetVehicleTunings(GbxReadSettings settings = default, bool exceptions = false) => vehicleTuningsFile?.GetNode(ref vehicleTunings, settings, exceptions) ?? vehicleTunings;

    private CMwRefBuffer? vehicleMaterials;
    [AppliedWithChunk<Chunk0A060000>]
    public CMwRefBuffer? VehicleMaterials { get => vehicleMaterialsFile?.GetNode(ref vehicleMaterials) ?? vehicleMaterials; set => vehicleMaterials = value; }
    private Components.GbxRefTableFile? vehicleMaterialsFile;
    public Components.GbxRefTableFile? VehicleMaterialsFile { get => vehicleMaterialsFile; set => vehicleMaterialsFile = value; }
    public CMwRefBuffer? GetVehicleMaterials(GbxReadSettings settings = default, bool exceptions = false) => vehicleMaterialsFile?.GetNode(ref vehicleMaterials, settings, exceptions) ?? vehicleMaterials;

    private CSceneVehicleEnvironment? environment;
    [AppliedWithChunk<Chunk0A060000>]
    public CSceneVehicleEnvironment? Environment { get => environment; set => environment = value; }

    private CPlugVehicleVisModelShared? vehicleStruct;
    [AppliedWithChunk<Chunk0A060000>]
    public CPlugVehicleVisModelShared? VehicleStruct { get => vehicleStructFile?.GetNode(ref vehicleStruct) ?? vehicleStruct; set => vehicleStruct = value; }
    private Components.GbxRefTableFile? vehicleStructFile;
    public Components.GbxRefTableFile? VehicleStructFile { get => vehicleStructFile; set => vehicleStructFile = value; }
    public CPlugVehicleVisModelShared? GetVehicleStruct(GbxReadSettings settings = default, bool exceptions = false) => vehicleStructFile?.GetNode(ref vehicleStruct, settings, exceptions) ?? vehicleStruct;

    /// <summary>
    /// Creates a new instance of <see cref="CSceneVehicle"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneVehicle() { }


    /// <summary>
    /// CSceneVehicle 0x000 chunk
    /// </summary>
    [Chunk(0x0A060000)]
    public partial class Chunk0A060000 : Chunk<CSceneVehicle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A060000;

        public CMwNod? U01;

        public override void ReadWrite(CSceneVehicle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugVehiclePhyTunings>(ref n.vehicleTunings, ref n.vehicleTuningsFile);
            rw.NodeRef<CMwRefBuffer>(ref n.vehicleMaterials, ref n.vehicleMaterialsFile);
            rw.NodeRef<CSceneVehicleEnvironment>(ref n.environment);
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CPlugVehicleVisModelShared>(ref n.vehicleStruct, ref n.vehicleStructFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A060000 => new Chunk0A060000(),
        _ => base.NewChunk(chunkId),
    };
}
