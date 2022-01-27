using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk<T> : Chunk, IReadableWritableChunk where T : Node
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
    void IReadableWritableChunk.Read(Node n, GameBoxReader r)
    {
        Read((T)n, r);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void IReadableWritableChunk.Write(Node n, GameBoxWriter w)
    {
        Write((T)n, w);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void IReadableWritableChunk.ReadWrite(Node n, GameBoxReaderWriter rw)
    {
        ReadWrite((T)n, rw);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    async Task IReadableWritableChunk.ReadAsync(Node n, GameBoxReader r, CancellationToken cancellationToken)
    {
        await ReadAsync((T)n, r, logger: null, cancellationToken);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    async Task IReadableWritableChunk.WriteAsync(Node n, GameBoxWriter w, CancellationToken cancellationToken)
    {
        await WriteAsync((T)n, w, logger: null, cancellationToken);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    async Task IReadableWritableChunk.ReadWriteAsync(Node n, GameBoxReaderWriter rw, CancellationToken cancellationToken)
    {
        await ReadWriteAsync((T)n, rw, logger: null, cancellationToken);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual void Read(T n, GameBoxReader r, ILogger? logger)
    {
        Read(n, r);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual void Read(T n, GameBoxReader r)
    {
        throw new ChunkReadNotImplementedException(Id, Node);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void Write(T n, GameBoxWriter w, ILogger? logger)
    {
        Write(n, w);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void Write(T n, GameBoxWriter w)
    {
        throw new ChunkWriteNotImplementedException(Id, Node);
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
        if (rw.Reader is not null)
        {
            Read(n, rw.Reader, logger);
        }
        else if (rw.Writer is not null)
        {
            Write(n, rw.Writer, logger);
        }
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual Task ReadAsync(T n, GameBoxReader r, ILogger? logger, CancellationToken cancellationToken = default)
    {
        throw new ChunkReadNotImplementedException(Id, Node);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual Task WriteAsync(T n, GameBoxWriter w, ILogger? logger, CancellationToken cancellationToken = default)
    {
        throw new ChunkWriteNotImplementedException(Id, Node);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual async Task ReadWriteAsync(T n, GameBoxReaderWriter rw, ILogger? logger, CancellationToken cancellationToken = default)
    {
        if (rw.Reader is not null)
        {
            await ReadAsync(n, rw.Reader, logger, cancellationToken);
        }
        else if (rw.Writer is not null)
        {
            await WriteAsync(n, rw.Writer, logger, cancellationToken);
        }
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

