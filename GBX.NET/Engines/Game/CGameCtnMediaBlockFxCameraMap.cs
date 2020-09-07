using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03139000)]
    public class CGameCtnMediaBlockFxCameraMap : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockFxCameraMap(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03139000)]
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

        [Chunk(0x03139001)]
        public class Chunk001 : Chunk
        {
            public Chunk001(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);

                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.FileRef(Unknown);
            }
        }
    }
}
