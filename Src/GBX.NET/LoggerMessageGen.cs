namespace GBX.NET;

internal static partial class LoggerMessageGen
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "0x{chunkHex} ({progressPercentage}%)")]
    public static partial void LogChunkProgress(this ILogger logger, string chunkHex, float progressPercentage);

    [LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "DONE! ({time}ms)")]
    public static partial void LogNodeComplete(this ILogger logger, double time);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "0x{chunkHex} (???%)")]
    public static partial void LogChunkProgressSeekless(this ILogger logger, string chunkHex);

    [LoggerMessage(EventId = 4, Level = LogLevel.Debug, Message = "0x{chunkHex} [unknown] ({size} bytes)")]
    public static partial void LogChunkUnknownSkippable(this ILogger logger, string chunkHex, int size);

    [LoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "Unexpected end of the stream: {position}/{length}")]
    public static partial void LogUnexpectedEndOfStream(this ILogger logger, long position, long length);

    [LoggerMessage(EventId = 6, Level = LogLevel.Debug, Message = "Version: {version}")]
    public static partial void LogVersion(this ILogger logger, short version);

    [LoggerMessage(EventId = 7, Level = LogLevel.Debug, Message = "Format: {format}")]
    public static partial void LogFormat(this ILogger logger, GameBoxFormat format);

    [LoggerMessage(EventId = 8, Level = LogLevel.Debug, Message = "Ref. table compression: {compression}")]
    public static partial void LogRefTableCompression(this ILogger logger, GameBoxCompression compression);

    [LoggerMessage(EventId = 9, Level = LogLevel.Debug, Message = "Body compression: {compression}")]
    public static partial void LogBodyCompression(this ILogger logger, GameBoxCompression compression);

    [LoggerMessage(EventId = 10, Level = LogLevel.Debug, Message = "Unknown byte: {unknownByte}")]
    public static partial void LogUnknownByte(this ILogger logger, char unknownByte);

    [LoggerMessage(EventId = 11, Level = LogLevel.Debug, Message = "Class ID: 0x{classId}")]
    public static partial void LogClassId(this ILogger logger, string classId);

    [LoggerMessage(EventId = 12, Level = LogLevel.Debug, Message = "User data size: {size} kB")]
    public static partial void LogUserDataSize(this ILogger logger, float size);

    [LoggerMessage(EventId = 13, Level = LogLevel.Debug, Message = "Number of nodes: {numNodes}")]
    public static partial void LogNumberOfNodes(this ILogger logger, int numNodes);

    [LoggerMessage(EventId = 14, Level = LogLevel.Debug, Message = "- 0x{classId} | {size} B")]
    public static partial void LogHeaderChunk(this ILogger logger, string classId, int size);

    [LoggerMessage(EventId = 15, Level = LogLevel.Debug, Message = "- 0x{classId} | {size} B (Heavy)")]
    public static partial void LogHeaderChunkHeavy(this ILogger logger, string classId, int size);

    [LoggerMessage(EventId = 16, Level = LogLevel.Warning, Message = "Node ID 0x{classId} contained in the header is not implemented. ({className})")]
    public static partial void LogHeaderChunkNodeNotImplemented(this ILogger logger, string classId, string className);

    [LoggerMessage(EventId = 17, Level = LogLevel.Warning, Message = "Discarded external node: {externalNode}")]
    public static partial void LogDiscardedExternalNode(this ILogger logger, GameBoxRefTable.File externalNode);
}