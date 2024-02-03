namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
internal sealed class ChunkGenerationOptionsAttribute : Attribute
{
    public ChunkStructureKind StructureKind { get; set; }
}
