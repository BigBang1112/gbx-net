namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_GameScores
{

    public partial CGamePlayerOfficialScores? OfficialScores1 { get => officialScores1Ref?.Resolve(); set => NodeRef.SetEmbedded(ref officialScores1Ref, value); }

    public partial CGamePlayerOfficialScores? OfficialScores2 { get => officialScores2Ref?.Resolve(); set => NodeRef.SetEmbedded(ref officialScores2Ref, value); }

    public partial class STrainingMedalsScores
    {
        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public CGamePlayerOfficialScores? OfficialScores { get => officialScoresRef?.Resolve(); set => NodeRef.SetEmbedded(ref officialScoresRef, value); }
    }
}
