using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03081000)]
    public class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx
    {
        public CGameCtnMediaBlockFxBlurDepth(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03081001)]
        public class Chunk001 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk001(CGameCtnMediaBlockFxBlurDepth node) : base(node)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Keys = r.ReadArray(i =>
                {
                    return new Key()
                    {
                        Time = r.ReadSingle(),
                        LensSize = r.ReadSingle(),
                        ForceFocus = r.ReadBoolean(),
                        FocusZ = r.ReadSingle(),
                    };
                });
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.LensSize);
                    w.Write(x.ForceFocus);
                    w.Write(x.FocusZ);
                });
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float LensSize { get; set; }
            public bool ForceFocus { get; set; }
            public float FocusZ { get; set; }
        }
    }
}
