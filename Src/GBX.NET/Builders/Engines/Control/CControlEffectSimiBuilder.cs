namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder : ICControlEffectSimiBuilder
{
    public IList<CControlEffectSimi.Key>? Keys { get; set; }

    public CControlEffectSimiBuilder WithKeys(IList<CControlEffectSimi.Key> keys)
    {
        Keys = keys;
        return this;
    }

    public CControlEffectSimiBuilder WithKeys(params CControlEffectSimi.Key[] keys)
    {
        Keys = keys;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        IBuilderForTMSX<ICControlEffectSimiBuilder, CControlEffectSimi>.ForTMSX() => ForTMSX();
    GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        IBuilderForTMU<ICControlEffectSimiBuilder, CControlEffectSimi>.ForTMU() => ForTMU();
    GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        IBuilderForTMUF<ICControlEffectSimiBuilder, CControlEffectSimi>.ForTMUF() => ForTMUF();
    GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        IBuilderForTM2<ICControlEffectSimiBuilder, CControlEffectSimi>.ForTM2() => ForTM2();
    GameBuilder<ICControlEffectSimiBuilder, CControlEffectSimi>
        IBuilderForTM2020<ICControlEffectSimiBuilder, CControlEffectSimi>.ForTM2020() => ForTM2020();

    internal CControlEffectSimi NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CControlEffectSimi>(0x07010000);
        node.Keys = Keys ?? new List<CControlEffectSimi.Key>();
        return node;
    }
}
