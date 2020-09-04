using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0316D000)]
    public class CGameCtnMediaBlockFxCameraBlend : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockFxCameraBlend(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x0316D000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public Key[] Keys { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                Keys = rw.Array(Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        CaptureWeight = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.CaptureWeight);
                });
            }
        }

        public class Key : MediaBlockKey
        {
            public float CaptureWeight { get; set; }
        }
    }
}
