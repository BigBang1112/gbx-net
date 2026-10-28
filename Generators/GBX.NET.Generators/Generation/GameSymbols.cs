namespace GBX.NET.Generators.Generation;

// The compilation facts read by the game version output. The compilation changes
// with every edit, while these values usually do not. Comparing them by value
// lets the incremental pipeline skip regeneration after unrelated edits.
internal sealed class GameSymbols : IEquatable<GameSymbols>
{
    private GameSymbols(bool hasRuntime, bool hasBuilderRuntime, bool defaultImplementations,
        ImmutableArray<string> classContract, ImmutableArray<KeyValuePair<string, long>> games,
        ImmutableDictionary<string, string> propertyTypes, ImmutableDictionary<string, long> methodMasks)
    {
        HasRuntime = hasRuntime;
        HasBuilderRuntime = hasBuilderRuntime;
        DefaultImplementations = defaultImplementations;
        ClassContract = classContract;
        Games = games;
        PropertyTypes = propertyTypes;
        MethodMasks = methodMasks;
    }

    public bool HasRuntime { get; }
    public bool HasBuilderRuntime { get; }
    public bool DefaultImplementations { get; }

    // IClassVersion<T> members redeclared on a derived common view.
    public ImmutableArray<string> ClassContract { get; }

    // GameVersion constants in declaration order.
    public ImmutableArray<KeyValuePair<string, long>> Games { get; }

    // Keyed by PropertyKey and MethodKey.
    public ImmutableDictionary<string, string> PropertyTypes { get; }
    public ImmutableDictionary<string, long> MethodMasks { get; }

    public static string PropertyKey(LayoutModel owner, PropertyDeclarationSyntax property)
        => owner.Name + "." + property.Identifier.ValueText;

    public static string MethodKey(LayoutModel owner, MethodDeclarationSyntax method)
        => owner.Name + "." + method.Identifier.ValueText + method.ParameterList;

    public static GameSymbols Create(GenerationPlan plan, Compilation compilation, ImmutableArray<SyntaxTree> sources)
    {
        // The added library declarations shadow their referenced metadata,
        // which lets the semantic model inspect the same handwritten syntax.
        if (!sources.IsEmpty) compilation = compilation.AddSyntaxTrees(sources);
        var classVersion = compilation.GetTypeByMetadataName("GBX.NET.IClassVersion`1");
        var gameVersion = compilation.GetTypeByMetadataName("GBX.NET.GameVersion");
        var hasBuilderRuntime = compilation.GetTypeByMetadataName("GBX.NET.Builders.NodeBuilder`2") is not null &&
            compilation.GetTypeByMetadataName("GBX.NET.Builders.NodeBuilderState`1") is not null;
        var defaultImplementations = compilation.SupportsRuntimeCapability(RuntimeCapability.DefaultImplementationsOfInterfaces);
        if (classVersion is null || gameVersion is null)
            return new(false, hasBuilderRuntime, defaultImplementations, [], [],
                ImmutableDictionary<string, string>.Empty, ImmutableDictionary<string, long>.Empty);

        var games = gameVersion.GetMembers().OfType<IFieldSymbol>().Where(static f => f.HasConstantValue)
            .Select(static f => new KeyValuePair<string, long>(f.Name, Convert.ToInt64(f.ConstantValue))).ToImmutableArray();
        var propertyTypes = ImmutableDictionary.CreateBuilder<string, string>(StringComparer.Ordinal);
        var methodMasks = ImmutableDictionary.CreateBuilder<string, long>(StringComparer.Ordinal);
        foreach (var owner in plan.Layouts.Values)
        {
            // An edit inside a member body keeps the plan cached, along with syntax from an
            // earlier compilation. Read the declarations of this compilation through the symbol.
            // The source declaration also wins over the referenced metadata of a builder package.
            if (owner.Existing is null || compilation.GetTypeByMetadataName(owner.File.TypeKey) is not { } type) continue;

            foreach (var reference in type.DeclaringSyntaxReferences)
            {
                if (reference.GetSyntax() is not TypeDeclarationSyntax declaration) continue;
                var model = compilation.GetSemanticModel(declaration.SyntaxTree);

                foreach (var property in declaration.Members.OfType<PropertyDeclarationSyntax>())
                {
                    if (property.ExplicitInterfaceSpecifier is not null || !property.Modifiers.Any(SyntaxKind.PublicKeyword) ||
                        property.Modifiers.Any(SyntaxKind.StaticKeyword)) continue;
                    if (model.GetDeclaredSymbol(property) is not IPropertySymbol symbol || symbol.Type.TypeKind == TypeKind.Error) continue;
                    propertyTypes[PropertyKey(owner, property)] = symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                        SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier));
                }

                foreach (var method in declaration.Members.OfType<MethodDeclarationSyntax>())
                {
                    var attribute = method.AttributeLists.SelectMany(static l => l.Attributes)
                        .FirstOrDefault(static a => a.Name.ToString().Contains("MethodGameVersion"));
                    if (attribute?.ArgumentList?.Arguments.Count != 1) continue;
                    var constant = model.GetConstantValue(attribute.ArgumentList.Arguments[0].Expression);
                    if (constant.HasValue) methodMasks[MethodKey(owner, method)] = Convert.ToInt64(constant.Value);
                }
            }
        }

        return new(true, hasBuilderRuntime, defaultImplementations, ClassContractLines(classVersion), games,
            propertyTypes.ToImmutable(), methodMasks.ToImmutable());
    }

    // A derived node retains IClassVersion<Derived> for typed parsing, while
    // its base view carries IClassVersion<Base>. Redeclare their shared
    // operations once on the derived common view to avoid ambiguous calls.
    private static ImmutableArray<string> ClassContractLines(INamedTypeSymbol contract)
    {
        var lines = ImmutableArray.CreateBuilder<string>();
        foreach (var property in contract.GetMembers().OfType<IPropertySymbol>())
            lines.Add($"new {property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {SyntaxOverlap.Escape(property.Name)} {{ get; }}");
        foreach (var method in contract.GetMembers().OfType<IMethodSymbol>().Where(static m => m.MethodKind == MethodKind.Ordinary))
        {
            var parameters = string.Join(", ", method.Parameters.Select(p => p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) +
                " " + SyntaxOverlap.Escape(p.Name) + (p.HasExplicitDefaultValue ? " = " + DefaultParameter(p.ExplicitDefaultValue) : "")));
            lines.Add($"new {method.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {SyntaxOverlap.Escape(method.Name)}({parameters});");
        }
        return lines.ToImmutable();
    }

    private static string DefaultParameter(object? value) => value switch
    {
        null => "default",
        bool boolean => boolean ? "true" : "false",
        string text => SymbolDisplay.FormatLiteral(text, true),
        _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? "default"
    };

    public bool Equals(GameSymbols? other) => other is not null && HasRuntime == other.HasRuntime &&
        HasBuilderRuntime == other.HasBuilderRuntime && DefaultImplementations == other.DefaultImplementations &&
        ClassContract.SequenceEqual(other.ClassContract) && Games.SequenceEqual(other.Games) &&
        Same(PropertyTypes, other.PropertyTypes) && Same(MethodMasks, other.MethodMasks);

    public override bool Equals(object? obj) => Equals(obj as GameSymbols);

    public override int GetHashCode() => (Games.Length, PropertyTypes.Count, MethodMasks.Count).GetHashCode();

    private static bool Same<TValue>(ImmutableDictionary<string, TValue> left, ImmutableDictionary<string, TValue> right)
        => left.Count == right.Count && left.All(pair => right.TryGetValue(pair.Key, out var value) &&
            EqualityComparer<TValue>.Default.Equals(pair.Value, value));
}
