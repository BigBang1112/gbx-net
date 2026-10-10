using GBX.NET.Engines.Plug;
using GBX.NET.Engines.GameData;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class ContextualArchiveTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ArrayReadableWritable_WithReaderAndWriter_PreservesContextAndVersion(bool byteLengthPrefix)
    {
        var node = new CPlugAnimFile { TimingFixedPeriodVersion = 1, ClipFlagsVersion = 6 };
        CPlugAnimFile.LegacyBaked[] values =
        [
            new()
            {
                Timing = new() { FrameCount = 5, FramePeriod = 40, Looping = true },
                JointGroupIndex = 3,
                LocalJointsData = [],
                Flags = new() { WorldSpeedKmh = 12, IsPartial = true, DifferenceFromClip = "" },
                RootMotionFrames = [],
                FloatChannelIds = [],
                FloatChannelData = []
            }
        ];
        using var source = new MemoryStream();
        using (var rw = new GbxReaderWriter(new GbxWriter(source)))
        {
            rw.ArrayReadableWritable(values, node, byteLengthPrefix, version: 5);
        }

        source.Position = 0;
        using var destination = new MemoryStream();
        using var reader = new GbxReader(source);
        using var writer = new GbxWriter(destination);
        using var copier = new GbxReaderWriter(reader, writer);
        CPlugAnimFile.LegacyBaked[]? restored = null;
        copier.ArrayReadableWritable(ref restored, node, byteLengthPrefix, version: 5);

        await GbxAssert.AreDeeplyEqual(values, restored);
        await Assert.That(destination.ToArray()).IsEquivalentTo(source.ToArray(), CollectionOrdering.Matching);
        await Assert.That(source.Position).IsEqualTo(source.Length);
    }

    [Test]
    public async Task ArrayReadableWritable_NegativeLength_RejectsInvalidArchive()
    {
        using var source = new MemoryStream(BitConverter.GetBytes(-1));
        using var rw = new GbxReaderWriter(new GbxReader(source));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Task.Run(() =>
            rw.ArrayReadableWritable<CPlugAnimFile.LegacyBaked, CPlugAnimFile>(null, new CPlugAnimFile())));
    }

    [Test]
    public async Task ParticleBlock_Version9_StoresEmitterChunksDirectly()
    {
        var emitter = new CPlugParticleEmitterModel { IsSplashMode = true, ShadowMapTexelSize = 2.5f };
        emitter.CreateChunk<CPlugParticleEmitterModel.Chunk090B3002>().Version = 3;
        var block = new CGameActionModel.ParticleBlock { Keys = [], U01 = [], U07 = 1, ParticleEmitter = emitter };
        using var stream = new MemoryStream();
        using (var rw = new GbxReaderWriter(new GbxWriter(stream)))
        {
            rw.ReadableWritable(block, version: 9);
        }
        await Assert.That(BitConverter.ToUInt32(stream.ToArray(), 24)).IsEqualTo(0x090B3002u);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var restored = reader.ReadReadable<CGameActionModel.ParticleBlock>(version: 9);
        await GbxAssert.AreDeeplyEqual(block, restored);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }
}
