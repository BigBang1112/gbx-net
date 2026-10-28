namespace GBX.NET.Generators.Generation;

// Emits the game version API into the node library and the builders into the
// builder package. Both outputs come from the same game node model, so a builder
// stage and its game interface always agree on member availability.
internal static class GameVersionWriter
{
    private static readonly DiagnosticDescriptor InvalidApi = new("GBXNETGEN300", "Cannot generate game version API",
        "Cannot generate game version API for {0}: {1}", "GBX.NET.Generators", DiagnosticSeverity.Error, true);
    private static readonly DiagnosticDescriptor MissingMember = new("GBXNETGEN302", "Game member context is incomplete",
        "Cannot project {0}: {1}", "GBX.NET.Generators", DiagnosticSeverity.Info, true);

    public static void Generate(SourceProductionContext context, GenerationPlan plan, GameSymbols symbols, bool buildersOnly)
    {
        // Lightweight serializer fixtures and downstream consumers need not provide
        // the library's game version runtime. Do not emit an API they cannot support.
        if (!symbols.HasRuntime) return;
        // Builders live in a separate package. The node library does not
        // provide their runtime, so it emits only the game version API.
        var builders = symbols.HasBuilderRuntime;
        var apis = !buildersOnly;
        if (!builders && !apis) return;

        GameVersionModel model;
        try
        {
            model = new GameVersionModel(plan, GameNodeModel.Create(plan, symbols), symbols.DefaultImplementations);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            context.ReportDiagnostic(Diagnostic.Create(InvalidApi, Location.None, "game metadata", exception.Message));
            return;
        }
        var factories = new List<GameNodeModel>();
        if (builders) NodeBuilderWriter.WriteAssignments(context, model);
        foreach (var node in model.Nodes.Values.OrderBy(static n => n.Layout.File.TypeKey, StringComparer.Ordinal))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            var location = Location.Create(node.Layout.File.Path, default, default);
            if (node.MetadataError is not null)
            {
                context.ReportDiagnostic(Diagnostic.Create(InvalidApi, location, node.Layout.Name, node.MetadataError));
                continue;
            }
            if (node.Games.Count == 0) continue;
            try
            {
                if (apis)
                {
                    foreach (var error in node.MemberErrors)
                        context.ReportDiagnostic(Diagnostic.Create(MissingMember, location, node.Layout.Name, error));
                    context.AddSource($"Interfaces/{node.Layout.File.Engine}/{node.InterfaceName}.g.cs", GameInterfaceWriter.Interfaces(node, model, symbols.ClassContract));
                    context.AddSource($"InterfaceImplementations/{node.Layout.File.Engine}/{node.Layout.Name}.g.cs", GameInterfaceWriter.Implementation(node, model));
                }
                if (builders && node.HasBuilder)
                {
                    context.AddSource($"Builders/{node.Layout.File.Engine}/{node.BuilderName}.g.cs", NodeBuilderWriter.Builder(node, model));
                    factories.Add(node);
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                context.ReportDiagnostic(Diagnostic.Create(InvalidApi, location, node.Layout.Name, exception.Message));
            }
        }
        if (builders) NodeBuilderWriter.WriteFactories(context, factories);
    }
}
