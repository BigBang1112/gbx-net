using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Color grading
    /// </summary>
    [Node(0x03186000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockColorGrading : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public FileRef Image { get; set; }

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockColorGrading 0x000 chunk
        /// </summary>
        [Chunk(0x03186000)]
        public class Chunk03186000 : Chunk<CGameCtnMediaBlockColorGrading>
        {
            public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
            {
                n.Image = rw.FileRef(n.Image);
            }
        }

        #endregion

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockColorGrading 0x001 chunk
        /// </summary>
        [Chunk(0x03186001)]
        public class Chunk03186001 : Chunk<CGameCtnMediaBlockColorGrading>
        {
            public override void ReadWrite(CGameCtnMediaBlockColorGrading n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Intensity = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Intensity);
                });
            }
        }

        #endregion

        #endregion

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
        }

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockColorGrading node;

            public FileRef Image => node.Image;
            public Key[] Keys => node.Keys;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockColorGrading node) => this.node = node;
        }

        #endregion
    }
}
