using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03078000)]
    public class CGameCtnMediaTrack : Node
    {
        public string Name { get; set; }

        public List<CGameCtnMediaBlock> Blocks { get; set; }

        public CGameCtnMediaTrack(ILookbackable lookbackable) : base(lookbackable)
        {

        }

        public CGameCtnMediaTrack(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        public override string ToString()
        {
            return Name;
        }

        [Chunk(0x03078001)]
        public class Chunk03078001 : Chunk<CGameCtnMediaTrack>
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                n.Name = rw.String(n.Name);
                Unknown1 = rw.Int32(Unknown1); // 10, probably version
                n.Blocks = rw.Array(n.Blocks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaBlock>(true),
                    x => rw.Writer.Write(x))?.ToList();
                Unknown2 = rw.Int32(Unknown2);
            }
        }

        [Chunk(0x03078002)]
        public class Chunk03078002 : Chunk<CGameCtnMediaTrack>
        {
            public bool Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Boolean(Unknown1); // 1
            }
        }

        [Chunk(0x03078003)]
        public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
        {
            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown); // 0
            }
        }

        /// <summary>
        /// CGameCtnMediaTrack 0x004 chunk. This chunk should be removed in the new versions of ManiaPlanet.
        /// </summary>
        [Chunk(0x03078004)]
        public class Chunk03078004 : Chunk<CGameCtnMediaTrack>
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
            }
        }

        [Chunk(0x03078005)]
        public class Chunk03078005 : Chunk<CGameCtnMediaTrack>
        {
            public int Version { get; set; } = 1;
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public float Unknown4 { get; set; } = -1;
            public float Unknown5 { get; set; } = -1;

            public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Unknown1 = rw.Int32(Unknown1);
                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Int32(Unknown3);
                Unknown4 = rw.Single(Unknown4);
                Unknown5 = rw.Single(Unknown5);
            }
        }
    }
}
