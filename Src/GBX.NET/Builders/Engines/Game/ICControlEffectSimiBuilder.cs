using GBX.NET.Engines.Control;
using System.Collections.Generic;

namespace GBX.NET.Builders.Engines.Game
{
    public interface ICControlEffectSimiBuilder : IBuilder
    {
        IList<CControlEffectSimi.Key>? Keys { get; set; }

        CControlEffectSimiBuilder WithKeys(IList<CControlEffectSimi.Key> keys);
        CControlEffectSimiBuilder WithKeys(params CControlEffectSimi.Key[] keys);
    }
}