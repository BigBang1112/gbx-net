using GBX.NET.Engines.Control;
using System;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CControlEffectSimiBuilder
    {
        public class TMUF : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        {
            public TMUF(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

            public override CControlEffectSimi Build()
            {
                Node.CreateChunk<CControlEffectSimi.Chunk07010005>();
                return Node;
            }
        }
    }
}
