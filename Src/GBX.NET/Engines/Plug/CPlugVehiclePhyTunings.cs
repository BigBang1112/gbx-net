namespace GBX.NET.Engines.Plug;

[Node(0x090EC000)] // previously CSceneVehicleTunings
[NodeExtension("VehicleTunings")]
public class CPlugVehiclePhyTunings : CMwNod
{
    private CPlugVehiclePhyTuning[] tuning;
    private int tuningIndex;

    [NodeMember]
    [AppliedWithChunk<Chunk090EC000>]
    public CPlugVehiclePhyTuning[] Tuning { get => tuning; set => tuning = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090EC000>]
    public int TuningIndex { get => tuningIndex; set => tuningIndex = value; }

    internal CPlugVehiclePhyTunings()
    {
        tuning = Array.Empty<CPlugVehiclePhyTuning>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVehiclePhyTunings 0x000 chunk
    /// </summary>
    [Chunk(0x090EC000)]
    public class Chunk090EC000 : Chunk<CPlugVehiclePhyTunings>
    {
        public override void ReadWrite(CPlugVehiclePhyTunings n, GameBoxReaderWriter rw)
        {
            rw.Int32(10);
            rw.ArrayNode(ref n.tuning!);
            rw.Int32(ref n.tuningIndex);
        }
    }

    #endregion
}
