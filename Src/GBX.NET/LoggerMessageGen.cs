namespace GBX.NET;

internal static partial class LoggerMessageGen
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "0x{chunkHex} ({progressPercentage}%)", EventId = 1)]
    public static partial void LogChunkProgress(this ILogger logger, string chunkHex, float progressPercentage);

    [LoggerMessage(Level = LogLevel.Debug, Message = "DONE! ({time}ms)", EventId = 2)]
    public static partial void LogNodeComplete(this ILogger logger, double time);
}
