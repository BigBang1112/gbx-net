using System;

namespace GBX.NET.Engines.Game
{
    [Node(0x03133000)]
    public sealed class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
    {
        #region Properties

        [NodeMember]
        public TimeSpan Start { get; set; }

        [NodeMember]
        public TimeSpan End { get; set; } = TimeSpan.FromSeconds(3);

        [NodeMember]
        public int Target { get; set; }

        #endregion

        #region Constructors

        private CGameCtnMediaBlockVehicleLight()
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
                n.Start = rw.Single_s(n.Start);
                n.End = rw.Single_s(n.End);
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
