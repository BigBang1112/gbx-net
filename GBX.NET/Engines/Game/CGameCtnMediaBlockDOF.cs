using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03126000)]
    public class CGameCtnMediaBlockDOF : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockDOF(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03126002)]
        public class Chunk002 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk002(CGameCtnMediaBlockDOF node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var zFocus = r.ReadSingle();
                    var lensSize = r.ReadSingle();
                    var a = r.ReadInt32();
                    var b = r.ReadInt32();
                    var c = r.ReadInt32();
                    var d = r.ReadInt32();

                    return new Key()
                    {
                        Time = time,
                        ZFocus = zFocus,
                        LensSize = lensSize,
                        Unknown = new object[] { a, b, c, d }
                    };
                });
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.ZFocus);
                    w.Write(x.LensSize);
                    w.Write((int)x.Unknown[0]);
                    w.Write((int)x.Unknown[1]);
                    w.Write((int)x.Unknown[2]);
                    w.Write((int)x.Unknown[3]);
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float ZFocus { get; set; }
            public float LensSize { get; set; }

            public object[] Unknown { get; set; }
        }
    }
}
