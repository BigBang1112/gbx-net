namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public CGameCtnMediaBlockSoundBuilder Loops() => WithIsLooping(true);

    public partial class TMF
    {
        public TMF WithMusic(bool value) => WithIsMusic(value);
    }

    public partial class MP4
    {
        public MP4 WithMusic(bool value) => WithIsMusic(value);
        public MP4 StopsWithClip(bool value) => WithStopWithClip(value);
    }

    public partial class TM2020
    {
        public TM2020 WithMusic(bool value) => WithIsMusic(value);
        public TM2020 StopsWithClip(bool value) => WithStopWithClip(value);
    }
}
