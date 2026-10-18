using GBX.NET;
using GBX.NET.PAK;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Immutable;

namespace GbxCLI;

internal sealed class CSharpSession
{
    private readonly object globals;
    private readonly ScriptOptions options;
    private ScriptState<object>? state;

    public CSharpSession(object globals)
    {
        var targetType = globals.GetType();
        this.globals = targetType.IsValueType || targetType.IsArray
            ? Activator.CreateInstance(typeof(CSharpGlobals<>).MakeGenericType(targetType), globals)!
            : globals;

        options = ScriptOptions.Default
            .AddReferences(typeof(Gbx).Assembly, typeof(Pak).Assembly, typeof(Enumerable).Assembly,
                typeof(ImmutableDictionary<,>).Assembly, this.globals.GetType().Assembly, targetType.Assembly)
            .AddImports("System", "System.Collections.Generic", "System.Linq", "System.Threading.Tasks", "GBX.NET", "GBX.NET.PAK");

        if (targetType.Namespace is string typeNamespace)
        {
            options = options.AddImports(typeNamespace);
        }
    }

    public async Task<object?> EvaluateAsync(string code)
    {
        state = state is null
            ? await CSharpScript.RunAsync(code, options, globals: globals)
            : await state.ContinueWithAsync(code, options);

        return state.ReturnValue;
    }
}
