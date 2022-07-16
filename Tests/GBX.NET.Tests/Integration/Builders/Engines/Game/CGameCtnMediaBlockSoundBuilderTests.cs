using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Game;
using System;
using Xunit;

namespace GBX.NET.Tests.Integration.Builders.Engines.Game;

public class CGameCtnMediaBlockSoundBuilderTests
{
    public static FileRef GetSampleSound() => new();
    public static CGameCtnMediaBlockSound.Key[] GetSampleKeys() => new CGameCtnMediaBlockSound.Key[]
    {
        new()
        {
            Time = TimeSpan.Zero,
            Volume = 0
        },
        new()
        {
            Time = TimeSpan.FromSeconds(3),
            Volume = 1
        },
        new()
        {
            Time = TimeSpan.FromSeconds(5),
            Volume = 0.5f
        }
    };

    private static CGameCtnMediaBlockSound BuildNode(Func<CGameCtnMediaBlockSoundBuilder,
        GameBuilder<CGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>> func, FileRef sound, CGameCtnMediaBlockSound.Key[] keys)
    {
        var builder = new CGameCtnMediaBlockSoundBuilder()
            .WithSound(sound)
            .WithKeys(keys)
            .WithPlayCount(5)
            .Loops();
        return func.Invoke(builder).Build();
    }

    private static void ForX_ParametersShouldMatch(Func<CGameCtnMediaBlockSoundBuilder,
        GameBuilder<CGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>> func)
    {
        var sound = GetSampleSound();
        var keys = GetSampleKeys();

        var node = BuildNode(func, sound, keys);

        Assert.Equal(expected: sound, actual: node.Sound);
        Assert.Equal(expected: keys, actual: node.Keys);
    }

    private static void ForX_ChunksShouldMatch(Func<CGameCtnMediaBlockSoundBuilder,
        GameBuilder<CGameCtnMediaBlockSoundBuilder, CGameCtnMediaBlockSound>> func, Action<CGameCtnMediaBlockSound> chunkAssert)
    {
        var sound = GetSampleSound();
        var keys = GetSampleKeys();

        var node = BuildNode(func, sound, keys);
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
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7002>());
        });
    }

    [Fact]
    public void ForTMU_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMU(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7002>());
        });
    }

    [Fact]
    public void ForTMUF_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMUF(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
        });
    }

    [Fact]
    public void ForTM2_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
        });
    }

    [Fact]
    public void ForTM2020_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2020(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
        });
    }
}
