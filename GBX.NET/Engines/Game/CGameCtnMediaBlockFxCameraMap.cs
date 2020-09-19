using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03139000)]
    public class CGameCtnMediaBlockFxCameraMap : CGameCtnMediaBlock
    {
        public float Start { get; set; }
        public float End { get; set; }

        [Chunk(0x03139000)]
        public class Chunk03139000 : Chunk<CGameCtnMediaBlockFxCameraMap>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
            }
        }

        [Chunk(0x03139001)]
        public class Chunk03139001 : Chunk<CGameCtnMediaBlockFxCameraMap>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
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
