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
        public string Name
        {
            get => GetValue<Chunk003, Chunk00D>(x => x.Name, x => x.Name) as string;
            set => SetValue<Chunk003, Chunk00D>(x => x.Name = value, x => x.Name = value);
        }

        public List<CGameCtnMediaTrack> Tracks
        {
            get => GetValue<Chunk003, Chunk005, Chunk00D>(
                x => x.Tracks,
                x => x.Tracks,
                x => x.Tracks) as List<CGameCtnMediaTrack>;
        }

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
        public class Chunk003 : Chunk
        {
            public int Version { get; set; }
            public List<CGameCtnMediaTrack> Tracks { get; set; } = new List<CGameCtnMediaTrack>();
            public string Name { get; set; }

            public Chunk003(CGameCtnMediaClip node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Tracks = rw.Array(Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();
                Name = rw.String(Name);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x03079004)]
        public class Chunk004 : Chunk
        {
            public Chunk004(CGameCtnMediaClip node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.NodeRef(Unknown);
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x03079000, 0x005)]
        public class Chunk005 : Chunk
        {
            public int Version { get; set; }
            public List<CGameCtnMediaTrack> Tracks { get; set; } = new List<CGameCtnMediaTrack>();
            public string Name { get; set; }

            public Chunk005(CGameCtnMediaClip node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Tracks = rw.Array(Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();

                Name = rw.String(Name);
            }
        }

        [Chunk(0x03079007)]
        public class Chunk007 : Chunk
        {
            public Chunk007(CGameCtnMediaClip node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0307900D)]
        public class Chunk00D : Chunk
        {
            public int Version { get; set; } = 10;
            public List<CGameCtnMediaTrack> Tracks { get; set; } = new List<CGameCtnMediaTrack>();
            public string Name { get; set; }

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }
            public float Unknown6 { get; set; } = 0.2f;
            public int Unknown7 { get; set; } = -1;

            public Chunk00D(CGameCtnMediaClip node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Int32(Unknown1);
                Version = rw.Int32(Version);

                Tracks = rw.Array(Tracks?.ToArray(),
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaTrack>(),
                    x => rw.Writer.Write(x))?.ToList();

                Name = rw.String(Name);

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
