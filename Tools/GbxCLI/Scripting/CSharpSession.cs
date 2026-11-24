using GBX.NET;
using GBX.NET.PAK;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Immutable;

namespace GbxCLI;

/// <summary>
/// Evaluates C# submissions in order, so later ones can use variables from earlier ones.
/// </summary>
internal sealed class CSharpSession
{
    private readonly object globals;
    private readonly ScriptOptions options;
    private ScriptState<object>? state;

    /// <param name="globals">The object whose members the code can use directly, such as the opened <see cref="Gbx"/>.</param>
    /// <param name="filePath">Path of a script file, which lets it load other scripts and references relative to it.</param>
    public CSharpSession(object globals, string? filePath = null)
    {
        this.globals = globals;
        options = CreateOptions(globals.GetType(), filePath);
    }

    /// <summary>
    /// Options with GBX.NET and the type of the globals referenced and imported.
    /// </summary>
    /// <param name="filePath">Path of a script file, which lets it load other scripts and references relative to it.</param>
    public static ScriptOptions CreateOptions(Type globalsType, string? filePath = null)
    {
        var options = ScriptOptions.Default
            .AddReferences(typeof(Gbx).Assembly, typeof(Pak).Assembly, typeof(Enumerable).Assembly,
                typeof(ImmutableDictionary<,>).Assembly, typeof(TmEssentials.TimeInt32).Assembly, globalsType.Assembly)
            .AddImports("System", "System.Collections.Generic", "System.Linq", "System.Threading.Tasks", "GBX.NET", "GBX.NET.PAK");

        if (globalsType.Namespace is string typeNamespace)
        {
            options = options.AddImports(typeNamespace);
        }

        if (filePath is not null)
        {
            var fullPath = Path.GetFullPath(filePath);
            var directory = Path.GetDirectoryName(fullPath)!;

            options = options.WithFilePath(fullPath)
                .WithSourceResolver(new SourceFileResolver([], directory))
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithBaseDirectory(directory))
                .AddReferences(typeof(GBX.NET.LZO.Lzo).Assembly, typeof(GBX.NET.ZLib.ZLib).Assembly, typeof(GBX.NET.Crypto.Cry).Assembly)
                .AddImports("System.IO", "GBX.NET.Engines.Game", "GBX.NET.Engines.MwFoundations");
        }

        return options;
    }

    /// <returns>The value of the last expression, or null.</returns>
    public async Task<object?> EvaluateAsync(string code, CancellationToken cancellationToken = default)
    {
        state = state is null
            ? await CSharpScript.RunAsync(code, options, globals, globals.GetType(), cancellationToken)
            : await state.ContinueWithAsync(code, options, cancellationToken);

        return state.ReturnValue;
    }
}
