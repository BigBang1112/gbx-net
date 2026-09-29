namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallengeParameters
{
    private CGameCtnGhost? raceValidateGhost;
    [AppliedWithChunk<Chunk0305B00D>]
    [AppliedWithChunk<Chunk0305B00F>]
    public CGameCtnGhost? RaceValidateGhost { get => raceValidateGhost; set => raceValidateGhost = value; }
}
