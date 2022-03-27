using GBX.NET.Engines.Game;
using System;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class NodeTests
{
    [Fact]
    public void ToGbx_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx();

        Assert.Equal(node, gbx.Node);
    }

    [Fact]
    public void ToGbx_NonGenericParameter_NoExceptionIsThrown()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        node.ToGbx(nonGeneric: true);
    }

    [Fact]
    public void ToGbx_NonGenericParameter_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx(nonGeneric: true);

        Assert.Equal(node, gbx.Node);
    }

    [Fact]
    public void ToGbx_Generic_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx<CGameCtnMediaBlockSound>();

        Assert.Equal(node, gbx.Node);
    }

    // test GetChunk

    [Fact]
    public void GetChunk_ChunkIsNotNull_ReturnsChunk()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();
        var chunk = node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>();

        Assert.NotNull(chunk);
    }

    [Fact]
    public void GetChunk_ChunkIsNull_ReturnsNull()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();
        var chunk = node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7001>();

        Assert.Null(chunk);
    }

    [Fact]
    public void GetChunk_ChunkIsNotNull_ReturnsChunkOfType()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();
        var chunk = node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>();

        Assert.IsType<CGameCtnMediaBlockSound.Chunk030A7004>(chunk);
    }
}
