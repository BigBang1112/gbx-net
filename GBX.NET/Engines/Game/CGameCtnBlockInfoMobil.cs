namespace GBX.NET.Engines.Game
{
    [Node(0x03122000)]
    public class CGameCtnBlockInfoMobil : Node
    {
        [Chunk(0x03122002)]
        public class Chunk03122002 : Chunk<CGameCtnBlockInfoMobil>
        {
            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03122003)]
        public class Chunk03122003 : Chunk<CGameCtnBlockInfoMobil>
        {
            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03122004)]
        public class Chunk03122004 : Chunk<CGameCtnBlockInfoMobil>
        {
            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown); //CGameCtnBlockInfoMobilLink
                rw.Int32(Unknown);
            }
        }
    }
}