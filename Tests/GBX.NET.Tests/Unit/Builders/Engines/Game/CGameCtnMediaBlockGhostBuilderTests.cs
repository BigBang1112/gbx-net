using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Game;
using TmEssentials;
using Xunit;

namespace GBX.NET.Tests.Unit.Builders.Engines.Game;

public class CGameCtnMediaBlockGhostBuilderTests
{
    public static int GetSampleStartOffset() => 5;

    private static CGameCtnMediaBlockGhostBuilder NewBuilder()
    {
        return new CGameCtnMediaBlockGhostBuilder(new CGameCtnGhost());
    }

    [Fact]
    public void WithStartOffset_ShouldSetStartOffset()
    {
        var expected = GetSampleStartOffset();

        var builder = NewBuilder()
            .WithStartOffset(expected);

        Assert.Equal(expected, actual: builder.StartOffset);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = new CGameCtnMediaBlockGhost();

        var actual = NewBuilder().NewNode();

        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedStartOffset = GetSampleStartOffset();

        var builder = NewBuilder();
        builder.StartOffset = expectedStartOffset;
        var node = builder.NewNode();

        Assert.Equal(expectedStartOffset, actual: node.StartOffset);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = NewBuilder().NewNode();

        Assert.NotNull(node.GhostModel);
        Assert.Equal(expected: 0, actual: node.StartOffset);
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var expectedStart = TimeSingle.FromSeconds(3);
        var expectedEnd = TimeSingle.FromSeconds(6);

        var node = NewBuilder()
            .ForTMUF()
            .WithTimeRange(expectedStart, expectedEnd)
            .Build();

        Assert.Equal(expectedStart, actual: node.Start);
        Assert.Equal(expectedEnd, actual: node.End);
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockGhost.Chunk030E5001>());
    }

    [Fact]
    public void TMUF_WithTimeRange_ShouldSetStartAndEnd()
    {
        var expectedStart = TimeSingle.FromSeconds(3);
        var expectedEnd = TimeSingle.FromSeconds(6);

        var builder = NewBuilder()
            .ForTMUF()
            .WithTimeRange(expectedStart, expectedEnd);

        Assert.Equal(expectedStart, actual: builder.Start);
        Assert.Equal(expectedEnd, actual: builder.End);
    }

    [Fact]
    public void TMUF_StartingAt_ShouldSetStart()
    {
        var expectedStart = TimeSingle.FromSeconds(3);

        var builder = NewBuilder()
            .ForTMUF()
            .StartingAt(expectedStart);

        Assert.Equal(expectedStart, actual: builder.Start);
    }

    [Fact]
    public void TMUF_EndingAt_ShouldSetStartAndEnd()
    {
        var expectedEnd = TimeSingle.FromSeconds(6);

        var builder = NewBuilder()
            .ForTMUF()
            .EndingAt(expectedEnd);

        Assert.Equal(expectedEnd, actual: builder.End);
    }
}
