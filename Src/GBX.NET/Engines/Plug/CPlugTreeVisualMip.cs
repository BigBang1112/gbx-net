using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x09015000)]
    public sealed class CPlugTreeVisualMip : CPlugTree
    {
        private IDictionary<float, CPlugTree> levels;

        public IDictionary<float, CPlugTree> Levels
        {
            get => levels;
            set => levels = value;
        }

        private CPlugTreeVisualMip()
        {
            levels = null!;
        }

        [Chunk(0x09015002)]
        public class Chunk09015002 : Chunk<CPlugTreeVisualMip>
        {
            public override void ReadWrite(CPlugTreeVisualMip n, GameBoxReaderWriter rw)
            {
                rw.DictionaryNode(ref n.levels!);
            }
        }
    }
}
