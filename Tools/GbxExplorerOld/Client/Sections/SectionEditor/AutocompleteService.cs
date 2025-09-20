using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class AutocompleteService
{
    public IEnumerable<string> GetSuggestions(string code, int position, List<MetadataReference> references)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation.Create("UserCode")
            .AddReferences(references)
            .AddSyntaxTrees(tree)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var model = compilation.GetSemanticModel(tree);
        var root = tree.GetRoot();
        var token = root.FindToken(position);

        // Grab text already typed (prefix for filtering)
        var prefix = token.IsKind(SyntaxKind.IdentifierToken) ? token.ValueText : string.Empty;
        var prevChar = token.SpanStart > 0 ? code[token.SpanStart - 1] : '\0';

        // Member completion after dot (map.Blo, Console.Wri…)
        if (prevChar == '.' || token.IsKind(SyntaxKind.DotToken))
        {
            var memberAccess = token.Parent as MemberAccessExpressionSyntax
                ?? token.Parent?.Parent as MemberAccessExpressionSyntax;

            if (memberAccess?.Expression != null)
            {
                var typeInfo = model.GetTypeInfo(memberAccess.Expression).Type;
                if (typeInfo != null)
                {
                    return typeInfo.GetMembers()
                        .Where(m =>
                            m.DeclaredAccessibility == Accessibility.Public &&
                            !m.Name.StartsWith("get_") &&
                            !m.Name.StartsWith("set_") &&
                            m.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        .Select(m => m.Name)
                        .Distinct();
                }
            }
        }

        // Scope completion after whitespace ( " ma" )
        if (token.IsKind(SyntaxKind.IdentifierToken) && char.IsWhiteSpace(prevChar))
        {
            return model.LookupSymbols(position)
                .Where(s =>
                    !s.Name.StartsWith("get_") &&
                    !s.Name.StartsWith("set_") &&
                    s.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Name)
                .Distinct();
        }

        // Fallback
        return model.LookupSymbols(position)
            .Where(s => !s.Name.StartsWith("get_") && !s.Name.StartsWith("set_"))
            .Select(s => s.Name)
            .Distinct();
    }
}
