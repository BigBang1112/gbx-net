namespace GBX.NET;

public sealed class HeaderChunk : Chunk, IHeaderChunk
{
    private readonly uint headerChunkId;

    /// <exception cref="NotSupportedException"></exception>
    public bool Discovered
    {
        get => false;
        set => throw new NotSupportedException("Cannot discover an unknown header chunk.");
    }

    public byte[] Data { get; set; }

    public bool IsHeavy { get; set; }
    public GameBox? Gbx { get; set; } // Shouldn't be there forever

    public HeaderChunk(uint id, byte[] data, bool isHeavy = false) : base(null!)
    {
        headerChunkId = id;

        Data = data;
        IsHeavy = isHeavy;
    }

    protected override uint GetId()
    {
        return headerChunkId;
    }

    public void Discover() => throw new NotSupportedException("Cannot discover an unknown header chunk.");

    public void ReadWrite(GameBoxReaderWriter rw)
    {
        if (rw.Writer is not null)
        {
            Write(rw.Writer);
        }
    }

    public void Write(GameBoxWriter w)
    {
        w.Write(Data);
    }

    public async Task ReadWriteAsync(GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
    {
        if (rw.Writer is not null)
        {
            await WriteAsync(rw.Writer, cancellationToken);
        }
    }

    public async Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken = default)
    {
        await w.WriteBytesAsync(Data, cancellationToken);
    }

    void IReadableWritableChunk.Read(Node n, GameBoxReader r)
    {
        throw new NotSupportedException();
    }

    void IReadableWritableChunk.Write(Node n, GameBoxWriter w)
    {
        Write(w);
    }

    void IReadableWritableChunk.ReadWrite(Node n, GameBoxReaderWriter rw)
    {
        ReadWrite(rw);
    }

    Task IReadableWritableChunk.ReadAsync(Node n, GameBoxReader r, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    async Task IReadableWritableChunk.WriteAsync(Node n, GameBoxWriter w, CancellationToken cancellationToken)
    {
        await WriteAsync(w, cancellationToken);
    }

    async Task IReadableWritableChunk.ReadWriteAsync(Node n, GameBoxReaderWriter rw, CancellationToken cancellationToken)
    {
        await ReadWriteAsync(rw, cancellationToken);
    }

    public override string ToString()
    {
        return $"Header chunk 0x{Id:X8}";
    }
}
