using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Game;
using System;
using Xunit;

namespace GBX.NET.Tests.Integration.Builders.Engines.Game;

public class CGameCtnMediaBlockGhostTests
{
    public static int GetSampleStartOffset() => 5;

    private static CGameCtnMediaBlockGhost BuildNode(Func<CGameCtnMediaBlockGhostBuilder,
        GameBuilder<CGameCtnMediaBlockGhostBuilder, CGameCtnMediaBlockGhost>> func, int startOffset)
    {
        // Instead of GetNodeInstance, use builder of CGameCtnGhost instead (once available)
        var builder = new CGameCtnMediaBlockGhostBuilder(new CGameCtnGhost())
            .WithStartOffset(startOffset);
        return func.Invoke(builder).Build();
    }

    private static void ForX_ParametersShouldMatch(Func<CGameCtnMediaBlockGhostBuilder,
        GameBuilder<CGameCtnMediaBlockGhostBuilder, CGameCtnMediaBlockGhost>> func)
    {
        var startOffset = GetSampleStartOffset();

        var node = BuildNode(func, startOffset);

        Assert.Equal(expected: startOffset, actual: node.StartOffset);
    }

    private static void ForX_ChunksShouldMatch(Func<CGameCtnMediaBlockGhostBuilder,
        GameBuilder<CGameCtnMediaBlockGhostBuilder, CGameCtnMediaBlockGhost>> func, Action<CGameCtnMediaBlockGhost> chunkAssert)
    {
        var startOffset = GetSampleStartOffset();

        var node = BuildNode(func, startOffset);
        
        chunkAssert.Invoke(node);
    }

    [Fact] public void ForTMUF_ParametersShouldMatch() => ForX_ParametersShouldMatch(x => x.ForTMUF());

    [Fact]
    public void ForTMUF_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMUF(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaBlockGhost.Chunk030E5001>());
        });
    }
}
