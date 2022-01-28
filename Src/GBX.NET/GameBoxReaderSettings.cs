namespace GBX.NET;

public class GameBoxReaderSettings
{
    /// <summary>
    /// State used for reading node references or lookback strings.
    /// </summary>
    public Guid? StateGuid { get; }

    /// <summary>
    /// A delegate collection that gets executed throughout the asynchronous reading.
    /// </summary>
    public GameBoxAsyncReadAction? AsyncAction { get; }

    public GameBoxReaderSettings(Guid? stateGuid, GameBoxAsyncReadAction? asyncAction)
    {
        StateGuid = stateGuid;
        AsyncAction = asyncAction;
    }
}
