namespace GBX.NET;

public class GameBoxReadProgress
{
    private readonly GameBoxHeaderInfo? headerInfo;

    /// <summary>
    /// Reading stage of GBX.
    /// </summary>
    public GameBoxReadProgressStage Stage { get; }
    /// <summary>
    /// Progress in percentage of each stage.
    /// </summary>
    public float Percentage { get; }
    /// <summary>
    /// Gradually updated GBX object.
    /// </summary>
    public GameBox? GBX { get; }
    /// <summary>
    /// Chunk that has been currently read. This is null in <see cref="GameBoxReadProgressStage.Header"/> and <see cref="GameBoxReadProgressStage.RefTable"/> stages.
    /// </summary>
    public Chunk? Chunk { get; }

    public GameBoxHeaderInfo? HeaderInfo
    {
        get => headerInfo ?? GBX?.Header;
    }

    public GameBoxReadProgress()
    {

    }

    public GameBoxReadProgress(GameBoxHeaderInfo headerInfo)
    {
        this.headerInfo = headerInfo;

        Stage = GameBoxReadProgressStage.Header;
        Percentage = 1;
    }

    public GameBoxReadProgress(GameBoxReadProgressStage stage, float percentage, GameBox? gbx)
        : this(stage, percentage, gbx, null) { }

    public GameBoxReadProgress(GameBoxReadProgressStage stage, float percentage, GameBox? gbx, Chunk? chunk)
    {
        Stage = stage;
        Percentage = percentage;
        GBX = gbx;
        Chunk = chunk;
    }
}
