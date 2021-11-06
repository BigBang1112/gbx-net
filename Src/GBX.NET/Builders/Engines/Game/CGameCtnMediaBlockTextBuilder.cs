using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CGameCtnMediaBlockTextBuilder : ICGameCtnMediaBlockTextBuilder
    {
        public string? Text { get; set; }
        public CControlEffectSimi? Effect { get; set; }

        public CGameCtnMediaBlockTextBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public CGameCtnMediaBlockTextBuilder WithEffect(CControlEffectSimi effect)
        {
            Effect = effect;
            return this;
        }

        public TMSX ForTMSX() => new(this, NewNode());
        public TMUF ForTMUF() => new(this, NewNode());
        public TM2 ForTM2() => new(this, NewNode());

        private CGameCtnMediaBlockText NewNode()
        {
            var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockText>(0x030A8000);
            node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8001>();
            node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8002>();
            return node;
        }
    }
}
