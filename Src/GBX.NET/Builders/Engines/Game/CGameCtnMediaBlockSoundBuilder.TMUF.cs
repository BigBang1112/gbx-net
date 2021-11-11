using GBX.NET.Engines.Game;
using System;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TMUF : GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public TMUF(ICGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7003>();
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7004>();
            return Node;
        }
    }
}
