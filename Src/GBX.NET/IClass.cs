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
    [Hexadecimal]
    static abstract uint Id { get; }
#endif

    bool IsWriteSupported { get; }

    /// <summary>
    /// A set of body chunks. Sorting by chunk ID is not always guaranteed.
    /// </summary>
    IChunkSet Chunks { get; }

    /// <summary>
    /// Reads and/or writes data via the reader/writer and uses this object.
    /// </summary>
    /// <param name="rw">A reader/writer.</param>
    void ReadWrite(GbxReaderWriter rw);

    IClass DeepClone();

    GameVersion GameVersion { get; }
    bool IsGameVersion(GameVersion version, bool strict = false);
    bool CanBeGameVersion(GameVersion version);

    Gbx ToGbx(GbxHeaderBasic headerBasic);
    Gbx ToGbx();

    void Save(Stream stream, GbxWriteSettings settings = default);
    void Save(string fileName, GbxWriteSettings settings = default);

    T CreateChunk<T>() where T : IChunk, new();
    T? GetChunk<T>() where T : IChunk;
    bool RemoveChunk<T>() where T : IChunk;
    bool ContainsChunk<T>() where T : IChunk;
}