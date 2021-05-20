namespace GBX.NET.Engines.Game
{
    [Node(0x03133000)]
    public class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public int Target { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03133000)]
        public class Chunk03133000 : Chunk<CGameCtnMediaBlockVehicleLight>
        {
            public override void ReadWrite(CGameCtnMediaBlockVehicleLight n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
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
}
