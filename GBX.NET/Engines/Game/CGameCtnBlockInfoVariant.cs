namespace GBX.NET.Engines.Game
{
    [Node(0x0315B000)]
    public class CGameCtnBlockInfoVariant : Node
    {
        public string Name { get; set; }

        [Chunk(0x0315B002)]
        public class Chunk0315B002 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B003)]
        public class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int16(Unknown);
                rw.Byte(Unknown);
            }
        }

        [Chunk(0x0315B004)]
        public class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int16(Unknown);
            }
        }

        [Chunk(0x0315B005)]
        public class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                var mobil = Parse<CGameCtnBlockInfoMobil>(rw.Reader);
            }
        }

        [Chunk(0x0315B006)]
        public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B007)]
        public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B008)]
        public class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                n.Name = rw.LookbackString(n.Name);
            }
        }
    }
}