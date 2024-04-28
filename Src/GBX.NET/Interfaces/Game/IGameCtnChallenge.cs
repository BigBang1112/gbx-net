namespace GBX.NET.Interfaces.Game;

public interface IGameCtnChallenge : IClassVersion<CGameCtnChallenge>
{
    string MapUid { get; set; }
    IList<CGameCtnBlock> Blocks { get; set; }
}
