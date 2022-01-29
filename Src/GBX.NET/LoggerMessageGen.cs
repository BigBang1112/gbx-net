namespace GBX.NET;

internal static partial class LoggerMessageGen
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "0x{chunkHex} ({progressPercentage}%)", EventId = 1)]
    public static partial void LogChunkProgress(this ILogger logger, string chunkHex, float progressPercentage);

    [LoggerMessage(Level = LogLevel.Debug, Message = "DONE! ({time}ms)", EventId = 2)]
    public static partial void LogNodeComplete(this ILogger logger, double time);

    [LoggerMessage(Level = LogLevel.Debug, Message = "0x{chunkHex} (???%)", EventId = 3)]
    public static partial void LogChunkProgressSeekless(this ILogger logger, string chunkHex);

    [LoggerMessage(Level = LogLevel.Debug, Message = "0x{chunkHex} [unknown] ({size} bytes)", EventId = 4)]
    public static partial void LogChunkUnknownSkippable(this ILogger logger, string chunkHex, int size);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Unexpected end of the stream: {position}/{length}", EventId = 5)]
    public static partial void LogUnexpectedEndOfStream(this ILogger logger, long position, long length);
}
