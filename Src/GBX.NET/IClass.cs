namespace GBX.NET;

/// <summary>
/// A Gbx class interface.
/// </summary>
public interface IClass
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// ID of the class.
    /// </summary>
    static abstract uint Id { get; }

    /// <summary>
    /// Creates a new instance of a header chunk that matches the given chunk ID under this class scope, including the inherited classes.
    /// </summary>
    /// <param name="chunkId">Chunk ID of the header chunk.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs in the switch statement. For wider range of available chunks, use <see cref="ClassManager.NewHeaderChunk(uint)"/>.</remarks>
    /// <returns>A new instance of the header chunk.</returns>
    static abstract IHeaderChunk? NewHeaderChunk(uint chunkId);

    /// <summary>
    /// Creates a new instance of a chunk that matches the given chunk ID under this class scope, including the inherited classes.
    /// </summary>
    /// <param name="chunkId">Chunk ID of the chunk.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs in the switch statement. For wider range of available chunks, use <see cref="ClassManager.NewChunk(uint)"/>.</remarks>
    /// <returns>A new instance of the chunk.</returns>
    static abstract IChunk? NewChunk(uint chunkId);

    static abstract T Read<T>(T node, IGbxReaderWriter rw) where T : IClass;
#endif

    IChunkSet Chunks { get; }

    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses this object.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(IGbxReaderWriter rw);
}