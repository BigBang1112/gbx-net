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

    // Games where only an opt-in chunk applies the member. A builder never creates
    // that chunk, so a value set through the builder would not be written.
    public HashSet<string> OptInGames { get; } = new(StringComparer.Ordinal);
    public bool Buildable => Writable && OptInGames.Count == 0;
}
