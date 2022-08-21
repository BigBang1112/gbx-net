namespace GBX.NET.Engines.Game;

[Node(0x030EA000)]
public class CGameCampaignPlayerScores : CMwNod
{
    [NodeMember]
    public string? CampaignId { get; private set; }

    protected CGameCampaignPlayerScores()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CGameCampaignPlayerScores 0x001 chunk
    /// </summary>
    [Chunk(0x030EA001)]
    public class Chunk030EA001 : Chunk<CGameCampaignPlayerScores>
    {
        public bool U02;
        public ulong U03;
        public byte U04;
        public ulong U05;
        public bool U06;
        public int U07;
        public int U08;
        public int U09;
        public bool U10;
        public (int, bool, int, int, int, bool)[]? U11;

        public override void Read(CGameCampaignPlayerScores n, GameBoxReader r)
        {
            n.CampaignId = r.ReadId();

            // CGameScoresVersion::ArchiveScoresVersion
            U02 = r.ReadBoolean(asByte: true);
            if (U02)
            {
                U03 = r.ReadUInt64(); // SSystemTime
                U04 = r.ReadByte();
            }
            //

            U05 = r.ReadUInt64(); // SSystemTime

            // CTransactionalNatural::Archive
            U06 = r.ReadBoolean(asByte: true);
            if (U06)
            {
                U07 = r.ReadInt32();
                U08 = r.ReadInt32();
                U09 = r.ReadInt32();
                U10 = r.ReadBoolean();
            }
            //

            U11 = r.ReadArray(r =>
            {
                var u01 = r.ReadInt32();
                var u02 = r.ReadBoolean(asByte: true);
                var u03 = default(int);
                var u04 = default(int);
                var u05 = default(int);
                var u06 = default(bool);

                if (u02)
                {
                    u03 = r.ReadInt32();
                    u04 = r.ReadInt32();
                    u05 = r.ReadInt32();
                    u06 = r.ReadBoolean();
                }

                return (u01, u02, u03, u04, u05, u06);
            });
        }
    }

    #endregion
}