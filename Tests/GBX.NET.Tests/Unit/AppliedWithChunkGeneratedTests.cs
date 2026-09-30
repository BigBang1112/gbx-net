using GBX.NET.Attributes;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using System.Reflection;

namespace GBX.NET.Tests.Unit;

public class AppliedWithChunkGeneratedTests
{
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
}
