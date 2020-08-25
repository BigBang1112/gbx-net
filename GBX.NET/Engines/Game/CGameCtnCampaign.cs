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
        public CGameCtnChallengeGroup[] MapGroups
        {
            get => GetValue<Chunk000>(x => x.MapGroups) as CGameCtnChallengeGroup[];
        }

        public int Index
        {
            get => (int)GetValue<Chunk00A>(x => x.Index);
            set => SetValue<Chunk00A>(x => x.Index = value);
        }

        public CGameCtnCampaign(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03090000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public CGameCtnChallengeGroup[] MapGroups { get; set; }

            public Chunk000(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                MapGroups = rw.Array(MapGroups,
                    i => rw.Reader.ReadNodeRef<CGameCtnChallengeGroup>(),
                    x => rw.Writer.Write(x));
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x03090006)]
        public class Chunk006 : Chunk
        {
            public string CampaignID { get; set; }

            public Chunk006(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                CampaignID = rw.LookbackString(CampaignID);
            }
        }

        #endregion

        #region 0x009 skippable chunk

        [Chunk(0x03090009, true)]
        public class Chunk009 : Chunk
        {
            public Chunk009(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Byte(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00A skippable chunk

        [Chunk(0x0309000A, true)]
        public class Chunk00A : Chunk
        {
            public int Index { get; set; }

            public Chunk00A(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Index = rw.Int32(Index);
            }
        }

        #endregion

        #region 0x00D chunk

        [Chunk(0x0309000D)]
        public class Chunk00D : Chunk
        {
            public Chunk00D(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00E chunk

        [Chunk(0x0309000E)]
        public class Chunk00E : Chunk
        {
            public Chunk00E(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x00F skippable chunk

        [Chunk(0x0309000F, true)]
        public class Chunk00F : Chunk
        {
            public string Name { get; set; }
            public int Type { get; set; }
            public int UnlockType { get; set; }

            public Chunk00F(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Name = rw.String(Name);
                Type = rw.Int32(Type);
                UnlockType = rw.Int32(UnlockType);
            }
        }

        #endregion

        #region 0x010 chunk

        [Chunk(0x03090010)]
        public class Chunk010 : Chunk
        {
            public Chunk010(CGameCtnCampaign node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #endregion
    }
}
