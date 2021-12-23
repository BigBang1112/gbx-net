namespace GBX.NET.Builders.Engines.Control;

public partial class CControlEffectSimiBuilder : Builder
{
    public IList<CControlEffectSimi.Key>? Keys { get; set; }
    public bool IsCentered { get; set; }

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

    public CControlEffectSimiBuilder Centered()
    {
        IsCentered = true;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    internal CControlEffectSimi NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CControlEffectSimi>(0x07010000);
        node.Keys = Keys ?? new List<CControlEffectSimi.Key>();
        node.Centered = IsCentered;
        return node;
    }
}
