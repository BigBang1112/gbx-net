namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_InputBindingsConfig
{

    public partial CInputBindingsConfig? Config { get => configRef?.Resolve(); set => NodeRef.SetEmbedded(ref configRef, value); }
}
