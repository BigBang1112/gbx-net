namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockTime
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        public float TimeValue { get; set; }
        public float Tangent { get; set; }
    }
}
