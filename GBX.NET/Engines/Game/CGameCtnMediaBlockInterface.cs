using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03195000)]
    public class CGameCtnMediaBlockInterface : CGameCtnMediaBlock
    {
        public float Start { get; set; }
        public float End { get; set; }
        public bool ShowInterface { get; set; }
        public string Manialink { get; set; }

        public CGameCtnMediaBlockInterface(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03195000)]
        public class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockInterface n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.ShowInterface = rw.Boolean(n.ShowInterface);
                n.Manialink = rw.String(n.Manialink);
            }
        }
    }
}
