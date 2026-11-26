namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_ChallengesScores
{
    /// <summary>
    /// Reads or writes the old archive (ArchiveOldVersion). Online-save payloads omit the local data of each record.
    /// </summary>
    public virtual void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (!IsOnlineSaveArchivation)
        {
            rw.Int32(ref u01);
        }

        rw.ArrayReadableWritable<OldChallengeScores, CGamePlayerProfileChunk_ChallengesScores>(ref oldChallengesScores!, this, version: v);

        if (v >= 3)
        {
            rw.Array<int>(ref oldChallengesScoresU01!, oldChallengesScores?.Length ?? 0);
        }
    }
}
