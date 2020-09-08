using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03079000)]
    public class CGameCtnMediaClip : Node
    {
        public string Name { get; set; }

        public List<CGameCtnMediaTrack> Tracks { get; set; }

        public CGameCtnMediaClip(ILookbackable lookbackable) : base(lookbackable)
        {

        }

        public CGameCtnMediaClip(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        public override string ToString()
        {
            return Name;
        }

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
                rw.Int32(Unknown);
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
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0307900D)]
        public class Chunk0307900D : Chunk<CGameCtnMediaClip>
        {
            public int Version { get; set; } = 10;

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }
            public float Unknown6 { get; set; } = 0.2f;
            public int Unknown7 { get; set; } = -1;

            public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
                Version = rw.Int32(Version);

                n.Tracks = rw.Array(n.Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();

                n.Name = rw.String(n.Name);

                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Int32(Unknown3);
                Unknown4 = rw.Int32(Unknown4);
                Unknown5 = rw.Int32(Unknown5);
                Unknown6 = rw.Single(Unknown6);
                Unknown7 = rw.Int32(Unknown7); // -1
            }
        }

        #endregion

        #endregion
    }
}
