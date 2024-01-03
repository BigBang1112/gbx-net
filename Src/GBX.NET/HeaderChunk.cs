namespace GBX.NET;

/// <summary>
/// A data chunk located in the header part.
/// </summary>
public interface IHeaderChunk : IChunk
{
    /// <summary>
    /// Indicates whether the validation of the chunk is gonna be skipped during the launch of the game.
    /// </summary>
    bool IsHeavy { get; set; }
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
    public uint Id { get; } = id;

    /// <inheritdoc />
    public bool IsHeavy { get; set; }

    /// <summary>
    /// Unknown data of the chunk.
    /// </summary>
    public byte[] Data { get; set; } = [];
}