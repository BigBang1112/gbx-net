using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03172000)]
    public class CGameCtnMediaBlockColoringBase : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockColoringBase(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03172000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public Key[] Keys { get; set; }
            public int BaseIndex { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);

                Keys = rw.Array(Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Hue = rw.Reader.ReadSingle(),
                    Intensity = rw.Reader.ReadSingle(),
                    Unknown = rw.Reader.ReadInt16()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Hue);
                    rw.Writer.Write(x.Intensity);
                    rw.Writer.Write(x.Unknown);
                });
                
                BaseIndex = rw.Int32(BaseIndex);
            }
        }

        public class Key : MediaBlockKey
        {
            public float Hue { get; set; }
            public float Intensity { get; set; }
            public short Unknown { get; set; }
        }
    }
}
