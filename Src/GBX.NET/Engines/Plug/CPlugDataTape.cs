using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Plug;

[Node(0x090CE000)]
public class CPlugDataTape : CMwNod
{
    protected CPlugDataTape()
    {

    }

    [Chunk(0x090CE001)]
    public class Chunk090CE001 : Chunk<CPlugDataTape>, IVersionable
    {
        private int version;
        private int U01;
        private int[]? U02;
        private int[]? U03;
        private byte[]? U04;
        private int U05;
        private byte[]? U06;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugDataTape n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
            rw.Array<int>(ref U02);
            rw.Array<int>(ref U03);
            rw.Bytes(ref U04); // floats
            rw.Int32(ref U05);
            rw.Bytes(ref U06);
        }
    }
}
