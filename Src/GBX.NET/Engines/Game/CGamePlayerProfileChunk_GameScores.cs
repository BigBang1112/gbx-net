namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_GameScores : IReadableWritable
{

    public partial CGamePlayerOfficialScores? OfficialScores1 { get => officialScores1Ref?.Resolve(); set => NodeRef.SetEmbedded(ref officialScores1Ref, value); }

    public partial CGamePlayerOfficialScores? OfficialScores2 { get => officialScores2Ref?.Resolve(); set => NodeRef.SetEmbedded(ref officialScores2Ref, value); }

    private NodeRef<CGameCampaignPlayerScores>?[]? oldCampaignPlayerScoresRefs;

    /// <summary>
    /// Campaign scores of the old archive, which the game reads and discards.
    /// </summary>
    public CGameCampaignPlayerScores?[]? OldCampaignPlayerScores
    {
        get => oldCampaignPlayerScoresRefs?.Select(static x => x?.Resolve()).ToArray();
        set => oldCampaignPlayerScoresRefs = value?.Select(static x =>
        {
            NodeRef<CGameCampaignPlayerScores>? reference = null;
            NodeRef.SetEmbedded(ref reference, x);
            return reference;
        }).ToArray();
    }

    public string? OldU01 { get; set; }
    public string?[]? OldU02 { get; set; }
    public DateTime?[]? OldU03 { get; set; }

    /// <summary>
    /// Reads or writes the old archive (ArchiveOldVersion). Payloads before version 3 store the scores only in buffer memory.
    /// </summary>
    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (v < 3 && (v == 0 || !IsBufferMemoryArchivation))
        {
            if (v == 0)
            {
                throw new NotSupportedException("Version 0 of the old game scores archive is not supported.");
            }

            return;
        }

        var trainingCount = rw.Int32(trainingMedalsScores?.Length ?? 0);

        if (rw.Reader is not null)
        {
            trainingMedalsScores = new STrainingMedalsScores[trainingCount];
        }

        for (var i = 0; i < trainingCount; i++)
        {
            (trainingMedalsScores![i] ??= new()).ReadWriteOld(rw, this);
        }

        ReadWriteArchivedNode(rw, ref officialScores1Ref);

        var campaignCount = rw.Int32(oldCampaignPlayerScoresRefs?.Length ?? 0);

        if (rw.Reader is not null)
        {
            oldCampaignPlayerScoresRefs = new NodeRef<CGameCampaignPlayerScores>?[campaignCount];
        }

        for (var i = 0; i < campaignCount; i++)
        {
            ReadWriteArchivedNode(rw, ref oldCampaignPlayerScoresRefs![i]);
        }

        ReadWriteArchivedNode(rw, ref officialScores2Ref);
        rw.ArrayReadableWritable<LadderMatchResult>(ref ladderMatchResults!);
        OldU01 = rw.String(OldU01);

        var count = rw.Int32(OldU02?.Length ?? 0);

        if (rw.Reader is not null)
        {
            OldU02 = new string?[count];
            OldU03 = new DateTime?[count];
        }

        for (var i = 0; i < count; i++)
        {
            OldU02![i] = rw.Id(OldU02[i]);
            OldU03![i] = rw.SystemTime(OldU03[i]);
        }
    }

    public partial class STrainingMedalsScores
    {
        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public CGamePlayerOfficialScores? OfficialScores { get => officialScoresRef?.Resolve(); set => NodeRef.SetEmbedded(ref officialScoresRef, value); }

        internal void ReadWriteOld(GbxReaderWriter rw, CGamePlayerProfileChunk_GameScores n)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
            n.ReadWriteArchivedNode(rw, ref officialScoresRef);
        }
    }
}
