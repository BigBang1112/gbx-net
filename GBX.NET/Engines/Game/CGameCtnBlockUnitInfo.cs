namespace GBX.NET.Engines.Game
{
    [Node(0x03036000)]
    public class CGameCtnBlockUnitInfo : Node
    {
        public Int3 OffsetE { get; set; }
        public CGameCtnBlockInfoClip[] Clips { get; set; }
        public string TerrainModifierId { get; set; }

        public override string ToString() => OffsetE.ToString();

        [Chunk(0x03036000)]
        public class Chunk03036000 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                n.OffsetE = rw.Int3(n.OffsetE);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03036001)]
        public class Chunk03036001 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Id(Unknown); // Desert, Grass
                rw.Single(Unknown);
                rw.Single(Unknown);
            }
        }

        [Chunk(0x03036003)]
        public class Chunk03036003 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03036004)]
        public class Chunk03036004 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03036005)]
        public class Chunk03036005 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                n.TerrainModifierId = rw.Id(n.TerrainModifierId);
            }
        }

        [Chunk(0x03036007)]
        public class Chunk03036007 : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0303600B)]
        public class Chunk0303600B : Chunk<CGameCtnBlockUnitInfo>
        {
            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0303600C)]
        public class Chunk0303600C : Chunk<CGameCtnBlockUnitInfo>
        {
            public byte[] UnknownData { get; set; }

            public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
            {
                /*rw.Int32(Unknown);
                rw.Int16(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);*/

                UnknownData = rw.Reader.ReadTillFacade();
            }
        }
    }
}
