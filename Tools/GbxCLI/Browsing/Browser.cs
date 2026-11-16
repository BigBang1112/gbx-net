using GBX.NET;

namespace GbxCLI.Browsing;

/// <summary>
/// Shows one object at a time with numbered members. A number opens a member, <c>..</c> goes back, <c>/</c> goes to the root,
/// <c>n</c> and <c>p</c> move between pages, <c>f &lt;pattern&gt;</c> filters archive entries, and <c>q</c> quits.
/// </summary>
internal sealed class Browser
{
    public const int PageSize = 20;

    private const string HelpText = "Enter a number to open an item, .. to go back, / for the root, n and p for the next and previous page, f <pattern> to filter archive entries (f alone clears it), or q to quit.";

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

        var browser = new Browser(context);
        browser.PushFile(file.Name, file.Value, file);
        await browser.RunAsync();
    }

    private async Task RunAsync()
    {
        await RenderAsync();

        while (true)
        {
            await Console.WriteAsync($"{CliConsole.Escape(Current.TypeName)}> ");

            var line = await context.Input.ReadLineAsync(context.CancellationToken);

            if (line is null)
            {
                return;
            }

            var command = line.Trim();

            switch (command)
            {
                case "q" or "quit" or "exit":
                    return;
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

                    frames.RemoveAt(frames.Count - 1);
                    await RenderAsync();
                    break;
                case "/":
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
            if (item.OpenEntryAsync is not null)
            {
                var value = await item.OpenEntryAsync(context.CancellationToken);
                PushFile(item.Name, value, Current.Owner);
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
                    frames.Add(new BrowserFrame(Current.GetChildTitle(item.Name), reference.Node!));
                    await RenderAsync();
                    break;
                case null or NodeRef:
                    // External references stay unresolved, so browsing never loads other files.
                    await Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(item.Name)}:{CliConsole.Reset} {CliConsole.Escape(ValueFormatter.Describe(item.Value))}");
                    break;
                case var value when ValueInspector.IsScalar(value):
                    // The full value, without the length limit of the list.
                    var text = ValueFormatter.Clean(ValueFormatter.Describe(value), int.MaxValue);
                    await Console.WriteLineAsync($"{CliConsole.Label}{CliConsole.Escape(item.Name)}:{CliConsole.Reset} {(item.IsFormatted ? Console.Formatted(text) : CliConsole.Escape(text))}");
                    break;
                case var value:
                    frames.Add(new BrowserFrame(Current.GetChildTitle(item.Name), value));
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
    /// Adds an opened file. A Gbx with a node opens on the node, and <c>..</c> goes to the Gbx.
    /// </summary>
    /// <param name="owner">The file object that owns entries of an archive, so they are disposed with it.</param>
    private void PushFile(string name, object value, FileObject? owner)
    {
        frames.Add(new BrowserFrame(name, value, isFile: true) { Owner = owner });

        if (value is Gbx { Node: { } node })
        {
            frames.Add(new BrowserFrame("Node", node));
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
