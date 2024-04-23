namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockText : CGameCtnMediaBlock.IHasKeys
{
    IEnumerable<IKey> IHasKeys.Keys => Effect?.Keys ?? [];
}
