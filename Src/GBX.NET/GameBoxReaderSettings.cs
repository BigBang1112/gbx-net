namespace GBX.NET;

public class GameBoxReaderSettings
{
    /// <summary>
    /// A Gbx temporary kind of state used for reading node references and lookback strings.
    /// </summary>
    public GameBox? Gbx { get; }

    /// <summary>
    /// A delegate collection that gets executed throughout the asynchronous reading.
    /// </summary>
    public GameBoxAsyncReadAction? AsyncAction { get; }

    public GameBoxReaderSettings(GameBox? gbx, GameBoxAsyncReadAction? asyncAction)
    {
        Gbx = gbx;
        AsyncAction = asyncAction;
    }

    public GameBox GetGbxOrThrow()
    {
        return Gbx ?? throw new PropertyNullException(nameof(Gbx));
    }
}
