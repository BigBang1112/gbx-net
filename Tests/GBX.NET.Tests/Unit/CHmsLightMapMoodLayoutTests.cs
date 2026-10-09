using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CHmsLightMapMoodLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    public async Task MoodChunksKeepNativeFieldOrderAndLegacyValues(int offset, bool skyUseClouds)
    {
        var payload = Payload(w =>
        {
            if (offset == 4) w.Write(0); // Chunk version.
            if (offset <= 1)
            {
                foreach (var value in new[] { 11f, 22f, 0.25f, 0.75f, 1.5f }) w.Write(value);
                if (offset == 0) return;
            }
            w.Write(9.5f); // MaxHDR.
            if (offset == 1) return;
            if (offset >= 3)
            {
                w.Write(0.5f); // BounceFactor.
                w.Write(0.125f); // SkyFactor.
            }
            if (offset == 4) w.Write(skyUseClouds ? 1 : 0); // Native boolean occupies four bytes.
            foreach (var value in new[] { 1.25f, -2.25f, 3.25f }) w.Write(value);
        });
        var node = new CHmsLightMapMood();
        var chunk = (Chunk<CHmsLightMapMood>)Activator.CreateInstance(
            typeof(CHmsLightMapMood).GetNestedType($"Chunk{0x06023000 + offset:X8}")!)!;

        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }

        if (offset <= 1)
        {
            await Assert.That(node.AmbientRange).IsEqualTo(new Vec2(0.25f, 0.75f));
            await Assert.That(node.DirectionalScale).IsEqualTo(1.5f);
        }
        await Assert.That(node.MaxHDR).IsEqualTo(offset == 0 ? 3f : 9.5f);
        await Assert.That(node.BounceFactor).IsEqualTo(offset < 3 ? 1f : 0.5f);
        await Assert.That(node.SkyFactor).IsEqualTo(offset < 3 ? 1f : 0.125f);
        await Assert.That(node.SkyUseClouds).IsEqualTo(offset != 4 || skyUseClouds);
        await Assert.That(chunk.GameVersion).IsEqualTo(offset switch
        {
            0 => GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020,
            4 => GameVersion.MP3 | GameVersion.TMT | GameVersion.TM2020,
            _ => GameVersion.Unspecified
        });

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        chunk.ReadWrite(node, writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ConstructorDefaultsMatchNativeLegacyAndCurrentPayloads()
    {
        var node = new CHmsLightMapMood();
        await Assert.That(node.AmbientRange).IsEqualTo(new Vec2(0.35f, 1));
        await Assert.That(node.DirectionalScale).IsEqualTo(1f);
        await Assert.That(node.MaxHDR).IsEqualTo(3f);
        await Assert.That(node.BounceFactor).IsEqualTo(1f);
        await Assert.That(node.SkyFactor).IsEqualTo(1f);
        await Assert.That(node.SkyUseClouds).IsTrue();

        var expectedLegacy = Payload(w =>
        {
            foreach (var value in new[] { 2f, 2f, 0.35f, 1f, 1f }) w.Write(value);
        });
        var expectedCurrent = Payload(w =>
        {
            w.Write(0); // Version.
            foreach (var value in new[] { 3f, 1f, 1f }) w.Write(value);
            w.Write(1); // SkyUseClouds.
            foreach (var value in new[] { 1f, 0f, 0f }) w.Write(value);
        });
        await Assert.That(WritePayload(rw => new CHmsLightMapMood.Chunk06023000().ReadWrite(node, rw)))
            .IsEquivalentTo(expectedLegacy, CollectionOrdering.Matching);
        await Assert.That(WritePayload(rw => new CHmsLightMapMood.Chunk06023004().ReadWrite(node, rw)))
            .IsEquivalentTo(expectedCurrent, CollectionOrdering.Matching);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static byte[] WritePayload(Action<GbxReaderWriter> serialize)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        serialize(rw);
        return stream.ToArray();
    }
}
