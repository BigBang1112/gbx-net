using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03195000)]
    public class CGameCtnMediaBlockInterface : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockInterface(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03195000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public float Start { get; set; }
            public float End { get; set; }
            public bool ShowInterface { get; set; }
            public string Manialink { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Start = rw.Single(Start);
                End = rw.Single(End);
                ShowInterface = rw.Boolean(ShowInterface);
                Manialink = rw.String(Manialink);
            }
        }
    }
}
