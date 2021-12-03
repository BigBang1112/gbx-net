namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockSoundBuilder : ICGameCtnMediaBlockSoundBuilder
{
    public FileRef? Sound { get; set; }
    public IList<CGameCtnMediaBlockSound.Key>? Keys { get; set; }
    public int PlayCount { get; set; } = 1;
    public bool IsLooping { get; set; }

    public CGameCtnMediaBlockSoundBuilder WithSound(FileRef sound)
    {
        Sound = sound;
        return this;
    }

    public CGameCtnMediaBlockSoundBuilder WithKeys(IList<CGameCtnMediaBlockSound.Key> keys)
    {
        Keys = keys;
        return this;
    }

    public CGameCtnMediaBlockSoundBuilder WithKeys(params CGameCtnMediaBlockSound.Key[] keys)
    {
        Keys = keys;
        return this;
    }

    public CGameCtnMediaBlockSoundBuilder WithPlayCount(int playCount)
    {
        PlayCount = playCount;
        return this;
    }

    public CGameCtnMediaBlockSoundBuilder Loops()
    {
        IsLooping = true;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
        IBuilderForTMSX<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>.ForTMSX() => ForTMSX();
    GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
        IBuilderForTMU<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>.ForTMU() => ForTMU();
    GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
        IBuilderForTMUF<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>.ForTMUF() => ForTMUF();
    GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
        IBuilderForTM2<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>.ForTM2() => ForTM2();
    GameBuilder<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>
        IBuilderForTM2020<ICGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>.ForTM2020() => ForTM2020();

    internal CGameCtnMediaBlockSound NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockSound>(0x030A7000);

        node.Sound = Sound ?? new FileRef();
        node.Keys = Keys ?? new List<CGameCtnMediaBlockSound.Key>();
        node.PlayCount = PlayCount;
        node.IsLooping = IsLooping;

        return node;
    }
}
