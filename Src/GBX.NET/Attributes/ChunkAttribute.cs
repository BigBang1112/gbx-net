namespace GBX.NET.Attributes;

/// <summary>
/// Attribute that assigns specific metadata to a chunk.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ChunkAttribute : Attribute
{
    /// <summary>
    /// Full ID of the chunk.
    /// </summary>
    public uint ID { get; }
    
    /// <summary>
    /// Very short lowercase description of the chunk.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// If the chunk should be read immediately after finding. You should always set this to true if the chunk is skippable and contains a lookback string (or meta). This property is ignored for unskippable chunks (those inheriting <see cref="Chunk{T}"/>).
    /// </summary>
    public bool ProcessSync { get; set; }

    /// <summary>
    /// If the chunk shouldn't be read immediately, but should be read before being written back. You should usually set this to true in cases where there's an <see cref="Id"/> in skippable chunk and <see cref="ProcessSync"/> wasn't set to true, as this fixes problems with <see cref="Id"/> indexes.
    /// </summary>
    private bool ParseBeforeWrite { get; set; }

    /// <summary>
    /// Assigns an ID to a chunk.
    /// </summary>
    /// <param name="chunkID">Full ID of the chunk.</param>
    /// <param name="description">Very short description of the chunk.</param>
    public ChunkAttribute(uint chunkID, string description = "")
    {
        ID = chunkID;
        Description = description;
    }

    /// <summary>
    /// Gets the class part of <see cref="ID"/>.
    /// </summary>
    /// <returns>Class part of ID.</returns>
    public uint GetClassPart() => ID & 0xFFFFF000;

    /// <summary>
    /// Gets the chunk part of <see cref="ID"/>.
    /// </summary>
    /// <returns>Chunk part of ID.</returns>
    public uint GetChunkPart() => ID & 0xFFF;
}
