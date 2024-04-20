namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecoration
{
    public override IHeaderChunk? CreateHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            var chunk = new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
            Chunks.Add(chunk);
            return chunk;
        }

        return base.CreateHeaderChunk(chunkId);
    }
}
