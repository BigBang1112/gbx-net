namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class AppliedWithChunkAttribute : Attribute
{
    public Type ChunkType { get; }

    public AppliedWithChunkAttribute(Type chunkType)
    {
        ChunkType = chunkType;
    }
}
