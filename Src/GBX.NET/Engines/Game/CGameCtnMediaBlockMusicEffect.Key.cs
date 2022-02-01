namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockMusicEffect
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float musicVolume;
        private float soundVolume;

        public float MusicVolume { get => musicVolume; set => musicVolume = value; }
        public float SoundVolume { get => soundVolume; set => soundVolume = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref musicVolume);

            if (version >= 1)
            {
                rw.Single(ref soundVolume);
            }
        }
    }
}
