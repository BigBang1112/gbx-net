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

        await console.WriteLineAsync($"{CliConsole.Label}C# prompt for {CliConsole.Escape(file.InputPath)}. Members of the opened {ValueFormatter.GetTypeName(file.Value.GetType())} can be used directly. Enter save [path] to write a Gbx, or q to quit.{CliConsole.Reset}");

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

            if (line.Trim() is "save" or ['s', 'a', 'v', 'e', ' ', ..])
            {
                await SaveAsync(file, line.Trim()[4..], context);
                continue;
            }

            try
            {
                var result = await session.EvaluateAsync(line, context.CancellationToken);

                // A statement such as a declaration has no value to show.
                if (result is not null || !line.TrimEnd().EndsWith(';'))
                {
                    await new ConsoleValueWriter(console) { HasExternalReferences = ValueInspector.HasExternalReferences(file.Value) }.WriteResultAsync(result);
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

    /// <summary>
    /// Writes the opened Gbx for <c>save [--force] [--no-backup] [path]</c>. Errors are shown, and the prompt continues.
    /// </summary>
    private static async Task SaveAsync(FileObject file, string arguments, CliContext context)
    {
        var rest = arguments.Trim();
        var backup = true;
        var force = false;

        while (rest.Split(' ', 2) is [var option and ("--force" or "--no-backup"), .. var remaining])
        {
            force |= option == "--force";
            backup &= option != "--no-backup";
            rest = remaining is [var path] ? path.TrimStart() : "";
        }

        var outputPath = Cli.CleanPath(rest) is { Length: > 0 } cleaned ? cleaned : null;

        try
        {
            var gbx = file.GetWritableGbx(outputPath);
            await CheckedGbxWriter.WriteAsync(gbx, outputPath ?? file.InputPath, context.Console, backup, force, context.CancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await context.Console.WriteLineAsync($"{CliConsole.Error}error:{CliConsole.Reset} {CliConsole.Escape(exception.Message)}");
        }
    }
}
