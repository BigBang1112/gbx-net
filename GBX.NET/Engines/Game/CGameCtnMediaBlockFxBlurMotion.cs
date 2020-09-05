using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03082000)]
    public class CGameCtnMediaBlockFxBlurMotion : CGameCtnMediaBlockFxBlur
    {
        public float Start
        {
            get => (float)GetValue<Chunk000>(x => x.Start);
            set => SetValue<Chunk000>(x => x.Start = value);
        }

        public float End
        {
            get => (float)GetValue<Chunk000>(x => x.End);
            set => SetValue<Chunk000>(x => x.End = value);
        }

        public CGameCtnMediaBlockFxBlurMotion(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03082000)]
        public class Chunk000 : Chunk
        {
            public float Start { get; set; }
            public float End { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
            }
        }

        #endregion

        #endregion
    }
}
