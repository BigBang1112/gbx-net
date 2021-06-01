using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0307D000)]
    public class CGameCtnMediaBlockUi : CGameCtnMediaBlock
    {
        #region Fields

        private float start;
        private float end = 3;

        #endregion

        #region Properties

        [NodeMember]
        public float Start
        {
            get => start;
            set => start = value;
        }

        [NodeMember]
        public float End
        {
            get => end;
            set => end = value;
        }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x0307D001)]
        public class Chunk0307D001 : Chunk<CGameCtnMediaBlockUi>
        {
            public override void ReadWrite(CGameCtnMediaBlockUi n, GameBoxReaderWriter rw)
            {
                rw.Single(ref n.start);
                rw.Single(ref n.end);
            }
        }

        #endregion

        #endregion
    }
}
