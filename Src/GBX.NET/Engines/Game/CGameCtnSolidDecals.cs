namespace GBX.NET.Engines.Game;

[Node(0x03121000)]
public class CGameCtnSolidDecals : CMwNod
{
    protected CGameCtnSolidDecals()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnSolidDecals 0x001 chunk
    /// </summary>
    [Chunk(0x03121001)]
    public class Chunk03121001 : Chunk<CGameCtnSolidDecals>
    {
        public int U01;
        public byte[]? U02;

        public override void ReadWrite(CGameCtnSolidDecals n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Bytes(ref U02);
        }
    }

    #endregion
}
