namespace GBX.NET;

public abstract class GameBoxPart
{
    public GameBox GBX { get; }

    public GameBoxPart(GameBox gbx) => GBX = gbx;
}
