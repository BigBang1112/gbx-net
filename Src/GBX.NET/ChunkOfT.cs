using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk<T> : Chunk, IReadableWritableChunk where T : Node
{
    public bool IsHeader => this is IHeaderChunk;
    public bool IsBody => !IsHeader;

    protected override uint GetId()
    {
        return NodeManager.ChunkIdsByType[GetType()];
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
        if (n is T t)
        {
            ReadWrite(t, rw);
        }
        else if (this is IHeaderChunk headerChunk)
        {
            headerChunk.ReadWrite(rw);
        }
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    async Task IReadableWritableChunk.ReadAsync(Node n, GameBoxReader r, CancellationToken cancellationToken)
    {
        await ReadAsync((T)n, r, cancellationToken);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    async Task IReadableWritableChunk.WriteAsync(Node n, GameBoxWriter w, CancellationToken cancellationToken)
    {
        await WriteAsync((T)n, w, cancellationToken);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    async Task IReadableWritableChunk.ReadWriteAsync(Node n, GameBoxReaderWriter rw, CancellationToken cancellationToken)
    {
        await ReadWriteAsync((T)n, rw, cancellationToken);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual void Read(T n, GameBoxReader r)
    {
        throw new ChunkReadNotImplementedException(Id, n);
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void Write(T n, GameBoxWriter w)
    {
        throw new ChunkWriteNotImplementedException(Id, n);
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual void ReadWrite(T n, GameBoxReaderWriter rw)
    {
        if (rw.Reader is not null)
        {
            Read(n, rw.Reader);
        }

        if (rw.Writer is not null)
        {
            Write(n, rw.Writer);
        }
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    public virtual Task ReadAsync(T n, GameBoxReader r, CancellationToken cancellationToken = default)
    {
        Read(n, r);
        return Task.CompletedTask;
    }

    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual Task WriteAsync(T n, GameBoxWriter w, CancellationToken cancellationToken = default)
    {
        Write(n, w);
        return Task.CompletedTask;
    }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    public virtual async Task ReadWriteAsync(T n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
    {
        if (rw.Reader is not null)
        {
            await ReadAsync(n, rw.Reader, cancellationToken);
        }
        
        if (rw.Writer is not null)
        {
            await WriteAsync(n, rw.Writer, cancellationToken);
        }
    }

    public override string ToString()
    {
        var att = NodeManager.ChunkAttributesById[Id];
        var desc = att.Description;
        var version = (this as IVersionable)?.Version;
        return $"{typeof(T).Name} chunk 0x{Id:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(version is null ? "" : $" [v{version}]")}";
    }
}

