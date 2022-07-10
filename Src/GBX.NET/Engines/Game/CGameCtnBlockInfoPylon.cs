namespace GBX.NET.Engines.Game;

[Node(0x03055000)]
public class CGameCtnBlockInfoPylon : CGameCtnBlockInfo
{
    public enum EPylonAmount
    {
        PylonsX4,
        PylonsX8
    }

    public enum EPylonPlacement
    {
        SustainRoadCore,
        SustainRoadExterior
    }
    
    private float pylonOffset;
    private EPylonAmount pylonAmount;
    private EPylonPlacement pylonPlacement;
    private int blockHeightOffset;

    protected CGameCtnBlockInfoPylon()
    {
        
    }

    [NodeMember(ExactlyNamed = true)]
    public float PylonOffset { get => pylonOffset; set => pylonOffset = value; }

    [NodeMember(ExactlyNamed = true)]
    public EPylonAmount PylonAmount { get => pylonAmount; set => pylonAmount = value; }

    [NodeMember(ExactlyNamed = true)]
    public EPylonPlacement PylonPlacement { get => pylonPlacement; set => pylonPlacement = value; }

    [NodeMember(ExactlyNamed = true)]
    public int BlockHeightOffset { get => blockHeightOffset; set => blockHeightOffset = value; }

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnBlockInfoPylon 0x002 chunk
    /// </summary>
    [Chunk(0x03055002)]
    public class Chunk03055002 : Chunk<CGameCtnBlockInfoPylon>
    {
        public override void ReadWrite(CGameCtnBlockInfoPylon n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.pylonOffset);
            rw.EnumInt32<EPylonAmount>(ref n.pylonAmount);
            rw.EnumInt32<EPylonPlacement>(ref n.pylonPlacement);
            rw.Int32(ref n.blockHeightOffset);
        }
    }

    #endregion
}