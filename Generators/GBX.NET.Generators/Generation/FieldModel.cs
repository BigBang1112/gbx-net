using ChunkL.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class FieldModel(string name, FieldDeclaration declaration, bool unknown, bool version)
{
    public string Name { get; } = name;
    public FieldDeclaration Declaration { get; } = declaration;
    public bool IsUnknown { get; } = unknown;
    public bool IsVersion { get; } = version;
    public bool IsLocal => LayoutModel.Has(Declaration.Attributes, "local");
    public List<FieldDeclaration> Occurrences { get; } = [];
}
