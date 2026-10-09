using GBX.NET.Components;
using GBX.NET.Engines.System;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.System;

[Category("Unit")]
public class CSystemConfigLayoutTests
{
    private const int sentinel = 0x11223344;

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AudioQualityAndSpeakerConfigurationUseDistinctSlots(bool openAl)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write(0.25f);
            writer.Write(0.75f);
            writer.Write(2); // Global quality: High.
            writer.Write(1); // Acceleration: HardwareOnly.
            writer.Write(2); // 3D quality: HrtfFull.
            writer.Write(false);
            writer.Write(true);
            if (openAl) writer.Write(true); // Unresolved audio flag.
            writer.Write(9); // Speaker configuration: 5.1.
            if (openAl) writer.Write("Device");
        });
        var node = new CSystemConfig();
        var oldChunk = new CSystemConfig.Chunk0B005028();
        var newChunk = new CSystemConfig.Chunk0B00504F();
        Action<GbxReaderWriter> readWrite = rw =>
        {
            if (openAl) newChunk.ReadWrite(node, rw);
            else oldChunk.ReadWrite(node, rw);
        };
        ReadChunk(payload, readWrite);
        await Assert.That(node.AudioGlobalQuality).IsEqualTo(2);
        await Assert.That(node.AudioAcceleration_Dx9).IsEqualTo(CSystemConfig.EAudioAcceleration.HardwareOnly);
        await Assert.That(node.AudioQuality3d_Dx9).IsEqualTo(CSystemConfig.EAudioQuality3d.HrtfFull);
        await Assert.That(node.AudioSpeakerConfig).IsEqualTo(CSystemConfig.EAudioSpeakerConfig._5_1);
        if (openAl)
        {
            await Assert.That(newChunk.U01).IsTrue();
            await Assert.That(node.AudioDevice_Oal).IsEqualTo("Device");
        }
        await AssertRoundTrip(payload, readWrite);
    }

    [Test]
    public async Task DeprecatedStringsRemainIndependent()
    {
        using var payload = CreatePayload(writer =>
        {
            for (var i = 1; i <= 6; i++) writer.Write($"Value{i}");
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B005009();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(new[] { chunk.U01, chunk.U02, chunk.U03, chunk.U04, chunk.U05, chunk.U06 })
            .IsEquivalentTo(new string?[] { "Value1", "Value2", "Value3", "Value4", "Value5", "Value6" }, CollectionOrdering.Matching);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyPlayerSettingsConsumeDisplaySize()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write(false);
            writer.Write(23); // Unresolved player setting.
            writer.Write(7); // Display size, retained before native normalization to 3.
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B00503D();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U01).IsEqualTo(23);
        await Assert.That(node.PlayerInfoDisplaySize).IsEqualTo(7);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task CarParticlesAndOcclusionUseTheirNativeSlots()
    {
        using var payload = CreatePayload(writer =>
        {
            foreach (var value in new[] { 3, 2, 1, 2, 0, 16, 1 }) writer.Write(value);
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B005052();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.TmCarParticlesQuality).IsEqualTo(CSystemConfig.ETmCarParticlesQuality.HighMediumOpponents);
        await Assert.That(node.PlayerShadow).IsEqualTo(1);
        await Assert.That(node.PlayerOcclusion).IsEqualTo(CSystemConfig.EPlayerOcclusion.All);
        await Assert.That(node.TmMaxOpponents).IsEqualTo(16);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task FileTransferCacheSizePreservesNativeWidth(bool wide)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write(false);
            if (wide) writer.Write(0x1122334455667788UL);
            else writer.Write(0xfedcba98U);
            writer.Write(true); // Unresolved file-transfer flag.
            writer.Write(false);
            writer.Write(true);
            writer.Write(false);
            writer.Write("Locator");
            writer.Write("Blacklist");
            writer.Write(true);
        });
        var node = new CSystemConfig();
        var oldChunk = new CSystemConfig.Chunk0B005030();
        var newChunk = new CSystemConfig.Chunk0B005054();
        Action<GbxReaderWriter> readWrite = rw =>
        {
            if (wide) newChunk.ReadWrite(node, rw);
            else oldChunk.ReadWrite(node, rw);
        };
        ReadChunk(payload, readWrite);
        if (wide) await Assert.That(node.FileTransferMaxCacheSize).IsEqualTo(0x1122334455667788UL);
        else await Assert.That(node.FileTransferMaxCacheSize32).IsEqualTo(0xfedcba98U);
        await AssertRoundTrip(payload, readWrite);
    }

    [Test]
    public async Task FirewallExecutableChecksumsPreserveUnsignedValues()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write("MasterServer");
            writer.Write("Path");
            writer.Write(2);
            writer.Write(0x89abcdefU);
            writer.Write(0xfedcba98U);
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B005044();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.NetworkFirewallTestedExeChecksums!)
            .IsEquivalentTo(new[] { 0x89abcdefU, 0xfedcba98U }, CollectionOrdering.Matching);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ParentalLockPreservesTimestampAndPasswordHash()
    {
        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var hash = new UInt128(0x1122334455667788UL, 0x99aabbccddeeff00UL);
        using var payload = CreatePayload(writer =>
        {
            writer.WriteFileTime(timestamp);
            writer.Write(0x99aabbccddeeff00UL);
            writer.Write(0x1122334455667788UL);
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B00503E();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ParentalLockLastUnlockedTime!.Value.ToUniversalTime()).IsEqualTo(timestamp);
        await Assert.That(node.ParentalLockPasswordHash).IsEqualTo(hash);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task BadWordListUrlUsesVersionTwo(int version)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(version);
            writer.Write("AntiCheat");
            if (version >= 2) writer.Write("BadWords");
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B00505B();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AntiCheatServerUrl).IsEqualTo("AntiCheat");
        await Assert.That(node.BadWordListUrl).IsEqualTo(version >= 2 ? "BadWords" : null);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task TrackmaniaInputsIncludeJoystickEnablement()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(0);
            foreach (var value in new[] { true, false, true, false, true }) writer.Write(value);
        });
        var node = new CSystemConfig();
        var chunk = new CSystemConfig.Chunk0B005060();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.InputsEnableJoysticks).IsTrue();
        await Assert.That(node.InputsFreezeUnusedAxes).IsFalse();
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task UnsupportedVersionsStopBeforeReadingSettings(bool inputs)
    {
        using var payload = CreatePayload(writer => writer.Write(inputs ? 1 : 3));
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        var node = new CSystemConfig();
        if (inputs)
            Assert.Throws<NotSupportedException>(() => new CSystemConfig.Chunk0B005060().ReadWrite(node, rw));
        else
            Assert.Throws<NotSupportedException>(() => new CSystemConfig.Chunk0B00505B().ReadWrite(node, rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
    }

    private static MemoryStream CreatePayload(Action<GbxWriter> write)
    {
        var payload = new MemoryStream();
        using var writer = new GbxWriter(payload);
        write(writer);
        writer.Write(sentinel);
        return payload;
    }

    private static void ReadChunk(MemoryStream payload, Action<GbxReaderWriter> readWrite)
    {
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        readWrite(rw);
        if (reader.ReadInt32() != sentinel || payload.Position != payload.Length)
            throw new InvalidDataException("Chunk did not consume its native payload exactly.");
    }

    private static async Task AssertRoundTrip(MemoryStream payload, Action<GbxReaderWriter> readWrite)
    {
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            readWrite(rw);
            writer.Write(sentinel);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
