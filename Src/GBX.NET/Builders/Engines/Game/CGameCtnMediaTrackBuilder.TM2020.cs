namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public class TM2020 : GameBuilder<CGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
    {
        public bool IsCycling { get; set; }

        public TM2020(CGameCtnMediaTrackBuilder baseBuilder, CGameCtnMediaTrack node) : base(baseBuilder, node) { }

        public TM2020 Cycles()
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
