using GBX.NET.Engines.Game;

namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public class TM2 : GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
    {
        public TM2(ICGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

        public override CGameCtnMediaTrack Build()
        {
            Node.CreateChunk<CGameCtnMediaTrack.Chunk03078005>().Version = 1;
            return Node;
        }
    }
}
