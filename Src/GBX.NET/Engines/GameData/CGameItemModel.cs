namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
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
