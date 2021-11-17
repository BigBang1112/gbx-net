namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public class TMSX : GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
    {
        public TMSX(ICGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

        public override CGameCtnMediaTrack Build()
        {
            Node.CreateChunk<CGameCtnMediaTrack.Chunk03078002>();
            Node.CreateChunk<CGameCtnMediaTrack.Chunk03078003>();
            return Node;
        }
    }
}
