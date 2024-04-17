namespace GBX.NET.Engines.Game;

public partial class CGameCampaignPlayerScores
{
    public string? CampaignId { get; private set; }

    public partial class Chunk030EA001
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
        public GameModeScores[]? U11;

        public override void Read(CGameCampaignPlayerScores n, GbxReader r)
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

            U11 = r.ReadArrayReadable<GameModeScores>();
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite, PrivateSet = true)]
    public partial class GameModeScores;
}
