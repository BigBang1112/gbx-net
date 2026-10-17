namespace GBX.NET.Engines.Scene;

public partial class CSceneVehicleCar
{
    public partial class Chunk0A02B003
    {
        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef(n.References.VehicleTunings);
        }
    }

    public partial class Chunk0A02B008
    {
        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef(n.References.VehicleMaterials);
        }
    }

    public partial class Chunk0A02B014
    {
        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef(n.References.VehicleStruct);
        }
    }
}
