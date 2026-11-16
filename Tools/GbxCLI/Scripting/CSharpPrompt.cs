using Microsoft.CodeAnalysis.Scripting;

namespace GbxCLI;

/// <summary>
/// An interactive C# prompt for <c>eval -i</c>. Each line is one submission in the same session.
/// </summary>
internal static class CSharpPrompt
{
    public static async Task RunAsync(CSharpSession session, FileObject file, CliContext context)
    {
        var console = context.Console;

        await console.WriteLineAsync($"{CliConsole.Label}C# prompt for {CliConsole.Escape(file.InputPath)}. Members of the opened {ValueFormatter.GetTypeName(file.Value.GetType())} can be used directly. Enter q to quit.{CliConsole.Reset}");

        while (true)
        {
            await console.WriteAsync($"{CliConsole.Label}>{CliConsole.Reset} ");

            var line = await context.Input.ReadLineAsync(context.CancellationToken);

            if (line is null || line.Trim() == "q")
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var result = await session.EvaluateAsync(line, context.CancellationToken);

                // A statement such as a declaration has no value to show.
                if (result is not null || !line.TrimEnd().EndsWith(';'))
                {
                    await new ConsoleValueWriter(console).WriteResultAsync(result);
                }
            }
            catch (CompilationErrorException exception)
            {
                foreach (var diagnostic in exception.Diagnostics)
                {
                    await console.WriteLineAsync($"{CliConsole.Error}{CliConsole.Escape(diagnostic.ToString())}{CliConsole.Reset}");
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                await console.WriteLineAsync($"{CliConsole.Error}{exception.GetType().Name}:{CliConsole.Reset} {CliConsole.Escape(exception.Message)}");
            }
        }
    }
}
