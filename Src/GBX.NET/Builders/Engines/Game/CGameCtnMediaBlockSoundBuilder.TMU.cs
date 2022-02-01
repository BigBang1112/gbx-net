namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TMU : GameBuilder<CGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public TMU(CGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7001>();
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7002>();
            return Node;
        }
    }
}
