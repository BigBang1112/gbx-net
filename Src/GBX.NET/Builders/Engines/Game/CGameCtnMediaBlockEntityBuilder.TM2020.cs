namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockEntityBuilder
{
    public class TM2020 : GameBuilder<CGameCtnMediaBlockEntityBuilder, CGameCtnMediaBlockEntity>
    {
        public IList<CGameCtnMediaBlockEntity.Key>? Keys { get; set; }
        public Ident? PlayerModel { get; set; }

        public TM2020(CGameCtnMediaBlockEntityBuilder baseBuilder, CGameCtnMediaBlockEntity node) : base(baseBuilder, node)
        {
        }

        public TM2020 WithKeys(IList<CGameCtnMediaBlockEntity.Key> keys)
        {
            Keys = keys;
            return this;
        }

        public TM2020 WithKeys(params CGameCtnMediaBlockEntity.Key[] keys)
        {
            Keys = keys;
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

            chunk.Version = 6;

            Node.PlayerModel = PlayerModel ?? ("CarSport", 10003, "Nadeo");
            Node.Keys = Keys ?? new List<CGameCtnMediaBlockEntity.Key>();

            return Node;
        }
    }
}