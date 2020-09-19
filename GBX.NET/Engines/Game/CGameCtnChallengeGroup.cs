using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Group of maps (0x0308F000)
    /// </summary>
    [Node(0x0308F000)]
    public class CGameCtnChallengeGroup : Node
    {
        public string Default { get; set; }
        public MapInfo[] MapInfos { get; set; }

        #region Chunks

        #region 0x002 chunk (default)

        /// <summary>
        /// CGameCtnChallengeGroup 0x002 chunk (default)
        /// </summary>
        [Chunk(0x0308F002, "default")]
        public class Chunk0308F002 : Chunk<CGameCtnChallengeGroup>
        {
            public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
            {
                n.Default = rw.String(n.Default);
            }
        }

        #endregion

        #region 0x00B chunk (map infos)

        /// <summary>
        /// CGameCtnChallengeGroup 0x00B chunk (map infos)
        /// </summary>
        [Chunk(0x0308F00B, "map infos")]
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
