using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A8000)]
    public class CGameCtnMediaBlockText : CGameCtnMediaBlock
    {
        public string Text
        {
            get => GetValue<Chunk001>(x => x.Text) as string;
            set => SetValue<Chunk001>(x => x.Text = value);
        }

        public CGameCtnMediaBlockText(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A8001)]
        public class Chunk001 : Chunk
        {
            public string Text { get; set; }
            public CControlEffectSimi Simi { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Text = rw.String(Text);
                Simi = rw.NodeRef<CControlEffectSimi>(Simi);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030A8002)]
        public class Chunk002 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
            }
        }

        #endregion

        #endregion
    }
}