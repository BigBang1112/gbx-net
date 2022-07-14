using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Game;
using System;
using Xunit;

namespace GBX.NET.Tests.Integration.Builders.Engines.Game;

public class CGameCtnMediaTrackBuilderTests
{
    public static string GetSampleName() => Unit.Builders.Engines.Game.CGameCtnMediaTrackBuilderTests.GetSampleName();
    public static CGameCtnMediaBlock[] GetSampleBlocksForTM2() => Unit.Builders.Engines.Game.CGameCtnMediaTrackBuilderTests.GetSampleBlocksForTMUF();

    private static CGameCtnMediaTrack BuildNode(Func<CGameCtnMediaTrackBuilder,
        GameBuilder<CGameCtnMediaTrackBuilder, CGameCtnMediaTrack>> func, string name, params CGameCtnMediaBlock[] blocks)
    {
        var builder = new CGameCtnMediaTrackBuilder()
            .WithName(name)
            .WithBlocks(blocks);
        return func.Invoke(builder).Build();
    }

    private static void ForX_ParametersShouldMatch(Func<CGameCtnMediaTrackBuilder,
        GameBuilder<CGameCtnMediaTrackBuilder, CGameCtnMediaTrack>> func)
    {
        var name = GetSampleName();
        var blocks = GetSampleBlocksForTM2();

        var node = BuildNode(func, name, blocks);

        Assert.Equal(expected: name, actual: node.Name);
        Assert.Equal(expected: blocks, actual: node.Blocks);
    }

    private static void ForX_ChunksShouldMatch(Func<CGameCtnMediaTrackBuilder,
        GameBuilder<CGameCtnMediaTrackBuilder, CGameCtnMediaTrack>> func, Action<CGameCtnMediaTrack> chunkAssert)
    {
        var name = GetSampleName();
        var blocks = GetSampleBlocksForTM2();

        var node = BuildNode(func, name, blocks);
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
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078002>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078003>());
        });
    }

    [Fact]
    public void ForTMU_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMU(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078002>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078003>());
        });
    }

    [Fact]
    public void ForTMUF_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMUF(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078004>());
        });
    }

    [Fact]
    public void ForTM2_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078005>());
        });
    }

    [Fact]
    public void ForTM2020_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2020(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078005>());
        });
    }
}
