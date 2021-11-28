namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder : ICGameCtnMediaBlockTextBuilder
{
    public string? Text { get; set; }
    public CControlEffectSimi? Effect { get; set; }
    public Vec3 Color { get; set; }

    public CGameCtnMediaBlockTextBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    public CGameCtnMediaBlockTextBuilder WithEffect(CControlEffectSimi effect)
    {
        Effect = effect;
        return this;
    }

    public CGameCtnMediaBlockTextBuilder WithColor(Vec3 color)
    {
        Color = color;
        return this;
    }

    public TMSX ForTMSX() => new(this, NewNode());
    public TMU ForTMU() => new(this, NewNode());
    public TMUF ForTMUF() => new(this, NewNode());
    public TM2 ForTM2() => new(this, NewNode());
    public TM2020 ForTM2020() => new(this, NewNode());

    GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        IBuilderForTMSX<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>.ForTMSX() => ForTMSX();
    GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        IBuilderForTMU<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>.ForTMU() => ForTMU();
    GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        IBuilderForTMUF<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>.ForTMUF() => ForTMUF();
    GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        IBuilderForTM2<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>.ForTM2() => ForTM2();
    GameBuilder<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>
        IBuilderForTM2020<ICGameCtnMediaBlockTextBuilder, CGameCtnMediaBlockText>.ForTM2020() => ForTM2020();

    internal CGameCtnMediaBlockText NewNode()
    {
        var node = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockText>(0x030A8000);
        node.Text = Text ?? string.Empty;
        node.Effect = Effect!;
        node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8001>();
        node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8002>();
        return node;
    }
}
