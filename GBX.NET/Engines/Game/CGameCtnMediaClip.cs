using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03079000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaClip : Node
    {
        #region Properties

        /// <summary>
        /// Name of the clip. This property is null if the clip is an intro, ambiance or podium.
        /// </summary>
        [NodeMember]
        public string Name { get; set; }

        /// <summary>
        /// List of MediaTracker tracks.
        /// </summary>
        [NodeMember]
        public List<CGameCtnMediaTrack> Tracks { get; set; }

        [NodeMember]
        public bool StopWhenRespawn { get; set; }

        [NodeMember]
        public bool StopWhenLeave { get; set; }

        [NodeMember]
        public int LocalPlayerClipEntIndex { get; set; }

        #endregion

        #region Methods

        public override string ToString() => string.IsNullOrEmpty(Name) ? "Unnamed clip" : Name;

        #endregion

        #region Chunks

        #region 0x003 chunk

        [Chunk(0x03079003)]
        public class Chunk03079003 : Chunk<CGameCtnMediaClip>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Tracks = rw.Array(n.Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();
                n.Name = rw.String(n.Name);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x03079004)]
        public class Chunk03079004 : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(Unknown);
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03079005)]
        public class Chunk03079005 : Chunk<CGameCtnMediaClip>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Tracks = rw.Array(n.Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();

                n.Name = rw.String(n.Name);
            }
        }

        #endregion

        #region 0x007 chunk

        [Chunk(0x03079007)]
        public class Chunk03079007 : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                n.LocalPlayerClipEntIndex = rw.Int32(n.LocalPlayerClipEntIndex);
            }
        }

        #endregion

        #region 0x008 chunk

        [Chunk(0x03079008)]
        public class Chunk03079008 : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Single(Unknown); // 0.2
            }
        }

        #endregion

        #region 0x009 chunk

        [Chunk(0x03079009)]
        public class Chunk03079009 : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.String(Unknown);
            }
        }

        #endregion

        #region 0x00A chunk

        /// <summary>
        /// ManiaPlanet 3
        /// </summary>
        [Chunk(0x0307900A)]
        public class Chunk0307900A : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                n.StopWhenLeave = rw.Boolean(n.StopWhenLeave);
            }
        }

        #endregion

        #region 0x00B chunk

        /// <summary>
        /// ManiaPlanet 3
        /// </summary>
        [Chunk(0x0307900B)]
        public class Chunk0307900B : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown); // 99% StopWhenRespawn
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0307900C)]
        public class Chunk0307900C : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0307900D)]
        public class Chunk0307900D : Chunk<CGameCtnMediaClip>
        {
            public int Version { get; set; } = 10;

            public int U01 { get; set; }
            public bool U03 { get; set; }
            public string U05 { get; set; }
            public float U06 { get; set; } = 0.2f;
            public int U07 { get; set; } = -1;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                U01 = rw.Int32(U01);
                Version = rw.Int32(Version);

                n.Tracks = rw.Array(n.Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();

                n.Name = rw.String(n.Name);

                n.StopWhenLeave = rw.Boolean(n.StopWhenLeave);
                U03 = rw.Boolean(U03);
                n.StopWhenRespawn = rw.Boolean(n.StopWhenRespawn);
                U05 = rw.String(U05);
                U06 = rw.Single(U06);
                n.LocalPlayerClipEntIndex = rw.Int32(n.LocalPlayerClipEntIndex);
            }
        }

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaClip node;

            public string Name => node.Name;
            public List<CGameCtnMediaTrack> Tracks => node.Tracks;
            public bool StopWhenRespawn => node.StopWhenRespawn;
            public bool StopWhenLeave => node.StopWhenLeave;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaClip node) => this.node = node;
        }

        #endregion
    }
}
