using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator(LanguageNames.CSharp)]
public class ClassNewGenerator : ClassChunkLMixedGenerator
{
    private const bool Debug = false;

    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (Debug && !Debugger.IsAttached) Debugger.Launch();

        base.Initialize(context);
    }

    protected override void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<ImmutableArray<ClassDataModel>> transformed)
    {
        context.RegisterSourceOutput(transformed, GenerateSource);
    }

    private void GenerateSource(SourceProductionContext context, ImmutableArray<ClassDataModel> classInfos)
    {
        var inheritanceInverted = new Dictionary<string, Dictionary<uint, string>>();

        // does not work properly, misses recursion
        foreach (var classInfo in classInfos)
        {
            if (classInfo.Inherits is null || (classInfo.TypeSymbol is not null && classInfo.TypeSymbol.IsAbstract))
            {
                continue;
            }

            if (!inheritanceInverted.ContainsKey(classInfo.Inherits))
            {
                inheritanceInverted[classInfo.Inherits] = [];
            }

            inheritanceInverted[classInfo.Inherits].Add(classInfo.Id, classInfo.Name);
        }

        foreach (var classInfo in classInfos)
        {
            var sb = new StringBuilder();

            sb.Append("namespace GBX.NET.Engines.");
            sb.Append(classInfo.Engine);
            sb.AppendLine(";");

            sb.AppendLine();

            sb.Append("public partial class ");
            sb.AppendLine(classInfo.Name);
            sb.AppendLine("{");

            sb.Append("    public static ");

            if (classInfo.Name != "CMwNod")
            {
                sb.Append("new ");
            }

            sb.AppendLine("IClass? New(uint classId) => classId switch");
            sb.AppendLine("    {");

            if (classInfo.TypeSymbol is null || !classInfo.TypeSymbol.IsAbstract)
            {
                sb.Append("        0x");
                sb.Append(classInfo.Id.ToString("X8"));
                sb.Append(" => new ");
                sb.Append(classInfo.Name);
                sb.AppendLine("(),");
            }

            if (inheritanceInverted.TryGetValue(classInfo.Name, out var classes))
            {
                foreach (var classIdAndName in classes)
                {
                    sb.Append("        0x");
                    sb.Append(classIdAndName.Key.ToString("X8"));
                    sb.Append(" => new ");
                    sb.Append(classIdAndName.Value);
                    sb.AppendLine("(),");
                }
            }

            sb.AppendLine("        _ => null");

            sb.AppendLine("    };");

            sb.AppendLine("}");

            context.AddSource(classInfo.Name, sb.ToString());
        }
    }
}