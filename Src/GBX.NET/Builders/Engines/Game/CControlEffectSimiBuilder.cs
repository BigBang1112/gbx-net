using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CControlEffectSimiBuilder : ICControlEffectSimiBuilder
    {
        public IList<CControlEffectSimi.Key>? Keys { get; set; }

        public CControlEffectSimiBuilder WithKeys(IList<CControlEffectSimi.Key> keys)
        {
            Keys = keys;
            return this;
        }

        public CControlEffectSimiBuilder WithKeys(params CControlEffectSimi.Key[] keys)
        {
            Keys = keys;
            return this;
        }

        public TMSX ForTMSX() => new(this, NewNode());
        public TMUF ForTMUF() => new(this, NewNode());
        public TM2 ForTM2() => new(this, NewNode());

        private CControlEffectSimi NewNode()
        {
            return NodeCacheManager.GetNodeInstance<CControlEffectSimi>(0x07010000);
        }
    }
}
