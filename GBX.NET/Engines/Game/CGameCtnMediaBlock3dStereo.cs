using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03024000)]
    public class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlock3dStereo(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03024000)]
        public class Chunk000 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        UpToMax = rw.Reader.ReadSingle(),
                        ScreenDist = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.UpToMax);
                    rw.Writer.Write(x.ScreenDist);
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float UpToMax { get; set; }
            public float ScreenDist { get; set; }
        }
    }
}
