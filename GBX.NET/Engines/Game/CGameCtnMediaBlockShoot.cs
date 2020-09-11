using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03145000)]
    public class CGameCtnMediaBlockShoot : CGameCtnMediaBlock
    {
        public float Start { get; set; }
        public float End { get; set; }

        public CGameCtnMediaBlockShoot(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03145000)]
        public class Chunk03145000 : Chunk<CGameCtnMediaBlockShoot>
        {
            public override void ReadWrite(CGameCtnMediaBlockShoot n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
            }
        }
    }
}
