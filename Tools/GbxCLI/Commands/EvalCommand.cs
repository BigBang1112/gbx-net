namespace GbxCLI.Commands;

internal static class EvalCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        string? outputPath = null;
        var expressions = new List<string>();
        var interactive = false;
        var write = false;
        var dryRun = false;
        var backup = true;
        var force = false;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help" when inputPath is null:
                    await CliHelp.WriteCommandAsync(context.Console, "eval");
                    return;
                case "-i" or "--interactive":
                    interactive = true;
                    break;
                case "-w" or "--write":
                    write = true;
                    break;
                case "-o" or "--output":
                    outputPath = outputPath is null ? args.NextValue("--output") : throw new ArgumentException("Output can only be specified once.");
                    break;
                case "-n" or "--dry-run":
                    dryRun = true;
                    break;
                case "--no-backup":
                    backup = false;
                    break;
                case "--force":
                    force = true;
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg when string.IsNullOrWhiteSpace(arg):
                    throw new ArgumentException("C# expressions for eval cannot be empty.");
                case var arg:
                    expressions.Add(arg);
                    break;
            }
        }

        write |= outputPath is not null || dryRun;

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("eval");
        }

        if (!interactive && expressions.Count == 0)
        {
            throw new ArgumentException("Missing C# expression for eval.");
        }

        if (write && interactive)
        {
            throw new ArgumentException("Use save in the C# prompt instead of --write, --output or --dry-run with -i.");
        }

        if (!write && (!backup || force))
        {
            throw new ArgumentException("--no-backup and --force only apply with --write, --output or --dry-run.");
        }

        if (GbxBatch.IsBatch(inputPath))
        {
            if (interactive)
            {
                throw new ArgumentException("The C# prompt (-i) opens one file, not a folder or pattern.");
            }

            await RunBatchAsync(inputPath, expressions, write ? new GbxBatchWrite(outputPath, dryRun, backup, force, CompareWithOriginal: true) : null, context);
            return;
        }

        await using var file = await FileObject.OpenAsync(inputPath, context.CancellationToken);

        // Fails before the expressions run, so they don't seem to have been saved. A dry run writes nothing, so it needs no output for an archive entry.
        var gbx = !write ? null : dryRun ? file.GetGbx() : file.GetWritableGbx(outputPath);

        var session = new CSharpSession(file.Value);

        foreach (var expression in expressions)
        {
            var result = await session.EvaluateAsync(expression, context.CancellationToken);
            await new ConsoleValueWriter(context.Console) { HasExternalReferences = ValueInspector.HasExternalReferences(file.Value) }.WriteResultAsync(result);
        }

        if (gbx is not null && dryRun)
        {
            // The same check as a real write, so a dry run also shows whether the file could be written.
            await CheckedGbxWriter.SaveAsync(gbx, force, context.CancellationToken);
            await context.Console.WriteLineAsync($"{CliConsole.Label}Dry run, so nothing was written.{CliConsole.Reset}");
        }
        else if (gbx is not null)
        {
            await CheckedGbxWriter.WriteAsync(gbx, outputPath ?? file.InputPath, context.Console, backup, force, context.CancellationToken);
        }

        if (interactive)
        {
            await CSharpPrompt.RunAsync(session, file, context);
        }
    }

    /// <summary>
    /// Runs the expressions against every Gbx of a folder or pattern. Without <paramref name="write"/>, each file shows the values
    /// of its expressions on one line. With it, each file shows whether it changed.
    /// </summary>
    private static Task RunBatchAsync(string inputPath, List<string> expressions, GbxBatchWrite? write, CliContext context)
    {
        var compiled = new CompiledExpressions(expressions);

        return new GbxBatchRunner(context, write).RunAsync(inputPath, async (gbx, cancellationToken) =>
        {
            var values = await compiled.EvaluateAsync(gbx, cancellationToken);

            // A statement such as an assignment with ; has no value to show, like in the C# prompt.
            var shown = values.Where((value, i) => value is not null || !expressions[i].TrimEnd().EndsWith(';'))
                .Select(value => ValueFormatter.Clean(ValueFormatter.Describe(value)));

            return new GbxBatchEdit(Changed: null, write is null ? string.Join(" | ", shown) : null);
        });
    }
}
