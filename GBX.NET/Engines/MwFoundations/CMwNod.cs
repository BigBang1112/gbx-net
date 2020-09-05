using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.MwFoundations
{
    [Node(0x01001000)]
    public class CMwNod : Node
    {
        public CMwNod(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk (FolderDep)

        [Chunk(0x01001000)]
        public class Chunk000 : Chunk
        {
            public string[] Dependencies { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Dependencies = r.ReadArray(i => r.ReadString());
            }
        }

        #endregion

        #endregion
    }
}
