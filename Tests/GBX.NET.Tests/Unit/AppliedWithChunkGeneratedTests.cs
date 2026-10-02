using GBX.NET.Attributes;
using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

public class AppliedWithChunkGeneratedTests
{
    [Test]
    [Arguments(nameof(CGameCtnChallenge.MapType))]
    [Arguments(nameof(CGameCtnChallenge.MapStyle))]
    public async Task ChallengePartialPropertiesUseLayoutVersionBounds(string propertyName)
    {
        var applied = typeof(CGameCtnChallenge).GetProperty(propertyName)!
            .GetCustomAttributes<AppliedWithChunkAttribute>()
            .Single(x => x.ChunkType == typeof(CGameCtnChallenge.HeaderChunk03043003));

        await Assert.That(applied.Applies(new CGameCtnChallenge.HeaderChunk03043003 { Version = 5 })).IsFalse();
        await Assert.That(applied.Applies(new CGameCtnChallenge.HeaderChunk03043003 { Version = 6 })).IsTrue();
    }

    [Test]
    [Arguments(nameof(CGameCtnChallenge.MapUid), nameof(CGameCtnChallenge.MapInfo))]
    [Arguments(nameof(CGameCtnChallenge.Collection), nameof(CGameCtnChallenge.MapInfo))]
    [Arguments(nameof(CGameCtnChallenge.NbBlocks), nameof(CGameCtnChallenge.Blocks))]
    [Arguments(nameof(CGameCtnChallenge.NbBakedBlocks), nameof(CGameCtnChallenge.BakedBlocks))]
    public async Task ChallengeComputedPropertiesKeepSourceChunkMetadata(string propertyName, string sourceName)
    {
        var source = GetMetadata(sourceName);
        await Assert.That(source).IsNotEmpty();
        await Assert.That(GetMetadata(propertyName)).IsEquivalentTo(source);

        static (Type ChunkType, int SinceVersion, int? UpToVersion)[] GetMetadata(string name) =>
            typeof(CGameCtnChallenge).GetProperty(name)!.GetCustomAttributes<AppliedWithChunkAttribute>()
                .Select(x => (x.ChunkType, x.SinceVersion, x.UpToVersion)).ToArray();
    }

    [Test]
    public async Task ChallengeHandwrittenChunksSupplyPartialPropertyMetadata()
    {
        var properties = new (string Name, Type ChunkType)[]
        {
            (nameof(CGameCtnChallenge.Thumbnail), typeof(CGameCtnChallenge.HeaderChunk03043007)),
            (nameof(CGameCtnChallenge.MapInfo), typeof(CGameCtnChallenge.Chunk0304301F)),
            (nameof(CGameCtnChallenge.Size), typeof(CGameCtnChallenge.Chunk0304301F)),
            (nameof(CGameCtnChallenge.AuthorLogin), typeof(CGameCtnChallenge.HeaderChunk03043003)),
            (nameof(CGameCtnChallenge.Decoration), typeof(CGameCtnChallenge.Chunk0304301F)),
            (nameof(CGameCtnChallenge.Blocks), typeof(CGameCtnChallenge.Chunk0304301F)),
            (nameof(CGameCtnChallenge.HasLightmaps), typeof(CGameCtnChallenge.Chunk0304303D)),
            (nameof(CGameCtnChallenge.LightmapVersion), typeof(CGameCtnChallenge.Chunk0304305B)),
            (nameof(CGameCtnChallenge.LightmapCacheData), typeof(CGameCtnChallenge.Chunk0304305B)),
            (nameof(CGameCtnChallenge.LightmapCache), typeof(CGameCtnChallenge.Chunk0304305B)),
            (nameof(CGameCtnChallenge.LightmapFrames), typeof(CGameCtnChallenge.Chunk0304305B)),
            (nameof(CGameCtnChallenge.AnchoredObjects), typeof(CGameCtnChallenge.Chunk03043040)),
            (nameof(CGameCtnChallenge.ZoneGenealogy), typeof(CGameCtnChallenge.Chunk03043043)),
            (nameof(CGameCtnChallenge.BakedBlocks), typeof(CGameCtnChallenge.Chunk03043048)),
            (nameof(CGameCtnChallenge.BakedClipsAdditionalData), typeof(CGameCtnChallenge.Chunk03043048)),
            (nameof(CGameCtnChallenge.EmbeddedZipData), typeof(CGameCtnChallenge.Chunk03043054)),
            (nameof(CGameCtnChallenge.MacroblockInstances), typeof(CGameCtnChallenge.Chunk03043069)),
            ("Textures", typeof(CGameCtnChallenge.Chunk03043054))
        };

        foreach (var (name, chunkType) in properties)
        {
            var property = typeof(CGameCtnChallenge).GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!;
            await Assert.That(property.GetCustomAttributes<AppliedWithChunkAttribute>())
                .Contains(x => x.ChunkType == chunkType);
        }

        var textures = typeof(CGameCtnChallenge).GetProperty("Textures", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetCustomAttributes<AppliedWithChunkAttribute>().Single();
        await Assert.That(textures.SinceVersion).IsEqualTo(1);
    }

    [Test]
    public async Task GeneratedSinceVersionControlsChunkApplicability()
    {
        var applied = typeof(CGameCtnChallenge).GetProperty(nameof(CGameCtnChallenge.Cost))!
            .GetCustomAttributes<AppliedWithChunkAttribute>()
            .Single(x => x.ChunkType == typeof(CGameCtnChallenge.HeaderChunk03043002));

        await Assert.That(applied.SinceVersion).IsEqualTo(4);
        await Assert.That(applied.UpToVersion).IsNull();
        await Assert.That(applied.Applies(new CGameCtnChallenge.HeaderChunk03043002 { Version = 3 })).IsFalse();
        await Assert.That(applied.Applies(new CGameCtnChallenge.HeaderChunk03043002 { Version = 4 })).IsTrue();
    }

    [Test]
    public async Task GeneratedUpperBoundControlsChunkApplicability()
    {
        var applied = typeof(CPlugAnimFile).GetProperty(nameof(CPlugAnimFile.SkelEditionVersion))!
            .GetCustomAttributes<AppliedWithChunkAttribute>()
            .Single(x => x.ChunkType == typeof(CPlugAnimFile.Chunk090B0003));

        await Assert.That(applied.SinceVersion).IsEqualTo(3);
        await Assert.That(applied.UpToVersion).IsEqualTo(9);
        await Assert.That(applied.Applies(new CPlugAnimFile.Chunk090B0003 { Version = 2 })).IsFalse();
        await Assert.That(applied.Applies(new CPlugAnimFile.Chunk090B0003 { Version = 3 })).IsTrue();
        await Assert.That(applied.Applies(new CPlugAnimFile.Chunk090B0003 { Version = 9 })).IsTrue();
        await Assert.That(applied.Applies(new CPlugAnimFile.Chunk090B0003 { Version = 10 })).IsFalse();
    }

    [Test]
    public async Task BaseChunkMembersHaveAnnotationsForEachSerializingDerivedChunk()
    {
        var applied = typeof(CControlIconIndex).GetProperty(nameof(CControlIconIndex.IndexOff))!
            .GetCustomAttributes<AppliedWithChunkAttribute>()
            .ToDictionary(x => x.ChunkType);

        await Assert.That(applied.Keys).IsEquivalentTo(new[]
        {
            typeof(CControlIconIndex.Chunk0702B000),
            typeof(CControlIconIndex.Chunk0702B001),
            typeof(CControlIconIndex.Chunk0702B002)
        });
        await Assert.That(applied[typeof(CControlIconIndex.Chunk0702B000)]
            .Applies(new CControlIconIndex.Chunk0702B000())).IsTrue();
        await Assert.That(applied[typeof(CControlIconIndex.Chunk0702B000)]
            .Applies(new CControlIconIndex.Chunk0702B001())).IsFalse();
        await Assert.That(applied[typeof(CControlIconIndex.Chunk0702B001)]
            .Applies(new CControlIconIndex.Chunk0702B001())).IsTrue();
        await Assert.That(applied[typeof(CControlIconIndex.Chunk0702B002)]
            .Applies(new CControlIconIndex.Chunk0702B002())).IsTrue();
    }
}
