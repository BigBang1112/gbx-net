namespace GBX.NET;

public class GameBoxAsyncWriteAction
{
    public Func<Node, Chunk?, Task>? AfterChunkIteration { get; init; }
}