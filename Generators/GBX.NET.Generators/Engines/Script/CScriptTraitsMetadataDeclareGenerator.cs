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

            builder.AppendLine($@"    /// <summary>
    /// Declares a metadata variable as <c>{enumName}</c>.
    /// </summary>
    /// <param name=""name"">The name of the variable.</param>
    /// <param name=""value"">A value of {mapped}.</param>
    public void Declare(string name, {mapped} value)
    {{
        Remove(name);
        Traits.Add(new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), name, value));
    }}
    
    /// <summary>
    /// Declares a metadata array variable as <c>{enumName}[Void]</c>.
    /// </summary>
    /// <param name=""name"">The name of the variable.</param>
    /// <param name=""value"">Any enumerable of {mapped}. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<{mapped}> value)
    {{
        Remove(name);
        Traits.Add(new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.{enumName})),
            name, value.Select(x => (ScriptTrait)new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), """", x)).ToList()));
    }}
");
        }

        foreach (var keyMember in enumScriptTypeMembers)
        {
            var keyEnumName = keyMember.Name;
            var mappedKey = MapEnumToType(keyEnumName);

            if (mappedKey is null)
            {
                continue;
            }
            
            foreach (var valueMember in enumScriptTypeMembers)
            {
                var valueEnumName = valueMember.Name;
                var mappedValue = MapEnumToType(valueEnumName);

                if (mappedValue is null)
                {
                    continue;
                }

                builder.AppendLine($@"    /// <summary>
    /// Declares a metadata associative array variable as <c>{valueEnumName}[{keyEnumName}]</c>.
    /// </summary>
    /// <param name=""name"">The name of the variable.</param>
    /// <param name=""value"">Any dictionary with key of {mappedKey} and value of {mappedValue}. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<{mappedKey}, {mappedValue}> value)
    {{
        Remove(name);
        Traits.Add(new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.{keyEnumName}), new ScriptType(EScriptType.{valueEnumName})),
            name, value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<{mappedKey}>(new ScriptType(EScriptType.{keyEnumName}), """", x.Key),
                x => (ScriptTrait)new ScriptTrait<{mappedValue}>(new ScriptType(EScriptType.{valueEnumName}), """", x.Value)
            )));
    }}
");
            }
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
        "Vec2" => fieldName,
        "Vec3" => fieldName,
        "Int3" => fieldName,
        "Int2" => fieldName,
        _ => null
    };
}
