namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockGhostBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaBlockGhostBuilder, CGameCtnMediaBlockGhost>
    {
        public TimeSingle Start { get; set; }
        public TimeSingle End { get; set; } = TimeSingle.FromSeconds(3);

        public TMUF(CGameCtnMediaBlockGhostBuilder baseBuilder, CGameCtnMediaBlockGhost node) : base(baseBuilder, node)
        {

        }

        public TMUF StartingAt(TimeSingle start)
        {
            Start = start;
            return this;
        }

        public TMUF EndingAt(TimeSingle end)
        {
            End = end;
            return this;
        }

        public TMUF WithTimeRange(TimeSingle start, TimeSingle end)
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
