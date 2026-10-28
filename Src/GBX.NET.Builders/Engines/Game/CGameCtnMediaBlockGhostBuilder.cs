namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockGhostBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public partial class TMF
    {
        public TMF StartingAt(TimeSingle value) => WithStart(value);
        public TMF EndingAt(TimeSingle value) => WithEnd(value);
        public TMF WithTimeRange(TimeSingle start, TimeSingle end) => WithStart(start).WithEnd(end);
    }
}
