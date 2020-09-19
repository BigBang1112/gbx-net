using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0312A000)]
    public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock
    {
        public float Start { get; set; }
        public float End { get; set; }
        public string Manialink { get; set; }

        [Chunk(0x0312A001)]
        public class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockManialink n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.Manialink = rw.String(n.Manialink);
            }
        }
    }
}
