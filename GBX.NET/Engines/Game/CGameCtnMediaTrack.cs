using System.Collections.Generic;

namespace GBX.NET.Engines.Game
{
    [Node(0x03078000)]
    public class CGameCtnMediaTrack : Node
    {
        #region Fields

        private string name;
        private List<CGameCtnMediaBlock> blocks;
        private bool isKeepPlaying = true;
        private bool isCycling;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the track.
        /// </summary>
        [NodeMember]
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// List of blocks.
        /// </summary>
        [NodeMember]
        public List<CGameCtnMediaBlock> Blocks
        {
            get => blocks;
            set => blocks = value;
        }

        /// <summary>
        /// If the track should keep playing the last block after it ends.
        /// </summary>
        [NodeMember]
        public bool IsKeepPlaying
        {
            get => isKeepPlaying;
            set => isKeepPlaying = value;
        }

        [NodeMember]
        public bool IsCycling
        {
            get => isCycling;
            set => isCycling = value;
        }

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
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.name);
                rw.Int32(ref u01); // 10, probably version
                rw.ListNode<CGameCtnMediaBlock>(ref n.blocks);
                rw.Int32(ref u02);
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
                rw.Boolean(ref n.isKeepPlaying);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03078003)]
        public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01); // 0
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
                rw.Boolean(ref n.isKeepPlaying);
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03078005)]
        public class Chunk03078005 : Chunk<CGameCtnMediaTrack>
        {
            private int version = 1;
            private int u02;
            private float u04 = -1;
            private float u05 = -1;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public float U04
            {
                get => u04;
                set => u04 = value;
            }

            public float U05
            {
                get => u05;
                set => u05 = value;
            }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Boolean(ref n.isKeepPlaying);
                rw.Int32(ref u02);
                rw.Boolean(ref n.isCycling);
                rw.Single(ref u04);
                rw.Single(ref u05);
            }
        }

        #endregion

        #endregion
    }
}
