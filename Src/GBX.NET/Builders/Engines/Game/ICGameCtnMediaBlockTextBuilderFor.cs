using GBX.NET.Engines.Game;

namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaBlockTextBuilderFor :
    IBuilderForTMSX<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>,
    IBuilderForTMU<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>,
    IBuilderForTMUF<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>,
    IBuilderForTM2<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
{

}
