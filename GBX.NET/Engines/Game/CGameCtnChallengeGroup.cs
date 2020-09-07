using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0308F000)]
    public class CGameCtnChallengeGroup : Node
    {
        public string Default { get; set; }
        public MapInfo[] MapInfos { get; set; }

        public CGameCtnChallengeGroup(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x0308F002)]
        public class Chunk0308F002 : Chunk<CGameCtnChallengeGroup>
        {
            public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
            {
                n.Default = rw.String(n.Default);
            }
        }

        #endregion

        #region 0x00B chunk

        [Chunk(0x0308F00B)]
        public class Chunk0308F00B : Chunk<CGameCtnChallengeGroup>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.MapInfos = rw.Array(n.MapInfos, i => new MapInfo()
                {
                    Metadata = rw.Reader.ReadMeta(),
                    FilePath = rw.Reader.ReadString()
                },
                x =>
                {
                    rw.Writer.Write(x.Metadata);
                    rw.Writer.Write(x.FilePath);
                });
            }
        }

        #endregion

        #endregion

        public class MapInfo
        {
            public Meta Metadata { get; set; }
            public string FilePath { get; set; }

            public override string ToString()
            {
                return Path.GetFileName(FilePath);
            }
        }
    }
}
