using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// Declares a metadata variable as <c>Boolean</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="bool"/>.</param>
    public void Declare(string name, bool value)
    {
        Traits[name] = new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Boolean[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="bool"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<bool> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Boolean)),
            value.Select(x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x)).ToList());
    }

    public bool? GetBoolean(string name)
    {
        return (Get(name) as ScriptTrait<bool>)?.Value;
    }

    public bool TryGetBoolean(string name, out bool value)
    {
        var val = GetBoolean(name);
        value = val ?? default;
        return val is not null;
    }

    public List<bool>? GetBooleanArray(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<bool>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanArray(string name, [NotNullWhen(true)] out List<bool>? value)
#else
    public bool TryGetBooleanArray(string name, out List<bool> value)
#endif
    {
        var val = GetBooleanArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, bool> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, bool>? GetStructBooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

    public IDictionary<bool, ScriptStructTrait>? GetBooleanStructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructBooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, bool>? value)
#else
    public bool TryGetStructBooleanAssociativeArray(string name, out IDictionary<ScriptStructTrait, bool> value)
#endif
    {
        var val = GetStructBooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanStructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, ScriptStructTrait>? value)
#else
    public bool TryGetBooleanStructAssociativeArray(string name, out IDictionary<bool, ScriptStructTrait> value)
#endif
    {
        var val = GetBooleanStructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Integer</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="int"/>.</param>
    public void Declare(string name, int value)
    {
        Traits[name] = new ScriptTrait<int>(new ScriptType(EScriptType.Integer), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Integer[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="int"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<int> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Integer)),
            value.Select(x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x)).ToList());
    }

    public int? GetInteger(string name)
    {
        return (Get(name) as ScriptTrait<int>)?.Value;
    }

    public bool TryGetInteger(string name, out int value)
    {
        var val = GetInteger(name);
        value = val ?? default;
        return val is not null;
    }

    public List<int>? GetIntegerArray(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<int>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerArray(string name, [NotNullWhen(true)] out List<int>? value)
#else
    public bool TryGetIntegerArray(string name, out List<int> value)
#endif
    {
        var val = GetIntegerArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, int> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, int>? GetStructIntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

    public IDictionary<int, ScriptStructTrait>? GetIntegerStructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructIntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, int>? value)
#else
    public bool TryGetStructIntegerAssociativeArray(string name, out IDictionary<ScriptStructTrait, int> value)
#endif
    {
        var val = GetStructIntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerStructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, ScriptStructTrait>? value)
#else
    public bool TryGetIntegerStructAssociativeArray(string name, out IDictionary<int, ScriptStructTrait> value)
#endif
    {
        var val = GetIntegerStructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Real</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="float"/>.</param>
    public void Declare(string name, float value)
    {
        Traits[name] = new ScriptTrait<float>(new ScriptType(EScriptType.Real), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Real[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="float"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<float> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Real)),
            value.Select(x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x)).ToList());
    }

    public float? GetReal(string name)
    {
        return (Get(name) as ScriptTrait<float>)?.Value;
    }

    public bool TryGetReal(string name, out float value)
    {
        var val = GetReal(name);
        value = val ?? default;
        return val is not null;
    }

    public List<float>? GetRealArray(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<float>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealArray(string name, [NotNullWhen(true)] out List<float>? value)
#else
    public bool TryGetRealArray(string name, out List<float> value)
#endif
    {
        var val = GetRealArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, float> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, float>? GetStructRealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

    public IDictionary<float, ScriptStructTrait>? GetRealStructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructRealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, float>? value)
#else
    public bool TryGetStructRealAssociativeArray(string name, out IDictionary<ScriptStructTrait, float> value)
#endif
    {
        var val = GetStructRealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealStructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, ScriptStructTrait>? value)
#else
    public bool TryGetRealStructAssociativeArray(string name, out IDictionary<float, ScriptStructTrait> value)
#endif
    {
        var val = GetRealStructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Text</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="string"/>.</param>
    public void Declare(string name, string value)
    {
        Traits[name] = new ScriptTrait<string>(new ScriptType(EScriptType.Text), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Text[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="string"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<string> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Text)),
            value.Select(x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x)).ToList());
    }

    public string? GetText(string name)
    {
        return (Get(name) as ScriptTrait<string>)?.Value;
    }

    public bool TryGetText(string name, out string value)
    {
        var val = GetText(name);
        value = val ?? default;
        return val is not null;
    }

    public List<string>? GetTextArray(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<string>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextArray(string name, [NotNullWhen(true)] out List<string>? value)
#else
    public bool TryGetTextArray(string name, out List<string> value)
#endif
    {
        var val = GetTextArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, string> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, string>? GetStructTextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

    public IDictionary<string, ScriptStructTrait>? GetTextStructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructTextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, string>? value)
#else
    public bool TryGetStructTextAssociativeArray(string name, out IDictionary<ScriptStructTrait, string> value)
#endif
    {
        var val = GetStructTextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextStructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, ScriptStructTrait>? value)
#else
    public bool TryGetTextStructAssociativeArray(string name, out IDictionary<string, ScriptStructTrait> value)
#endif
    {
        var val = GetTextStructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Vec2</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="Vec2"/>.</param>
    public void Declare(string name, Vec2 value)
    {
        Traits[name] = new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Vec2[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="Vec2"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<Vec2> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Vec2)),
            value.Select(x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x)).ToList());
    }

    public Vec2? GetVec2(string name)
    {
        return (Get(name) as ScriptTrait<Vec2>)?.Value;
    }

    public bool TryGetVec2(string name, out Vec2 value)
    {
        var val = GetVec2(name);
        value = val ?? default;
        return val is not null;
    }

    public List<Vec2>? GetVec2Array(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<Vec2>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2Array(string name, [NotNullWhen(true)] out List<Vec2>? value)
#else
    public bool TryGetVec2Array(string name, out List<Vec2> value)
#endif
    {
        var val = GetVec2Array(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, Vec2> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, Vec2>? GetStructVec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

    public IDictionary<Vec2, ScriptStructTrait>? GetVec2StructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructVec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, Vec2>? value)
#else
    public bool TryGetStructVec2AssociativeArray(string name, out IDictionary<ScriptStructTrait, Vec2> value)
#endif
    {
        var val = GetStructVec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2StructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, ScriptStructTrait>? value)
#else
    public bool TryGetVec2StructAssociativeArray(string name, out IDictionary<Vec2, ScriptStructTrait> value)
#endif
    {
        var val = GetVec2StructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Vec3</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="Vec3"/>.</param>
    public void Declare(string name, Vec3 value)
    {
        Traits[name] = new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Vec3[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="Vec3"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<Vec3> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Vec3)),
            value.Select(x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x)).ToList());
    }

    public Vec3? GetVec3(string name)
    {
        return (Get(name) as ScriptTrait<Vec3>)?.Value;
    }

    public bool TryGetVec3(string name, out Vec3 value)
    {
        var val = GetVec3(name);
        value = val ?? default;
        return val is not null;
    }

    public List<Vec3>? GetVec3Array(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<Vec3>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3Array(string name, [NotNullWhen(true)] out List<Vec3>? value)
#else
    public bool TryGetVec3Array(string name, out List<Vec3> value)
#endif
    {
        var val = GetVec3Array(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, Vec3> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, Vec3>? GetStructVec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

    public IDictionary<Vec3, ScriptStructTrait>? GetVec3StructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructVec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, Vec3>? value)
#else
    public bool TryGetStructVec3AssociativeArray(string name, out IDictionary<ScriptStructTrait, Vec3> value)
#endif
    {
        var val = GetStructVec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3StructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, ScriptStructTrait>? value)
#else
    public bool TryGetVec3StructAssociativeArray(string name, out IDictionary<Vec3, ScriptStructTrait> value)
#endif
    {
        var val = GetVec3StructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Int3</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="Int3"/>.</param>
    public void Declare(string name, Int3 value)
    {
        Traits[name] = new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Int3[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="Int3"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<Int3> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Int3)),
            value.Select(x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x)).ToList());
    }

    public Int3? GetInt3(string name)
    {
        return (Get(name) as ScriptTrait<Int3>)?.Value;
    }

    public bool TryGetInt3(string name, out Int3 value)
    {
        var val = GetInt3(name);
        value = val ?? default;
        return val is not null;
    }

    public List<Int3>? GetInt3Array(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<Int3>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3Array(string name, [NotNullWhen(true)] out List<Int3>? value)
#else
    public bool TryGetInt3Array(string name, out List<Int3> value)
#endif
    {
        var val = GetInt3Array(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, Int3> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, Int3>? GetStructInt3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

    public IDictionary<Int3, ScriptStructTrait>? GetInt3StructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructInt3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, Int3>? value)
#else
    public bool TryGetStructInt3AssociativeArray(string name, out IDictionary<ScriptStructTrait, Int3> value)
#endif
    {
        var val = GetStructInt3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3StructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, ScriptStructTrait>? value)
#else
    public bool TryGetInt3StructAssociativeArray(string name, out IDictionary<Int3, ScriptStructTrait> value)
#endif
    {
        var val = GetInt3StructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata variable as <c>Int2</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of <see href="Int2"/>.</param>
    public void Declare(string name, Int2 value)
    {
        Traits[name] = new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), value);
    }
    
    /// <summary>
    /// Declares a metadata array variable as <c>Int2[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of <see href="Int2"/>. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<Int2> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Int2)),
            value.Select(x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x)).ToList());
    }

    public Int2? GetInt2(string name)
    {
        return (Get(name) as ScriptTrait<Int2>)?.Value;
    }

    public bool TryGetInt2(string name, out Int2 value)
    {
        var val = GetInt2(name);
        value = val ?? default;
        return val is not null;
    }

    public List<Int2>? GetInt2Array(string name)
    {
        return (Get(name) as ScriptArrayTrait)?.Value
            .Select(x => ((ScriptTrait<Int2>)x).Value)
            .ToList();
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2Array(string name, [NotNullWhen(true)] out List<Int2>? value)
#else
    public bool TryGetInt2Array(string name, out List<Int2> value)
#endif
    {
        var val = GetInt2Array(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of Struct. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, ScriptStructTrait> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Struct)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)x.Value
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Struct[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of Struct builder. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, ScriptStructTraitBuilder> value)
    {
        Declare(name, value.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTrait, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Struct), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)x.Key,
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Struct]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of Struct builder and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<ScriptStructTraitBuilder, Int2> value)
    {
        Declare(name, value.ToDictionary(x => x.Key.Build(), x => x.Value));
    }

    public IDictionary<ScriptStructTrait, Int2>? GetStructInt2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => (ScriptStructTrait)x.Key,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

    public IDictionary<Int2, ScriptStructTrait>? GetInt2StructAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => (ScriptStructTrait)x.Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStructInt2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<ScriptStructTrait, Int2>? value)
#else
    public bool TryGetStructInt2AssociativeArray(string name, out IDictionary<ScriptStructTrait, Int2> value)
#endif
    {
        var val = GetStructInt2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2StructAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, ScriptStructTrait>? value)
#else
    public bool TryGetInt2StructAssociativeArray(string name, out IDictionary<Int2, ScriptStructTrait> value)
#endif
    {
        var val = GetInt2StructAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<bool, bool>? GetBooleanBooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanBooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, bool>? value)
#else
    public bool TryGetBooleanBooleanAssociativeArray(string name, out IDictionary<bool, bool> value)
#endif
    {
        var val = GetBooleanBooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<bool, int>? GetBooleanIntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanIntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, int>? value)
#else
    public bool TryGetBooleanIntegerAssociativeArray(string name, out IDictionary<bool, int> value)
#endif
    {
        var val = GetBooleanIntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<bool, float>? GetBooleanRealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanRealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, float>? value)
#else
    public bool TryGetBooleanRealAssociativeArray(string name, out IDictionary<bool, float> value)
#endif
    {
        var val = GetBooleanRealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<bool, string>? GetBooleanTextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanTextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, string>? value)
#else
    public bool TryGetBooleanTextAssociativeArray(string name, out IDictionary<bool, string> value)
#endif
    {
        var val = GetBooleanTextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<bool, Vec2>? GetBooleanVec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanVec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, Vec2>? value)
#else
    public bool TryGetBooleanVec2AssociativeArray(string name, out IDictionary<bool, Vec2> value)
#endif
    {
        var val = GetBooleanVec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<bool, Vec3>? GetBooleanVec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanVec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, Vec3>? value)
#else
    public bool TryGetBooleanVec3AssociativeArray(string name, out IDictionary<bool, Vec3> value)
#endif
    {
        var val = GetBooleanVec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<bool, Int3>? GetBooleanInt3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanInt3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, Int3>? value)
#else
    public bool TryGetBooleanInt3AssociativeArray(string name, out IDictionary<bool, Int3> value)
#endif
    {
        var val = GetBooleanInt3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Boolean]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="bool"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<bool, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Boolean), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<bool, Int2>? GetBooleanInt2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<bool>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetBooleanInt2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<bool, Int2>? value)
#else
    public bool TryGetBooleanInt2AssociativeArray(string name, out IDictionary<bool, Int2> value)
#endif
    {
        var val = GetBooleanInt2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<int, bool>? GetIntegerBooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerBooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, bool>? value)
#else
    public bool TryGetIntegerBooleanAssociativeArray(string name, out IDictionary<int, bool> value)
#endif
    {
        var val = GetIntegerBooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<int, int>? GetIntegerIntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerIntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, int>? value)
#else
    public bool TryGetIntegerIntegerAssociativeArray(string name, out IDictionary<int, int> value)
#endif
    {
        var val = GetIntegerIntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<int, float>? GetIntegerRealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerRealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, float>? value)
#else
    public bool TryGetIntegerRealAssociativeArray(string name, out IDictionary<int, float> value)
#endif
    {
        var val = GetIntegerRealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<int, string>? GetIntegerTextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerTextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, string>? value)
#else
    public bool TryGetIntegerTextAssociativeArray(string name, out IDictionary<int, string> value)
#endif
    {
        var val = GetIntegerTextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<int, Vec2>? GetIntegerVec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerVec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, Vec2>? value)
#else
    public bool TryGetIntegerVec2AssociativeArray(string name, out IDictionary<int, Vec2> value)
#endif
    {
        var val = GetIntegerVec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<int, Vec3>? GetIntegerVec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerVec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, Vec3>? value)
#else
    public bool TryGetIntegerVec3AssociativeArray(string name, out IDictionary<int, Vec3> value)
#endif
    {
        var val = GetIntegerVec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<int, Int3>? GetIntegerInt3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerInt3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, Int3>? value)
#else
    public bool TryGetIntegerInt3AssociativeArray(string name, out IDictionary<int, Int3> value)
#endif
    {
        var val = GetIntegerInt3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Integer]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="int"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<int, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Integer), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<int, Int2>? GetIntegerInt2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<int>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetIntegerInt2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<int, Int2>? value)
#else
    public bool TryGetIntegerInt2AssociativeArray(string name, out IDictionary<int, Int2> value)
#endif
    {
        var val = GetIntegerInt2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<float, bool>? GetRealBooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealBooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, bool>? value)
#else
    public bool TryGetRealBooleanAssociativeArray(string name, out IDictionary<float, bool> value)
#endif
    {
        var val = GetRealBooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<float, int>? GetRealIntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealIntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, int>? value)
#else
    public bool TryGetRealIntegerAssociativeArray(string name, out IDictionary<float, int> value)
#endif
    {
        var val = GetRealIntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<float, float>? GetRealRealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealRealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, float>? value)
#else
    public bool TryGetRealRealAssociativeArray(string name, out IDictionary<float, float> value)
#endif
    {
        var val = GetRealRealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<float, string>? GetRealTextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealTextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, string>? value)
#else
    public bool TryGetRealTextAssociativeArray(string name, out IDictionary<float, string> value)
#endif
    {
        var val = GetRealTextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<float, Vec2>? GetRealVec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealVec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, Vec2>? value)
#else
    public bool TryGetRealVec2AssociativeArray(string name, out IDictionary<float, Vec2> value)
#endif
    {
        var val = GetRealVec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<float, Vec3>? GetRealVec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealVec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, Vec3>? value)
#else
    public bool TryGetRealVec3AssociativeArray(string name, out IDictionary<float, Vec3> value)
#endif
    {
        var val = GetRealVec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<float, Int3>? GetRealInt3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealInt3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, Int3>? value)
#else
    public bool TryGetRealInt3AssociativeArray(string name, out IDictionary<float, Int3> value)
#endif
    {
        var val = GetRealInt3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Real]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="float"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<float, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Real), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<float, Int2>? GetRealInt2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<float>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetRealInt2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<float, Int2>? value)
#else
    public bool TryGetRealInt2AssociativeArray(string name, out IDictionary<float, Int2> value)
#endif
    {
        var val = GetRealInt2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<string, bool>? GetTextBooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextBooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, bool>? value)
#else
    public bool TryGetTextBooleanAssociativeArray(string name, out IDictionary<string, bool> value)
#endif
    {
        var val = GetTextBooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<string, int>? GetTextIntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextIntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, int>? value)
#else
    public bool TryGetTextIntegerAssociativeArray(string name, out IDictionary<string, int> value)
#endif
    {
        var val = GetTextIntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<string, float>? GetTextRealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextRealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, float>? value)
#else
    public bool TryGetTextRealAssociativeArray(string name, out IDictionary<string, float> value)
#endif
    {
        var val = GetTextRealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<string, string>? GetTextTextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextTextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, string>? value)
#else
    public bool TryGetTextTextAssociativeArray(string name, out IDictionary<string, string> value)
#endif
    {
        var val = GetTextTextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<string, Vec2>? GetTextVec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextVec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, Vec2>? value)
#else
    public bool TryGetTextVec2AssociativeArray(string name, out IDictionary<string, Vec2> value)
#endif
    {
        var val = GetTextVec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<string, Vec3>? GetTextVec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextVec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, Vec3>? value)
#else
    public bool TryGetTextVec3AssociativeArray(string name, out IDictionary<string, Vec3> value)
#endif
    {
        var val = GetTextVec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<string, Int3>? GetTextInt3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextInt3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, Int3>? value)
#else
    public bool TryGetTextInt3AssociativeArray(string name, out IDictionary<string, Int3> value)
#endif
    {
        var val = GetTextInt3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Text]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="string"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<string, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Text), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<string, Int2>? GetTextInt2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<string>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetTextInt2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<string, Int2>? value)
#else
    public bool TryGetTextInt2AssociativeArray(string name, out IDictionary<string, Int2> value)
#endif
    {
        var val = GetTextInt2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<Vec2, bool>? GetVec2BooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2BooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, bool>? value)
#else
    public bool TryGetVec2BooleanAssociativeArray(string name, out IDictionary<Vec2, bool> value)
#endif
    {
        var val = GetVec2BooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<Vec2, int>? GetVec2IntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2IntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, int>? value)
#else
    public bool TryGetVec2IntegerAssociativeArray(string name, out IDictionary<Vec2, int> value)
#endif
    {
        var val = GetVec2IntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<Vec2, float>? GetVec2RealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2RealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, float>? value)
#else
    public bool TryGetVec2RealAssociativeArray(string name, out IDictionary<Vec2, float> value)
#endif
    {
        var val = GetVec2RealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<Vec2, string>? GetVec2TextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2TextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, string>? value)
#else
    public bool TryGetVec2TextAssociativeArray(string name, out IDictionary<Vec2, string> value)
#endif
    {
        var val = GetVec2TextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<Vec2, Vec2>? GetVec2Vec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2Vec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, Vec2>? value)
#else
    public bool TryGetVec2Vec2AssociativeArray(string name, out IDictionary<Vec2, Vec2> value)
#endif
    {
        var val = GetVec2Vec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<Vec2, Vec3>? GetVec2Vec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2Vec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, Vec3>? value)
#else
    public bool TryGetVec2Vec3AssociativeArray(string name, out IDictionary<Vec2, Vec3> value)
#endif
    {
        var val = GetVec2Vec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<Vec2, Int3>? GetVec2Int3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2Int3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, Int3>? value)
#else
    public bool TryGetVec2Int3AssociativeArray(string name, out IDictionary<Vec2, Int3> value)
#endif
    {
        var val = GetVec2Int3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Vec2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec2"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec2, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec2), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<Vec2, Int2>? GetVec2Int2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec2>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec2Int2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec2, Int2>? value)
#else
    public bool TryGetVec2Int2AssociativeArray(string name, out IDictionary<Vec2, Int2> value)
#endif
    {
        var val = GetVec2Int2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<Vec3, bool>? GetVec3BooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3BooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, bool>? value)
#else
    public bool TryGetVec3BooleanAssociativeArray(string name, out IDictionary<Vec3, bool> value)
#endif
    {
        var val = GetVec3BooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<Vec3, int>? GetVec3IntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3IntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, int>? value)
#else
    public bool TryGetVec3IntegerAssociativeArray(string name, out IDictionary<Vec3, int> value)
#endif
    {
        var val = GetVec3IntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<Vec3, float>? GetVec3RealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3RealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, float>? value)
#else
    public bool TryGetVec3RealAssociativeArray(string name, out IDictionary<Vec3, float> value)
#endif
    {
        var val = GetVec3RealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<Vec3, string>? GetVec3TextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3TextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, string>? value)
#else
    public bool TryGetVec3TextAssociativeArray(string name, out IDictionary<Vec3, string> value)
#endif
    {
        var val = GetVec3TextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<Vec3, Vec2>? GetVec3Vec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3Vec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, Vec2>? value)
#else
    public bool TryGetVec3Vec2AssociativeArray(string name, out IDictionary<Vec3, Vec2> value)
#endif
    {
        var val = GetVec3Vec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<Vec3, Vec3>? GetVec3Vec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3Vec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, Vec3>? value)
#else
    public bool TryGetVec3Vec3AssociativeArray(string name, out IDictionary<Vec3, Vec3> value)
#endif
    {
        var val = GetVec3Vec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<Vec3, Int3>? GetVec3Int3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3Int3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, Int3>? value)
#else
    public bool TryGetVec3Int3AssociativeArray(string name, out IDictionary<Vec3, Int3> value)
#endif
    {
        var val = GetVec3Int3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Vec3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Vec3"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Vec3, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Vec3), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<Vec3, Int2>? GetVec3Int2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Vec3>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetVec3Int2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Vec3, Int2>? value)
#else
    public bool TryGetVec3Int2AssociativeArray(string name, out IDictionary<Vec3, Int2> value)
#endif
    {
        var val = GetVec3Int2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<Int3, bool>? GetInt3BooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3BooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, bool>? value)
#else
    public bool TryGetInt3BooleanAssociativeArray(string name, out IDictionary<Int3, bool> value)
#endif
    {
        var val = GetInt3BooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<Int3, int>? GetInt3IntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3IntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, int>? value)
#else
    public bool TryGetInt3IntegerAssociativeArray(string name, out IDictionary<Int3, int> value)
#endif
    {
        var val = GetInt3IntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<Int3, float>? GetInt3RealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3RealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, float>? value)
#else
    public bool TryGetInt3RealAssociativeArray(string name, out IDictionary<Int3, float> value)
#endif
    {
        var val = GetInt3RealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<Int3, string>? GetInt3TextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3TextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, string>? value)
#else
    public bool TryGetInt3TextAssociativeArray(string name, out IDictionary<Int3, string> value)
#endif
    {
        var val = GetInt3TextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<Int3, Vec2>? GetInt3Vec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3Vec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, Vec2>? value)
#else
    public bool TryGetInt3Vec2AssociativeArray(string name, out IDictionary<Int3, Vec2> value)
#endif
    {
        var val = GetInt3Vec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<Int3, Vec3>? GetInt3Vec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3Vec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, Vec3>? value)
#else
    public bool TryGetInt3Vec3AssociativeArray(string name, out IDictionary<Int3, Vec3> value)
#endif
    {
        var val = GetInt3Vec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<Int3, Int3>? GetInt3Int3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3Int3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, Int3>? value)
#else
    public bool TryGetInt3Int3AssociativeArray(string name, out IDictionary<Int3, Int3> value)
#endif
    {
        var val = GetInt3Int3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Int3]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int3"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int3, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int3), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<Int3, Int2>? GetInt3Int2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int3>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt3Int2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int3, Int2>? value)
#else
    public bool TryGetInt3Int2AssociativeArray(string name, out IDictionary<Int3, Int2> value)
#endif
    {
        var val = GetInt3Int2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Boolean[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="bool"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, bool> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Boolean)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), x.Value)
            ));
    }

    public IDictionary<Int2, bool>? GetInt2BooleanAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<bool>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2BooleanAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, bool>? value)
#else
    public bool TryGetInt2BooleanAssociativeArray(string name, out IDictionary<Int2, bool> value)
#endif
    {
        var val = GetInt2BooleanAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Integer[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="int"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, int> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Integer)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<int>(new ScriptType(EScriptType.Integer), x.Value)
            ));
    }

    public IDictionary<Int2, int>? GetInt2IntegerAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<int>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2IntegerAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, int>? value)
#else
    public bool TryGetInt2IntegerAssociativeArray(string name, out IDictionary<Int2, int> value)
#endif
    {
        var val = GetInt2IntegerAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Real[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="float"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, float> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Real)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<float>(new ScriptType(EScriptType.Real), x.Value)
            ));
    }

    public IDictionary<Int2, float>? GetInt2RealAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<float>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2RealAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, float>? value)
#else
    public bool TryGetInt2RealAssociativeArray(string name, out IDictionary<Int2, float> value)
#endif
    {
        var val = GetInt2RealAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Text[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="string"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, string> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Text)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<string>(new ScriptType(EScriptType.Text), x.Value)
            ));
    }

    public IDictionary<Int2, string>? GetInt2TextAssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<string>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2TextAssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, string>? value)
#else
    public bool TryGetInt2TextAssociativeArray(string name, out IDictionary<Int2, string> value)
#endif
    {
        var val = GetInt2TextAssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec2[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="Vec2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, Vec2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Vec2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), x.Value)
            ));
    }

    public IDictionary<Int2, Vec2>? GetInt2Vec2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<Vec2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2Vec2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, Vec2>? value)
#else
    public bool TryGetInt2Vec2AssociativeArray(string name, out IDictionary<Int2, Vec2> value)
#endif
    {
        var val = GetInt2Vec2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Vec3[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="Vec3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, Vec3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Vec3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), x.Value)
            ));
    }

    public IDictionary<Int2, Vec3>? GetInt2Vec3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<Vec3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2Vec3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, Vec3>? value)
#else
    public bool TryGetInt2Vec3AssociativeArray(string name, out IDictionary<Int2, Vec3> value)
#endif
    {
        var val = GetInt2Vec3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int3[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="Int3"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, Int3> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Int3)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), x.Value)
            ));
    }

    public IDictionary<Int2, Int3>? GetInt2Int3AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<Int3>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2Int3AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, Int3>? value)
#else
    public bool TryGetInt2Int3AssociativeArray(string name, out IDictionary<Int2, Int3> value)
#endif
    {
        var val = GetInt2Int3AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

    /// <summary>
    /// Declares a metadata associative array variable as <c>Int2[Int2]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any dictionary with key of <see href="Int2"/> and value of <see href="Int2"/>. It is always reconstructed into a new dictionary.</param>
    public void Declare(string name, IDictionary<Int2, Int2> value)
    {
        Traits[name] = new ScriptDictionaryTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Int2), new ScriptType(EScriptType.Int2)),
            value.ToDictionary(
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Key),
                x => (ScriptTrait)new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), x.Value)
            ));
    }

    public IDictionary<Int2, Int2>? GetInt2Int2AssociativeArray(string name)
    {
        return (Get(name) as ScriptDictionaryTrait)?.Value.ToDictionary(
            x => ((ScriptTrait<Int2>)x.Key).Value,
            x => ((ScriptTrait<Int2>)x.Value).Value);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetInt2Int2AssociativeArray(string name, [NotNullWhen(true)] out IDictionary<Int2, Int2>? value)
#else
    public bool TryGetInt2Int2AssociativeArray(string name, out IDictionary<Int2, Int2> value)
#endif
    {
        var val = GetInt2Int2AssociativeArray(name);
        value = val ?? default!;
        return val is not null;
    }

}
