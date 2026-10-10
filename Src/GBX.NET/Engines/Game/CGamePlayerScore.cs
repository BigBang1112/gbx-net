namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public partial class Score
    {
        public Score()
        {
            PersonalBest = new(-1);
        }

        [Obsolete("Use EditPlayTimeSeconds instead.")]
        public int U09 { get => EditPlayTimeSeconds; set => EditPlayTimeSeconds = value; }

        [Obsolete("Use RacePlayTimeSeconds instead.")]
        public int U10 { get => RacePlayTimeSeconds; set => RacePlayTimeSeconds = value; }

        [Obsolete("Use NetPlayTimeSeconds instead.")]
        public int U11 { get => NetPlayTimeSeconds; set => NetPlayTimeSeconds = value; }

        [Obsolete("Use ResetCount instead.")]
        public short U12 { get => ResetCount; set => ResetCount = value; }

        [Obsolete("Use FinishCount instead.")]
        public short U13 { get => FinishCount; set => FinishCount = value; }

        [Obsolete("Use PlatformBestResetCount instead.")]
        public int U14 { get => PlatformBestResetCount; set => PlatformBestResetCount = value; }

        [Obsolete("Use PlatformMaxCompletedCount instead.")]
        public int U15 { get => PlatformMaxCompletedCount; set => PlatformMaxCompletedCount = value; }

        [Obsolete("Use StuntsBestScore instead.")]
        public int U16 { get => StuntsBestScore; set => StuntsBestScore = value; }

        [Obsolete("Use OfficialRecordTime instead.")]
        public DateTime? U17 { get => OfficialRecordTime; set => OfficialRecordTime = value; }

        [Obsolete("Use OfficialMedal instead.")]
        public int U21 { get => OfficialMedal; set => OfficialMedal = value; }

        [Obsolete("Use PlayMode instead.")]
        public byte U22 { get => unchecked((byte)PlayMode); set => PlayMode = unchecked((EChallengePlayModeMS)value); }

        [Obsolete("Use OfficialBestRecord instead.")]
        public int U23 { get => OfficialBestRecord; set => OfficialBestRecord = value; }

        [Obsolete("Use SubmittedEditPlayTimeSeconds instead.")]
        public int U29 { get => SubmittedEditPlayTimeSeconds; set => SubmittedEditPlayTimeSeconds = value; }

        [Obsolete("Use SubmittedRacePlayTimeSeconds instead.")]
        public int U30 { get => SubmittedRacePlayTimeSeconds; set => SubmittedRacePlayTimeSeconds = value; }

        [Obsolete("Use SubmittedNetPlayTimeSeconds instead.")]
        public int U31 { get => SubmittedNetPlayTimeSeconds; set => SubmittedNetPlayTimeSeconds = value; }

        [Obsolete("Use SubmittedResetCount instead.")]
        public short U32 { get => SubmittedResetCount; set => SubmittedResetCount = value; }

        [Obsolete("Use SubmittedFinishCount instead.")]
        public short U33 { get => SubmittedFinishCount; set => SubmittedFinishCount = value; }
    }

    public partial class TrainingMedalsScore
    {
        [Obsolete("Use PlayMode instead.")]
        public int U01 { get => unchecked((int)PlayMode); set => PlayMode = unchecked((CGameCtnChallenge.PlayMode)value); }
    }

    public partial class CampaignRecordsState
    {
        [Obsolete("Use LastUpdatedTime instead.")]
        public DateTime? U01 { get => LastUpdatedTime; set => LastUpdatedTime = value; }
    }
}
