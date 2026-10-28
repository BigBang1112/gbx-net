using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;

namespace GBX.NET.Generators.Generation;

internal static class ReferenceModel
{
    public static bool IsNode(FieldDeclaration field, LayoutModel layout,
        IReadOnlyDictionary<string, LayoutModel> layouts, IReadOnlyDictionary<string, ExistingType> existing)
    {
        if (LayoutModel.Has(field.Attributes, "direct") || LayoutModel.Has(field.Attributes, "meta") || field.Type.CastTarget is not null)
            return false;
        var name = WireTypes.Map(field.Type.Name);
        if (name == "CMwNod") return true;
        if (WireTypes.Primitive(name) || layout.Archives.ContainsKey(name) || name.Contains('.')) return false;
        if (layouts.TryGetValue(name, out var referenced)) return referenced.SelfArchive is null || field.Type.ChunkPreference;
        if (existing.TryGetValue(layout.File.Namespace + "." + name, out var type) || existing.TryGetValue("GBX.NET." + name, out type))
            return type.BaseTypes.Any(static x => x.ToString().Split('.').Last() == "CMwNod");
        return false;
    }

    public static string Element(FieldModel field) => WireTypes.Map(field.StorageDeclaration.Type.Name) +
        (field.StorageDeclaration.Type.ArrayDimensions > 0 && field.Occurrences.Any(static x => x.Type.IsNullable) ? "?" : "");
    public static string Handle(FieldModel field, bool chunk)
    {
        var name = (chunk ? SyntaxOverlap.Escape(field.Name) : SyntaxOverlap.Backing(field.Name)).TrimStart('@');
        var handle = name + "Ref";
        while (field.Scope.Fields.Any(other => !ReferenceEquals(other, field) &&
            (chunk ? SyntaxOverlap.Escape(other.Name) : SyntaxOverlap.Backing(other.Name)).TrimStart('@') == handle))
        {
            name += "Node";
            handle = name + "Ref";
        }
        return handle;
    }
    public static string CollectionType(FieldModel field) => "NodeRefList<" + Element(field) + ">" +
        string.Concat(Enumerable.Repeat("[]", field.StorageDeclaration.Type.ArrayDimensions - 1));
}
