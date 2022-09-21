using GBX.NET.Generators.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace GBX.NET.Generators.Engines.Script;

[Generator]
public class CScriptTraitsMetadataMethodGenerator : SourceGenerator
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

        var builder = new StringBuilder(@"using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace GBX.NET.Engines.Script;

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
    /// <param name=""value"">A value of <see href=""{mapped}""/>.</param>
    public void Declare(string name, {mapped} value)
    {{
        Remove(name);
        Traits.Add(new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), name, value));
    }}
    
    /// <summary>
    /// Declares a metadata array variable as <c>{enumName}[Void]</c>.
    /// </summary>
    /// <param name=""name"">The name of the variable.</param>
    /// <param name=""value"">Any enumerable of <see href=""{mapped}""/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<{mapped}> value)
    {{
        Remove(name);
        Traits.Add(new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.{enumName})),
            name, value.Select(x => (ScriptTrait)new ScriptTrait<{mapped}>(new ScriptType(EScriptType.{enumName}), """", x)).ToList()));
    }}

    public {mapped}? Get{enumName}(string name)
    {{
        return (Get(name) as ScriptTrait<{mapped}>)?.Value;
    }}

    public bool TryGet{enumName}(string name, out {mapped} value)
    {{
        var val = Get{enumName}(name);
        value = val ?? default;
        return val is not null;
    }}

    public IList<{mapped}>? Get{enumName}Array(string name)
    {{
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<{mapped}>)x).Value)
            .ToList();
    }}

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGet{enumName}Array(string name, [NotNullWhen(true)] out IList<{mapped}>? value)
#else
    public bool TryGet{enumName}Array(string name, out IList<{mapped}> value)
#endif
    {{
        var val = Get{enumName}Array(name);
        value = val ?? default!;
        return val is not null;
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
    /// <param name=""value"">Any dictionary with key of <see href=""{mappedKey}""/> and value of <see href=""{mappedValue}""/>. It is always reconstructed into a new dictionary.</param>
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

    public IDictionary<{mappedKey}, {mappedValue}>? Get{keyEnumName}{valueEnumName}AssociativeArray(string name)
    {{
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<{mappedKey}>)x.Key).Value,
            x => ((ScriptTrait<{mappedValue}>)x.Value).Value);
    }}

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGet{keyEnumName}{valueEnumName}AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<{mappedKey}, {mappedValue}>? value)
#else
    public bool TryGet{keyEnumName}{valueEnumName}AssociativeArray(string name, out IDictionary<{mappedKey}, {mappedValue}> value)
#endif
    {{
        var val = Get{keyEnumName}{valueEnumName}AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
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
        "Struct" => "ScriptStructTrait",
        _ => null
    };
}
