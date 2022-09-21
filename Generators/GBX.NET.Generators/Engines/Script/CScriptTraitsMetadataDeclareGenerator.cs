using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators.Engines.Script;

[Generator]
public class CScriptTraitsMetadataDeclareGenerator : SourceGenerator
{
    public override bool Debug => false;

    public override void Execute(GeneratorExecutionContext context)
    {
        var type = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Engines.Script")?
            .GetTypeMembers()
            .FirstOrDefault(x => x.Name == "CScriptTraitsMetadata") ?? throw new Exception("CScriptTraitsMetadata not found.");

        var enumScriptType = type
            .GetMembers()
            .OfType<INamedTypeSymbol>()
            .FirstOrDefault(x => x.Name == "EScriptType") ?? throw new Exception("EScriptType not found.");

        var enumScriptTypeMembers = enumScriptType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .ToList();

        var builder = new StringBuilder(@"namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
");

        foreach (var member in enumScriptTypeMembers)
        {
            var enumName = member.Name;
            var mapped = MapEnumToType(enumName);

            if (mapped is null)
            {
                continue;
            }

            builder.AppendLine($@"    public void Declare(string name, {mapped} value)
    {{
        Remove(name);
        Traits.Add(new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), name, value));
    }}

    public void Declare(string name, IEnumerable<{mapped}> value)
    {{
        Remove(name);
        Traits.Add(new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.{enumName})),
            name, value.Select(x => new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), """", x)).ToArray()));
    }}
");
        }

        builder.AppendLine("}");

        context.AddSource($"CScriptTraitsMetadata.g.cs", builder.ToString());
    }

    private static string? MapEnumToType(string fieldName) => fieldName switch
    {
        "Boolean" => "bool",
        "Integer" => "int",
        "Real" => "float",
        "Text" => "string",
        "Vec2" => "Vec2",
        "Vec3" => "Vec3",
        "Int3" => "Int3",
        "Int2" => "Int2",
        _ => null
    };
}
