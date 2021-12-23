using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Control;
using GBX.NET.Engines.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration.Builders.Engines.Control;

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

    private static CControlEffectSimi BuildNode(Func<CControlEffectSimiBuilder,
        GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>> func, CControlEffectSimi.Key[] keys)
    {
        var builder = new CControlEffectSimiBuilder()
            .WithKeys(keys)
            .Centered();
        return func.Invoke(builder).Build();
    }

    private static void ForX_ParametersShouldMatch(Func<CControlEffectSimiBuilder,
        GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>> func)
    {
        var keys = GetSampleKeys();
        var node = BuildNode(func, keys);

        Assert.Equal(expected: keys, actual: node.Keys);
        Assert.True(node.Centered);
    }

    private static void ForX_ChunksShouldMatch(Func<CControlEffectSimiBuilder,
        GameBuilder<CControlEffectSimiBuilder, CControlEffectSimi>> func, Action<CControlEffectSimi> chunkAssert)
    {
        var keys = GetSampleKeys();
        var node = BuildNode(func, keys);
        chunkAssert.Invoke(node);
    }

    [Fact] public void ForTMSX_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTMSX());
    [Fact] public void ForTMU_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTMU());
    [Fact] public void ForTMUF_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTMUF());
    [Fact] public void ForTM2_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTM2());
    [Fact] public void ForTM2020_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTM2020());

    [Fact]
    public void ForTMSX_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMSX(), node =>
        {
            Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010002>());
        });
    }

    [Fact]
    public void ForTMU_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMU(), node =>
        {
            Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010004>());
        });
    }

    [Fact]
    public void ForTMUF_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMUF(), node =>
        {
            Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
        });
    }

    [Fact]
    public void ForTM2_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2(), node =>
        {
            Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
        });
    }

    [Fact]
    public void ForTM2020_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2020(), node =>
        {
            Assert.NotNull(node.GetChunk<CControlEffectSimi.Chunk07010005>());
        });
    }
}
