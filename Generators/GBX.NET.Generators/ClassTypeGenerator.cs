using ChunkL;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Generators;

[Generator]
public class ClassTypeGenerator : ClassChunkLMixedGenerator
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
        var builder = new StringBuilder();

        builder.AppendLine("using GBX.NET.Components;");
        builder.AppendLine();
        builder.AppendLine("namespace GBX.NET.Managers;");
        builder.AppendLine();
        builder.AppendLine("public static partial class ClassManager");
        builder.AppendLine("{");
        builder.AppendLine("    internal static Dictionary<Type, uint> ClassIds { get; } = new()");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        {{ typeof(GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}), 0x{classInfo.Id:X8} }},");
        }

        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    public static partial Type? GetType(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.Id:X8} => typeof(GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial IClass? New(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            if (classInfo.TypeSymbol?.IsAbstract == true)
            {
                continue;
            }

            builder.AppendLine($"        0x{classInfo.Id:X8} => new GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}(),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.Id:X8} => new GbxHeader<GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}>(basic),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial Gbx? NewGbx(GbxHeader header, IClass node) => header.ClassId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.AppendLine($"        0x{classInfo.Id:X8} => new Gbx<GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}>((GbxHeader<GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name}>)header, (GBX.NET.Engines.{classInfo.Engine}.{classInfo.Name})node),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine("}");

        context.AddSource("ClassManager.ClassType", builder.ToString());
    }
}