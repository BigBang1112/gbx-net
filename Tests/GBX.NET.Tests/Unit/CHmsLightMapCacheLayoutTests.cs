using GBX.NET.Engines.Hms;
using GBX.NET.Exceptions;
using GBX.NET.Extensions;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.IO.Compression;
using System.Text;

namespace GBX.NET.Tests.Unit;

[NotInParallel]
public class CHmsLightMapCacheLayoutTests
{
    [Test]
    [Arguments("MP3 001")]
    [Arguments("MP4 001")]
    [Arguments("MP4 002")]
    [Arguments("TM2020 001")]
    public async Task EmbeddedCacheInMapFixturesCanBeLoaded(string suffix)
    {
        using var zlib = new TestZLibScope();
        var map = Gbx.ParseNode<GBX.NET.Engines.Game.CGameCtnChallenge>(
            TestFiles.Gbx("CGameCtnChallenge", $"GBX-NET 2 CGameCtnChallenge {suffix}.Map.Gbx"));
        await Assert.That(map.LightmapCacheData).IsNotNull();
        var cache = map.LightmapCache;
        await Assert.That(cache).IsNotNull();
        await Assert.That(cache!.Chunks.Any(c => c.Id == 0x0602201A)).IsTrue();
        var chunk = (CHmsLightMapCache.Chunk0602201A)cache.Chunks.Single(c => c.Id == 0x0602201A);
        if (chunk.Version >= 3) await Assert.That(cache.Frames).IsNotNull();
        await Assert.That(cache.Mapping).IsNotNull();
        _ = cache.Mapping!.ZlibData2Decompressed1;
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task EarlyCacheVersionsKeepCloudBooleanAndOptionalAmbient(int version)
    {
        using var zlib = new TestZLibScope();
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0); // Maps.
            for (var i = 0; i < 8; i++) w.Write(i);
            for (var i = 0; i < 4; i++) w.Write(i + 0.5f);
            w.Write(1); // SkyUseClouds.
            w.Write(0);
            w.Write(1);
            WriteMapping(w, 0, 0);
            if (version >= 1) w.Write(1);
            if (version == 2) for (var i = 0; i < 3; i++) w.Write((short)0);
        });
        var cache = new CHmsLightMapCache();
        var chunk = new CHmsLightMapCache.Chunk0602201A();
        await RoundTrip(payload, rw => chunk.ReadWrite(cache, rw));
        await Assert.That(cache.SkyUseCloudsLegacy).IsTrue();
    }

    [Test]
    [Arguments(0x000)]
    [Arguments(0x001)]
    [Arguments(0x002)]
    [Arguments(0x003)]
    [Arguments(0x004)]
    [Arguments(0x005)]
    [Arguments(0x006)]
    [Arguments(0x007)]
    [Arguments(0x008)]
    [Arguments(0x009)]
    [Arguments(0x00A)]
    [Arguments(0x00C)]
    [Arguments(0x010)]
    public async Task LegacyMapRecordsKeepTheirNativeCountsAndWidths(int offset)
    {
        var payload = Payload(w =>
        {
            w.Write(3); // Lookback ID version.
            w.Write(uint.MaxValue); // Empty legacy collection ID.
            if (offset >= 5) w.Write(uint.MaxValue); // Empty decoration ID.
            w.Write(2); // Map count.
            var width = offset < 2 ? 2 : offset < 7 ? 3 : 5;
            for (var i = 0; i < 2 * width; i++) w.Write(100 + i);
            if (offset != 0) { w.Write(1); w.Write(32); }
            w.Write(25);
            w.Write(25);
            if (offset == 0) return;
            w.Write(0.5f);
            w.Write(0.75f);
            if (offset >= 3) w.Write(1); // SortMode.
            if (offset >= 4) w.Write(2); // AllocMode.
            if (offset >= 0xA) w.Write(1); // AllocModeT3.
            if (offset >= 6) w.Write(777); // Discarded legacy natural.
            if (offset >= 8) w.Write(1); // HasBumpLegacy.
            if (offset >= 9) w.Write(3.5f); // MaxHDRLegacy.
            if (offset >= 0xC) w.Write(3); // CompressMode.
            if (offset >= 0x10) w.Write(1); // BumpNorm.
        });
        var cache = new CHmsLightMapCache();
        var chunk = (Chunk<CHmsLightMapCache>)Activator.CreateInstance(
            typeof(CHmsLightMapCache).GetNestedType($"Chunk{0x06022000 + offset:X8}")!)!;
        await RoundTrip(payload, rw => chunk.ReadWrite(cache, rw));
        await Assert.That(cache.LightDirSampleCount).IsEqualTo(25);
        await Assert.That(cache.LightPntSampleCount).IsEqualTo(25);
        if (offset >= 7)
        {
            await Assert.That(cache.Maps![1].BlockPerMap).IsEqualTo(new Int2(105, 106));
            await Assert.That(cache.Maps[1].UsedBlockCount).IsEqualTo(107);
            await Assert.That(cache.Maps[1].OutsideBlockCount).IsEqualTo(new Int2(108, 109));
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task LegacyVersionedCachePreservesMappingAndHdrBranches(int version)
    {
        using var zlib = new TestZLibScope();
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(2); // SMap count.
            for (var i = 0; i < 10; i++) w.Write(100 + i);
            w.Write(1);
            foreach (var value in new[] { 32, 25, 25 }) w.Write(value);
            w.Write(0.5f);
            w.Write(0.75f);
            foreach (var value in new[] { 1, 2, 1 }) w.Write(value);
            w.Write(3.5f); // MaxHDRLegacy.
            w.Write(3); // CompressMode.
            w.Write(1); // BumpNorm.
            if (version == 1) w.Write(2048);
            if (version >= 2) WriteMapping(w, 0, 0);
            if (version >= 4) w.Write(1);
            if (version >= 5) w.Write(4.5f);
            if (version >= 6) w.Write(0);
        });
        var node = new CHmsLightMapCache();
        var chunk = new CHmsLightMapCache.Chunk06022014();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Maps![1].UsedBlockCount).IsEqualTo(107);
        if (version >= 5) await Assert.That(node.MaxHDRMoodLegacy).IsEqualTo(4.5f);
        if (version >= 6) await Assert.That(node.SpriteOriginY_WasWronglyTop).IsFalse();
    }

    [Test]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    public async Task FrameVersionBranchesPreserveFollowingFields(int version)
    {
        using var zlib = new TestZLibScope();
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1); // SMap count.
            for (var i = 0; i < 5; i++) w.Write(100 + i);
            foreach (var value in new[] { 32, 25, 25, 1, 2, 1, 3, 1 }) w.Write(value);
            if (version >= 6) w.Write(4); // Bump.
            w.Write(2); // Frame count, so a missing byte also breaks the second frame.
            for (var frame = 0; frame < 2; frame++)
            {
                w.Write(0); // Time-of-day archive version.
                w.Write(uint.MaxValue);
                if (version >= 10) w.Write(12.5f + frame);
                foreach (var value in new[] { 3f, 4f, 0.5f, 0.75f }) w.Write(value + frame);
                w.Write(1); // SkyUseClouds.
                for (var i = 0; i < 3; i++) w.Write((short)0); // Half floats.
                if (version >= 4)
                {
                    w.Write(1); // StoreLAmbient.
                    w.Write(version >= 13 ? 2 : 1); // LocalLight_Storage / legacy bool.
                }
                if (version >= 12) w.Write(2); // LocalLight_Switch.
                if (version is 6 or 7 or >= 9)
                    foreach (var value in new[] { 5f, 6f, 7f }) w.Write(value + frame);
                if (version == 8)
                    foreach (var value in new[] { 8f, 9f, 10f, 11f }) w.Write(value + frame);
                if (version >= 7) w.Write(4); // Frame bump mode.
                if (version >= 14) w.Write((byte)(0xA0 + frame));
            }
            w.Write(1);
            w.Write(0);
            WriteMapping(w, 0, 0);
            w.Write(1); // GPU platform.
            if (version >= 5) w.Write(2.25f);
            if (version >= 11)
            {
                w.Write(0x12345678);
                w.Write(0x23456789);
            }
            if (version >= 15) w.Write(0x3456789A);
        });
        var node = new CHmsLightMapCache();
        var chunk = new CHmsLightMapCache.Chunk0602201A();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Frames!.Length).IsEqualTo(2);
        await Assert.That(node.Frames[1].BounceFactor).IsEqualTo(1.5f);
        if (version >= 13)
            await Assert.That(node.Frames[1].LocalLight_Storage).IsEqualTo(CHmsLightMapCache.ELocalLightStorage.OnlyRgbAccum);
        if (version >= 14) await Assert.That(node.Frames[1].U08).IsEqualTo((byte)0xA1);
        await Assert.That(node.LightAmbSampleCount).IsEqualTo(32);
    }

    [Test]
    [Arguments(0)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task MappingCoordinatesUsePlanesBeforeVersion7AndPackedRecordsAfterwards(int version)
    {
        using var zlib = new TestZLibScope();
        var payload = Payload(w => WriteMapping(w, version, 2));
        var mapping = new CHmsLightMapCache.SMapping();
        using (var input = new MemoryStream(payload))
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            mapping.ReadWrite(rw);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        await Assert.That(mapping.ZlibData2!.Parsed).IsFalse();
        await Assert.That(mapping.ZlibData2Decompressed1!).IsEquivalentTo(new short[] { 1, 2 }, CollectionOrdering.Matching);
        await Assert.That(mapping.ZlibData2Decompressed2!).IsEquivalentTo(new short[] { 11, 12 }, CollectionOrdering.Matching);
        await Assert.That(mapping.ZlibData2Decompressed3!).IsEquivalentTo(new short[] { 21, 22 }, CollectionOrdering.Matching);
        if (version >= 7)
            await Assert.That(mapping.ZlibData2Decompressed4!).IsEquivalentTo(new short[] { -1, -2 }, CollectionOrdering.Matching);
        else
            await Assert.That(mapping.ZlibData2Decompressed4).IsNull();
        // After lazy parsing, writing exercises reconstruction instead of copying compressed bytes.
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) mapping.ReadWrite(rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task CacheIdDynamicTimeKeepsItsFullUInt32Range(int version)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(0x123456789ABCDEF0ul);
            w.Write(3); // Lookback ID version.
            w.Write(12); // Numeric collection ID, without lookback strings.
            w.Write(uint.MaxValue); // Empty decoration ID.
            w.Write(2);
            if (version < 4) w.Write(0);
            if (version >= 2) { w.Write(0); w.Write(uint.MaxValue); }
            if (version >= 3) { w.Write(0); w.Write(0xFEDCBA98u); }
            if (version >= 5) { w.Write(3); w.Write(Encoding.UTF8.GetBytes("uid")); }
        });
        var node = new CHmsLightMapCache();
        var chunk = new CHmsLightMapCache.Chunk06022015();
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        if (version >= 3) await Assert.That(node.DynamicTime).IsEqualTo(0xFEDCBA98u);
    }

    [Test]
    public async Task LegacySkippabilityAndWriterMetadataMatchNativeFlags()
    {
        for (var offset = 0; offset <= 0x1A; offset++)
        {
            var chunk = (Chunk<CHmsLightMapCache>)Activator.CreateInstance(
                typeof(CHmsLightMapCache).GetNestedType($"Chunk{0x06022000 + offset:X8}")!)!;
            await Assert.That(chunk is ISkippableChunk).IsEqualTo(offset >= 8);
            var writer = offset is 0xB or 0xF or 0x13 or >= 0x15;
            var games = writer ? GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020
                : offset switch
                {
                    5 => GameVersion.VSK5,
                    7 => GameVersion.TMF,
                    _ => GameVersion.Unspecified
                };
            await Assert.That(chunk.GameVersion).IsEqualTo(games);
        }
    }

    private static void WriteMapping(BinaryWriter w, int version, int count)
    {
        w.Write(version);
        if (version >= 8) w.Write(123);
        w.Write(256);
        w.Write(128);
        for (var i = 0; i < 6; i++) w.Write((float)i);
        if (version >= 6) w.Write(1);
        w.Write(count);
        if (version >= 1) WriteCompressed(w, Payload(p => { for (var i = 0; i < count; i++) p.Write(0.5f + i); }));
        WriteCompressed(w, Payload(p =>
        {
            if (version < 7)
            {
                for (var component = 0; component < 3; component++)
                    for (var i = 0; i < count; i++) p.Write((short)(component * 10 + i + 1));
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    for (var component = 0; component < 3; component++) p.Write((short)(component * 10 + i + 1));
                    p.Write((short)(-1 - i));
                }
            }
        }));
        WriteCompressed(w, new byte[count * 4]);
        WriteCompressed(w, new byte[count * 4]);
        if (version >= 4) w.Write(0);
        if (version >= 2) WriteCompressed(w, new byte[4]); // Empty data, or zero arrays at v5+.
    }

    private static void WriteCompressed(BinaryWriter writer, byte[] payload)
    {
        using var stream = new MemoryStream();
        using (var compressed = new ZLibStream(stream, CompressionLevel.Optimal, leaveOpen: true)) compressed.Write(payload);
        writer.Write(payload.Length);
        writer.Write((int)stream.Length);
        writer.Write(stream.ToArray());
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true)) suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);
        serialize(rw);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var writerWriter = new GbxReaderWriter(writer)) serialize(writerWriter);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }

    private sealed class TestZLibScope : IZLib, IDisposable
    {
        private readonly IZLib? previous;

        public TestZLibScope()
        {
            try { previous = Gbx.ZLib; }
            catch (ZLibNotDefinedException) { }
            Gbx.ZLib = this;
        }

        public void Dispose() => Gbx.ZLib = previous!;

        public void Compress(Stream input, Stream output)
        {
            using var stream = new ZLibStream(output, CompressionLevel.Optimal, leaveOpen: true);
            // .NET 8 needs a write to emit an empty zlib stream; CopyTo skips empty input.
            stream.Write(ReadOnlySpan<byte>.Empty);
            input.CopyTo(stream);
        }

        public void Decompress(Stream input, Stream output)
        {
            using var stream = Decompress(input);
            stream.CopyTo(output);
        }

        public Stream Decompress(Stream input) => new ZLibStream(input, CompressionMode.Decompress, leaveOpen: true);
    }
}
