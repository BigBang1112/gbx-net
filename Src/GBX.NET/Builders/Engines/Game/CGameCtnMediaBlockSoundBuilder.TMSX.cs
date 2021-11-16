using GBX.NET.Engines.Game;
using System;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TMSX : GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public TMSX(ICGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7001>();
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7002>();
            return Node;
        }
    }
}
