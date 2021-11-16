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
            X = 0.5f,
            Y = 0.5f,
            Rotation = 0.2f,
            ScaleX = 0.9f,
            ScaleY = 1f,
            Opacity = 1f,
            Depth = 0.5f
        },
        new()
        {
            Time = TimeSpan.FromSeconds(3),
            X = 0.6f,
            Y = 0.8f,
            Rotation = 0.5f,
            ScaleX = 1.1f,
            ScaleY = 1.5f,
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
    public void NewNode_ShouldReturnInstance()
    {
        var expected = NodeCacheManager.GetNodeInstance<CControlEffectSimi>(0x07010000);

        var actual = new CControlEffectSimiBuilder().NewNode();

        Assert.Equal(expected.ID, actual.ID);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expected = GetSampleKeys();

        var node = new CControlEffectSimiBuilder { Keys = expected }
            .NewNode();

        Assert.Equal(expected, actual: node.Keys);
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
        var node = new CControlEffectSimiBuilder().ForTMSX().Build();
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010002>());
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder().ForTMU().Build();
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010004>());
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder().ForTMUF().Build();
        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CControlEffectSimiBuilder().ForTM2().Build();

        Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
    }
}
