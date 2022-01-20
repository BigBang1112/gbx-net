namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlock
{
    public interface IHasTwoKeys
    {
        TimeSpan Start { get; set; }
        TimeSpan End { get; set; }
    }
}
