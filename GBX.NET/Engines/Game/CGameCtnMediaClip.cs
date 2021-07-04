using System.Collections.Generic;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    [Node(0x03079000)]
    public class CGameCtnMediaClip : CMwNod
    {
        #region Fields

        private string name;
        private IList<CGameCtnMediaTrack> tracks = new List<CGameCtnMediaTrack>();
        private bool stopWhenRespawn;
        private bool stopWhenLeave;
        private int localPlayerClipEntIndex = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the clip. This property is null if the clip is an intro, ambiance or podium.
        /// </summary>
        [NodeMember]
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// List of MediaTracker tracks.
        /// </summary>
        [NodeMember]
        public IList<CGameCtnMediaTrack> Tracks
        {
            get => tracks;
            set => tracks = value;
        }

        /// <summary>
        /// Stop the clip when player respawns.
        /// </summary>
        [NodeMember]
        public bool StopWhenRespawn
        {
            get => stopWhenRespawn;
            set => stopWhenRespawn = value;
        }

        /// <summary>
        /// Stop the clip when player leaves the trigger.
        /// </summary>
        [NodeMember]
        public bool StopWhenLeave
        {
            get => stopWhenLeave;
            set => stopWhenLeave = value;
        }

        [NodeMember]
        public int LocalPlayerClipEntIndex
        {
            get => localPlayerClipEntIndex;
            set => localPlayerClipEntIndex = value;
        }

        #endregion

        #region Methods

        public override string ToString() => string.IsNullOrEmpty(Name) ? "Unnamed clip" : Name;

        #endregion

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x03079002)]
        public class Chunk03079002 : Chunk<CGameCtnMediaClip>, IVersionable
        {
            private int version;

            public int U01;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.ListNode(ref n.tracks);
                rw.String(ref n.name);
                rw.Int32(ref U01);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03079003)]
        public class Chunk03079003 : Chunk<CGameCtnMediaClip>
        {
            private int tracksVersion;

            public int TracksVersion
            {
                get => tracksVersion;
                set => tracksVersion = value;
            }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref tracksVersion);
                rw.ListNode(ref n.tracks);

                rw.String(ref n.name);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x03079004)]
        public class Chunk03079004 : Chunk<CGameCtnMediaClip>
        {
            public CMwNod U01;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.NodeRef(ref U01);
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03079005)]
        public class Chunk03079005 : Chunk<CGameCtnMediaClip>
        {
            private int tracksVersion = 10;

            public int TracksVersion
            {
                get => tracksVersion;
                set => tracksVersion = value;
            }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref tracksVersion);
                rw.ListNode<CGameCtnMediaTrack>(ref n.tracks);
                rw.String(ref n.name);
            }
        }

        #endregion

        #region 0x007 chunk

        [Chunk(0x03079007)]
        public class Chunk03079007 : Chunk<CGameCtnMediaClip>
        {
            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref n.localPlayerClipEntIndex);
            }
        }

        #endregion

        #region 0x008 chunk

        [Chunk(0x03079008)]
        public class Chunk03079008 : Chunk<CGameCtnMediaClip>
        {
            public float U01;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Single(ref U01); // 0.2
            }
        }

        #endregion

        #region 0x009 chunk

        [Chunk(0x03079009)]
        public class Chunk03079009 : Chunk<CGameCtnMediaClip>
        {
            public string U01;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.String(ref U01);
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
                rw.Boolean(ref n.stopWhenLeave);
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
            public bool U01;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref U01); // 99% StopWhenRespawn
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0307900C)]
        public class Chunk0307900C : Chunk<CGameCtnMediaClip>
        {
            public int U01;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0307900D)]
        public class Chunk0307900D : Chunk<CGameCtnMediaClip>, IVersionable
        {
            private int version;
            private int tracksVersion = 10;

            public int U01;
            public bool U03;
            public string U05;
            public float U06 = 0.2f;
            public int U07 = -1;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public int TracksVersion
            {
                get => tracksVersion;
                set => tracksVersion = value;
            }

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                rw.Int32(ref tracksVersion);
                rw.ListNode<CGameCtnMediaTrack>(ref n.tracks);

                rw.String(ref n.name);

                rw.Boolean(ref n.stopWhenLeave);
                rw.Boolean(ref U03);
                rw.Boolean(ref n.stopWhenRespawn);
                rw.String(ref U05);
                rw.Single(ref U06);
                rw.Int32(ref n.localPlayerClipEntIndex);
            }
        }

        #endregion

        #endregion
    }
}
