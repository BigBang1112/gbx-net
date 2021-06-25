using System;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A9000)]
    public class CGameCtnMediaBlockTrails : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
    {
        #region Fields

        private TimeSpan start;
        private TimeSpan end = TimeSpan.FromSeconds(3);

        #endregion

        #region Properties

        [NodeMember]
        public TimeSpan Start
        {
            get => start;
            set => start = value;
        }

        [NodeMember]
        public TimeSpan End
        {
            get => end;
            set => end = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030A9000)]
        public class Chunk030A9000 : Chunk<CGameCtnMediaBlockTrails>
        {
            public override void ReadWrite(CGameCtnMediaBlockTrails n, GameBoxReaderWriter rw)
            {
                rw.Single_s(ref n.start);
                rw.Single_s(ref n.end);
            }
        }

        #endregion

        #endregion
    }
}
