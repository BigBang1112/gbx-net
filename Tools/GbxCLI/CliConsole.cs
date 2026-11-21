using TmEssentials;

namespace GbxCLI;

/// <summary>
/// Writes text with Trackmania formatting codes, as ANSI colors in a console or as plain text otherwise.
/// </summary>
/// <remarks>
/// Text from files, such as paths and exception messages, has to go through <see cref="Escape"/>,
/// because a <c>$</c> would otherwise start a code.
/// </remarks>
internal sealed class CliConsole(TextWriter writer, bool colors)
{
    public const string Label = "$888";
    public const string Heading = "$o$0cf";
    public const string Written = "$4c4";
    public const string Error = "$f44";
    public const string Reset = "$z";

    public TextWriter Writer { get; } = writer;

    public bool Colors { get; } = colors;

    /// <summary>
    /// Creates a console with colors, if <paramref name="writer"/> is the standard output or error of a console that shows them.
    /// Colors are off when the output is redirected, for any other writer, and when <c>NO_COLOR</c> is set.
    /// </summary>
    public static CliConsole Create(TextWriter writer)
    {
        var isError = writer == Console.Error;
        var isConsole = isError ? !Console.IsErrorRedirected : writer == Console.Out && !Console.IsOutputRedirected;
        var colors = isConsole
            && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NO_COLOR"))
            && ConsoleMode.TryEnableVirtualTerminal(isError);

        return new CliConsole(writer, colors);
    }

    /// <summary>
    /// Doubles every <c>$</c>, so the text is written as it is.
    /// </summary>
    public static string Escape(string text) => text.Replace("$", "$$");

    /// <summary>
    /// Text that can already use Trackmania formatting, such as a map name. It is written unescaped in color,
    /// so it shows in its in-game colors, and escaped otherwise, so plain output keeps the original value.
    /// </summary>
    public string Formatted(string text) => Colors ? text : Escape(text);

    public Task WriteLineAsync(string text) => Writer.WriteLineAsync(Format(text));

    /// <summary>
    /// Writes without a line break, such as for a prompt.
    /// </summary>
    public Task WriteAsync(string text) => Writer.WriteAsync(Format(text));

    public string Format(string text)
    {
        if (!Colors)
        {
            return TextFormatter.Deformat(text);
        }

        try
        {
            return TextFormatter.FormatAnsi(text);
        }
        catch (FormatException)
        {
            // Some codes, such as links, are not supported in ANSI.
            return TextFormatter.Deformat(text);
        }
    }
}
