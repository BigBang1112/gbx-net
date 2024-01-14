namespace GBX.NET;

/// <summary>
/// A data chunk.
/// </summary>
public interface IChunk
{
    /// <summary>
    /// ID of the chunk.
    /// </summary>
    uint Id { get; }

    IChunk DeepClone();
}

/// <summary>
/// A data chunk.
/// </summary>
public abstract class Chunk : IReadableWritableChunk
{
    /// <inheritdoc />
    public abstract uint Id { get; }

    internal virtual void Read(IClass n, GbxReader r)
    {

    }

    internal virtual void Write(IClass n, GbxWriter w)
    {

    }

    internal virtual void ReadWrite(IClass n, GbxReaderWriter rw)
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

    /// <inheritdoc />
    public virtual void Read(IClass n, IGbxReader r) => Read(n, (GbxReader)r);

    /// <inheritdoc />
    public virtual void Write(IClass n, IGbxWriter w) => Write(n, (GbxWriter)w);

    /// <inheritdoc />
    public virtual void ReadWrite(IClass n, IGbxReaderWriter rw) => ReadWrite(n, (GbxReaderWriter)rw);

    public abstract IChunk DeepClone();
}