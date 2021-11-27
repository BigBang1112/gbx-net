namespace GBX.NET.Builders.Engines.Control;

public interface ICControlEffectSimiBuilder : IBuilder, ICControlEffectSimiBuilderFor
{
    IList<CControlEffectSimi.Key>? Keys { get; set; }

    CControlEffectSimiBuilder WithKeys(IList<CControlEffectSimi.Key> keys);
    CControlEffectSimiBuilder WithKeys(params CControlEffectSimi.Key[] keys);
}
