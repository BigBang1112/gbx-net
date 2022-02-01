using GBX.NET.Builders;
using GBX.NET.Builders.Engines.Control;
using GBX.NET.Builders.Engines.Game;
using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Tests.Integration.Builders.Engines.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration.Builders.Engines.Game;

public class CGameCtnMediaClipBuilderTests
{
    public static string GetSampleName() => Unit.Builders.Engines.Game.CGameCtnMediaClipBuilderTests.GetSampleName();
    public static CGameCtnMediaTrack[] GetSampleTracksForTM2() => Unit.Builders.Engines.Game.CGameCtnMediaClipBuilderTests.GetSampleTracksForTMUF();

    private static CGameCtnMediaClip BuildNode(Func<CGameCtnMediaClipBuilder,
        GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>> func, string name, params CGameCtnMediaTrack[] tracks)
    {
        var builder = new CGameCtnMediaClipBuilder()
            .WithName(name)
            .WithTracks(tracks);
        return func.Invoke(builder).Build();
    }

    private static void ForX_ParametersShouldMatch(Func<CGameCtnMediaClipBuilder,
        GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>> func)
    {
        var name = GetSampleName();
        var tracks = GetSampleTracksForTM2();

        var node = BuildNode(func, name, tracks);

        Assert.Equal(expected: name, actual: node.Name);
        Assert.Equal(expected: tracks, actual: node.Tracks);
    }

    private static void ForX_ChunksShouldMatch(Func<CGameCtnMediaClipBuilder,
        GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>> func, Action<CGameCtnMediaClip> chunkAssert)
    {
        var name = GetSampleName();
        var tracks = GetSampleTracksForTM2();

        var node = BuildNode(func, name, tracks);
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
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079003>());
        });
    }

    [Fact]
    public void ForTMU_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMU(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079003>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079004>());
        });
    }

    [Fact]
    public void ForTMUF_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTMUF(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079004>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079005>());
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk03079007>());
        });
    }

    [Fact]
    public void ForTM2_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>());
        });
    }

    [Fact]
    public void ForTM2020_ChunksShouldMatch()
    {
        ForX_ChunksShouldMatch(x => x.ForTM2020(), node =>
        {
            Assert.NotNull(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>());
        });
    }
}
