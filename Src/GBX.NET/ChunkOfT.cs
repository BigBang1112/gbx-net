using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk<T> : Chunk, IChunk where T : CMwNod
{
    [IgnoreDataMember]
    public new T Node
    {
        get => (T)base.Node;
        internal set => base.Node = value;
    }

    public bool IsHeader => this is IHeaderChunk;
    public bool IsBody => !IsHeader;

    protected Chunk() : base(null!)
    {

    }

    protected Chunk(T node) : base(node)
    {

    }

    protected override uint GetId()
    {
        return NodeCacheManager.GetChunkIdByType(typeof(T), GetType());
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    void IChunk.Read(Node n, GameBoxReader r)
    {
        Read((T)n, r);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void IChunk.Write(Node n, GameBoxWriter w)
    {
        Write((T)n, w);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void IChunk.ReadWrite(Node n, GameBoxReaderWriter rw)
    {
        ReadWrite((T)n, rw);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual void Read(T n, GameBoxReader r, ILogger? logger)
    {
        throw new ChunkReadNotImplementedException(Id, Node);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual void Read(T n, GameBoxReader r)
    {
        Read(n, r, logger: null);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void Write(T n, GameBoxWriter w, ILogger? logger)
    {
        throw new ChunkWriteNotImplementedException(Id, Node);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void Write(T n, GameBoxWriter w)
    {
        Write(n, w, logger: null);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
    {
        ReadWrite(n, rw, logger: null);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void ReadWrite(T n, GameBoxReaderWriter rw, ILogger? logger)
    {
        if (rw.Reader != null)
            Read(n, rw.Reader);
        else if (rw.Writer != null)
            Write(n, rw.Writer);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public byte[] ToByteArray()
    {
        if (this is ILookbackable l)
        {
            l.IdWritten = false;
            l.IdStrings.Clear();
        }

        using var ms = new MemoryStream();
        using var w = CreateWriter(ms);
        var rw = new GameBoxReaderWriter(w);
        ReadWrite(Node, rw);
        return ms.ToArray();
    }

    public override string ToString()
    {
        var att = NodeCacheManager.ChunkAttributesByType[GetType()]
            .FirstOrDefault(x => x is ChunkAttribute) as ChunkAttribute;
        var desc = att?.Description;
        var version = (this as IVersionable)?.Version;
        return $"{typeof(T).Name} chunk 0x{Id:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(version is null ? "" : $" [v{version}]")}";
    }
}

