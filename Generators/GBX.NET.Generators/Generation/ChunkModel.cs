using ChunkL.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class ChunkModel(uint id, string name, ChunkDeclaration declaration, ScopeModel scope)
{
    public uint Id { get; } = id;
    public string Name { get; } = name;
    public ChunkDeclaration Declaration { get; } = declaration;
    public ScopeModel Scope { get; } = scope;
    public bool IsHeader => LayoutModel.Has(Declaration.Attributes, "header");
}
