namespace GBX.NET.Engines.Game;

/// <summary>
/// Profile chunk with map data, replaced by later profile chunks.
/// </summary>
public partial class CGamePlayerProfileChunk_Challenge : IReadableWritable
{
    private NodeRef<CGameCtnChallenge>? challengeRef;

    /// <summary>
    /// Map of archive version 0.
    /// </summary>
    public CGameCtnChallenge? Challenge { get => challengeRef?.Resolve(); set => NodeRef.SetEmbedded(ref challengeRef, value); }

    public string? U01 { get; set; }
    public byte[]? U02 { get; set; }

    /// <summary>
    /// Reads or writes the old archive (ArchiveOldVersion).
    /// </summary>
    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (v == 0)
        {
            ReadWriteArchivedNode(rw, ref challengeRef);
            return;
        }

        if (v >= 2)
        {
            U01 = rw.String(U01);
        }

        U02 = rw.Data(U02);
    }
}
