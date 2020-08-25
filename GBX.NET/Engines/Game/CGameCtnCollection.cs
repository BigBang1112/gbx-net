using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03033000)]
    public class CGameCtnCollection : CMwNod
    {
        public CGameCtnCollection(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x03033001)]
        public class Chunk001 : Chunk, ILookbackable
        {
            int? ILookbackable.LookbackVersion { get; set; } = 3;
            List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
            bool ILookbackable.LookbackWritten { get; set; }

            public Chunk001(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadByte();
                var collectionID = r.ReadByte();
                _ = r.ReadBytes(6);
                var collectionPackMask = r.ReadByte();
                var displayName = r.ReadString();
                _ = r.ReadInt32();
                var collectionIcon = r.ReadString();
                _ = r.ReadArray<int>(2);
                var blockInfoFlat = r.ReadLookbackString(this);
                var vehicle = r.ReadMeta(this);
                _ = r.ReadInt32();
                _ = r.ReadArray<float>(4);
                var loadingScreen = r.ReadString();
                _ = r.ReadArray<int>(7);
                _ = r.ReadString();
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03033002)]
        public class Chunk002 : Chunk
        {
            public Chunk002(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadByte();
                _ = r.ReadString();
                _ = r.ReadInt32();
                _ = r.ReadString();
                _ = r.ReadArray<int>(3);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x03033003)]
        public class Chunk003 : Chunk
        {
            public Chunk003(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadByte();
                _ = r.ReadString();
            }
        }

        #endregion

        #region 0x009 chunk

        [Chunk(0x03033009)]
        public class Chunk009 : Chunk
        {
            public Chunk009(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadLookbackString();
                _ = r.ReadInt32();
                _ = r.ReadArray(x => r.ReadInt32());
                _ = r.ReadInt32();
                _ = r.ReadArray<float>(3);
                var vehicle = r.ReadMeta();
            }
        }

        #endregion

        #region 0x00C chunk

        [Chunk(0x0303300C)]
        public class Chunk00C : Chunk
        {
            public Chunk00C(CGameCtnCollection node, int? length) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0303300D)]
        public class Chunk00D : Chunk
        {
            public Chunk00D(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
                r.ReadInt32();
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x0303300E)]
        public class Chunk00E : Chunk
        {
            public Chunk00E(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x011 chunk

        [Chunk(0x03033011)]
        public class Chunk011 : Chunk
        {
            public Chunk011(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x019 chunk

        [Chunk(0x03033019)]
        public class Chunk019 : Chunk
        {
            public Chunk019(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadInt32();
            }
        }

        #endregion

        #region 0x01A chunk

        [Chunk(0x0303301A)]
        public class Chunk01A : Chunk
        {
            public Chunk01A(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32(); // -1
                _ = r.ReadArray<float>(11);
            }
        }

        #endregion

        #region 0x01D chunk

        [Chunk(0x0303301D)]
        public class Chunk01D : Chunk
        {
            public Chunk01D(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
                _ = r.ReadMeta();
                _ = r.ReadInt32();
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x01F chunk

        [Chunk(0x0303301F)]
        public class Chunk01F : Chunk
        {
            public Chunk01F(CGameCtnCollection node, int? length) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x020 chunk

        [Chunk(0x03033020)]
        public class Chunk020 : Chunk
        {
            public Chunk020(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadString();
                _ = r.ReadInt32();
                _ = r.ReadString();
                _ = r.ReadString();
            }
        }

        #endregion

        #region 0x021 chunk

        [Chunk(0x03033021)]
        public class Chunk021 : Chunk
        {
            public Chunk021(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadString();
            }
        }

        #endregion

        #region 0x027 chunk

        [Chunk(0x03033027)]
        public class Chunk027 : Chunk
        {
            public Chunk027(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x028 chunk

        [Chunk(0x03033028)]
        public class Chunk028 : Chunk
        {
            public Chunk028(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x029 chunk

        [Chunk(0x03033029)]
        public class Chunk029 : Chunk
        {
            public Chunk029(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x02A chunk

        [Chunk(0x0303302A)]
        public class Chunk02A : Chunk
        {
            public Chunk02A(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x02C chunk

        [Chunk(0x0303302C)]
        public class Chunk02C : Chunk
        {
            public Chunk02C(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadArray<int>(4);
            }
        }

        #endregion

        #region 0x02F chunk

        [Chunk(0x0303302F)]
        public class Chunk02F : Chunk
        {
            public Chunk02F(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadVec3();
            }
        }

        #endregion

        #region 0x030 chunk

        [Chunk(0x03033030)]
        public class Chunk030 : Chunk
        {
            public Chunk030(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x031 chunk

        [Chunk(0x03033031)]
        public class Chunk031 : Chunk
        {
            public Chunk031(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #region 0x033 chunk

        [Chunk(0x03033033)]
        public class Chunk033 : Chunk
        {
            public Chunk033(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
                _ = r.ReadArray(i => r.ReadLookbackString());
            }
        }

        #endregion

        #region 0x034 chunk

        [Chunk(0x03033034)]
        public class Chunk034 : Chunk
        {
            public Chunk034(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadArray<int>(3);
            }
        }

        #endregion

        #region 0x036 chunk

        [Chunk(0x03033036)]
        public class Chunk036 : Chunk
        {
            public Chunk036(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadArray<int>(2);
            }
        }

        #endregion

        #region 0x037 chunk

        [Chunk(0x03033037)]
        public class Chunk037 : Chunk
        {
            public Chunk037(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadSingle();
            }
        }

        #endregion

        #region 0x038 chunk

        [Chunk(0x03033038)]
        public class Chunk038 : Chunk
        {
            public Chunk038(CGameCtnCollection node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                _ = r.ReadInt32();
            }
        }

        #endregion

        #endregion
    }
}
