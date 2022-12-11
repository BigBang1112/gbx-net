namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTextBuilder : Builder
{
    public string? Text { get; set; }
    public CControlEffectSimi Effect { get; }
    public Vec3 Color { get; set; }

    public CGameCtnMediaBlockTextBuilder(CControlEffectSimi effect)
    {
        Effect = effect;
    }

    public CGameCtnMediaBlockTextBuilder WithText(string text)
    {
        Text = text;
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

    internal CGameCtnMediaBlockText NewNode()
    {
        var node = new CGameCtnMediaBlockText
        {
            Text = Text ?? string.Empty,
            Effect = Effect,
            Color = Color
        };
        
        node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8001>();
        node.CreateChunk<CGameCtnMediaBlockText.Chunk030A8002>();
        
        return node;
    }
}
