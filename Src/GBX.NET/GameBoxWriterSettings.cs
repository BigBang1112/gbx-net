namespace GBX.NET;

public class GameBoxWriterSettings
{
    /// <summary>
    /// A state used for writing node references or lookback strings.
    /// </summary>
    public GameBox? Gbx { get; }

    public IDRemap Remap { get; }

    /// <summary>
    /// A delegate collection that gets executed throughout the asynchronous writing.
    /// </summary>
    public GameBoxAsyncWriteAction? AsyncAction { get; }

    public GameBoxWriterSettings(GameBox? gbx, IDRemap remap, GameBoxAsyncWriteAction? asyncAction)
    {
        Gbx = gbx;
        Remap = remap;
        AsyncAction = asyncAction;
    }

    public GameBox GetGbxOrThrow()
    {
        return Gbx ?? throw new PropertyNullException(nameof(Gbx));
    }
}
