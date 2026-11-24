using GBX.NET;

namespace GbxCLI;

/// <summary>
/// Runs an edit on every Gbx that a folder or pattern selects, and continues after failures. Each file gets one line of output,
/// and a summary follows. With <see cref="GbxBatchWrite"/>, only the changed files are checked and written.
/// </summary>
/// <param name="write">How changed files are written, or null to only show what the edit returns.</param>
internal sealed class GbxBatchRunner(CliContext context, GbxBatchWrite? write)
{
    private int evaluated;
    private int changed;
    private int unchanged;
    private int failed;
    private int skipped;

    private CliConsole Console => context.Console;

    /// <exception cref="FileNotFoundException">The path selects no files.</exception>
    /// <exception cref="InvalidDataException">Some files failed, which is thrown after the summary.</exception>
    public async Task RunAsync(string path, Func<Gbx, CancellationToken, Task<GbxBatchEdit>> edit)
    {
        if (write is { OutputDirectory: null, DryRun: false } && GbxBatch.IsInArchive(path))
        {
            throw new InvalidOperationException("Gbx files inside an archive cannot be written back to the archive. Use -o to write them to a folder.");
        }

        string? root = null;

        if (write?.OutputDirectory is { } outputDirectory)
        {
            if (File.Exists(outputDirectory))
            {
                throw new IOException($"Cannot write to '{outputDirectory}', because it is a file. With a folder or pattern, -o is a folder.");
            }

            root = OutputFile.GetRoot(outputDirectory);
        }

        var selected = 0;

        await foreach (var item in GbxBatch.EnumerateAsync(path, context.CancellationToken))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            selected++;

            try
            {
                if (await item.OpenAsync(context.CancellationToken) is not { } gbx)
                {
                    skipped++;
                    continue;
                }

                await ProcessAsync(item, gbx, edit, root);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                failed++;
                await WriteLineAsync(item, $"{CliConsole.Error}{exception.GetType().Name}:{CliConsole.Reset} {CliConsole.Escape(exception.Message.ReplaceLineEndings(" "))}");
            }
        }

        if (selected == 0)
        {
            throw new FileNotFoundException($"No Gbx files were found for '{path}'.", path);
        }

        await Console.WriteLineAsync(CliConsole.Escape(GetSummary()));

        if (failed > 0)
        {
            throw new InvalidDataException($"{failed} Gbx files failed.");
        }
    }

    private async Task ProcessAsync(GbxBatchItem item, Gbx gbx, Func<Gbx, CancellationToken, Task<GbxBatchEdit>> edit, string? root)
    {
        var original = write is { CompareWithOriginal: true } ? await item.OpenAsync(context.CancellationToken) : null;
        var result = await edit(gbx, context.CancellationToken);

        if (write is null)
        {
            evaluated++;
            await WriteLineAsync(item, CliConsole.Escape(result.Text ?? string.Empty));
            return;
        }

        var firstChange = result.Changed is null ? GbxPropertyComparer.FindDifference(original!, gbx, context.CancellationToken) : null;

        // An unchanged file is never saved, so a class that GBX.NET can't write only fails when it changed.
        if (!(result.Changed ?? firstChange is not null))
        {
            unchanged++;
            await WriteLineAsync(item, $"{CliConsole.Label}unchanged{CliConsole.Reset}");
            return;
        }

        var data = CheckedGbxWriter.Save(gbx);
        var difference = await CheckedGbxWriter.FindDifferenceAsync(gbx, data, context.CancellationToken);

        if (difference is not null && !write.Force)
        {
            throw CheckedGbxWriter.NotWritten(difference);
        }

        if (!write.DryRun)
        {
            var destination = root is null ? item.FilePath! : OutputFile.GetDestination(root, item.RelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await CheckedGbxWriter.WriteDataAsync(data, destination, write.Backup, context.CancellationToken);
        }

        changed++;

        var status = write.DryRun ? "would change" : "changed";
        var details = (result.Text ?? (firstChange is null ? null : $"first difference: {firstChange.TrimEnd('.')}")) is { } text ? $" ({text})" : "";
        var warning = difference is null ? "" : $"{CliConsole.Error} warning:{CliConsole.Reset} {CliConsole.Escape($"writing anyway, although {difference}")}";

        await WriteLineAsync(item, $"{CliConsole.Written}{status}{CliConsole.Reset}{CliConsole.Escape(details)}{warning}");
    }

    private Task WriteLineAsync(GbxBatchItem item, string text)
        => Console.WriteLineAsync($"{CliConsole.Escape(item.DisplayPath)}: {text}");

    private string GetSummary()
    {
        var rest = $"{failed} failed; {skipped} non-Gbx files skipped.";

        return write switch
        {
            null => $"Evaluated {evaluated} Gbx files; {rest}",
            { DryRun: true } => $"{changed} Gbx files would change; {unchanged} unchanged; {rest} Dry run, so nothing was written.",
            _ => $"Changed {changed} Gbx files; {unchanged} unchanged; {rest}"
        };
    }
}
