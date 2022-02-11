namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockGhostBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaBlockGhostBuilder, CGameCtnMediaBlockGhost>
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; } = TimeSpan.FromSeconds(3);

        public TMUF(CGameCtnMediaBlockGhostBuilder baseBuilder, CGameCtnMediaBlockGhost node) : base(baseBuilder, node)
        {

        }

        public TMUF StartingAt(TimeSpan start)
        {
            Start = start;
            return this;
        }

        public TMUF EndingAt(TimeSpan end)
        {
            End = end;
            return this;
        }

        public TMUF WithTimeRange(TimeSpan start, TimeSpan end)
        {
            Start = start;
            End = end;
            return this;
        }

        public override CGameCtnMediaBlockGhost Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockGhost.Chunk030E5001>();
            Node.Start = Start;
            Node.End = End;

            return Node;
        }
    }
}
