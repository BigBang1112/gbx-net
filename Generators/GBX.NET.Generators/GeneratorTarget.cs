namespace GBX.NET.Generators;

// A builder package generates only builders. It reads the node library's
// handwritten sources as additional files, because the referenced assembly
// does not carry the syntax the generation plan inspects.
internal static class GeneratorTarget
{
    private const string Property = "build_property.GbxGeneratorTarget";

    public static IncrementalValueProvider<bool> BuildersOnly(this IncrementalGeneratorInitializationContext context)
        => context.AnalyzerConfigOptionsProvider.Select(static (options, _) => options.GlobalOptions.TryGetValue(Property, out var target) &&
            string.Equals(target, "Builders", StringComparison.OrdinalIgnoreCase));

    // The node library already contains this output. A builder package would duplicate it.
    public static void RegisterLibraryOutput<T>(this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T> source, IncrementalValueProvider<bool> buildersOnly, Action<SourceProductionContext, T> action)
    {
        context.RegisterSourceOutput(source.Combine(buildersOnly), (sourceContext, input) =>
        {
            if (!input.Right) action(sourceContext, input.Left);
        });
    }

    public static void RegisterLibraryOutput<T>(this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T> source, IncrementalValueProvider<bool> buildersOnly, Action<SourceProductionContext, T> action)
    {
        context.RegisterSourceOutput(source.Combine(buildersOnly), (sourceContext, input) =>
        {
            if (!input.Right) action(sourceContext, input.Left);
        });
    }
}
