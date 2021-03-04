using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Skin for a block (0x03059000)
    /// </summary>
    [Node(0x03059000)]
    public class CGameCtnBlockSkin : Node
    {
        #region Properties

        [NodeMember]
        public string Text { get; set; }

        [NodeMember]
        public FileRef PackDesc { get; set; } = new FileRef();

        [NodeMember]
        public FileRef ParentPackDesc { get; set; } = new FileRef();

        /// <summary>
        /// Second skin for the skinnable block. Available in TM®.
        /// </summary>
        [NodeMember]
        public FileRef SecondaryPackDesc { get; set; } = new FileRef();

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnBlockSkin 0x000 chunk
        /// </summary>
        [Chunk(0x03059000)]
        public class Chunk03059000 : Chunk<CGameCtnBlockSkin>
        {
            public string Ignored { get; set; }

            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                n.Text = rw.String(n.Text);
                Ignored = rw.String(Ignored);
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
                n.Text = rw.String(n.Text);
                n.PackDesc = rw.FileRef(n.PackDesc);
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
                n.Text = rw.String(n.Text);
                n.PackDesc = rw.FileRef(n.PackDesc);
                n.ParentPackDesc = rw.FileRef(n.ParentPackDesc);
            }
        }

        #endregion

        #region 0x003 chunk (secondary skin)

        /// <summary>
        /// CGameCtnBlockSkin 0x003 chunk (secondary skin)
        /// </summary>
        [Chunk(0x03059003, "secondary skin")]
        public class Chunk03059003 : Chunk<CGameCtnBlockSkin>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnBlockSkin n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.SecondaryPackDesc = rw.FileRef(n.SecondaryPackDesc);
            }
        }

        #endregion

        #endregion
    }
}
