using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using System.Linq;
using Xunit;

namespace GBX.NET.Tests.Unit.Builders.Engines.Game;

public class CGameCtnMediaTrackBuilderTests
{
    public static string GetSampleName() => "Random Track 1";
    public static CGameCtnMediaBlock[] GetSampleBlocksForTMUF() => new[]
    {
        CGameCtnMediaBlockText.Create(CControlEffectSimi.Create().ForTMUF().Build())
            .ForTMUF()
            .Build()
    };

    [Fact]
    public void WithName_ShouldSetName()
    {
        var expected = GetSampleName();

        var builder = new CGameCtnMediaTrackBuilder()
            .WithName(expected);

        Assert.Equal(expected, actual: builder.Name);
    }

    [Fact]
    public void WithBlocks_Params_ShouldSetBlocks()
    {
        var expected = GetSampleBlocksForTMUF();

        var builder = new CGameCtnMediaTrackBuilder()
             .WithBlocks(expected);

        Assert.Equal(expected, actual: builder.Blocks);
    }

    [Fact]
    public void WithBlocks_List_ShouldSetBlocks()
    {
        var expected = GetSampleBlocksForTMUF().ToList();

        var builder = new CGameCtnMediaTrackBuilder()
             .WithBlocks(expected);

        Assert.Equal(expected, actual: builder.Blocks);
    }

    [Fact]
    public void KeepsPlaying_ShouldSetIsKeepPlaying()
    {
        var builder = new CGameCtnMediaTrackBuilder()
             .KeepsPlaying();

        Assert.True(builder.IsKeepPlaying);
    }

    [Fact]
    public void ReadOnly_ShouldSetIsReadOnly()
    {
        var builder = new CGameCtnMediaTrackBuilder()
             .ReadOnly();

        Assert.True(builder.IsReadOnly);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = new CGameCtnMediaTrack();

        var actual = new CGameCtnMediaTrackBuilder().NewNode();

        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedName = GetSampleName();
        var expectedBlocks = GetSampleBlocksForTMUF();

        var node = new CGameCtnMediaTrackBuilder
        {
            Name = expectedName,
            Blocks = expectedBlocks,
            IsKeepPlaying = true,
            IsReadOnly = true
        }
        .NewNode();

        Assert.Equal(expectedName, actual: node.Name);
        Assert.Equal(expectedBlocks, actual: node.Blocks);
        Assert.True(node.IsKeepPlaying);
        Assert.True(node.IsReadOnly);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = new CGameCtnMediaTrackBuilder().NewNode();

        Assert.NotNull(node.Name);
        Assert.NotNull(node.Blocks);
    }

    [Fact]
    public void TMSX_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaTrackBuilder().ForTMSX().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078002>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078003>());
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaTrackBuilder().ForTMU().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078002>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078003>());
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaTrackBuilder().ForTMUF().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078004>());
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaTrackBuilder().ForTM2()
            .Cycles()
            .Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078005>());
        Assert.True(node.IsCycling);
    }

    [Fact]
    public void TM2020_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaTrackBuilder().ForTM2020()
            .Cycles()
            .Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaTrack.Chunk03078005>());
        Assert.True(node.IsCycling);
    }
}
