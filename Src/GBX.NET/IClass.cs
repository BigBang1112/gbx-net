using GBX.NET.Components;

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

    GameVersion GameVersion { get; }
    bool IsGameVersion(GameVersion version, bool strict = false);
    bool CanBeGameVersion(GameVersion version);

    Gbx ToGbx(GbxHeaderBasic headerBasic);
    Gbx ToGbx();

    void Save(Stream stream, GbxWriteSettings settings = default);
    void Save(string fileName, GbxWriteSettings settings = default);
}