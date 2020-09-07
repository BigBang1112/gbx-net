using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03090000)]
    public class CGameCtnCampaign : Node
    {
        public CGameCtnChallengeGroup[] MapGroups { get; set; }

        public string CampaignID { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }
        public int Type { get; set; }
        public int UnlockType { get; set; }

        public CGameCtnCampaign(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03090000)]
        public class Chunk03090000 : Chunk<CGameCtnCampaign>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.MapGroups = rw.Array(n.MapGroups,
                    i => rw.Reader.ReadNodeRef<CGameCtnChallengeGroup>(),
                    x => rw.Writer.Write(x));
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x03090006)]
        public class Chunk03090006 : Chunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                n.CampaignID = rw.LookbackString(n.CampaignID);
            }
        }

        #endregion

        #region 0x009 skippable chunk

        [Chunk(0x03090009)]
        public class Chunk03090009 : SkippableChunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00A skippable chunk

        [Chunk(0x0309000A)]
        public class Chunk0309000A : SkippableChunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                n.Index = rw.Int32(n.Index);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0309000D)]
        public class Chunk0309000D : Chunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x0309000E)]
        public class Chunk0309000E : Chunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00F skippable chunk

        [Chunk(0x0309000F)]
        public class Chunk0309000F : SkippableChunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                n.Name = rw.String(n.Name);
                n.Type = rw.Int32(n.Type);
                n.UnlockType = rw.Int32(n.UnlockType);
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x03090010)]
        public class Chunk03090010 : Chunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #endregion
    }
}
