using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x0903A000)]
    public class CPlugMaterialCustom : CPlug
    {
        [Chunk(0x0903A004)]
        public class Chunk0903A004 : Chunk<CPlugMaterialCustom>
        {
            public int U01;

            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }

        [Chunk(0x0903A00A)]
        public class Chunk0903A00A : Chunk<CPlugMaterialCustom>
        {
            public int U01;
            public int U02;

            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
                rw.Int32(ref U02);
            }
        }

        [Chunk(0x0903A00C)]
        public class Chunk0903A00C : Chunk<CPlugMaterialCustom>
        {
            public int U01;
            public string U02;
            public int U03;
            public string U04;

            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
                rw.Id(ref U02);
                rw.Int32(ref U03);
                rw.Id(ref U04);

                // ...
            }
        }
    }
}
