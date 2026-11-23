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

        write |= outputPath is not null;

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
            throw new ArgumentException("Use save in the C# prompt instead of --write or --output with -i.");
        }

        if (!write && (!backup || force))
        {
            throw new ArgumentException("--no-backup and --force only apply with --write or --output.");
        }

        await using var file = await FileObject.OpenAsync(inputPath, context.CancellationToken);

        // Fails before the expressions run, so they don't seem to have been saved.
        var gbx = write ? file.GetWritableGbx(outputPath) : null;

        var session = new CSharpSession(file.Value);

        foreach (var expression in expressions)
        {
            var result = await session.EvaluateAsync(expression, context.CancellationToken);
            await new ConsoleValueWriter(context.Console) { HasExternalReferences = ValueInspector.HasExternalReferences(file.Value) }.WriteResultAsync(result);
        }

        if (gbx is not null)
        {
            await CheckedGbxWriter.WriteAsync(gbx, outputPath ?? file.InputPath, context.Console, backup, force, context.CancellationToken);
        }

        if (interactive)
        {
            await CSharpPrompt.RunAsync(session, file, context);
        }
    }
}
