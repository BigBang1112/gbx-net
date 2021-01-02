using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera effect inetrial tracking
    /// </summary>
    [Node(0x03166000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public bool Tracking { get; set; }

        [NodeMember]
        public bool AutoFocus { get; set; }

        [NodeMember]
        public bool AutoZoom { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraEffectInertialTracking 0x000 chunk
        /// </summary>
        [Chunk(0x03166000)]
        public class Chunk03166000 : Chunk<CGameCtnMediaBlockCameraEffectInertialTracking>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockCameraEffectInertialTracking n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.Tracking = rw.Boolean(n.Tracking);
                n.AutoZoom = rw.Boolean(n.AutoZoom);
                n.AutoFocus = rw.Boolean(n.AutoFocus);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private CGameCtnMediaBlockCameraEffectInertialTracking node;

            public float Start => node.Start;
            public float End => node.End;
            public bool Tracking => node.Tracking;
            public bool AutoZoom => node.AutoZoom;
            public bool AutoFocus => node.AutoFocus;

            public DebugView(CGameCtnMediaBlockCameraEffectInertialTracking node) => this.node = node;
        }

        #endregion
    }
}
