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
            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0903A00A)]
        public class Chunk0903A00A : Chunk<CPlugMaterialCustom>
        {
            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0903A00C)]
        public class Chunk0903A00C : Chunk<CPlugMaterialCustom>
        {
            public override void ReadWrite(CPlugMaterialCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Id(Unknown);
                rw.Int32(Unknown);
                rw.Id(Unknown);

                // ...
            }
        }
    }
}
