namespace GBX.NET.Engines.Game
{
    [Node(0x0315C000)]
    public class CGameCtnBlockInfoVariantGround : CGameCtnBlockInfoVariant
    {
        [Chunk(0x0315C001)]
        public class Chunk0315C001 : Chunk<CGameCtnBlockInfoVariantGround>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariantGround n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}
