namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
    {
        public TMUF(CGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

        public override CGameCtnMediaTrack Build()
        {
            Node.CreateChunk<CGameCtnMediaTrack.Chunk03078004>();
            return Node;
        }
    }
}
