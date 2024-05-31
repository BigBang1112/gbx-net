using GbxExplorerOld.Client.Models;

namespace GbxExplorerOld.Client;

public class MemoryLogger : ILogger
{
    private readonly MemoryLog _log;

    internal LogScopeModel? CurrentScope { get; set; }

    public MemoryLogger(MemoryLog log)
    {
        _log = log;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        if (state is string stateStr)
        {
            return CurrentScope = new LogScopeModel(stateStr, CurrentScope, this);
        }

        if (state is not IReadOnlyList<KeyValuePair<string, object>> logValues)
        {
            return new NullDisposable();
        }

        var str = logValues.FirstOrDefault(x => x.Key == "{OriginalFormat}").Value.ToString() ?? "";

        foreach (var (key, val) in logValues)
        {
            if (key == "{OriginalFormat}")
            {
                continue;
            }

            str = str.Replace($"{{{key}}}", val.ToString());
        }

        return CurrentScope = new LogScopeModel(str, CurrentScope, this);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        var logMsg = new LogMessageModel(logLevel, message, CurrentScope, exception, DateTime.Now);

        if (logLevel != LogLevel.Trace)
        {
            _log.Log(logMsg);
        }
    }
}
