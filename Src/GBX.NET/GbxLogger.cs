namespace GBX.NET;

public static class GbxLogger
{
    public enum Level
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    public delegate void LogEventHandler(string message, Level level, object?[] args);

    public static event LogEventHandler? LogEvent;

    public static Scope CreateScope(string name)
    {
        return new Scope { Name = name };
    }

    public sealed class Scope : IDisposable
    {
        public string? Name { get; init; }

        public static void LogTrace(string message)
        {
            if (LogEvent is null) return;
            LogEvent(message, Level.Trace, []);
        }

        public static void LogTrace<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg]);
        }

        public static void LogTrace<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1]);
        }

        public static void LogTrace<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1, arg2]);
        }

        public static void LogTrace<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1, arg2, arg3]);
        }

        public static void LogTrace<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogTrace<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogTrace<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Trace, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public static void LogDebug(string message)
        {
            LogEvent?.Invoke(message, Level.Debug, []);
        }

        public static void LogDebug<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg]);
        }

        public static void LogDebug<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1]);
        }

        public static void LogDebug<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1, arg2]);
        }

        public static void LogDebug<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1, arg2, arg3]);
        }

        public static void LogDebug<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogDebug<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogDebug<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Debug, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public static void LogInfo(string message)
        {
            LogEvent?.Invoke(message, Level.Info, []);
        }

        public static void LogInfo<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Info, [arg]);
        }

        public static void LogInfo<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1]);
        }

        public static void LogInfo<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1, arg2]);
        }

        public static void LogInfo<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1, arg2, arg3]);
        }

        public static void LogInfo<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogInfo<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogInfo<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Info, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public static void LogWarning(string message)
        {
            LogEvent?.Invoke(message, Level.Warning, []);
        }

        public static void LogWarning<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg]);
        }

        public static void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1]);
        }

        public static void LogWarning<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1, arg2]);
        }

        public static void LogWarning<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1, arg2, arg3]);
        }

        public static void LogWarning<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogWarning<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogWarning<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Warning, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public static void LogError(string message)
        {
            LogEvent?.Invoke(message, Level.Error, []);
        }

        public static void LogError<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Error, [arg]);
        }

        public static void LogError<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1]);
        }

        public static void LogError<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1, arg2]);
        }

        public static void LogError<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1, arg2, arg3]);
        }

        public static void LogError<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogError<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogError<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Error, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public static void LogCritical(string message)
        {
            LogEvent?.Invoke(message, Level.Critical, []);
        }

        public static void LogCritical<T>(string message, T arg)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg]);
        }

        public static void LogCritical<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1]);
        }

        public static void LogCritical<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1, arg2]);
        }

        public static void LogCritical<T0, T1, T2, T3>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1, arg2, arg3]);
        }

        public static void LogCritical<T0, T1, T2, T3, T4>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1, arg2, arg3, arg4]);
        }

        public static void LogCritical<T0, T1, T2, T3, T4, T5>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1, arg2, arg3, arg4, arg5]);
        }

        public static void LogCritical<T0, T1, T2, T3, T4, T5, T6>(string message, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LogEvent?.Invoke(message, Level.Critical, [arg0, arg1, arg2, arg3, arg4, arg5, arg6]);
        }

        public void Dispose()
        {
            LogTrace("Disposing scope '{Name}'", Name);
        }
    }
}
