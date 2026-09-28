using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace GBX.NET.Generators.Analysis;

/// <summary>
/// Syntax declared by an engine type, merged across all of its source partials.
/// No inherited, implicit or generated members are inferred here.
/// </summary>
internal sealed class ExistingType
{
    public string Namespace { get; }
    public string Name { get; }
    public int Arity { get; }
    public string? ContainingTypeKey { get; }

    // Namespace-qualified metadata-style identity: Outer`1+Inner, not just Inner.
    // This distinguishes engines, nesting and generic/non-generic archives.
    public string Key { get; }
    public ImmutableArray<BaseTypeDeclarationSyntax> Declarations { get; }
    public ImmutableArray<MemberDeclarationSyntax> Members { get; }
    public ImmutableDictionary<string, ImmutableArray<MemberDeclarationSyntax>> MembersByName { get; }
    public ImmutableArray<AttributeSyntax> Attributes { get; }
    public ImmutableArray<TypeSyntax> BaseTypes { get; }
    public bool IsPartial => Declarations.All(static x => x.Modifiers.Any(SyntaxKind.PartialKeyword));
    public bool IsAbstract => Declarations.Any(static x => x.Modifiers.Any(SyntaxKind.AbstractKeyword));

    // Retain full syntax for types, modifiers, accessors, initializers, parameters,
    // generic constraints, explicit interface names and body-vs-partial signatures.
    // A Read overload or an explicit IReadable.Read must not hide another signature.
    public IEnumerable<FieldDeclarationSyntax> Fields => Members.OfType<FieldDeclarationSyntax>();
    public IEnumerable<PropertyDeclarationSyntax> Properties => Members.OfType<PropertyDeclarationSyntax>();
    public IEnumerable<MethodDeclarationSyntax> Methods => Members.OfType<MethodDeclarationSyntax>();
    public IEnumerable<ConstructorDeclarationSyntax> Constructors => Members.OfType<ConstructorDeclarationSyntax>();

    // Primary constructors also reserve signatures, but have no ConstructorDeclaration.
    // Record positional parameters may imply properties; leave that decision to planning.
    public IEnumerable<ParameterListSyntax> PrimaryConstructors => Declarations
        .OfType<TypeDeclarationSyntax>()
        .Select(static x => x.ParameterList)
        .OfType<ParameterListSyntax>();

    public ExistingType(string @namespace, string name, int arity, string? containingTypeKey,
        string key, ImmutableArray<BaseTypeDeclarationSyntax> declarations)
    {
        Namespace = @namespace;
        Name = name;
        Arity = arity;
        ContainingTypeKey = containingTypeKey;
        Key = key;
        Declarations = declarations;
        Members = declarations.SelectMany(static x => x switch
        {
            TypeDeclarationSyntax type => (IEnumerable<MemberDeclarationSyntax>)type.Members,
            EnumDeclarationSyntax @enum => @enum.Members,
            _ => Enumerable.Empty<MemberDeclarationSyntax>()
        }).ToImmutableArray();
        Attributes = declarations.SelectMany(static x => x.AttributeLists)
            .SelectMany(static x => x.Attributes).ToImmutableArray();
        BaseTypes = declarations.Where(static x => x.BaseList is not null)
            .SelectMany(static x => x.BaseList!.Types).Select(static x => x.Type).ToImmutableArray();

        // Name lookup reserves fields/properties/nested types. Callable collisions need
        // signature comparison in the next stage; every overload remains in the array.
        // Explicit interface members get a qualified key rather than reserving a public
        // member with the same simple name. Escaped identifiers use their value text.
        MembersByName = Members.SelectMany(static member => GetNames(member)
                .Select(name => new KeyValuePair<string, MemberDeclarationSyntax>(name, member)))
            .GroupBy(static x => x.Key, StringComparer.Ordinal)
            .ToImmutableDictionary(static x => x.Key,
                static x => x.Select(static pair => pair.Value).ToImmutableArray(), StringComparer.Ordinal);
    }

    private static IEnumerable<string> GetNames(MemberDeclarationSyntax member)
    {
        switch (member)
        {
            case BaseFieldDeclarationSyntax field:
                foreach (var variable in field.Declaration.Variables)
                {
                    yield return variable.Identifier.ValueText;
                }
                break;
            case PropertyDeclarationSyntax property:
                yield return Qualify(property.ExplicitInterfaceSpecifier, property.Identifier.ValueText);
                break;
            case MethodDeclarationSyntax method:
                yield return Qualify(method.ExplicitInterfaceSpecifier, method.Identifier.ValueText);
                break;
            case ConstructorDeclarationSyntax constructor:
                yield return constructor.Modifiers.Any(SyntaxKind.StaticKeyword) ? ".cctor" : ".ctor";
                break;
            case BaseTypeDeclarationSyntax type:
                yield return type.Identifier.ValueText;
                break;
            case DelegateDeclarationSyntax @delegate:
                yield return @delegate.Identifier.ValueText;
                break;
            case EnumMemberDeclarationSyntax enumMember:
                yield return enumMember.Identifier.ValueText;
                break;
            case EventDeclarationSyntax @event:
                yield return Qualify(@event.ExplicitInterfaceSpecifier, @event.Identifier.ValueText);
                break;
            case IndexerDeclarationSyntax indexer:
                yield return Qualify(indexer.ExplicitInterfaceSpecifier, "this[]");
                break;
        }
    }

    private static string Qualify(ExplicitInterfaceSpecifierSyntax? specifier, string name) =>
        specifier is null ? name : $"{specifier.Name}.{name}";
}
