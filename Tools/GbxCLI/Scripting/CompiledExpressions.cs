using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace GbxCLI;

/// <summary>
/// Runs the same C# expressions against many files, for <c>eval</c> with a folder or pattern. They are compiled once for each
/// type of globals, such as <c>Gbx&lt;CGameCtnChallenge&gt;</c>, instead of once for each file.
/// </summary>
internal sealed class CompiledExpressions(IReadOnlyList<string> expressions)
{
    private readonly Dictionary<Type, Result> compiled = [];

    /// <summary>
    /// Runs the expressions in order in one session, like <see cref="CSharpSession"/>.
    /// </summary>
    /// <returns>The value of each expression, or null for a statement without one.</returns>
    /// <exception cref="CompilationErrorException">The expressions don't compile for the type of <paramref name="globals"/>.</exception>
    public async Task<object?[]> EvaluateAsync(object globals, CancellationToken cancellationToken = default)
    {
        var type = globals.GetType();

        if (!compiled.TryGetValue(type, out var result))
        {
            result = Compile(type);
            compiled[type] = result;
        }

        // A type that doesn't compile fails every file of that type, so it is compiled only once.
        var scripts = result.Scripts ?? throw result.Error!;

        var values = new object?[scripts.Length];
        ScriptState<object>? state = null;

        for (var i = 0; i < scripts.Length; i++)
        {
            state = state is null
                ? await scripts[i].RunAsync(globals, cancellationToken: cancellationToken)
                : await scripts[i].RunFromAsync(state, cancellationToken: cancellationToken);

            values[i] = state.ReturnValue;
        }

        return values;
    }

    private Result Compile(Type globalsType)
    {
        var options = CSharpSession.CreateOptions(globalsType);
        var scripts = new Script<object>[expressions.Count];

        for (var i = 0; i < scripts.Length; i++)
        {
            scripts[i] = i == 0
                ? CSharpScript.Create(expressions[i], options, globalsType)
                : scripts[i - 1].ContinueWith(expressions[i], options);

            var errors = scripts[i].Compile().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();

            if (errors.Length > 0)
            {
                return new Result(null, new CompilationErrorException(string.Join(" ", errors.Select(error => error.ToString())), [.. errors]));
            }
        }

        return new Result(scripts, null);
    }

    private sealed record Result(Script<object>[]? Scripts, CompilationErrorException? Error);
}
