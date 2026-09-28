namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03153000</remarks>
[Class(0x03153000)]
public partial class CGameBuddy : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x03153000;




    private int version;
    public int Version { get => version; set => version = value; }

    private string? login;
    public string? Login { get => login; set => login = value; }

    private int u03;
    public int U03 { get => u03; set => u03 = value; }

    private DateTime? u04;
    public DateTime? U04 { get => u04; set => u04 = value; }

    private int u07;
    public int U07 { get => u07; set => u07 = value; }

    private int u08;
    public int U08 { get => u08; set => u08 = value; }

    private int skillsRank;
    public int SkillsRank { get => skillsRank; set => skillsRank = value; }

    private int skillsPoints;
    public int SkillsPoints { get => skillsPoints; set => skillsPoints = value; }

    private int ladderRank;
    public int LadderRank { get => ladderRank; set => ladderRank = value; }

    private int ladderPoints;
    public int LadderPoints { get => ladderPoints; set => ladderPoints = value; }

    private PackDesc? u13;
    public PackDesc? U13 { get => u13; set => u13 = value; }

    private bool invited;
    public bool Invited { get => invited; set => invited = value; }

    private bool waitingConfimation;
    public bool WaitingConfimation { get => waitingConfimation; set => waitingConfimation = value; }

    private CampaignMedal[]? campaignMedals;
    public CampaignMedal[]? CampaignMedals { get => campaignMedals; set => campaignMedals = value; }

    private string? path;
    public string? Path { get => path; set => path = value; }

    private bool canReceiveMessages;
    public bool CanReceiveMessages { get => canReceiveMessages; set => canReceiveMessages = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.Int32(ref this.version);
        if (Version==0)
        {
            rw.String(ref login);
            rw.Int32(ref u03);
            rw.FileTime(ref u04);
        }
        if (Version>=1)
        {
            rw.String(ref login);
            rw.String(ref nickName);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref skillsRank);
            rw.Int32(ref skillsPoints);
            rw.Int32(ref ladderRank);
            rw.Int32(ref ladderPoints);
            rw.PackDesc(ref u13);
            if (Version>=2)
            {
                rw.Boolean(ref invited);
                rw.Boolean(ref waitingConfimation);
                if (Version>=3)
                {
                    rw.ArrayReadableWritable<CampaignMedal>(ref campaignMedals!);
                    if (Version>=4)
                    {
                        rw.String(ref path);
                        if (Version>=5)
                        {
                            rw.Boolean(ref canReceiveMessages);
                        }
                    }
                }
            }
        }
    }



    public sealed partial class CampaignMedal : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
        }
    }


}
