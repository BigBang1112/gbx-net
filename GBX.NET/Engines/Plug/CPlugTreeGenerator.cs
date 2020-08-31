using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x09051000)]
    public class CPlugTreeGenerator : CPlug
    {
        public CPlugTreeGenerator(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x09051000)]
        public class Chunk09051000 : Chunk
        {
            public int Version { get; set; }

            public Chunk09051000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
            }
        }
    }
}
