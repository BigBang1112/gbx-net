using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0303F000)]
    public class CGameGhost : Node
    {
        public int UncompressedSize
        {
            get => (int)GetValue<Chunk005, Chunk006>(x => x.UncompressedSize, x => x.Chunk005.UncompressedSize);
        }

        public int CompressedSize
        {
            get => (int)GetValue<Chunk005, Chunk006>(x => x.CompressedSize, x => x.Chunk005.CompressedSize);
        }

        public CGameGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x005 chunk

        [Chunk(0x0303F005)]
        public class Chunk005 : Chunk
        {
            public int UncompressedSize { get; set; }
            public int CompressedSize { get; set; }
            public byte[] Data { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                UncompressedSize = r.ReadInt32();
                CompressedSize = r.ReadInt32();
                Data = r.ReadBytes(CompressedSize);
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(UncompressedSize);
                w.Write(CompressedSize);
                w.Write(Data);
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x0303F006)]
        public class Chunk006 : Chunk
        {
            public bool IsReplaying { get; set; }
            public Chunk005 Chunk005 { get; } = new Chunk005();

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                IsReplaying = rw.Boolean(IsReplaying);
                Chunk005.ReadWrite(rw);
            }
        }

        #endregion

        #endregion
    }
}
