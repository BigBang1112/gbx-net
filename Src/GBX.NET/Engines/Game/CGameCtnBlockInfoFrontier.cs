namespace GBX.NET.Engines.Game;

[Node(0x03050000)]
[NodeExtension("EDFrontier")]
[NodeExtension("TMEDFrontier")]
public class CGameCtnBlockInfoFrontier : CGameCtnBlockInfo
{
    protected CGameCtnBlockInfoFrontier()
    {
        
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnBlockInfoFrontier 0x000 chunk
    /// </summary>
    [Chunk(0x03050000)]
    public class Chunk03050000 : Chunk<CGameCtnBlockInfoFrontier>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnBlockInfoFrontier n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion
}