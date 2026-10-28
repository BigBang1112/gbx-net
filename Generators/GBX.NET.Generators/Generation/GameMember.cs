namespace GBX.NET.Generators.Generation;

internal sealed class GameMember(LayoutModel owner, string name, string type, bool writable, FieldModel? field,
    bool reference, PropertyDeclarationSyntax? existing = null)
{
    public LayoutModel Owner { get; } = owner;
    public string Name { get; } = name;
    public string Type { get; } = type;
    public bool Writable { get; } = writable;
    public FieldModel? Field { get; } = field;
    public bool IsReference { get; } = reference;
    public PropertyDeclarationSyntax? Existing { get; } = existing;
    public HashSet<string> Games { get; } = new(StringComparer.Ordinal);
}
