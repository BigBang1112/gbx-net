namespace GBX.NET;

public abstract class GameBoxPart : ILookbackable
{
    int? ILookbackable.IdVersion { get; set; }
    List<string> ILookbackable.IdStrings { get; set; } = new List<string>();
    bool ILookbackable.IdWritten { get; set; }

    public GameBox GBX { get; }

    public GameBoxPart(GameBox gbx) => GBX = gbx;
}
