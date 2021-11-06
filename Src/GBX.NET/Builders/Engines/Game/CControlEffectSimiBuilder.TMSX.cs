using GBX.NET.Engines.Control;
using System;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CControlEffectSimiBuilder
    {
        public class TMSX : GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        {
            public TMSX(ICControlEffectSimiBuilder baseBuilder, CControlEffectSimi node) : base(baseBuilder, node) { }

            public override CControlEffectSimi Build()
            {
                Node.CreateChunk<CControlEffectSimi.Chunk07010002>();
                return Node;
            }
        }
    }
}
