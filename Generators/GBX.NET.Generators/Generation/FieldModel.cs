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

    // A vehicle tuning property can be stored as either a legacy CFuncKeysReal node or
    // as inline PlugCurve control points. Prefer the unified public representation when
    // both encodings use the same property name.
    public FieldDeclaration StorageDeclaration => Occurrences.Any(static x => x.Type.Name == "CFuncKeysReal")
        ? Occurrences.FirstOrDefault(static x => x.Type.Name == "PlugCurve" && x.Type.ArrayDimensions == 0) ?? Declaration
        : Declaration;
}
