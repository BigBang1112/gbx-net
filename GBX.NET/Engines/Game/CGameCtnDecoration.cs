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

        [Chunk(0x03038011)]
        public class Chunk011_038 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x013 chunk

        [Chunk(0x03038013)]
        public class Chunk013 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x014 chunk

        [Chunk(0x03038014)]
        public class Chunk014 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x015 chunk

        [Chunk(0x03038015)]
        public class Chunk015 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x016 chunk

        [Chunk(0x03038016)]
        public class Chunk016 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x017 chunk

        [Chunk(0x03038017)]
        public class Chunk017 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x018 chunk

        [Chunk(0x03038018)]
        public class Chunk018 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x019 chunk

        [Chunk(0x03038019)]
        public class Chunk019 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x01A chunk

        [Chunk(0x0303801A)]
        public class Chunk01A : Chunk
        {
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
