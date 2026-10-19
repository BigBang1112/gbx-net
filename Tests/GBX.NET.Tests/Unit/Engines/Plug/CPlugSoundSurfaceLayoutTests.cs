using GBX.NET.Engines.Function;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSoundSurfaceLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_ImpactSettings_PreserveVersionedPayload(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(1.7f);
            writer.Write(0.6f);
            writer.Write(0.03f);
            writer.Write(0.4f);
            if (version >= 2) writer.Write(true);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugSoundSurface();
        var chunk = new CPlugSoundSurface.Chunk0905E002();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.BigImpactAttenuation).IsEqualTo(1.7f);
        await Assert.That(node.SmallImpactAttenuation).IsEqualTo(0.6f);
        await Assert.That(node.ImpactOnlyMode).IsEqualTo(version >= 2);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_LegacySounds_ReadInterleavedReferencesWithoutArrayPrefixes()
    {
        var component = new CPlugSoundComponent { MaxVolume = 0.75f };
        component.CreateChunk<CPlugSoundComponent.Chunk0908F000>();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(420f);
            writer.Write(2);
            for (var i = 0; i < 2; i++)
            {
                writer.WriteNodeRef<CMwNod>(null);
                writer.WriteNodeRef<CMwNod>(null);
                writer.WriteNodeRef(component);
                writer.WriteNodeRef<CPlugSoundComponent>(null);
            }
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugSoundSurface();
        var chunk = new CPlugSoundSurface.Chunk0905E000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.EdMaxSpeedKmh).IsEqualTo(420f);
        await Assert.That(node.MaterialSounds.Length).IsEqualTo(2);
        await Assert.That(node.MaterialSounds[0].Texture!.MaxVolume).IsEqualTo(0.75f);
        await Assert.That(ReferenceEquals(node.MaterialSounds[0].Texture, node.MaterialSounds[1].Texture)).IsTrue();
        await Assert.That(node.MaterialSounds[0].SmallImpact).IsNull();
        await Assert.That(node.MaterialSounds[0].BigImpact).IsNull();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 1)]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    public async Task ReadWrite_ModernSounds_PreserveDirectCurveBodies(bool simple, int version)
    {
        var original = new CPlugSoundSurface(simple ? GameVersion.TM2020 : GameVersion.MP4)
        {
            MaterialSounds = [new(), new()]
        };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(350f);
            writer.Write(2);
            for (var i = 0; i < 4; i++) writer.WriteNodeRef<CPlugSoundComponent>(null);
            writer.WriteNode(original.SkidVolumeFromIntensity);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugSoundSurface();
        var chunk = new CPlugSoundSurface.Chunk0905E003();
        using (var input = new NonSeekableStream(payload.ToArray()))
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.EdMaxSpeedKmh).IsEqualTo(350f);
        await Assert.That(node.MaterialSounds.Length).IsEqualTo(2);
        await Assert.That(node.SkidVolumeFromIntensity!.GetType()).IsEqualTo(simple ? typeof(CPlugCurveSimpleNod) : typeof(CFuncKeysReal));
        var curve = (PlugCurve)node.SkidVolumeFromIntensity;
        await Assert.That(curve.Values).IsEquivalentTo(new Vec2[] { new(0, 0), new(0.5f, 0.707f), new(1, 1), new(2, 1.4f) }, CollectionOrdering.Matching);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }
}
