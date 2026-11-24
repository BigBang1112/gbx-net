namespace GBX.NET.Engines.Game;

/// <summary>
/// ManiaPlanet 1 profile chunk with a campaign node.
/// </summary>
public partial class CGamePlayerProfileChunk_Campaign : IReadableWritable
{
    private NodeRef<CMwNod>? nodeRef;

    public CMwNod? Node { get => nodeRef?.Resolve(); set => NodeRef.SetEmbedded(ref nodeRef, value); }

    /// <summary>
    /// Reads or writes the archive of ManiaPlanet 1.
    /// </summary>
    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        ReadWriteArchivedNode(rw, ref nodeRef);
    }
}
