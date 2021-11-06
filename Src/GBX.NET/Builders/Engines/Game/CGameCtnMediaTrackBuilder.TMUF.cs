using GBX.NET.Engines.Game;

namespace GBX.NET.Builders.Engines.Game
{
    public partial class CGameCtnMediaTrackBuilder
    {
        public class TMUF : GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
        {
            public TMUF(ICGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

            public override CGameCtnMediaTrack Build()
            {
                Node.CreateChunk<CGameCtnMediaTrack.Chunk03078004>();
                return Node;
            }
        }
    }
}
