namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0911E000</remarks>
[Node(0x0911E000)]
public class CPlugVehiclePhyModelCustom : CMwNod
{
    private float accelCoef;
    private float controlCoef;
    private float gravityCoef;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0911E000))]
    public float AccelCoef { get => accelCoef; set => accelCoef = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0911E000))]
    public float ControlCoef { get => controlCoef; set => controlCoef = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0911E000))]
    public float GravityCoef { get => gravityCoef; set => gravityCoef = value; }

    protected CPlugVehiclePhyModelCustom()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVehiclePhyModelCustom 0x000 chunk
    /// </summary>
    [Chunk(0x0911E000)]
    public class Chunk0911E000 : Chunk<CPlugVehiclePhyModelCustom>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugVehiclePhyModelCustom n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Single(ref n.accelCoef);
            rw.Single(ref n.controlCoef);
            rw.Single(ref n.gravityCoef);
        }
    }

    #endregion
}
