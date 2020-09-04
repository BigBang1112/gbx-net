using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0312A000)]
    public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockManialink(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x0312A001)]
        public class Chunk001 : Chunk
        {
            public int Version { get; set; }
            public float Start { get; set; }
            public float End { get; set; }
            public string Manialink { get; set; }

            public Chunk001(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Start = rw.Single(Start);
                End = rw.Single(End);
                Manialink = rw.String(Manialink);
            }
        }
    }
}
