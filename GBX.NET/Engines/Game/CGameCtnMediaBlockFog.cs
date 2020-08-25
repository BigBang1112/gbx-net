using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03199000)]
    public class CGameCtnMediaBlockFog : CGameCtnMediaBlock
    {
        public Key[] Keys
        {
            get => GetValue<Chunk000>(x => x.Keys) as Key[];
            set => SetValue<Chunk000>(x => x.Keys = value);
        }

        public CGameCtnMediaBlockFog(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        [Chunk(0x03199000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public Key[] Keys { get; set; }

            public Chunk000(CGameCtnMediaBlockFog node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Keys = rw.Array(Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(9);

                    return new Key()
                    {
                        Time = time,
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float[] Unknown { get; set; }
        }
    }
}
