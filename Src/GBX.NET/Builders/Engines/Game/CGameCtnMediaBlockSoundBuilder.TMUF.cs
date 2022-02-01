namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public bool IsMusic { get; set; }

        public TMUF(CGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public TMUF WithMusic(bool music)
        {
            IsMusic = music;
            return this;
        }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7003>();
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7004>();
            Node.IsMusic = IsMusic;
            return Node;
        }
    }
}
