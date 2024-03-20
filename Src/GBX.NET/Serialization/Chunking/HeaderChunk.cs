using GBX.NET.Managers;

namespace GBX.NET.Serialization.Chunking;

/// <summary>
/// A data chunk located in the header part.
/// </summary>
public interface IHeaderChunk : IChunk
{
    /// <summary>
    /// Indicates whether the validation of the chunk is gonna be skipped during the launch of the game.
    /// </summary>
    bool IsHeavy { get; set; }

    /// <summary>
    /// Shared node instance in case of an unknown Gbx class. This will be usually null, except when the Gbx class in unknown.
    /// </summary>
    IClass? Node { get; set; }
}

/// <summary>
/// A data chunk located in the header part.
/// </summary>
/// <param name="id">ID of the header chunk.</param>
public sealed class HeaderChunk(uint id) : IHeaderChunk
{
    /// <summary>
    /// ID of the header chunk.
    /// </summary>
    public uint Id => id;

    /// <inheritdoc />
    public bool IsHeavy { get; set; }

    /// <summary>
    /// Unknown data of the chunk.
    /// </summary>
    public byte[] Data { get; set; } = [];

    public bool Ignore => false;

    public GameVersion GameVersion => GameVersion.Unspecified;

    IClass? IHeaderChunk.Node { get; set; }

    public HeaderChunk DeepClone() => new(Id)
    {
        IsHeavy = IsHeavy,
        Data = Data.ToArray()
    };

    IChunk IChunk.DeepClone() => DeepClone();

    public override string ToString() => $"{ClassManager.GetName(Id & 0xFFFFF000)} unknown header chunk 0x{Id:X8}";
}