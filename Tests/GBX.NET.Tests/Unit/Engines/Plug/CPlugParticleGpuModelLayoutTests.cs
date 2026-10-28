using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugParticleGpuModelLayoutTests
{
    [Test]
    [Arguments(0, 44)]
    [Arguments(1, 44)]
    [Arguments(2, 48)]
    [Arguments(3, 52)]
    [Arguments(4, 56)]
    [Arguments(5, 60)]
    [Arguments(6, 68)]
    [Arguments(7, 72)]
    [Arguments(8, 80)]
    [Arguments(9, 104)]
    [Arguments(10, 112)]
    [Arguments(11, 120)]
    [Arguments(12, 124)]
    [Arguments(13, 128)]
    [Arguments(14, 136)]
    [Arguments(15, 140)]
    [Arguments(16, 148)]
    [Arguments(17, 160)]
    [Arguments(18, 164)]
    [Arguments(19, 168)]
    [Arguments(20, 184)]
    [Arguments(21, 184)]
    [Arguments(22, 188)]
    public async Task ReadWrite_Rendering_ConsumesNativePayloadAndPreservesBytes(int version, int length)
    {
        // Native payload sizes include the version, with null bitmap references.
        var bytes = new byte[length + 4];
        BitConverter.GetBytes(version).CopyTo(bytes, 0);
        BitConverter.GetBytes(-1).CopyTo(bytes, version >= 7 ? 12 : 8);
        if (version >= 9) BitConverter.GetBytes(-1).CopyTo(bytes, 80);
        if (version >= 12) BitConverter.GetBytes(-1).CopyTo(bytes, 120);
        if (version >= 18) BitConverter.GetBytes(-1).CopyTo(bytes, 160);
        BitConverter.GetBytes(0x12345678).CopyTo(bytes, length);

        if (version == 22)
        {
            BitConverter.GetBytes(2.5f).CopyTo(bytes, 4);
            BitConverter.GetBytes(3.5f).CopyTo(bytes, 24);
            BitConverter.GetBytes(0.25f).CopyTo(bytes, 28);
            BitConverter.GetBytes(3).CopyTo(bytes, 32);
            BitConverter.GetBytes(5).CopyTo(bytes, 36);
            BitConverter.GetBytes(0.75f).CopyTo(bytes, 184);
        }

        using var payload = new MemoryStream(bytes);
        var node = new CPlugParticleGpuModel();
        var chunk = new CPlugParticleGpuModel.Chunk090C6002();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo(length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }

        if (version == 22)
        {
            await Assert.That(node.Persistence).IsEqualTo(2.5f);
            await Assert.That(node.Intensity).IsEqualTo(3.5f);
            await Assert.That(node.IntensityVariation).IsEqualTo(0.25f);
            await Assert.That(node.TextureAtlasDimX).IsEqualTo(3);
            await Assert.That(node.TextureAtlasDimY).IsEqualTo(5);
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(bytes[..length], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, 20)]
    [Arguments(1, 20)]
    [Arguments(2, 28)]
    [Arguments(3, 32)]
    [Arguments(4, 32)]
    [Arguments(5, 36)]
    [Arguments(6, 40)]
    public async Task ReadWrite_LifetimeAndSize_ConvertsLegacyRangesWithoutChangingWireValues(int version, int length)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(3f);
            writer.Write(1f);
            writer.Write(6f);
            writer.Write(2f);
            while (payload.Length < length) writer.Write(0);
            writer.Write(0x12345678);
        }

        payload.Position = 0;
        var node = new CPlugParticleGpuModel();
        var chunk = new CPlugParticleGpuModel.Chunk090C6000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo(length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }

        await Assert.That(node.LifeMax).IsEqualTo(version <= 3 ? 4f : 3f);
        await Assert.That(node.SizeMax).IsEqualTo(version == 0 ? 4f : version <= 3 ? 8f : 6f);
        await Assert.That(node.LifeVar01).IsEqualTo(version <= 3 ? 0.5f : 1f);
        await Assert.That(node.SizeVar01).IsEqualTo(version <= 3 ? 0.5f : 2f);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..length], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_Tm2020Chunks_ConsumeTheirAddedFields()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(3);
            writer.Write(-7f);
            writer.Write(0.25f);
            writer.Write(true);
            writer.Write(false);
            writer.Write(true);
            writer.Write(true);
            writer.Write(2);
            writer.Write(0.2f);
            writer.Write(0.4f);
            writer.Write(0.8f);
            writer.Write(0x12345678);
        }

        payload.Position = 0;
        var node = new CPlugParticleGpuModel();
        var physics = new CPlugParticleGpuModel.Chunk090C6001();
        var parameters = new CPlugParticleGpuModel.Chunk090C6003();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            physics.ReadWrite(node, rw);
            parameters.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }
        await Assert.That(node.Weight).IsEqualTo(-7f);
        await Assert.That(node.Viscosity).IsEqualTo(0.25f);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            physics.ReadWrite(node, rw);
            parameters.ReadWrite(node, rw);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, 0.25f, 0.01f, 4f)]
    [Arguments(1, 2f, 0.5f, 1f)]
    [Arguments(2, 2f, 0.5f, 1f)]
    public async Task ReadWrite_LegacyRendering_UpgradesIntensityAndPreservesOriginalValues(int version, float decayPower, float fadeOut, float power)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(2f);
            writer.Write(-1);
            writer.Write(3f);
            writer.Write(0.5f);
            writer.Write(3);
            writer.Write(5);
            writer.Write(decayPower);
            writer.Write(0.1f);
            writer.Write(0.2f);
            writer.Write(true);
            if (version >= 2) writer.Write(true);
        }

        payload.Position = 0;
        var node = new CPlugParticleGpuModel { SizeMax = 4f, SizeVar01 = 0.5f };
        var chunk = new CPlugParticleGpuModel.Chunk090C6002();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Persistence).IsEqualTo(version == 0 ? 6f : 2f);
        await Assert.That(node.IntensityFadeIn).IsEqualTo(0.01f);
        await Assert.That(node.IntensityFadeOut).IsEqualTo(fadeOut);
        await Assert.That(node.IntensityPower).IsEqualTo(power);
        await Assert.That(node.UseEmitterHue).IsTrue();

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_BitmapFile_PreservesThePathWithoutAnEmbeddedReference()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(7);
            writer.Write(2f);
            writer.Write(true);
            writer.Write("Particles/Smoke.Bitmap.Gbx");
            writer.Write(new byte[56]);
        }

        payload.Position = 0;
        var node = new CPlugParticleGpuModel();
        var chunk = new CPlugParticleGpuModel.Chunk090C6002();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.BitmapIsFile).IsTrue();
        await Assert.That(node.BitmapFile).IsEqualTo("Particles/Smoke.Bitmap.Gbx");
        await Assert.That(node.Bitmap).IsNull();

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
