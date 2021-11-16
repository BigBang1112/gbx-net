using GBX.NET.Engines.Game;

namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaBlockSoundBuilderFor :
    IBuilderForTMSX<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>,
    IBuilderForTMU<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>,
    IBuilderForTMUF<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>,
    IBuilderForTM2<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
{

}
