using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

internal static class PackListCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        bool? resolved = null;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "pack");
                    return;
                case "--resolved" or "--unresolved":
                    var requested = args.Current == "--resolved";
                    resolved = resolved is null || resolved == requested
                        ? requested
                        : throw new ArgumentException("The --resolved and --unresolved options cannot be combined.");
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected("pack list", arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("pack list");
        }

        await using var file = await FileObject.OpenArchiveAsync(inputPath, context.CancellationToken);

        var entries = file.Value switch
        {
            // ZIP entries always have names, so none of them is an unresolved hash.
            ZipArchive zip => zip.Select(file.EntryPattern).Select(entry => (Path: entry.FullName, IsResolved: true)),
            Pak pak => pak.Select(file.EntryPattern).Select(entry => (entry.Path, entry.IsResolved)),
            _ => throw new InvalidOperationException("Unexpected archive type.")
        };

        var matched = false;

        foreach (var (path, isResolved) in entries)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            matched = true;

            // Listed paths are data for scripts and pipes, so they are written as they are.
            if (resolved is null || isResolved == resolved)
            {
                await context.Output.WriteLineAsync(PathPattern.ToDisplay(path));
            }
        }

        if (file.EntryPattern is not null && !matched)
        {
            throw new FileNotFoundException($"No files matching '{file.EntryPattern}' were found in '{file.Name}'.");
        }
    }
}
