using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A5000)]
    public class CGameCtnMediaBlockImage : CGameCtnMediaBlock
    {
        public CControlEffectSimi Simi
        {
            get => GetValue<Chunk000>(x => x.Simi) as CControlEffectSimi;
            set => SetValue<Chunk000>(x => x.Simi = value);
        }

        public FileRef Image
        {
            get => GetValue<Chunk000>(x => x.Image) as FileRef;
            set => SetValue<Chunk000>(x => x.Image = value);
        }

        public CGameCtnMediaBlockImage(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030A5000)]
        public class Chunk000 : Chunk
        {
            public CControlEffectSimi Simi { get; set; }
            public FileRef Image { get; set; }

            public Chunk000(CGameCtnMediaBlockImage node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Simi = rw.NodeRef<CControlEffectSimi>(Simi);
                Image = rw.FileRef(Image);
            }
        }
    }
}
