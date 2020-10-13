using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera ingame
    /// </summary>
    [Node(0x03084000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockCameraGame : CGameCtnMediaBlockCamera
    {
        #region Enums

        public enum EGameCam : int
        {
            Behind,
            Close,
            Internal,
            Orbital
        }

        public enum EGameCam2 : int
        {
            Default,
            Internal,
            External,
            Helico,
            Free,
            Spectator,
            External_2
        }

        #endregion

        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; } = 3;

        [NodeMember]
        public EGameCam? Type { get; set; }

        [NodeMember]
        public EGameCam2? Type2 { get; set; }

        [NodeMember]
        public int Target { get; set; } = -1;

        [NodeMember]
        public string GameCam { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraGame 0x000 chunk
        /// </summary>
        [Chunk(0x03084000)]
        public class Chunk03084000 : Chunk<CGameCtnMediaBlockCameraGame>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraGame 0x001 chunk
        /// </summary>
        [Chunk(0x03084001)]
        public class Chunk03084001 : Chunk<CGameCtnMediaBlockCameraGame>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.Type = (EGameCam)rw.Int32((int)n.Type.GetValueOrDefault());
                n.Target = rw.Int32(n.Target);
            }
        }

        #endregion

        #region 0x003 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraGame 0x003 chunk
        /// </summary>
        [Chunk(0x03084003)]
        public class Chunk03084003 : Chunk<CGameCtnMediaBlockCameraGame>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.GameCam = rw.LookbackString(n.GameCam);
                n.Target = rw.Int32(n.Target);
            }
        }

        #endregion

        #region 0x007 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraGame 0x007 chunk
        /// </summary>
        [Chunk(0x03084007)]
        public class Chunk03084007 : Chunk<CGameCtnMediaBlockCameraGame>
        {
            public override void ReadWrite(CGameCtnMediaBlockCameraGame n, GameBoxReaderWriter rw)
            {
                n.Type2 = (EGameCam2)rw.Int32((int)n.Type.GetValueOrDefault());
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);

                // not right

                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Array<float>(Unknown, 6);
                rw.Array<float>(Unknown, 5);
                rw.Array<uint>(Unknown, 5);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockCameraGame node;

            public float Start => node.Start;
            public float End => node.End;
            public EGameCam? Type => node.Type;
            public EGameCam2? Type2 => node.Type2;
            public int Target => node.Target;
            public string GameCam => node.GameCam;

            public DebugView(CGameCtnMediaBlockCameraGame node) => this.node = node;
        }

        #endregion
    }
}
