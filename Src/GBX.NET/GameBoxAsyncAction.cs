namespace GBX.NET;

public class GameBoxAsyncAction
{
    public Func<Task>? AfterChunkIteration { get; init; }
}
