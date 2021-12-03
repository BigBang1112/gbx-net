using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Control;
using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Unit.Builders.Engines.Control;

public class CControlEffectSimiBuilderTests
{
    public static CControlEffectSimi.Key[] GetSampleKeys() => new CControlEffectSimi.Key[]
    {
        new()
        {
            Time = TimeSpan.Zero,
            Position = (0.5f, 0.5f),
            Rotation = 0.2f,
            Scale = (0.9f, 1f),
            Opacity = 1f,
            Depth = 0.5f
        },
        new()
        {
            Time = TimeSpan.FromSeconds(3),
            Position = (0.6f, 0.8f),
            Rotation = 0.5f,
            Scale = (1.1f, 1.5f),
            Opacity = 0.5f,
            Depth = 0.2f
        }
    };

    [Fact]
    public void WithKeys_Params_ShouldSetKeys()
    {
        var expected = GetSampleKeys();

        var builder = new CControlEffectSimiBuilder()
            .WithKeys(expected);

        Assert.Equal(expected, actual: builder.Keys);
    }

    [Fact]
    public void WithKeys_List_ShouldSetKeys()
    {
        var expected = GetSampleKeys().ToList();

        var builder = new CControlEffectSimiBuilder()
            .WithKeys(expected);

        Assert.Equal(expected, actual: builder.Keys);
    }

    [Fact]
    public void Centered_ShouldSetIsCentered()
    {
        var builder = new CControlEffectSimiBuilder()
            .Centered();

        Assert.True(builder.IsCentered);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = NodeCacheManager.GetNodeInstance<CControlEffectSimi>(0x07010000);

        var actual = new CControlEffectSimiBuilder().NewNode();

        Assert.Equal(expected.ID, actual.ID);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedKeys = GetSampleKeys();

        var node = new CControlEffectSimiBuilder { Keys = expectedKeys, IsCentered = true }
            .NewNode();

        Assert.Equal(expectedKeys, actual: node.Keys);
        Assert.True(node.Centered);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = new CControlEffectSimiBuilder().NewNode();

        Assert.NotNull(node.Keys);
        Assert.Empty(node.Keys);
    }

    [Fact]
    public void TMSX_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder()
            .ForTMSX()
            .Build();

        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010002>());
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder()
            .ForTMU()
            .WithColorBlendMode(1)
            .ContinousEffect()
            .Build();

        Assert.Equal(expected: 1, actual: node.ColorBlendMode);
        Assert.True(node.IsContinousEffect);
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010004>());
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder()
            .ForTMUF()
            .WithColorBlendMode(1)
            .ContinousEffect()
            .Interpolated()
            .Build();

        Assert.Equal(expected: 1, actual: node.ColorBlendMode);
        Assert.True(node.IsContinousEffect);
        Assert.True(node.IsInterpolated);
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder()
            .ForTM2()
            .WithColorBlendMode(1)
            .ContinousEffect()
            .Interpolated()
            .Build();

        Assert.Equal(expected: 1, actual: node.ColorBlendMode);
        Assert.True(node.IsContinousEffect);
        Assert.True(node.IsInterpolated);
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
    }

    [Fact]
    public void TM2020_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder()
            .ForTM2020()
            .WithColorBlendMode(1)
            .ContinousEffect()
            .Interpolated()
            .Build();

        Assert.Equal(expected: 1, actual: node.ColorBlendMode);
        Assert.True(node.IsContinousEffect);
        Assert.True(node.IsInterpolated);
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
    }

    [Fact]
    public void TMU_WithColorBlendMode_ShouldSetColorBlendMode()
    {
        var expected = 1;

        var builder = new CControlEffectSimiBuilder()
            .ForTMU()
            .WithColorBlendMode(expected);

        Assert.Equal(expected, actual: builder.ColorBlendMode);
    }

    [Fact]
    public void TMU_ContinousEffect_ShouldSetIsContinousEffect()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTMU()
            .ContinousEffect();

        Assert.True(builder.IsContinousEffect);
    }

    [Fact]
    public void TMUF_WithColorBlendMode_ShouldSetColorBlendMode()
    {
        var expected = 1;

        var builder = new CControlEffectSimiBuilder()
            .ForTMUF()
            .WithColorBlendMode(expected);

        Assert.Equal(expected, actual: builder.ColorBlendMode);
    }

    [Fact]
    public void TMUF_ContinousEffect_ShouldSetIsContinousEffect()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTMUF()
            .ContinousEffect();

        Assert.True(builder.IsContinousEffect);
    }

    [Fact]
    public void TMUF_Interpolated_ShouldSetIsInterpolated()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTMUF()
            .Interpolated();

        Assert.True(builder.IsInterpolated);
    }

    [Fact]
    public void TM2_WithColorBlendMode_ShouldSetColorBlendMode()
    {
        var expected = 1;

        var builder = new CControlEffectSimiBuilder()
            .ForTM2()
            .WithColorBlendMode(expected);

        Assert.Equal(expected, actual: builder.ColorBlendMode);
    }

    [Fact]
    public void TM2_ContinousEffect_ShouldSetIsContinousEffect()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTM2()
            .ContinousEffect();

        Assert.True(builder.IsContinousEffect);
    }

    [Fact]
    public void TM2_Interpolated_ShouldSetIsInterpolated()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTM2()
            .Interpolated();

        Assert.True(builder.IsInterpolated);
    }

    [Fact]
    public void TM2020_WithColorBlendMode_ShouldSetColorBlendMode()
    {
        var expected = 1;

        var builder = new CControlEffectSimiBuilder()
            .ForTM2020()
            .WithColorBlendMode(expected);

        Assert.Equal(expected, actual: builder.ColorBlendMode);
    }

    [Fact]
    public void TM2020_ContinousEffect_ShouldSetIsContinousEffect()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTM2020()
            .ContinousEffect();

        Assert.True(builder.IsContinousEffect);
    }

    [Fact]
    public void TM2020_Interpolated_ShouldSetIsInterpolated()
    {
        var builder = new CControlEffectSimiBuilder()
            .ForTM2020()
            .Interpolated();

        Assert.True(builder.IsInterpolated);
    }
}
