using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Plug;

[Node(0x09009000)]
public class CPlugVisualIndexedLines : CPlugVisualIndexed
{
    protected CPlugVisualIndexedLines()
    {

    }

    [Chunk(0x09009001)]
    public class Chunk09009001 : Chunk<CPlugVisualIndexedLines>
    {
        public int U01;

        public override void ReadWrite(CPlugVisualIndexedLines n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
