using System.Reflection;

namespace GBX.NET;

public class HeaderChunk<T> : Chunk<T>, IHeaderChunk where T : CMwNod
{
    private readonly uint? id;

    public bool IsHeavy { get; set; }
    public byte[] Data { get; set; }

    public HeaderChunk()
    {
        Data = null!;
    }

    public HeaderChunk(byte[] data, uint? id = null, bool isHeavy = false)
    {
        this.id = id;

        Data = data;
        IsHeavy = isHeavy;
    }

    protected override uint GetId()
    {
        return id ?? NodeCacheManager.GetChunkIdByType(typeof(T), GetType());
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
        return $"{typeof(T).Name} header chunk 0x{Id:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}";
    }

    public void Write(GameBoxWriter w)
    {
        w.Write(Data);
    }

    public async Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        await w.WriteBytesAsync(Data, cancellationToken);
    }
}