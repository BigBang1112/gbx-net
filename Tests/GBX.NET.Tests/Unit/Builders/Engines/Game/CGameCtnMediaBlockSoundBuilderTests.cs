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

    [Fact]
    public void WithSound_ShouldSetSound()
    {
        var expected = GetSampleSound();

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .WithSound(expected);

        Assert.Equal(expected, actual: builder.Sound);
    }

    [Fact]
    public void WithKeys_Params_ShouldSetKeys()
    {
        var expected = GetSampleKeys();

        var builder = new CGameCtnMediaBlockSoundBuilder()
             .WithKeys(expected);

        Assert.Equal(expected, actual: builder.Keys);
    }

    [Fact]
    public void WithKeys_List_ShouldSetKeys()
    {
        var expected = GetSampleKeys().ToList();

        var builder = new CGameCtnMediaBlockSoundBuilder()
             .WithKeys(expected);

        Assert.Equal(expected, actual: builder.Keys);
    }

    [Fact]
    public void WithPlayCount_ShouldSetPlayCount()
    {
        var expected = 6;

        var builder = new CGameCtnMediaBlockSoundBuilder()
             .WithPlayCount(expected);

        Assert.Equal(expected, actual: builder.PlayCount);
    }

    [Fact]
    public void Loops_ShouldSetLooping()
    {
        var builder = new CGameCtnMediaBlockSoundBuilder()
             .Loops();

        Assert.True(builder.IsLooping);
    }

    [Fact]
    public void NewNode_ShouldReturnInstance()
    {
        var expected = NodeCacheManager.GetNodeInstance<CGameCtnMediaBlockSound>(0x030A7000);

        var actual = new CGameCtnMediaBlockSoundBuilder().NewNode();

        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public void NewNode_ShouldSetValues()
    {
        var expectedSound = GetSampleSound();
        var expectedKeys = GetSampleKeys();
        var expectedPlayCount = 5;
        var expectedLooping = true;

        var node = new CGameCtnMediaBlockSoundBuilder
        {
            Sound = expectedSound,
            Keys = expectedKeys,
            PlayCount = expectedPlayCount,
            IsLooping = expectedLooping
        }
        .NewNode();

        Assert.Equal(expectedSound, actual: node.Sound);
        Assert.Equal(expectedKeys, actual: node.Keys);
        Assert.Equal(expectedPlayCount, actual: node.PlayCount);
        Assert.Equal(expectedLooping, actual: node.IsLooping);
    }

    [Fact]
    public void NewNode_ShouldSetDefaultValues()
    {
        var node = new CGameCtnMediaBlockSoundBuilder().NewNode();

        Assert.NotNull(node.Sound);
        Assert.NotNull(node.Keys);
    }

    [Fact]
    public void TMSX_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockSoundBuilder().ForTMSX().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7002>());
    }

    [Fact]
    public void TMU_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockSoundBuilder().ForTMU().Build();

        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7001>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7002>());
    }

    [Fact]
    public void TMUF_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockSoundBuilder()
            .ForTMUF()
            .WithMusic(true)
            .Build();

        Assert.True(node.IsMusic);
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>());
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
    }

    [Fact]
    public void TMUF_WithMusic_ShouldSetIsMusic()
    {
        var expected = true;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTMUF()
            .WithMusic(expected);

        Assert.Equal(expected, actual: builder.IsMusic);
    }

    [Fact]
    public void TM2_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2()
            .WithMusic(true)
            .WithAudioToSpeech(true)
            .WithAudioToSpeechTarget(1)
            .Build();

        var chunk003 = node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>();

        Assert.Equal(expected: 2, actual: chunk003?.Version);
        Assert.True(node.IsMusic);
        Assert.True(node.AudioToSpeech);
        Assert.Equal(expected: 1, actual: node.AudioToSpeechTarget);
        Assert.NotNull(chunk003);
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
    }

    [Fact]
    public void TM2_WithMusic_ShouldSetIsMusic()
    {
        var expected = true;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2()
            .WithMusic(expected);

        Assert.Equal(expected, actual: builder.IsMusic);
    }

    [Fact]
    public void TM2_WithAudioToSpeech_ShouldSetAudioToSpeech()
    {
        var expected = true;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2()
            .WithAudioToSpeech(expected);

        Assert.Equal(expected, actual: builder.AudioToSpeech);
    }

    [Fact]
    public void TM2_WithAudioToSpeechTarget_ShouldSetAudioToSpeechTarget()
    {
        var expected = 1;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2()
            .WithAudioToSpeechTarget(expected);

        Assert.Equal(expected, actual: builder.AudioToSpeechTarget);
    }

    [Fact]
    public void TM2020_Build_ShouldHaveSpecifics()
    {
        var node = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2020()
            .WithMusic(true)
            .WithAudioToSpeech(true)
            .WithAudioToSpeechTarget(1)
            .Build();

        var chunk003 = node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7003>();

        Assert.Equal(expected: 2, actual: chunk003?.Version);
        Assert.True(node.IsMusic);
        Assert.True(node.AudioToSpeech);
        Assert.Equal(expected: 1, actual: node.AudioToSpeechTarget);
        Assert.NotNull(chunk003);
        Assert.NotNull(node.GetChunk<CGameCtnMediaBlockSound.Chunk030A7004>());
    }

    [Fact]
    public void TM2020_WithMusic_ShouldSetIsMusic()
    {
        var expected = true;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2020()
            .WithMusic(expected);

        Assert.Equal(expected, actual: builder.IsMusic);
    }

    [Fact]
    public void TM2020_WithAudioToSpeech_ShouldSetAudioToSpeech()
    {
        var expected = true;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2020()
            .WithAudioToSpeech(expected);

        Assert.Equal(expected, actual: builder.AudioToSpeech);
    }

    [Fact]
    public void TM2020_WithAudioToSpeechTarget_ShouldSetAudioToSpeechTarget()
    {
        var expected = 1;

        var builder = new CGameCtnMediaBlockSoundBuilder()
            .ForTM2020()
            .WithAudioToSpeechTarget(expected);

        Assert.Equal(expected, actual: builder.AudioToSpeechTarget);
    }
}
