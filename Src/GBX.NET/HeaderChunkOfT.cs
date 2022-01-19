using System.Reflection;

namespace GBX.NET;

public class HeaderChunk<T> : SkippableChunk<T>, IHeaderChunk where T : CMwNod
{
    public bool IsHeavy { get; set; }

    public HeaderChunk()
    {

    }

    public HeaderChunk(T node, byte[] data) : base(node, data)
    {

    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public override void ReadWrite(T n, GameBoxReaderWriter rw)
    {
        if (rw.Reader != null)
            Read(n, rw.Reader);

        if (rw.Writer != null)
            Write(n, rw.Writer);
    }

    public override string ToString()
    {
        var desc = GetType().GetCustomAttribute<ChunkAttribute>()?.Description;
        return $"{typeof(T).Name} header chunk 0x{GetChunkId():X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}";
    }
}