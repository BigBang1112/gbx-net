namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_InputBindingsConfig
{

    public partial CInputBindingsConfig? Config { get => configRef.Resolve(); set => configRef.SetEmbedded(value); }
}
