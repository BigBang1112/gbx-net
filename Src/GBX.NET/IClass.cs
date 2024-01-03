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
    /// Creates a new instance of a header chunk that matches the given chunk ID under this class scope, including the inherited classes. Do not override, implementation is source generated.
    /// </summary>
    /// <param name="chunkId">ID of the header chunk.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs in the switch statement. For wider range of available chunks, use <see cref="ClassManager.NewHeaderChunk(uint)"/>.</remarks>
    /// <returns>A new instance of the header chunk.</returns>
    static abstract IHeaderChunk? NewHeaderChunk(uint chunkId);

    /// <summary>
    /// Creates a new instance of a chunk that matches the given chunk ID under this class scope, including the inherited classes. Do not override, implementation is source generated.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs in the switch statement. For wider range of available chunks, use <see cref="ClassManager.NewChunk(uint)"/>.</remarks>
    /// <returns>A new instance of the chunk.</returns>
    static abstract IChunk? NewChunk(uint chunkId);

    /// <summary>
    /// Creates a new instance of a class that matches the given class ID, including the classes that inherit this one. Do not override, implementation is source generated.
    /// </summary>
    /// <param name="classId">ID of the class.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected classes in the switch statement. For wider range of available classes, use <see cref="ClassManager.New(uint)"/>.</remarks>
    /// <returns>A new instance of the class.</returns>
    static abstract IClass? New(uint classId);

    /// <summary>
    /// Reads the contents of the node using the reader/writer. Classes based on <see cref="CMwNod"/> use chunked reading, but this behaviour can be statically overridden.
    /// </summary>
    /// <typeparam name="T">Type of the class to focus on.</typeparam>
    /// <param name="node">Node to populate.</param>
    /// <param name="rw">Reader/writer.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs.</remarks>
    static abstract void Read<T>(T node, IGbxReaderWriter rw) where T : IClass;
#endif

    /// <summary>
    /// A set of body chunks. Sorting by chunk ID is not guaranteed.
    /// </summary>
    IChunkSet Chunks { get; }

    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses this object.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(IGbxReaderWriter rw);
}