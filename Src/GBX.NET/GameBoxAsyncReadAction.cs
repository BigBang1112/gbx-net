namespace GBX.NET;

public class GameBoxAsyncReadAction
{
    public Func<uint, CancellationToken, Task>? AfterClassId { get; init; }
    public Func<Node, Chunk?, Task>? AfterChunkIteration { get; init; }
    public Func<Task>? BeforeLzoDecompression { get; init; }
    public Func<Task>? AfterLzoDecompression { get; init; }
    public Func<GameBoxReader, uint, Task>? OnReadNode { get; init; }
}
