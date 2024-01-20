using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators.ClassDataGenerator;

[Generator]
public class ClassDataGenerator : ChunkLBasedGenerator
{
    private const bool Debug = false;

    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached) Debugger.Launch();

        base.Initialize(context);
    }

    protected override void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<ClassDataModel> transformed)
    {
        context.RegisterSourceOutput(transformed, GenerateSource);
    }

    private void GenerateSource(SourceProductionContext context, ClassDataModel classDataModel)
    {
        var sb = new StringBuilder();
        context.AddSource(classDataModel.ChunkL.DataModel.Header.Name, sb.ToString());
    }
}
