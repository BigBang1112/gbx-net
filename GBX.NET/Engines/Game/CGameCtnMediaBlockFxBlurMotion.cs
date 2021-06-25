using System;

namespace GBX.NET.Engines.Game
{
    [Node(0x03082000)]
    public class CGameCtnMediaBlockFxBlurMotion : CGameCtnMediaBlockFxBlur, CGameCtnMediaBlock.IHasTwoKeys
    {
        #region Fields

        public TimeSpan start;
        public TimeSpan end = TimeSpan.FromSeconds(3);

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

        [Chunk(0x03082000)]
        public class Chunk03082000 : Chunk<CGameCtnMediaBlockFxBlurMotion>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxBlurMotion n, GameBoxReaderWriter rw)
            {
                rw.Single_s(ref n.start);
                rw.Single_s(ref n.end);
            }
        }

        #endregion

        #endregion
    }
}
