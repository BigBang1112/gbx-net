using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Skin for a block (0x03059000)
    /// </summary>
    [Node(0x03059000)]
    public class CGameCtnBlockSkin : CMwNod
    {
        #region Fields

        private string? text;
        private FileRef packDesc;
        private FileRef parentPackDesc;
        private FileRef foregroundPackDesc;

        #endregion

        #region Properties

        [NodeMember]
        public string? Text
        {
            get => text;
            set => text = value;
        }

        [NodeMember]
        public FileRef PackDesc
        {
            get => packDesc;
            set => packDesc = value;
        }

        [NodeMember]
        public FileRef ParentPackDesc
        {
            get => parentPackDesc;
            set => parentPackDesc = value;
        }

        /// <summary>
        /// Second skin for the skinnable block. Available in TM®.
        /// </summary>
        [NodeMember]
        public FileRef ForegroundPackDesc
        {
            get => foregroundPackDesc;
            set => foregroundPackDesc = value;
        }

        #endregion

        #region Constructors

        private CGameCtnBlockSkin()
        {
            packDesc = null!;
            parentPackDesc = null!;
            foregroundPackDesc = null!;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnBlockSkin 0x000 chunk
        /// </summary>
        [Chunk(0x03059000)]
        public class Chunk03059000 : Chunk<CGameCtnBlockSkin>
        {
            public string? U01;

            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.text);
                rw.String(ref U01);
            }
        }

        #endregion

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnBlockSkin 0x001 chunk
        /// </summary>
        [Chunk(0x03059001)]
        public class Chunk03059001 : Chunk<CGameCtnBlockSkin>
        {
            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.text);
                rw.FileRef(ref n.packDesc);
            }
        }

        #endregion

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnBlockSkin 0x002 chunk (skin)
        /// </summary>
        [Chunk(0x03059002, "skin")]
        public class Chunk03059002 : Chunk<CGameCtnBlockSkin>
        {
            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.text);
                rw.FileRef(ref n.packDesc);
                rw.FileRef(ref n.parentPackDesc);
            }
        }

        #endregion

        #region 0x003 chunk (secondary skin)

        /// <summary>
        /// CGameCtnBlockSkin 0x003 chunk (secondary skin)
        /// </summary>
        [Chunk(0x03059003, "secondary skin")]
        public class Chunk03059003 : Chunk<CGameCtnBlockSkin>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.FileRef(ref n.foregroundPackDesc);
            }
        }

        #endregion

        #endregion
    }
}
