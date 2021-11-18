namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaTrackBuilderFor :
    IBuilderForTMSX<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>,
    IBuilderForTMU<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>,
    IBuilderForTMUF<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>,
    IBuilderForTM2<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>,
    IBuilderForTM2020<ICGameCtnMediaTrackBuilder, CGameCtnMediaTrack>
{

}
