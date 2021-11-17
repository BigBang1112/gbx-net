namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    public class TM2 : GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
    {
        public bool IsMusic { get; set; }
        public bool StopWithClip { get; set; }
        public bool AudioToSpeech { get; set; }
        public int AudioToSpeechTarget { get; set; }

        public TM2(ICGameCtnMediaBlockSoundBuilder baseBuilder, CGameCtnMediaBlockSound node) : base(baseBuilder, node) { }

        public TM2 WithMusic(bool music)
        {
            IsMusic = music;
            return this;
        }

        public TM2 StopsWithClip(bool stopWithClip)
        {
            StopWithClip = stopWithClip;
            return this;
        }

        public TM2 WithAudioToSpeech(bool audioToSpeech)
        {
            AudioToSpeech = audioToSpeech;
            return this;
        }

        public TM2 WithAudioToSpeechTarget(int audioToSpeechTarget)
        {
            AudioToSpeechTarget = audioToSpeechTarget;
            return this;
        }

        public override CGameCtnMediaBlockSound Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7003>().Version = 2;
            Node.CreateChunk<CGameCtnMediaBlockSound.Chunk030A7004>();
            Node.IsMusic = IsMusic;
            Node.StopWithClip = StopWithClip;
            Node.AudioToSpeech = AudioToSpeech;
            Node.AudioToSpeechTarget = AudioToSpeechTarget;

            return Node;
        }
    }
}
