namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public class TM2 : GameBuilder<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
    {
        public bool IsCycling { get; set; }

        public TM2(ICGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

        public TM2 Cycles()
        {
            IsCycling = true;
            return this;
        }

        public override CGameCtnMediaTrack Build()
        {
            Node.CreateChunk<CGameCtnMediaTrack.Chunk03078005>().Version = 1;
            Node.IsCycling = IsCycling;
            return Node;
        }
    }
}
