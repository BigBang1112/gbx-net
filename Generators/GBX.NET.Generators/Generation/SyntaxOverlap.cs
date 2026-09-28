using GBX.NET.Generators.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GBX.NET.Generators.Generation;

internal static class SyntaxOverlap
{
    public static string Escape(string name)
    {
        return SyntaxFacts.GetKeywordKind(name) != SyntaxKind.None ||
            SyntaxFacts.GetContextualKeywordKind(name) != SyntaxKind.None ? "@" + name : name;
    }

    public static string Backing(string name)
        => Escape(char.ToLowerInvariant(name[0]) + name.Substring(1));

    public static bool Has(ExistingType? type, string name)
        => type?.MembersByName.ContainsKey(name) == true;

    public static bool HasAttribute(ExistingType? type, string name)
    {
        return type?.Attributes.Any(x => Simple(x.Name.ToString()).Replace("Attribute", "") == name) == true;
    }

    public static string? Option(ExistingType? type, string name)
    {
        return type?.Attributes
            .Where(static x => x.Name.ToString().Contains("GenerationOptions"))
            .SelectMany(static x => x.ArgumentList?.Arguments ?? default)
            .FirstOrDefault(x => x.NameEquals?.Name.Identifier.ValueText == name)?.Expression.ToString().Split('.').Last();
    }

    public static string Simple(string name)
        => name.Replace("global::", "").Split('.').Last().TrimEnd('?');

    public static string Normalize(string name)
    {
        return Simple(name) switch
        {
            "Int32" => "int",
            "UInt32" => "uint",
            "Int16" => "short",
            "UInt16" => "ushort",
            "Byte" => "byte",
            "SByte" => "sbyte",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Single" => "float",
            "Double" => "double",
            "Boolean" => "bool",
            "String" => "string",
            _ => Simple(name)
        };
    }

    public static bool Method(ExistingType? type, string name, params string[] parameters)
    {
        return type?.Methods.Any(x =>
            x.Identifier.ValueText == name && x.ExplicitInterfaceSpecifier is null &&
            x.TypeParameterList is null && !x.Modifiers.Any(SyntaxKind.StaticKeyword) &&
            x.ParameterList.Parameters.Count == parameters.Length &&
            !x.ParameterList.Parameters.Any(static p => p.Modifiers.Any(SyntaxKind.RefKeyword) || p.Modifiers.Any(SyntaxKind.OutKeyword) || p.Modifiers.Any(SyntaxKind.InKeyword)) &&
            x.ParameterList.Parameters.Select(static p => Normalize(ResolveAlias(p.Type)))
                .SequenceEqual(parameters.Select(Normalize), StringComparer.Ordinal)) == true;
    }

    private static string ResolveAlias(TypeSyntax? syntax)
    {
        if (syntax is null)
        {
            return "";
        }

        var name = syntax.ToString();
        if (syntax is IdentifierNameSyntax identifier)
        {
            var imports = syntax.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().SelectMany(static x => x.Usings)
                .Concat(syntax.SyntaxTree.GetCompilationUnitRoot().Usings);
            var alias = imports.FirstOrDefault(x => x.Alias?.Name.Identifier.ValueText == identifier.Identifier.ValueText);
            if (alias?.Name is not null)
            {
                name = alias.Name.ToString();
            }
        }

        return name;
    }

    public static string? MemberType(ExistingType? type, string name)
    {
        var property = type?.Properties.FirstOrDefault(x => x.ExplicitInterfaceSpecifier is null && x.Identifier.ValueText == name);
        if (property is not null)
        {
            return property.Type.ToString();
        }

        return type?.Fields.FirstOrDefault(x => x.Declaration.Variables.Any(v => v.Identifier.ValueText == name))?.Declaration.Type.ToString();
    }
}
