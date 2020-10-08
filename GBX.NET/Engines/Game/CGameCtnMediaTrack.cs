using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03078000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaTrack : Node
    {
        /// <summary>
        /// Name of the track.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of blocks.
        /// </summary>
        public List<CGameCtnMediaBlock> Blocks { get; set; }

        public override string ToString() => Name;

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x03078001)]
        public class Chunk03078001 : Chunk<CGameCtnMediaTrack>
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                n.Name = rw.String(n.Name);
                Unknown1 = rw.Int32(Unknown1); // 10, probably version
                n.Blocks = rw.Array(n.Blocks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaBlock>(),
                    x => rw.Writer.Write(x))?.ToList();
                Unknown2 = rw.Int32(Unknown2);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03078002)]
        public class Chunk03078002 : Chunk<CGameCtnMediaTrack>
        {
            public bool Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Boolean(Unknown1); // 1
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03078003)]
        public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
        {
            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown); // 0
            }
        }

        #endregion

        #region 0x004 chunk

        /// <summary>
        /// CGameCtnMediaTrack 0x004 chunk. This chunk should be removed in the new versions of ManiaPlanet.
        /// </summary>
        [Chunk(0x03078004)]
        public class Chunk03078004 : Chunk<CGameCtnMediaTrack>
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
                // There is sometimes a second int
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03078005)]
        public class Chunk03078005 : Chunk<CGameCtnMediaTrack>
        {
            public int Version { get; set; } = 1;
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public float Unknown4 { get; set; } = -1;
            public float Unknown5 { get; set; } = -1;

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Unknown1 = rw.Int32(Unknown1);
                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Int32(Unknown3);
                Unknown4 = rw.Single(Unknown4);
                Unknown5 = rw.Single(Unknown5);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaTrack node;

            public string Name => node.Name;
            public List<CGameCtnMediaBlock> Blocks => node.Blocks;

            public DebugView(CGameCtnMediaTrack node) => this.node = node;
        }

        #endregion
    }
}
