using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03186000)]
    public class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockColorGrading(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03186000)]
        public class Chunk000 : Chunk
        {
            public FileRef Image { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Image = rw.FileRef(Image);
            }
        }

        [Chunk(0x03186001)]
        public class Chunk001 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk001(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        Intensity = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Intensity);
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float Intensity { get; set; }
        }
    }
}
