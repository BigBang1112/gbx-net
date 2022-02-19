namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Vehicle light (0x03133000)
/// </summary>
[Node(0x03133000)]
[NodeExtension("GameCtnMediaBlockVehicleLight")]
public class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Properties

    [NodeMember]
    public TimeSingle Start { get; set; }

    [NodeMember]
    public TimeSingle End { get; set; } = TimeSingle.FromSeconds(3);

    [NodeMember]
    public int Target { get; set; }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockVehicleLight()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03133000)]
    public class Chunk03133000 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GameBoxReaderWriter rw)
        {
            n.Start = rw.TimeSingle(n.Start);
            n.End = rw.TimeSingle(n.End);
        }
    }

    #endregion

    #region 0x001 chunk (target)

    [Chunk(0x03133001, "target")]
    public class Chunk03133001 : Chunk<CGameCtnMediaBlockVehicleLight>
    {
        public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GameBoxReaderWriter rw)
        {
            n.Target = rw.Int32(n.Target);
        }
    }

    #endregion

    #endregion
}
