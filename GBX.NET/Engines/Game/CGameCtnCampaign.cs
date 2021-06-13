using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Campaign info (0x03090000)
    /// </summary>
    /// <remarks>Information about a campaign.</remarks>
    [Node(0x03090000)]
    public class CGameCtnCampaign : CMwNod
    {
        #region Properties

        public CGameCtnChallengeGroup[] MapGroups { get; set; }

        public string CampaignID { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }
        public int Type { get; set; }
        public int UnlockType { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk (map groups)

        /// <summary>
        /// CGameCtnCampaign 0x000 chunk (map groups)
        /// </summary>
        [Chunk(0x03090000, "map groups")]
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

        #region 0x006 chunk (campaign ID)

        /// <summary>
        /// CGameCtnCampaign 0x006 chunk (campaign ID)
        /// </summary>
        [Chunk(0x03090006, "campaign ID")]
        public class Chunk03090006 : Chunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                n.CampaignID = rw.Id(n.CampaignID);
            }
        }

        #endregion

        #region 0x009 skippable chunk

        /// <summary>
        /// CGameCtnCampaign 0x009 skippable chunk
        /// </summary>
        [Chunk(0x03090009)]
        public class Chunk03090009 : SkippableChunk<CGameCtnCampaign>
        {
            public byte U01;
            public int U02;

            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Byte(ref U01);
                rw.Int32(ref U02);
            }
        }

        #endregion

        #region 0x00A skippable chunk (index)

        /// <summary>
        /// CGameCtnCampaign 0x00A skippable chunk (index)
        /// </summary>
        [Chunk(0x0309000A, "index")]
        public class Chunk0309000A : SkippableChunk<CGameCtnCampaign>
        {
            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                n.Index = rw.Int32(n.Index);
            }
        }

        #endregion

        #region 0x00D chunk

        /// <summary>
        /// CGameCtnCampaign 0x00D chunk
        /// </summary>
        [Chunk(0x0309000D)]
        public class Chunk0309000D : Chunk<CGameCtnCampaign>
        {
            public int U01;

            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }

        #endregion

        #region 0x00E chunk

        /// <summary>
        /// CGameCtnCampaign 0x00E chunk
        /// </summary>
        [Chunk(0x0309000E)]
        public class Chunk0309000E : Chunk<CGameCtnCampaign>
        {
            public int U01;

            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }

        #endregion

        #region 0x00F skippable chunk (name)


        /// <summary>
        /// CGameCtnCampaign 0x00F skippable chunk (name)
        /// </summary>
        [Chunk(0x0309000F, "name")]
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

        /// <summary>
        /// CGameCtnCampaign 0x010 chunk
        /// </summary>
        [Chunk(0x03090010)]
        public class Chunk03090010 : Chunk<CGameCtnCampaign>
        {
            public int U01;
            public int U02;

            public override void ReadWrite(CGameCtnCampaign n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
                rw.Int32(ref U02);
            }
        }

        #endregion

        #endregion
    }
}
