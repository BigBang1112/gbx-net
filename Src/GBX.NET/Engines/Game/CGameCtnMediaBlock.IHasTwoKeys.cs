namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlock
{
    public interface IHasTwoKeys
    {
        TimeSingle Start { get; set; }
        TimeSingle End { get; set; }
    }
}
