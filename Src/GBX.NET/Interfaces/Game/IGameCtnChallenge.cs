namespace GBX.NET.Interfaces.Game;

public partial interface IGameCtnChallenge : IClassVersion<CGameCtnChallenge>
{
    string MapUid { get; set; }
    [Obsolete("Use GetBlocks() on the selected game interface for scoped block access.")]
    NodeRefList<CGameCtnBlock> Blocks { get; set; }

    string GetEnvironment();
    void PlaceBlock(CGameCtnBlock block);
    void RemoveAllBlocks();
    bool RemoveBlock(CGameCtnBlock block);
}
