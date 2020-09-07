using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03127000)]
    public class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockToneMapping(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03127004)]
        public class Chunk004 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk004(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        Exposure = rw.Reader.ReadSingle(),
                        MaxHDR = rw.Reader.ReadSingle(),
                        LightTrailScale = rw.Reader.ReadSingle(),
                        Unknown = rw.Reader.ReadInt32()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Exposure);
                    rw.Writer.Write(x.MaxHDR);
                    rw.Writer.Write(x.LightTrailScale);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        public class Key : MediaBlockKey
        {
            public float Exposure { get; set; }
            public float MaxHDR { get; set; }
            public float LightTrailScale { get; set; }
            public int Unknown { get; set; }
        }
    }
}
