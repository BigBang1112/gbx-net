namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public partial class MP4
    {
        public MP4 StopsWhenRespawn() => WithStopWhenRespawn(true);
        public MP4 StopsWhenLeave() => WithStopWhenLeave(true);
    }

    public partial class TM2020
    {
        public TM2020 StopsWhenRespawn() => WithStopWhenRespawn(true);
        public TM2020 StopsWhenLeave() => WithStopWhenLeave(true);
    }
}
