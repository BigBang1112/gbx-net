using GBX.NET.Engines.Control;

namespace GBX.NET.Builders.Engines.Game
{
    public interface ICGameCtnMediaBlockTextBuilder : IBuilder
    {
        CControlEffectSimi? Effect { get; set; }
        string? Text { get; set; }

        CGameCtnMediaBlockTextBuilder WithEffect(CControlEffectSimi effect);
        CGameCtnMediaBlockTextBuilder WithText(string text);
    }
}