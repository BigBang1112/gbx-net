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
        public MapInfo[] MapInfos
        {
            get => GetValue<Chunk00B>(x => x.MapInfos) as MapInfo[];
            set => SetValue<Chunk00B>(x => x.MapInfos = value);
        }

        public CGameCtnChallengeGroup(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x0308F002)]
        public class Chunk002 : Chunk
        {
            public string Default { get; set; }

            public Chunk002(CGameCtnChallengeGroup node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Default = rw.String(Default);
            }
        }

        #endregion

        #region 0x00B chunk

        [Chunk(0x0308F00B)]
        public class Chunk00B : Chunk
        {
            public int Version { get; set; }
            public MapInfo[] MapInfos { get; set; }

            public Chunk00B(CGameCtnChallengeGroup node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                MapInfos = rw.Array(MapInfos, i =>
                {
                    var mapInfo = rw.Reader.ReadMeta();
                    var filePath = rw.Reader.ReadString();
                    return new MapInfo(mapInfo, filePath);
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
            public Meta Metadata { get; }
            public string FilePath { get; set; }

            public MapInfo(Meta metadata, string filePath)
            {
                Metadata = metadata;
                FilePath = filePath;
            }

            public override string ToString()
            {
                return Path.GetFileName(FilePath);
            }
        }
    }
}
