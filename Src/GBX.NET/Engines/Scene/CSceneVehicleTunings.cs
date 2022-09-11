namespace GBX.NET.Engines.Scene;

[Node(0x0A030000)]
[NodeExtension("VehicleTunings")]
public class CSceneVehicleTunings : CMwNod
{
    private CSceneVehicleCarTuning[] tuning;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0A030000))]
    public CSceneVehicleCarTuning[] Tuning { get => tuning; set => tuning = value; }

    protected CSceneVehicleTunings()
    {
        tuning = Array.Empty<CSceneVehicleCarTuning>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CSceneVehicleTunings 0x000 chunk
    /// </summary>
    [Chunk(0x0A030000)]
    public class Chunk0A030000 : Chunk<CSceneVehicleTunings>
    {
        public int U01;

        public override void ReadWrite(CSceneVehicleTunings n, GameBoxReaderWriter rw)
        {
            rw.Int32(10);
            rw.ArrayNode(ref n.tuning!);
            rw.Int32(ref U01);
        }
    }

    #endregion
}
