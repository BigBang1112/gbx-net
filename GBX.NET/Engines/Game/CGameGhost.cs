using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0303F000)]
    public class CGameGhost : Node
    {
        public bool IsReplaying { get; set; }

        public CGameGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x005 chunk

        [Chunk(0x0303F005)]
        public class Chunk0303F005 : Chunk<CGameGhost>
        {
            public int UncompressedSize { get; set; }
            public int CompressedSize { get; set; }
            public byte[] Data { get; set; }

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            { 
                UncompressedSize = rw.Int32(UncompressedSize);
                CompressedSize = rw.Int32(UncompressedSize);
                Data = rw.Bytes(Data, CompressedSize);
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x0303F006)]
        public class Chunk0303F006 : Chunk<CGameGhost>
        {
            public Chunk0303F005 Chunk005 { get; } = new Chunk0303F005();

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            {
                n.IsReplaying = rw.Boolean(n.IsReplaying);
                Chunk005.ReadWrite(n, rw);
            }
        }

        #endregion

        #endregion
    }
}
