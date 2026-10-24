using ChunkL.Syntax;

namespace GBX.NET.Generators.Generation;

internal sealed class FieldModel(string name, FieldDeclaration declaration, bool unknown, bool version, ScopeModel scope)
{
    public ScopeModel Scope { get; } = scope;
    public string Name { get; } = name;
    public FieldDeclaration Declaration { get; } = declaration;
    public bool IsUnknown { get; } = unknown;
    public bool IsVersion { get; } = version;
    public bool IsLocal => LayoutModel.Has(Declaration.Attributes, "local");
    public List<FieldDeclaration> Occurrences { get; } = [];

    // A curve property can be stored as a legacy CFuncKeysReal node, a direct legacy
    // CPlugCurveSimpleNod node, or inline PlugCurve control points. Prefer the unified
    // public representation when these encodings use the same property name.
    public FieldDeclaration StorageDeclaration => Occurrences.Any(IsLegacyCurve)
        ? Occurrences.FirstOrDefault(static x => x.Type.Name == "PlugCurve" && x.Type.ArrayDimensions == 0) ?? Declaration
        : Declaration;

    public static bool IsLegacyCurve(FieldDeclaration declaration) => declaration.Type.ArrayDimensions == 0 &&
        (declaration.Type.Name == "CFuncKeysReal" ||
        (declaration.Type.Name == "CPlugCurveSimpleNod" && LayoutModel.Has(declaration.Attributes, "direct")));
}
