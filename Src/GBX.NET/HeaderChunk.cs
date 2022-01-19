namespace GBX.NET;

public sealed class HeaderChunk : Chunk, IHeaderChunk
{
    private readonly uint headerChunkId;

    public bool Discovered
    {
        get => false;
        set => throw new NotSupportedException("Cannot discover an unknown header chunk.");
    }

    public byte[] Data { get; set; }

    public bool IsHeavy { get; set; }

    public HeaderChunk(uint id, byte[] data) : base(null!)
    {
        headerChunkId = id;
        Data = data;
    }

    protected override uint GetChunkId()
    {
        return headerChunkId;
    }

    public void Discover() => throw new NotSupportedException("Cannot discover an unknown header chunk.");

    public void ReadWrite(GameBoxReaderWriter rw)
    {
        if (rw.Writer != null)
            Write(rw.Writer);
    }

    public void Write(GameBoxWriter w) => w.WriteBytes(Data);

    public override string ToString()
    {
        return $"Header chunk 0x{Id:X8}";
    }
}
