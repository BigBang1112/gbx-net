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
        public int U09 { get => unchecked((int)EditPlayTimeSeconds); set => EditPlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use RacePlayTimeSeconds instead.")]
        public int U10 { get => unchecked((int)RacePlayTimeSeconds); set => RacePlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use NetPlayTimeSeconds instead.")]
        public int U11 { get => unchecked((int)NetPlayTimeSeconds); set => NetPlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use ResetCount instead.")]
        public short U12 { get => unchecked((short)ResetCount); set => ResetCount = unchecked((ushort)value); }

        [Obsolete("Use FinishCount instead.")]
        public short U13 { get => unchecked((short)FinishCount); set => FinishCount = unchecked((ushort)value); }

        [Obsolete("Use PlatformBestResetCount instead.")]
        public int U14 { get => unchecked((int)PlatformBestResetCount); set => PlatformBestResetCount = unchecked((uint)value); }

        [Obsolete("Use PlatformMaxCompletedCount instead.")]
        public int U15 { get => unchecked((int)PlatformMaxCompletedCount); set => PlatformMaxCompletedCount = unchecked((uint)value); }

        [Obsolete("Use StuntsBestScore instead.")]
        public int U16 { get => unchecked((int)StuntsBestScore); set => StuntsBestScore = unchecked((uint)value); }

        [Obsolete("Use OfficialRecordTime instead.")]
        public DateTime? U17 { get => OfficialRecordTime; set => OfficialRecordTime = value; }

        [Obsolete("Use OfficialMedal instead.")]
        public int U21 { get => unchecked((int)OfficialMedal); set => OfficialMedal = unchecked((uint)value); }

        [Obsolete("Use PlayMode instead.")]
        public byte U22 { get => unchecked((byte)PlayMode); set => PlayMode = unchecked((EChallengePlayModeMS)value); }

        [Obsolete("Use OfficialBestRecord instead.")]
        public int U23 { get => unchecked((int)OfficialBestRecord); set => OfficialBestRecord = unchecked((uint)value); }

        [Obsolete("Use SubmittedEditPlayTimeSeconds instead.")]
        public int U29 { get => unchecked((int)SubmittedEditPlayTimeSeconds); set => SubmittedEditPlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use SubmittedRacePlayTimeSeconds instead.")]
        public int U30 { get => unchecked((int)SubmittedRacePlayTimeSeconds); set => SubmittedRacePlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use SubmittedNetPlayTimeSeconds instead.")]
        public int U31 { get => unchecked((int)SubmittedNetPlayTimeSeconds); set => SubmittedNetPlayTimeSeconds = unchecked((uint)value); }

        [Obsolete("Use SubmittedResetCount instead.")]
        public short U32 { get => unchecked((short)SubmittedResetCount); set => SubmittedResetCount = unchecked((ushort)value); }

        [Obsolete("Use SubmittedFinishCount instead.")]
        public short U33 { get => unchecked((short)SubmittedFinishCount); set => SubmittedFinishCount = unchecked((ushort)value); }
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
