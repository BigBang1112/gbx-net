using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.MwFoundations
{
    [Node(0x01001000)]
    public class CMwNod : Node
    {
        public string[] Dependencies { get; set; }

        #region Chunks

        #region 0x000 chunk (FolderDep)

        [Chunk(0x01001000)]
        public class Chunk01001000 : Chunk<CMwNod>
        {
            public override void ReadWrite(CMwNod n, GameBoxReaderWriter rw)
            {
                n.Dependencies = rw.Array(n.Dependencies, i => rw.Reader.ReadString(), x => rw.Writer.Write(x));
            }
        }

        #endregion

        #endregion
    }
}
