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
        public string Name
        {
            get => GetValue<Chunk001>(x => x.Name) as string;
            set => SetValue<Chunk001>(x => x.Name = value);
        }

        public List<CGameCtnMediaBlock> Blocks
        {
            get => GetValue<Chunk001>(x => x.Blocks) as List<CGameCtnMediaBlock>;
            set => SetValue<Chunk001>(x => x.Blocks = value);
        }

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
        public class Chunk001 : Chunk
        {
            public string Name { get; set; }
            public List<CGameCtnMediaBlock> Blocks { get; set; } = new List<CGameCtnMediaBlock>();
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Name = rw.String(Name);
                Unknown1 = rw.Int32(Unknown1); // 10, probably version
                Blocks = rw.Array(Blocks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaBlock>(true),
                    x => rw.Writer.Write(x))?.ToList();
                Unknown2 = rw.Int32(Unknown2);
            }
        }

        [Chunk(0x03078002)]
        public class Chunk002 : Chunk
        {
            public bool Unknown1 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Boolean(Unknown1); // 1
            }
        }

        [Chunk(0x03078000, 0x003)]
        public class Chunk003 : Chunk
        {
            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown); // 0
            }
        }

        /// <summary>
        /// CGameCtnMediaTrack 0x004 chunk. This chunk should be removed in the new versions of ManiaPlanet.
        /// </summary>
        [Chunk(0x03078004)]
        public class Chunk004 : Chunk
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
            }
        }

        [Chunk(0x03078005)]
        public class Chunk005 : Chunk
        {
            public int Version { get; set; } = 1;
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public float Unknown4 { get; set; } = -1;
            public float Unknown5 { get; set; } = -1;

            public override void ReadWrite(GameBoxReaderWriter rw)
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
