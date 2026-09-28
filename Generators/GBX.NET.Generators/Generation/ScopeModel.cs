using ChunkL.Syntax;
using GBX.NET.Generators.Analysis;

namespace GBX.NET.Generators.Generation;

internal sealed class ScopeModel
{
    public ExistingType? Existing { get; }
    public AttributeList? Attributes { get; }
    public List<IBodyStatement> Body { get; }
    public List<FieldModel> Fields { get; } = [];
    public Dictionary<FieldDeclaration, FieldModel> Occurrences { get; } = [];
    public bool HasVersion => Fields.Any(static x => x.IsVersion);
    public bool Separate => SyntaxOverlap.Option(Existing, "StructureKind") == "SeparateReadAndWrite" ||
        SyntaxOverlap.Option(Existing, "StructureKind") == "1" ||
        (Existing?.Methods.Any(static x => x.Identifier.ValueText is "Read" or "Write" && x.Modifiers.Any(static t => t.ValueText == "override")) == true);
    
    private int unknownCount;

    public ScopeModel(ExistingType? existing, AttributeList? attributes, List<IBodyStatement> body)
    {
        Existing = existing;
        Attributes = attributes;
        Body = body;

        foreach (var field in Walk(body).OfType<FieldDeclaration>())
        {
            Add(field);
        }
    }

    public void Add(FieldDeclaration declaration)
    {
        if (Occurrences.ContainsKey(declaration))
        {
            return;
        }

        var isVersion = declaration.Type.Name is "version" or "versionb";

        if (declaration.IsSpecialKeyword && !isVersion)
        {
            return;
        }

        var unknown = string.IsNullOrEmpty(declaration.Name) ||
            (declaration.Name!.Length == 3 && declaration.Name[0] == 'U' && char.IsDigit(declaration.Name[1]) && char.IsDigit(declaration.Name[2]));
        
        if (unknown && !isVersion) 
        {
            unknownCount++;
        }
        
        var name = isVersion ? "Version" : string.IsNullOrEmpty(declaration.Name) ? $"U{unknownCount:00}" : declaration.Name!;
        
        var model = Fields.FirstOrDefault(x => x.Name == name);
        if (model is null)
        {
            model = new FieldModel(name, declaration, unknown && !isVersion, isVersion);
            Fields.Add(model);
        }

        model.Occurrences.Add(declaration);
        Occurrences.Add(declaration, model);
    }

    public static IEnumerable<IBodyStatement> Walk(IEnumerable<IBodyStatement> statements)
    {
        foreach (var statement in statements)
        {
            yield return statement;

            IEnumerable<IBodyStatement> children = statement switch
            {
                VersionCondition x => x.Body,
                IfStatement x => x.Body.Concat(x.ElseIfs.SelectMany(static y => y.Body)).Concat(x.Else?.Body ?? Enumerable.Empty<IBodyStatement>()),
                BlockStatement x => x.Body,
                LoopStatement x => x.Body,
                WhileStatement x => x.Body,
                SwitchStatement x => x.Cases.SelectMany(static y => y.Body).Concat(x.Default?.Body ?? Enumerable.Empty<IBodyStatement>()),
                _ => Enumerable.Empty<IBodyStatement>()
            };
            foreach (var child in Walk(children))
            {
                yield return child;
            }
        }
    }
}
