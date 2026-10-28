namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfo
{
    internal override IHeaderChunk? NewHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            return new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
        }

        return base.NewHeaderChunk(chunkId);
    }
}
