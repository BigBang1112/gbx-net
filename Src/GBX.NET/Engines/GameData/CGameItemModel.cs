namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    public CGameItemModel()
    {
        NadeoSkinsFids = new CMwNod?[7];
        DefaultPlacement = new();
    }

    internal override IHeaderChunk? NewHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            return new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
        }

        return base.NewHeaderChunk(chunkId);
    }
}
