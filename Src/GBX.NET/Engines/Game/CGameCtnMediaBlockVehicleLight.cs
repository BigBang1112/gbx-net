namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Vehicle light.
/// </summary>
/// <remarks>ID: 0x03133000</remarks>
[Node(0x03133000)]
[NodeExtension("GameCtnMediaBlockVehicleLight")]
public class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private int target;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03133000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03133000))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03133001))]
    public int Target { get => target; set => target = value; }

    protected CGameCtnMediaBlockVehicleLight()
    {

    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockVehicleLight 0x000 chunk
    /// </summary>
    [Chunk(0x03133000)]
    public class Chunk03133000 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #region 0x001 chunk (target)

    /// <summary>
    /// CGameCtnMediaBlockVehicleLight 0x001 chunk (target)
    /// </summary>
    [Chunk(0x03133001, "target")]
    public class Chunk03133001 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.target);
        }
    }

    #endregion

    #endregion
}
