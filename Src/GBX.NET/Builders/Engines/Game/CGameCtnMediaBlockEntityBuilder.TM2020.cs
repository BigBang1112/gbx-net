namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockEntityBuilder
{
    public class TM2020 : GameBuilder<CGameCtnMediaBlockEntityBuilder, CGameCtnMediaBlockEntity>
    {
        public TimeSingle Start { get; set; }
        public TimeSingle End { get; set; } = TimeSingle.FromSeconds(3);
        public Ident? PlayerModel { get; set; }

        public TM2020(CGameCtnMediaBlockEntityBuilder baseBuilder, CGameCtnMediaBlockEntity node) : base(baseBuilder, node)
        {
        }

        public TM2020 StartingAt(TimeSingle start)
        {
            Start = start;
            return this;
        }

        public TM2020 EndingAt(TimeSingle end)
        {
            End = end;
            return this;
        }

        public TM2020 WithTimeRange(TimeSingle start, TimeSingle end)
        {
            Start = start;
            End = end;
            return this;
        }

        public TM2020 WithPlayerModel(Ident playerModel)
        {
            PlayerModel = playerModel;
            return this;
        }

        public override CGameCtnMediaBlockEntity Build()
        {
            if (!Node.TryGetChunk<CGameCtnMediaBlockEntity.Chunk0329F000>(out var chunk))
            {
                throw new Exception("Chunk 0x0329F000 is missing.");
            }

            chunk.Version = 3;

            Node.PlayerModel = PlayerModel ?? ("CarSport", 10003, "Nadeo");
            Node.Start = Start;
            Node.End = End;

            return Node;
        }
    }
}