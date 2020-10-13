using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - 3D stereo (0x03024000)
    /// </summary>
    [Node(0x03024000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlock3dStereo 0x000 chunk
        /// </summary>
        [Chunk(0x03024000)]
        public class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
        {
            public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    UpToMax = rw.Reader.ReadSingle(),
                    ScreenDist = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.UpToMax);
                    rw.Writer.Write(x.ScreenDist);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float UpToMax { get; set; }
            public float ScreenDist { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlock3dStereo node;

            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlock3dStereo node) => this.node = node;
        }

        #endregion
    }
}
