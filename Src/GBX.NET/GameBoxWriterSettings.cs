namespace GBX.NET;

public class GameBoxWriterSettings
{
    /// <summary>
    /// State used for writing node references or lookback strings.
    /// </summary>
    public Guid? StateGuid { get; }

    public IDRemap Remap { get; }

    /// <summary>
    /// A delegate collection that gets executed throughout the asynchronous writing.
    /// </summary>
    public GameBoxAsyncWriteAction? AsyncAction { get; }

    public GameBoxWriterSettings(Guid? stateGuid, IDRemap remap, GameBoxAsyncWriteAction? asyncAction)
    {
        StateGuid = stateGuid;
        Remap = remap;
        AsyncAction = asyncAction;
    }
}
