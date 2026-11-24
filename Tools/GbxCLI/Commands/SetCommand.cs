using GBX.NET;

namespace GbxCLI.Commands;

/// <summary>
/// Assigns values to members of a Gbx, such as <c>Node.MapName="My map"</c>, and writes it after checking the saved data.
/// </summary>
internal static class SetCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        string? outputPath = null;
        var assignments = new List<(MemberPath Path, string Value)>();
        var dryRun = false;
        var backup = true;
        var force = false;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "set");
                    return;
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
                case var arg when arg.IndexOf('=') is > 0 and var separator:
                    assignments.Add((MemberPath.Parse(arg[..separator].Trim()), arg[(separator + 1)..]));
                    break;
                case var arg:
                    throw new ArgumentException($"Unexpected argument for set: {arg}. Use <member>=<value>, such as Node.MapName=\"My map\".");
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("set");
        }

        if (assignments.Count == 0)
        {
            throw new ArgumentException("Missing <member>=<value> for set.");
        }

        if (GbxBatch.IsBatch(inputPath))
        {
            var write = new GbxBatchWrite(outputPath, dryRun, backup, force);
            await new GbxBatchRunner(context, write).RunAsync(inputPath, (gbx, _) => Task.FromResult(Assign(gbx, assignments)));
            return;
        }

        await using var file = await FileObject.OpenAsync(inputPath, context.CancellationToken);

        // A dry run writes nothing, so a Gbx inside an archive can be tried without -o.
        var gbx = dryRun ? file.GetGbx() : file.GetWritableGbx(outputPath);

        foreach (var (path, value) in assignments)
        {
            var change = path.Assign(gbx, value);
            await context.Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(change.Path)}:{CliConsole.Reset} {Describe(change.OldValue)} {CliConsole.Label}->{CliConsole.Reset} {Describe(change.NewValue)}");
        }

        if (dryRun)
        {
            // The same check as a real write, so a dry run also shows whether the file could be written.
            await CheckedGbxWriter.SaveAsync(gbx, force, context.CancellationToken);
            await context.Console.WriteLineAsync($"{CliConsole.Label}Dry run, so nothing was written.{CliConsole.Reset}");
            return;
        }

        await CheckedGbxWriter.WriteAsync(gbx, outputPath ?? file.InputPath, context.Console, backup, force, context.CancellationToken);
    }

    /// <summary>
    /// Assigns the values to one Gbx of a batch. A file whose values already match is unchanged, so it isn't written.
    /// </summary>
    private static GbxBatchEdit Assign(Gbx gbx, List<(MemberPath Path, string Value)> assignments)
    {
        var changes = assignments.Select(assignment => assignment.Path.Assign(gbx, assignment.Value))
            .Where(change => !Equals(change.OldValue, change.NewValue))
            .ToArray();

        return new GbxBatchEdit(changes.Length > 0, changes.Length > 0
            ? string.Join(", ", changes.Select(change => $"{change.Path}: {Format(change.OldValue)} -> {Format(change.NewValue)}"))
            : null);
    }

    private static string Format(object? value) => ValueFormatter.Clean(ValueFormatter.Describe(value));

    private static string Describe(object? value) => CliConsole.Escape(Format(value));
}
