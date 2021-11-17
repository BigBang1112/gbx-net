namespace GBX.NET.Builders.Engines.Game;

public interface ICGameCtnMediaBlockSoundBuilder : IBuilder, ICGameCtnMediaBlockSoundBuilderFor
{
    IList<CGameCtnMediaBlockSound.Key>? Keys { get; set; }
    FileRef? Sound { get; set; }
    int PlayCount { get; set; }
    bool IsLooping { get; set; }

    CGameCtnMediaBlockSoundBuilder WithKeys(IList<CGameCtnMediaBlockSound.Key> keys);
    CGameCtnMediaBlockSoundBuilder WithKeys(params CGameCtnMediaBlockSound.Key[] keys);
    CGameCtnMediaBlockSoundBuilder WithSound(FileRef sound);
    CGameCtnMediaBlockSoundBuilder WithPlayCount(int playCount);
    CGameCtnMediaBlockSoundBuilder WithLooping(bool loop);
}
