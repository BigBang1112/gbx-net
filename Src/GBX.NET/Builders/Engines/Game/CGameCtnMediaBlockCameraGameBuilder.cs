namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockCameraGameBuilder
{
    [Obsolete("Use ForTMF() instead.")]
    public TMF ForTMUF() => ForTMF();

    public CGameCtnMediaBlockCameraGameBuilder StartingAt(TimeSingle value) => WithStart(value);
    public CGameCtnMediaBlockCameraGameBuilder EndingAt(TimeSingle value) => WithEnd(value);
    public CGameCtnMediaBlockCameraGameBuilder WithTimeRange(TimeSingle start, TimeSingle end) => WithStart(start).WithEnd(end);
}
