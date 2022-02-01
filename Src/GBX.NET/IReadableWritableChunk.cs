namespace GBX.NET;

public interface IReadableWritableChunk
{
    uint Id { get; }
    MemoryStream Unknown { get; }

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    void Read(Node n, GameBoxReader r);
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void Write(Node n, GameBoxWriter w);
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    void ReadWrite(Node n, GameBoxReaderWriter rw);

    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    Task ReadAsync(Node n, GameBoxReader r, CancellationToken cancellationToken = default);
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    Task WriteAsync(Node n, GameBoxWriter w, CancellationToken cancellationToken = default);
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="ChunkWriteNotImplementedException">Chunk does not support writing.</exception>
    Task ReadWriteAsync(Node n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default);
}