namespace GbxCLI.Commands;

/// <summary>
/// Lists the properties that differ between two Gbx files, compared like <c>verify --roundtrip</c> does.
/// </summary>
internal static class DiffCommand
{
    public const int DefaultMax = 200;

    /// <summary>
    /// How many changes <c>eval</c> lists before it writes a Gbx.
    /// </summary>
    public const int ChangesMax = 50;

    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? leftPath = null;
        string? rightPath = null;
        var max = DefaultMax;
        var strict = false;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "diff");
                    return;
                case "--max":
                    var value = args.NextValue("--max");
                    max = int.TryParse(value, out var number) && number >= 0
                        ? number
                        : throw new ArgumentException($"--max takes a number of differences, or 0 for all of them, not {value}.");
                    break;
                case "--strict":
                    strict = true;
                    break;
                case var arg when leftPath is null:
                    leftPath = arg;
                    break;
                case var arg when rightPath is null:
                    rightPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected("diff", arg);
            }
        }

        if (string.IsNullOrWhiteSpace(leftPath) || string.IsNullOrWhiteSpace(rightPath))
        {
            throw new ArgumentException("diff needs two Gbx files.");
        }

        await using var left = await FileObject.OpenAsync(leftPath, context.CancellationToken);
        await using var right = await FileObject.OpenAsync(rightPath, context.CancellationToken);

        var limit = max == 0 ? int.MaxValue : max;
        var differences = GbxPropertyComparer.FindDifferences(left.GetGbx(), right.GetGbx(), limit, cancellationToken: context.CancellationToken);

        await WriteDifferencesAsync(context.Console, differences, limit, indent: "");

        await context.Console.WriteLineAsync(differences.Count switch
        {
            0 => "No differences.",
            _ when differences.Count >= limit => $"Stopped after {differences.Count:N0} {(differences.Count == 1 ? "difference" : "differences")}. Use --max 0 to list all of them.",
            1 => "1 difference.",
            _ => $"{differences.Count:N0} differences."
        });

        if (strict && differences.Count > 0)
        {
            throw new InvalidDataException("The Gbx files differ.");
        }
    }

    /// <summary>
    /// Writes each difference on its own line, such as <c>Node.MapName: A -> B</c>.
    /// </summary>
    /// <param name="limit">The limit the differences were found with. Reaching it adds a line that more were not listed.</param>
    public static async Task WriteDifferencesAsync(CliConsole console, IReadOnlyList<GbxDifference> differences, int limit, string indent = "  ")
    {
        foreach (var difference in differences)
        {
            await console.WriteLineAsync($"{indent}{CliConsole.Label}{CliConsole.Escape(difference.Path)}:{CliConsole.Reset} {CliConsole.Escape(difference.Left)} {CliConsole.Label}->{CliConsole.Reset} {CliConsole.Escape(difference.Right)}");
        }

        if (indent.Length > 0 && differences.Count >= limit)
        {
            await console.WriteLineAsync($"{indent}{CliConsole.Label}... and possibly more, not listed.{CliConsole.Reset}");
        }
    }
}
