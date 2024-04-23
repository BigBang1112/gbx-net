namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockImage : CGameCtnMediaBlock.IHasKeys
{
    IEnumerable<IKey> IHasKeys.Keys => Effect?.Keys ?? [];
}
