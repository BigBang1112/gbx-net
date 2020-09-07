using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A4000)]
    public class CGameCtnMediaBlockCameraEffectShake : CGameCtnMediaBlockCameraEffect
    {
        public Key[] Keys { get; set; }

        public CGameCtnMediaBlockCameraEffectShake(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030A4000)]
        public class Chunk030A4000 : Chunk<CGameCtnMediaBlockCameraEffectShake>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraEffectShake n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Intensity = rw.Reader.ReadSingle(),
                    Speed = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Intensity);
                    rw.Writer.Write(x.Speed);
                });
            }
        }

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
            public float Speed { get; set; }
        }
    }
}
