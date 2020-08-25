using GBX.NET.Engines.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03038000)]
    public class CGameCtnDecoration : CGameCtnCollector
    {
        public CGameCtnDecoration(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x011 chunk

        [Chunk(0x03038000, 0x011)]
        public class Chunk011_038 : Chunk
        {
            public Chunk011_038(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x013 chunk

        [Chunk(0x03038000, 0x013)]
        public class Chunk013 : Chunk
        {
            public Chunk013(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x014 chunk

        [Chunk(0x03038000, 0x014)]
        public class Chunk014 : Chunk
        {
            public Chunk014(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03038000, 0x015)]
        public class Chunk015 : Chunk
        {
            public Chunk015(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x016 chunk

        [Chunk(0x03038000, 0x016)]
        public class Chunk016 : Chunk
        {
            public Chunk016(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x017 chunk

        [Chunk(0x03038000, 0x017)]
        public class Chunk017 : Chunk
        {
            public Chunk017(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x018 chunk

        [Chunk(0x03038000, 0x018)]
        public class Chunk018 : Chunk
        {
            public Chunk018(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x019 chunk

        [Chunk(0x03038000, 0x019)]
        public class Chunk019 : Chunk
        {
            public Chunk019(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x01A chunk

        [Chunk(0x03038000, 0x01A)]
        public class Chunk01A : Chunk
        {
            public Chunk01A(CGameCtnCollector node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #endregion
    }
}
