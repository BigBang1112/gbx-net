namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public CGameCtnMediaTrackBuilder KeepsPlaying() => WithIsKeepPlaying(true);
    public CGameCtnMediaTrackBuilder ReadOnly() => WithIsReadOnly(true);

    public partial class MP4
    {
        public MP4 Cycles() => WithIsCycling(true);
    }

    public partial class TM2020
    {
        public TM2020 Cycles() => WithIsCycling(true);
    }
}
