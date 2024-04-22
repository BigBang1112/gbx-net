namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlock
{
    public interface IHasKeys
    {
        IEnumerable<IKey> Keys { get; }
    }

    public interface IHasTwoKeys
    {
        TimeInt32 Start { get; set; }
        TimeInt32 End { get; set; }
    }
}
