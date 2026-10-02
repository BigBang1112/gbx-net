namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_GameScores
{
    private CGamePlayerOfficialScores? officialScores1;
    public CGamePlayerOfficialScores? OfficialScores1 { get => officialScores1; set => officialScores1 = value; }

    private CGamePlayerOfficialScores? officialScores2;
    public CGamePlayerOfficialScores? OfficialScores2 { get => officialScores2; set => officialScores2 = value; }

    public partial class STrainingMedalsScores
    {
        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private CGamePlayerOfficialScores? officialScores;
        public CGamePlayerOfficialScores? OfficialScores { get => officialScores; set => officialScores = value; }
    }
}
