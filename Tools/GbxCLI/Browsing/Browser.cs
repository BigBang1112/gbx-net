using GBX.NET;

namespace GbxCLI.Browsing;

/// <summary>
/// Shows one object at a time with numbered members. A number opens a member, <c>..</c> goes back, <c>/</c> goes to the root,
/// <c>n</c> and <c>p</c> move between pages, <c>f &lt;pattern&gt;</c> filters archive entries, and <c>q</c> quits.
/// <c>set &lt;number&gt; &lt;value&gt;</c> changes a member of a Gbx, and <c>save</c> or <c>save as &lt;path&gt;</c> writes it.
/// </summary>
internal sealed class Browser
{
    public const int PageSize = 20;

    private const string HelpText = "Enter a number to open an item, .. to go back, / for the root, n and p for the next and previous page, f <pattern> to filter archive entries (f alone clears it), set <number> <value> to change a value of a Gbx, save or save as <path> to write it (add --force to write despite the check), or q to quit.";

    private readonly CliContext context;
    private readonly List<BrowserFrame> frames = [];

    private CliConsole Console => context.Console;

    private BrowserFrame Current => frames[^1];

    private Browser(CliContext context)
    {
        this.context = context;
    }

    public static async Task RunAsync(string path, CliContext context)
    {
        await using var file = await FileObject.OpenAsync(path, context.CancellationToken);
        await RunAsync(file, context);
    }

    /// <summary>
    /// Browses a file that is already open. The caller disposes it.
    /// </summary>
    public static Task RunAsync(FileObject file, CliContext context)
    {
        var browser = new Browser(context);
        browser.PushFile(file.Name, file.Value, file);

        // A Gbx from an archive is never written back, so it needs save as.
        browser.frames[0].SavePath = file.Value is Gbx && !file.IsArchiveEntry ? file.InputPath : null;

        return browser.RunAsync();
    }

    private async Task RunAsync()
    {
        await RenderAsync();

        while (true)
        {
            // A star shows unsaved changes, like in an editor.
            await Console.WriteAsync($"{CliConsole.Escape(Current.TypeName)}{(frames.Any(frame => frame.IsDirty) ? "*" : "")}> ");

            var line = await context.Input.ReadLineAsync(context.CancellationToken);

            if (line is null)
            {
                return;
            }

            var command = line.Trim();

            switch (command)
            {
                case "q" or "quit" or "exit":
                    if (await ConfirmDiscardAsync(frames))
                    {
                        return;
                    }

                    break;
                case "":
                    await RenderAsync();
                    break;
                case "?" or "h" or "help":
                    await NoticeAsync(HelpText);
                    break;
                case "..":
                    if (frames.Count == 1)
                    {
                        await NoticeAsync("Already at the root.");
                        break;
                    }

                    if (!await ConfirmDiscardAsync([Current]))
                    {
                        break;
                    }

                    frames.RemoveAt(frames.Count - 1);
                    await RenderAsync();
                    break;
                case "/":
                    if (!await ConfirmDiscardAsync(frames.Skip(1)))
                    {
                        break;
                    }

                    frames.RemoveRange(1, frames.Count - 1);
                    await RenderAsync();
                    break;
                case "n" or "p":
                    var page = Current.Page + (command == "n" ? 1 : -1);

                    if (page < 0 || page >= Current.PageCount)
                    {
                        await NoticeAsync(command == "n" ? "Already on the last page." : "Already on the first page.");
                        break;
                    }

                    Current.Page = page;
                    await RenderAsync();
                    break;
                case "f" or ['f', ' ', ..]:
                    if (!Current.IsArchive)
                    {
                        await NoticeAsync("Filters only apply to archive entries.");
                        break;
                    }

                    var pattern = command[1..].Trim();
                    Current.Filter = pattern.Length == 0 ? null : pattern;
                    await RenderAsync();
                    break;
                case ['s', 'e', 't', ' ', ..]:
                    await SetAsync(command[4..].Trim());
                    break;
                case "save" or ['s', 'a', 'v', 'e', ' ', ..]:
                    await SaveAsync(command[4..].Trim());
                    break;
                case var number when int.TryParse(number, out var index):
                    await OpenAsync(index);
                    break;
                default:
                    await NoticeAsync($"Unknown command: {command}. Enter ? for help.");
                    break;
            }
        }
    }

    /// <summary>
    /// Opens the item with the 1-based <paramref name="index"/>. Errors are shown, and the session continues.
    /// </summary>
    private async Task OpenAsync(int index)
    {
        if (index < 1 || index > Current.Items.Count)
        {
            await NoticeAsync($"There is no item {index}.");
            return;
        }

        var item = Current.Items[index - 1];

        try
        {
            if (item.OpenEntryAsync is { } openEntryAsync)
            {
                object? value = null;

                // Ctrl+C cancels a slow entry, such as a large file in a PAK, and keeps the session.
                if (!await context.TryRunCancellableAsync(async cancellationToken => value = await openEntryAsync(cancellationToken)))
                {
                    await NoticeAsync("Opening was canceled.");
                    return;
                }

                PushFile(item.Name, value!, Current.Owner);
                await RenderAsync();
                return;
            }

            if (item.Error is not null)
            {
                await ErrorAsync(item.Error);
                return;
            }

            switch (item.Value)
            {
                case NodeRef { Kind: NodeRefKind.Embedded } reference:
                    frames.Add(new BrowserFrame(Current.GetChildTitle(item.Name), reference.Node!) { HasExternalReferences = Current.HasExternalReferences });
                    await RenderAsync();
                    break;
                case null or NodeRef:
                    // External references stay unresolved, so browsing never loads other files.
                    await Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(item.Name)}:{CliConsole.Reset} {CliConsole.Escape(ValueFormatter.Describe(item.Value))}");
                    break;
                case var value when ValueInspector.IsScalar(value):
                    // The full, exact value without the length limit of the list. The list already shows formatted text in color.
                    var text = ValueFormatter.Clean(ValueFormatter.Describe(value), int.MaxValue);
                    await Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(item.Name)}:{CliConsole.Reset} {CliConsole.Escape(text)}");
                    break;
                case var value:
                    frames.Add(new BrowserFrame(Current.GetChildTitle(item.Name), value) { HasExternalReferences = Current.HasExternalReferences });
                    await RenderAsync();
                    break;
            }
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await ErrorAsync(exception.Message);
        }
    }

    /// <summary>
    /// Assigns a value to a member of the current object for <c>set &lt;number&gt; &lt;value&gt;</c>, like <c>gbx set</c> does.
    /// The value can be in double quotes, so <c>""</c> sets an empty string.
    /// </summary>
    private async Task SetAsync(string arguments)
    {
        var parts = arguments.Split(' ', 2, StringSplitOptions.TrimEntries);

        if (parts.Length < 2 || !int.TryParse(parts[0], out var index))
        {
            await NoticeAsync("Use set <number> <value>, such as set 3 My map.");
            return;
        }

        if (index < 1 || index > Current.Items.Count)
        {
            await NoticeAsync($"There is no item {index}.");
            return;
        }

        var item = Current.Items[index - 1];
        var fileIndex = frames.FindLastIndex(frame => frame.IsFile);

        if (frames[fileIndex].Value is not Gbx gbx || item.OpenEntryAsync is not null)
        {
            await NoticeAsync("Only values of a Gbx can be set.");
            return;
        }

        if (item.Value is not null && !ValueInspector.IsScalar(item.Value))
        {
            await NoticeAsync($"{item.Name} has values of its own. Open it with {index}, then set them.");
            return;
        }

        var text = parts[1] is ['"', .., '"'] quoted ? quoted[1..^1] : parts[1];

        try
        {
            var change = MemberPath.Parse(Current.GetChildTitle(item.Name)).Assign(gbx, text);

            frames[fileIndex].IsDirty = true;
            RefreshFrames(fileIndex, gbx);

            await RenderAsync();
            await Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(change.Path)}:{CliConsole.Reset} {Describe(change.OldValue)} {CliConsole.Label}->{CliConsole.Reset} {Describe(change.NewValue)}");
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await ErrorAsync(exception.Message);
        }
    }

    /// <summary>
    /// Reads the values of the open frames again. A struct or a record that was changed is a new value, which the old frame doesn't show.
    /// </summary>
    private void RefreshFrames(int fileIndex, Gbx gbx)
    {
        for (var i = fileIndex + 1; i < frames.Count; i++)
        {
            try
            {
                if (MemberPath.Parse(frames[i].Title).GetValue(gbx) is { } value && !ReferenceEquals(value, frames[i].Value))
                {
                    frames[i] = frames[i].WithValue(value);
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                // A frame that a member path cannot reach, such as an item of a set, keeps its value.
            }

            frames[i].Refresh();
        }

        frames[fileIndex].Refresh();
    }

    /// <summary>
    /// Writes the Gbx of the current file for <c>save [--force]</c> or <c>save as [--force] &lt;path&gt;</c>, like <c>gbx eval --write</c>.
    /// </summary>
    private async Task SaveAsync(string arguments)
    {
        var saveAs = arguments == "as" || arguments.StartsWith("as ", StringComparison.Ordinal);
        var rest = saveAs ? arguments[2..].Trim() : arguments;
        var force = rest == "--force" || rest.StartsWith("--force ", StringComparison.Ordinal);
        rest = force ? rest["--force".Length..].Trim() : rest;

        var frame = frames[frames.FindLastIndex(frame => frame.IsFile)];

        if (frame.Value is not Gbx gbx)
        {
            await NoticeAsync("Only a Gbx can be saved.");
            return;
        }

        if (!saveAs && rest.Length > 0)
        {
            await NoticeAsync("Use save, or save as <path>.");
            return;
        }

        var path = saveAs ? Cli.CleanPath(rest) : frame.SavePath;

        if (string.IsNullOrEmpty(path))
        {
            await NoticeAsync(saveAs ? "Use save as <path>." : "This Gbx is inside an archive, which is never written. Use save as <path>.");
            return;
        }

        try
        {
            await CheckedGbxWriter.WriteAsync(gbx, path, Console, backup: true, force, context.CancellationToken);
            frame.IsDirty = false;
            frame.SavePath = path;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await ErrorAsync(exception.Message);
        }
    }

    /// <summary>
    /// Asks before files with unsaved changes are closed. Without more input, they are closed.
    /// </summary>
    /// <returns>True if the files can be closed.</returns>
    private async Task<bool> ConfirmDiscardAsync(IEnumerable<BrowserFrame> closed)
    {
        var dirty = closed.Where(frame => frame.IsDirty).Select(frame => frame.Title).ToArray();

        if (dirty.Length == 0)
        {
            return true;
        }

        await Console.WriteAsync($"{CliConsole.Label}{CliConsole.Escape(string.Join(", ", dirty))} has unsaved changes. Enter y to discard them:{CliConsole.Reset} ");

        var answer = await context.Input.ReadLineAsync(context.CancellationToken);

        return answer is null || answer.Trim().ToLowerInvariant() is "y" or "yes";
    }

    private static string Describe(object? value) => CliConsole.Escape(ValueFormatter.Clean(ValueFormatter.Describe(value)));

    /// <summary>
    /// Adds an opened file. A Gbx with a node opens on the node, and <c>..</c> goes to the Gbx.
    /// </summary>
    /// <param name="owner">The file object that owns entries of an archive, so they are disposed with it.</param>
    private void PushFile(string name, object value, FileObject? owner)
    {
        var hasExternalReferences = ValueInspector.HasExternalReferences(value);

        frames.Add(new BrowserFrame(name, value, isFile: true) { Owner = owner, HasExternalReferences = hasExternalReferences });

        if (value is Gbx { Node: { } node })
        {
            frames.Add(new BrowserFrame("Node", node) { HasExternalReferences = hasExternalReferences });
        }
    }

    private async Task RenderAsync()
    {
        var frame = Current;

        await Console.WriteLineAsync(string.Empty);
        await Console.WriteLineAsync($"{CliConsole.Heading}{CliConsole.Escape(frame.Title)}{CliConsole.Reset} {CliConsole.Label}({CliConsole.Escape(frame.TypeName)}){CliConsole.Reset}");

        if (frame.Value is GBX.NET.PAK.Pak { IsHeaderPrivate: true, Files.Count: 0 })
        {
            await Console.WriteLineAsync($"{CliConsole.Label}  The file index is unavailable, because the private header requires a key.{CliConsole.Reset}");
        }

        if (frame.Filter is not null)
        {
            await Console.WriteLineAsync($"{CliConsole.Label}  Filter: {CliConsole.Escape(frame.Filter)} ({frame.Items.Count:N0} entries){CliConsole.Reset}");
        }

        var start = frame.Page * PageSize;

        for (var i = start; i < Math.Min(start + PageSize, frame.Items.Count); i++)
        {
            var item = frame.Items[i];
            var description = ValueFormatter.Clean(item.Description);
            description = item.IsFormatted ? Console.Formatted(description) : CliConsole.Escape(description);

            await Console.WriteLineAsync($"  [{i + 1}] {CliConsole.Label}{CliConsole.Escape(item.Name)}:{CliConsole.Reset} {description}");
        }

        if (frame.Items.Count == 0)
        {
            await Console.WriteLineAsync($"{CliConsole.Label}  (no items){CliConsole.Reset}");
        }

        if (frame.UnsetCount > 0)
        {
            await Console.WriteLineAsync($"{CliConsole.Label}  {frame.UnsetCount} unset properties are not listed.{CliConsole.Reset}");
        }

        if (frame.PageCount > 1)
        {
            await Console.WriteLineAsync($"{CliConsole.Label}  Page {frame.Page + 1} of {frame.PageCount} ({frame.Items.Count:N0} items). n next page, p previous page.{CliConsole.Reset}");
        }
    }

    private Task NoticeAsync(string text) => Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(text)}{CliConsole.Reset}");

    private Task ErrorAsync(string message) => Console.WriteLineAsync($"{CliConsole.Error}error:{CliConsole.Reset} {CliConsole.Escape(message)}");
}
