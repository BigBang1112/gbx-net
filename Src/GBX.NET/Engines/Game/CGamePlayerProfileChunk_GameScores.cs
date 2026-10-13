namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_GameScores
{

    public CGamePlayerOfficialScores? OfficialScores1 { get => officialScores1Ref.Resolve(); set => officialScores1Ref.SetEmbedded(value); }

    public CGamePlayerOfficialScores? OfficialScores2 { get => officialScores2Ref.Resolve(); set => officialScores2Ref.SetEmbedded(value); }

    public partial class STrainingMedalsScores
    {
        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public CGamePlayerOfficialScores? OfficialScores { get => officialScoresRef.Resolve(); set => officialScoresRef.SetEmbedded(value); }
    }
}
