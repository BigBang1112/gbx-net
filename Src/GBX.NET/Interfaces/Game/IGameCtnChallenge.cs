﻿namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallenge : IClassVersion<CGameCtnChallenge>
{
    string MapUid { get; set; }
    IList<CGameCtnBlock> Blocks { get; set; }

    string GetEnvironment();
    void PlaceBlock(CGameCtnBlock block);
    void RemoveAllBlocks();
    bool RemoveBlock(CGameCtnBlock block);
}
