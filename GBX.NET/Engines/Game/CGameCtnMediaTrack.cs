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
        #region Properties

        /// <summary>
        /// Name of the track.
        /// </summary>
        [NodeMember]
        public string Name { get; set; }

        /// <summary>
        /// List of blocks.
        /// </summary>
        [NodeMember]
        public List<CGameCtnMediaBlock> Blocks { get; set; }

        /// <summary>
        /// If the track should keep playing the last block after it ends.
        /// </summary>
        public bool IsKeepPlaying { get; set; } = true;

        public bool IsCycling { get; set; }

        #endregion

        #region Methods

        public override string ToString() => Name;

        /// <summary>
        /// Transfers the MediaTracker track properties from either <see cref="Chunk03078002"/> (ESWC) or <see cref="Chunk03078004"/> (TMF)
        /// to <see cref="Chunk03078005"/> (ManiaPlanet and Trackmania®). If those chunks aren't presented, no action is performed.
        /// <see cref="Chunk03078003"/> is additionally removed for undiscovered safety of the chunk.
        /// </summary>
        /// <returns>True if any of the chunks were transfered.</returns>
        public bool TransferMediaTrackTo005()
        {
            var chunk002 = GetChunk<Chunk03078002>();
            var chunk004 = GetChunk<Chunk03078004>();

            if (chunk002 == null && chunk004 == null) return false;

            CreateChunk<Chunk03078005>();

            RemoveChunk<Chunk03078002>();
            RemoveChunk<Chunk03078003>(); // Removed atm for safety. TODO: inspect this chunk
            RemoveChunk<Chunk03078004>();

            return true;
        }

        #endregion

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

        /// <summary>
        /// CGameCtnMediaTrack 0x002 chunk. Represents <see cref="IsKeepPlaying"/> for ESWC tracks. This chunk should be removed or transfered
        /// to <see cref="Chunk03078005"/> in the new versions of ManiaPlanet with <see cref="TransferMediaTrackTo005"/>.
        /// </summary>
        [Chunk(0x03078002)]
        public class Chunk03078002 : Chunk<CGameCtnMediaTrack>
        {
            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                n.IsKeepPlaying = rw.Boolean(n.IsKeepPlaying);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03078003)]
        public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
        {
            public int U01 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                U01 = rw.Int32(U01); // 0
            }
        }

        #endregion

        #region 0x004 chunk

        /// <summary>
        /// CGameCtnMediaTrack 0x004 chunk. Represents <see cref="IsKeepPlaying"/> for TMF tracks. This chunk should be removed or transfered
        /// to <see cref="Chunk03078005"/> in the new versions of ManiaPlanet with <see cref="TransferMediaTrackTo005"/>.
        /// </summary>
        [Chunk(0x03078004)]
        public class Chunk03078004 : Chunk<CGameCtnMediaTrack>
        {
            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                n.IsKeepPlaying = rw.Boolean(n.IsKeepPlaying);
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03078005)]
        public class Chunk03078005 : Chunk<CGameCtnMediaTrack>
        {
            public int Version { get; set; } = 1;
            public int U02 { get; set; }
            public float U04 { get; set; } = -1;
            public float U05 { get; set; } = -1;

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.IsKeepPlaying = rw.Boolean(n.IsKeepPlaying);
                U02 = rw.Int32(U02);
                n.IsCycling = rw.Boolean(n.IsCycling);
                U04 = rw.Single(U04);
                U05 = rw.Single(U05);
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
            public bool IsKeepPlaying => node.IsKeepPlaying;
            public bool IsCycling => node.IsCycling;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaTrack node) => this.node = node;
        }

        #endregion
    }
}
