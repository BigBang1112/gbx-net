namespace GBX.NET.Generators.Generation;

internal sealed class GameMethod(LayoutModel owner, MethodDeclarationSyntax declaration, string returnType, string parameters, string arguments)
{
    public LayoutModel Owner { get; } = owner;
    public MethodDeclarationSyntax Declaration { get; } = declaration;
    public string ReturnType { get; } = returnType;
    public string Parameters { get; } = parameters;
    public string Arguments { get; } = arguments;
    public HashSet<string> Games { get; } = new(StringComparer.Ordinal);
}
