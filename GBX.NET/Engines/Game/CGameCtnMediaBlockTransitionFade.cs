using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030AB000)]
    public class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlock
    {
        public Key[] Keys
        {
            get => GetValue<Chunk000>(x => x.Keys) as Key[];
            set => SetValue<Chunk000>(x => x.Keys = value);
        }

        public Vector3 Color
        {
            get => (Vector3)GetValue<Chunk000>(x => x.Color);
            set => SetValue<Chunk000>(x => x.Color = value);
        }

        public CGameCtnMediaBlockTransitionFade(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030AB000)]
        public class Chunk000 : Chunk
        {
            public Key[] Keys { get; set; }
            public Vector3 Color { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var opacity = rw.Reader.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Opacity = opacity
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Opacity);
                });

                Color = rw.Vec3(Color);
                rw.Single(Unknown);
            }
        }

        public class Key
        {
            public float Time { get; set; }
            public float Opacity { get; set; }
        }
    }
}
