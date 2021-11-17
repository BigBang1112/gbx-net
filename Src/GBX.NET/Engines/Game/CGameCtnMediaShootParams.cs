namespace GBX.NET.Engines.Game;

[Node(0x03060000)]
public class CGameCtnMediaShootParams : CMwNod // CGameCtnMediaVideoParams
{
    private CGameCtnMediaShootParams()
    {

    }

    [Chunk(0x03060001)]
    public class Chunk03060001 : Chunk<CGameCtnMediaShootParams>
    {
        public override void ReadWrite(CGameCtnMediaShootParams n, GameBoxReaderWriter rw)
        {
            var u01 = rw.Array<int>(count: 10);
        }
    }
}
