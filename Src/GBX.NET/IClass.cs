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
    /// Creates a new instance of a class that matches the given class ID, including the classes that inherit this one. Do not override, implementation is source generated.
    /// </summary>
    /// <param name="classId">ID of the class.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected classes in the switch statement. For wider range of available classes, use <see cref="Managers.ClassManager.New(uint)"/>.</remarks>
    /// <returns>A new instance of the class.</returns>
    static abstract IClass? New(uint classId);

    /// <summary>
    /// Reads the contents of the node using the reader/writer. Classes based on <see cref="CMwNod"/> use chunked reading, but this behaviour can be statically overridden.
    /// </summary>
    /// <typeparam name="T">Type of the class to focus on.</typeparam>
    /// <param name="node">Node to populate.</param>
    /// <param name="rw">Reader/writer.</param>
    /// <remarks>This method is intended to help with trimming (tree shaking) in .NET 8+ by only using expected chunk classes and structs.</remarks>
    internal static abstract void Read<T>(T node, GbxReaderWriter rw) where T : IClass;
#endif

    /// <summary>
    /// A set of body chunks. Sorting by chunk ID is not always guaranteed.
    /// </summary>
    IChunkSet Chunks { get; }

    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses this object.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(GbxReaderWriter rw);

    /// <summary>
    /// Safe method to create a new header chunk and add it to the chunk set, if it is valid. Returns null if the chunk ID is not supported in the context of the class.
    /// </summary>
    /// <param name="chunkId">ID of the header chunk.</param>
    /// <returns>A new header chunk instance, or null if the ID is not supported in the context of the class.</returns>
    IHeaderChunk? CreateHeaderChunk(uint chunkId);

    /// <summary>
    /// Safe method to create a new chunk and add it to the chunk set, if it is valid. Returns null if the chunk ID is not supported in the context of the class.
    /// </summary>
    /// <param name="chunkId">ID of the chunk.</param>
    /// <returns>A new chunk instance, or null if the ID is not supported in the context of the class.</returns>
    IChunk? CreateChunk(uint chunkId);

    IClass DeepClone();

    GameVersion GetGameVersion();
}