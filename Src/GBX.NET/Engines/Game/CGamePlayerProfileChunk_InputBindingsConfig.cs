namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_InputBindingsConfig
{
    private CInputBindingsConfig? config;
    [AppliedWithChunk<Chunk0312F000>]
    public CInputBindingsConfig? Config { get => config; set => config = value; }
}
