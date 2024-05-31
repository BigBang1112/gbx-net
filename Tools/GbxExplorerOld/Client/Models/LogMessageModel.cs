using System.Text;

namespace GbxExplorerOld.Client.Models;

public class LogMessageModel
{
    public LogLevel LogLevel { get; }
    public string Message { get; }
    public LogScopeModel? Scope { get; }
    public Exception? Exception { get; }
    public DateTime Timestamp { get; }

    public bool Hovered { get; set; }
    public bool Selected { get; set; }

    public string TextColor => LogLevel switch
    {
        LogLevel.Information => "black",
        _ => "white",
    };

    public string ThemeColor => LogLevel switch
    {
        LogLevel.Information => "white",
        LogLevel.Debug => "gray",
        LogLevel.Warning => "darkgoldenrod",
        LogLevel.Error => "red",
        _ => "black",
    };

    public string LogLevelName => LogLevel switch
    {
        LogLevel.Information => "Info",
        LogLevel.Debug => "Debug",
        LogLevel.Warning => "Warn",
        LogLevel.Error => "Error",
        _ => "",
    };

    public string LogLevelNameUpper => LogLevel switch
    {
        LogLevel.Information => "INFO",
        LogLevel.Debug => "DEBUG",
        LogLevel.Warning => "WARN",
        LogLevel.Error => "ERROR",
        _ => "",
    };

    public LogMessageModel(LogLevel logLevel, string message, LogScopeModel? scope, Exception? exception, DateTime timestamp)
    {
        LogLevel = logLevel;
        Message = message;
        Scope = scope;
        Exception = exception;
        Timestamp = timestamp;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        
        foreach (var part in RecurseScopeStrings())
        {
            builder.Append(part);
        }
        
        builder.Append(Message);

        return builder.ToString();
    }

    private IEnumerable<LogScopeModel> RecurseScopes()
    {
        var curScope = Scope;

        while (curScope is not null)
        {
            yield return curScope;
            curScope = curScope.Parent;
        }
    }

    private IEnumerable<string> RecurseScopeStrings()
    {
        foreach (var scope in RecurseScopes().Reverse())
        {
            yield return $"{scope} -> ";
        }
    }
}
