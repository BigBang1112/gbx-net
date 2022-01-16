namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class AppliedWithChunkAttribute : Attribute
{
    public Type ChunkType { get; }

    public AppliedWithChunkAttribute(Type chunkType)
    {
        ChunkType = chunkType;
    }
}
