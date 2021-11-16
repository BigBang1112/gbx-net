using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Tests.Unit.Builders.Engines.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Unit.Builders.Engines.Game;

public class CGameCtnMediaBlockTextBuilderTests
{
    public static string GetSampleText() => "Random Text";
    public static CControlEffectSimi.Key[] GetSampleKeys() => CControlEffectSimiBuilderTests.GetSampleKeys();
    public static CControlEffectSimi GetSampleEffect() => CControlEffectSimi.Create()
        .WithKeys(GetSampleKeys())
        .ForTM2()
        .Build();

    [Fact]
    public void WithText_ShouldSetText()
    {
        var expected = GetSampleText();

        var builder = new CGameCtnMediaBlockTextBuilder()
            .WithText(expected);

        Assert.Equal(expected, actual: builder.Text);
    }

    [Fact]
    public void WithEffect_ShouldSetEffect()
    {
        var expected = GetSampleEffect();

        var builder = new CGameCtnMediaBlockTextBuilder()
             .WithEffect(expected);

        Assert.Equal(expected, actual: builder.Effect);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockText>(0x030A8000);

        var actual = new CGameCtnMediaBlockTextBuilder().NewNode();

        Assert.Equal(expected.ID, actual.ID);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedText = GetSampleText();
        var expectedEffect = GetSampleEffect();

        var node = new CGameCtnMediaBlockTextBuilder { Text = expectedText, Effect = expectedEffect }
            .NewNode();

        Assert.Equal(expectedText, actual: node.Text);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = new CGameCtnMediaBlockTextBuilder().NewNode();

        Assert.NotNull(node.Text);
    }

    [Fact]
    public void TMSX_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockTextBuilder().ForTMSX().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8002>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8003>());
        Assert.NotNull(node.Effect);
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockTextBuilder().ForTMU().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8002>());
        Assert.NotNull(node.Effect);
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockTextBuilder().ForTMUF().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8002>());
        Assert.NotNull(node.Effect);
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockTextBuilder().ForTM2().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockText.Chunk030A8002>());
        Assert.NotNull(node.Effect);
    }
}
