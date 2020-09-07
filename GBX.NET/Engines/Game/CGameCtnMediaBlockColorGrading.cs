using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03186000)]
    public class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock
    {
        public FileRef Image { get; set; }
        public Key[] Keys { get; set; }

        public CGameCtnMediaBlockColorGrading(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03186000)]
        public class Chunk03186000 : Chunk<CGameCtnMediaBlockColorGrading>
        {
            public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
            {
                n.Image = rw.FileRef(n.Image);
            }
        }

        [Chunk(0x03186001)]
        public class Chunk03186001 : Chunk<CGameCtnMediaBlockColorGrading>
        {
            public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Intensity = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Intensity);
                });
            }
        }

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
        }
    }
}
