namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public partial class Chunk0A02B003
    {
        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            var vehicleTuningsFile = n.VehicleTuningsFile;
            n.VehicleTunings = rw.NodeRef(n.VehicleTunings, ref vehicleTuningsFile);
            n.VehicleTuningsFile = vehicleTuningsFile;
        }
    }

    public partial class Chunk0A02B008
    {
        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            var vehicleMaterialsFile = n.VehicleMaterialsFile;
            n.VehicleMaterials = rw.NodeRef(n.VehicleMaterials, ref vehicleMaterialsFile);
            n.VehicleMaterialsFile = vehicleMaterialsFile;
        }
    }
}
