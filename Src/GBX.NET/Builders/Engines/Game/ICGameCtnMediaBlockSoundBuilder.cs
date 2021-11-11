using GBX.NET.Engines.Game;
using System.Collections.Generic;

namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaBlockSoundBuilder : IBuilder
{
    IList<CGameCtnMediaBlockSound.Key>? Keys { get; set; }
    FileRef? Sound { get; set; }

    CGameCtnMediaBlockSoundBuilder WithKeys(IList<CGameCtnMediaBlockSound.Key> keys);
    CGameCtnMediaBlockSoundBuilder WithKeys(params CGameCtnMediaBlockSound.Key[] keys);
    CGameCtnMediaBlockSoundBuilder WithSound(FileRef sound);
}
