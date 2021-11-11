using GBX.NET.Engines.Game;
using System;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TM2 : GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public TM2(ICGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7003>().Version = 2;
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7004>();
            return Node;
        }
    }
}
