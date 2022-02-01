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

public class CGameCtnMediaClipBuilderTests
{
    public static string GetSampleName() => "Random Clip 1";
    public static CGameCtnMediaTrack[] GetSampleTracksForTMUF() => new[]
    {
        CGameCtnMediaTrack.Create()
            .WithName(CGameCtnMediaTrackBuilderTests.GetSampleName())
            .WithBlocks(CGameCtnMediaBlockText.Create()
                .WithEffect(CControlEffectSimi.Create()
                    .ForTMUF()
                    .Build())
                .ForTMUF()
                .Build())
            .ForTMUF()
            .Build()
    };

    [Fact]
    public void WithName_ShouldSetName()
    {
        var expected = GetSampleName();

        var builder = new CGameCtnMediaClipBuilder()
            .WithName(expected);

        Assert.Equal(expected, actual: builder.Name);
    }

    [Fact]
    public void WithTracks_Params_ShouldSetTracks()
    {
        var expected = GetSampleTracksForTMUF();

        var builder = new CGameCtnMediaClipBuilder()
             .WithTracks(expected);

        Assert.Equal(expected, actual: builder.Tracks);
    }

    [Fact]
    public void WithTracks_List_ShouldSetTracks()
    {
        var expected = GetSampleTracksForTMUF().ToList();

        var builder = new CGameCtnMediaClipBuilder()
             .WithTracks(expected);

        Assert.Equal(expected, actual: builder.Tracks);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = NodeCacheManager.GetNodeInstance<CGameCtnMediaClip>(0x03079000);

        var actual = new CGameCtnMediaClipBuilder().NewNode();

        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedName = GetSampleName();
        var expectedTracks = GetSampleTracksForTMUF();

        var node = new CGameCtnMediaClipBuilder
        {
            Name = expectedName,
            Tracks = expectedTracks
        }
        .NewNode();

        Assert.Equal(expectedName, actual: node.Name);
        Assert.Equal(expectedTracks, actual: node.Tracks);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = new CGameCtnMediaClipBuilder().NewNode();

        Assert.NotNull(node.Name);
        Assert.NotNull(node.Tracks);
    }

    [Fact]
    public void TMSX_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaClipBuilder().ForTMSX().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079003>());
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaClipBuilder().ForTMU().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079003>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079004>());
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaClipBuilder().ForTMUF()
            .WithLocalPlayerClipEntIndex(1)
            .Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079004>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079005>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079007>());
        Assert.Equal(expected: 1, actual: node.LocalPlayerClipEntIndex);
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaClipBuilder().ForTM2()
            .WithLocalPlayerClipEntIndex(1)
            .StopsWhenRespawn()
            .StopsWhenLeave()
            .Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>());
        Assert.Equal(expected: 1, actual: node.LocalPlayerClipEntIndex);
        Assert.True(node.StopWhenRespawn);
        Assert.True(node.StopWhenLeave);
    }

    [Fact]
    public void TM2020_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaClipBuilder().ForTM2020()
            .WithLocalPlayerClipEntIndex(1)
            .StopsWhenRespawn()
            .StopsWhenLeave()
            .Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>());
        Assert.Equal(expected: 1, actual: node.LocalPlayerClipEntIndex);
        Assert.True(node.StopWhenRespawn);
        Assert.True(node.StopWhenLeave);
    }
}
