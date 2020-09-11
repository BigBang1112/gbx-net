using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.TrackMania
{
    [Node(0x2407F000)]
    public class CCtnMediaBlockEventTrackMania : CGameCtnMediaBlock
    {
        public CCtnMediaBlockEventTrackMania(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x2407F000)]
        public class Chunk2407F000 : Chunk<CCtnMediaBlockEventTrackMania>
        {
            public override void Read(CCtnMediaBlockEventTrackMania n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadTillFacade();
            }
        }

        [Chunk(0x2407F003)]
        public class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
        {
            public override void Read(CCtnMediaBlockEventTrackMania n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadTillFacade();
            }
        }
    }
}
