using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Plug;

[Node(0x0904A000)]
public class CPlugVisual2D : CPlugVisual
{
    protected CPlugVisual2D()
    {

    }

    [Chunk(0x0904A000)]
    public class Chunk0904A000 : Chunk<CPlugVisual2D>
    {
        public float[]? U01;

        public override void Read(CPlugVisual2D n, GameBoxReader r)
        {
            U01 = r.ReadArray<float>(r.ReadInt32() * 8);
        }

        public override void Write(CPlugVisual2D n, GameBoxWriter w)
        {
            if (U01 is null)
            {
                w.Write(0);
                return;
            }

            w.Write(U01.Length / 8);
            w.WriteArray_NoPrefix(U01);
        }
    }
}
