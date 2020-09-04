using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03133000)]
    public class CGameCtnMediaBlockVehicleLight : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockVehicleLight(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03133000)]
        public class Chunk000 : Chunk
        {
            public float Start { get; set; }
            public float End { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
            }
        }

        [Chunk(0x03133001)]
        public class Chunk001 : Chunk
        {
            public int Target { get; set; }

            public Chunk001(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Target = rw.Int32(Target);
            }
        }
    }
}
