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
        if (type is null) return false;
        foreach (var attribute in type.Attributes)
        {
            var attributeName = AttributeName(attribute.Name);
            if (attributeName == name || attributeName == name + "Attribute") return true;
        }

        return false;
    }

    public static string? Option(ExistingType? type, string name)
    {
        if (type is null) return null;
        foreach (var attribute in type.Attributes)
        {
            if (!AttributeName(attribute.Name).Contains("GenerationOptions") || attribute.ArgumentList is null) continue;
            foreach (var argument in attribute.ArgumentList.Arguments)
            {
                if (argument.NameEquals?.Name.Identifier.ValueText != name) continue;
                return argument.Expression is MemberAccessExpressionSyntax member
                    ? member.Name.Identifier.ValueText : argument.Expression.ToString();
            }
        }

        return null;
    }

    private static string AttributeName(NameSyntax name) => name switch
    {
        QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
        AliasQualifiedNameSyntax alias => alias.Name.Identifier.ValueText,
        SimpleNameSyntax simple => simple.Identifier.ValueText,
        _ => name.ToString()
    };

    public static string Simple(string name)
    {
        var separator = name.LastIndexOf('.');
        if (separator >= 0) name = name.Substring(separator + 1);
        if (name.StartsWith("global::", StringComparison.Ordinal)) name = name.Substring(8);
        return name.TrimEnd('?');
    }

    public static string Normalize(string name)
    {
        var simple = Simple(name);
        return simple switch
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
            _ => simple
        };
    }

    public static bool Method(ExistingType? type, string name, params string[] parameters)
    {
        if (type is null || !type.MembersByName.TryGetValue(name, out var members)) return false;
        foreach (var member in members)
        {
            if (member is not MethodDeclarationSyntax method || method.ExplicitInterfaceSpecifier is not null ||
                method.TypeParameterList is not null || method.Modifiers.Any(SyntaxKind.StaticKeyword) ||
                method.ParameterList.Parameters.Count != parameters.Length) continue;

            if (ParametersMatch(method.ParameterList.Parameters, parameters)) return true;
        }

        return false;
    }

    private static bool ParametersMatch(SeparatedSyntaxList<ParameterSyntax> actual, string[] expected)
    {
        for (var i = 0; i < actual.Count; i++)
        {
            var parameter = actual[i];
            if (parameter.Modifiers.Any(SyntaxKind.RefKeyword) || parameter.Modifiers.Any(SyntaxKind.OutKeyword) ||
                parameter.Modifiers.Any(SyntaxKind.InKeyword) ||
                Normalize(ResolveAlias(parameter.Type)) != Normalize(expected[i])) return false;
        }

        return true;
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
        if (type is null || !type.MembersByName.TryGetValue(name, out var members)) return null;
        foreach (var member in members)
        {
            if (member is PropertyDeclarationSyntax { ExplicitInterfaceSpecifier: null } property)
                return property.Type.ToString();
        }

        foreach (var member in members)
        {
            if (member is FieldDeclarationSyntax field) return field.Declaration.Type.ToString();
        }

        return null;
    }

    public static PropertyDeclarationSyntax? PartialPropertyImplementation(ExistingType? type, string name)
    {
        if (type is null || !type.MembersByName.TryGetValue(name, out var members)) return null;
        PropertyDeclarationSyntax? property = null;
        foreach (var member in members)
        {
            if (member is not PropertyDeclarationSyntax { ExplicitInterfaceSpecifier: null } candidate) continue;
            if (property is not null) return null;
            property = candidate;
        }

        return property?.Modifiers.Any(SyntaxKind.PartialKeyword) == true &&
            property.AccessorList?.Accessors.All(x => x.Body is not null || x.ExpressionBody is not null) == true
            ? property : null;
    }
}
