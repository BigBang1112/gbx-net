namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_InputBindingsConfig : IReadableWritable
{

    public partial CInputBindingsConfig? Config { get => configRef?.Resolve(); set => NodeRef.SetEmbedded(ref configRef, value); }

    /// <summary>
    /// Reads or writes the old archive (ArchiveOldVersion).
    /// </summary>
    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        ReadWriteArchivedNode(rw, ref configRef);
    }
}
