namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
internal sealed class ArchiveGenerationOptionsAttribute : Attribute
{
    public StructureKind StructureKind { get; set; }
}
