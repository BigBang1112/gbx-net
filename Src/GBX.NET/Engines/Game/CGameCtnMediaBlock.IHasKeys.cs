namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlock
{
    public interface IHasKeys
    {
        IEnumerable<Key> Keys { get; set; }
    }
}
