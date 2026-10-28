using Microsoft.Extensions.Logging;

namespace GBX.NET.Tests.Infrastructure;

internal sealed class TestLogger : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Debug;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            TestContext.Current!.Output.WriteLine($"[{logLevel}] {formatter(state, exception)}");
        }
    }
}
