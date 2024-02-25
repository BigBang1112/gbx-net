using GBX.NET.Generators.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace GBX.NET.Generators.SubGenerators;

internal static class ClassManagerSubGenerator
{
    public static void GenerateSource(SourceProductionContext context, ImmutableDictionary<string, ClassDataModel> classInfos)
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
            builder.Append("        { typeof(");
            builder.Append(classInfo.Key);
            builder.Append($"), 0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.AppendLine(" },");
        }

        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    public static partial Type? GetType(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.Append("        0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.Append(" => typeof(");
            builder.Append(classInfo.Key);
            builder.AppendLine("),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial IClass? New(uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            if (classInfo.Value.TypeSymbol?.IsAbstract == true)
            {
                continue;
            }

            builder.Append("        0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.Append(" => new ");
            builder.Append(classInfo.Key);
            builder.AppendLine("(),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial GbxHeader? NewHeader(GbxHeaderBasic basic, uint classId) => classId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.Append("        0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.Append(" => new GbxHeader<");
            builder.Append(classInfo.Key);
            builder.AppendLine(">(basic),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial Gbx? NewGbx(GbxHeader header, IClass node) => header.ClassId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            builder.Append("        0x");
            builder.Append(classInfo.Value.Id.GetValueOrDefault().ToString("X8"));
            builder.Append(" => new Gbx<");
            builder.Append(classInfo.Key);
            builder.Append(">((GbxHeader<");
            builder.Append(classInfo.Key);
            builder.Append(">)header, (");
            builder.Append(classInfo.Key);
            builder.AppendLine(")node),");
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial IHeaderChunk? NewHeaderChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            foreach (var pair in classInfo.Value.HeaderChunks)
            {
                var chunkInfo = pair.Value;

                builder.Append("        0x");
                builder.Append(chunkInfo.Id.ToString("X8"));
                builder.Append(" => new ");
                builder.Append(classInfo.Key);
                builder.Append(".HeaderChunk");
                builder.Append(chunkInfo.Id.ToString("X8"));
                builder.AppendLine("(),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");
        builder.AppendLine();
        builder.AppendLine("    internal static partial IChunk? NewChunk(uint chunkId) => chunkId switch");
        builder.AppendLine("    {");

        foreach (var classInfo in classInfos)
        {
            foreach (var pair in classInfo.Value.Chunks)
            {
                var chunkInfo = pair.Value;

                builder.Append("        0x");
                builder.Append(chunkInfo.Id.ToString("X8"));
                builder.Append(" => new ");
                builder.Append(classInfo.Key);
                builder.Append(".Chunk");
                builder.Append(chunkInfo.Id.ToString("X8"));
                builder.AppendLine("(),");
            }
        }

        builder.AppendLine("        _ => null");
        builder.AppendLine("    };");

        builder.AppendLine("}");

        context.AddSource("Managers/ClassManager.ClassType", builder.ToString());
    }
}
