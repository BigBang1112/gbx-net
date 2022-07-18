namespace GBX.NET.Attributes;

/// <summary>
/// Tells which chunk is used to write a certain node memeber.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class AppliedWithChunkAttribute : Attribute
{
    /// <summary>
    /// Type of the chunk.
    /// </summary>
    public Type ChunkType { get; }

    /// <summary>
    /// Since which version this member is written to the chunk. Ignored on chunks without <see cref="IVersionable"/>.
    /// </summary>
    public int SinceVersion { get; set; }

    /// <summary>
    /// Up to which version the member is written to the chunk. -1 (default value) means there's no known version limit. Ignored on chunks without <see cref="IVersionable"/>.
    /// </summary>
    public int UpToVersion { get; set; } = -1;

    /// <summary>
    /// Creates the <see cref="AppliedWithChunkAttribute"/>.
    /// </summary>
    /// <param name="chunkType">Type of the chunk.</param>
    public AppliedWithChunkAttribute(Type chunkType)
    {
        ChunkType = chunkType;
    }
}
