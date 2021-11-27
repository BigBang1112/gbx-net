namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaClipBuilderFor :
    IBuilderForTMSX<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>,
    IBuilderForTMU<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>,
    IBuilderForTMUF<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>,
    IBuilderForTM2<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>,
    IBuilderForTM2020<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
{
}