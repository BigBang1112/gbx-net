namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlock
{
    public interface IHasKeys
    {
        IEnumerable<IKey> Keys { get; }
    }

    public interface IHasTwoKeys
    {
        TimeSingle Start { get; set; }
        TimeSingle End { get; set; }
    }
}
