using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace GBX.NET.Tool.CLI;

public class SpectreConsoleLogger : ILogger
{
    private readonly LogLevel logLevel;

    public SpectreConsoleLogger(LogLevel logLevel = LogLevel.Trace)
    {
        this.logLevel = logLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= this.logLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        AnsiConsole.MarkupLine($" {GetLevelString(logLevel)} {Markup.Escape(formatter(state, exception))}");
    }

    private static string GetLevelString(LogLevel level) => level switch
    {
        LogLevel.Trace => "[dim grey]TRACE[/]:",
        LogLevel.Debug => "[dim grey]DEBUG[/]:",
        LogLevel.Information => " [deepskyblue2]INFO[/]:",
        LogLevel.Warning => " [bold orange3]WARN[/]:",
        LogLevel.Error => "[bold red]ERROR[/]:",
        LogLevel.Critical => " [bold underline red]CRIT[/]:",
        LogLevel.None => string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(level))
    };
}
