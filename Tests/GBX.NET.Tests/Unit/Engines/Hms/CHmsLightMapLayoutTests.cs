using GBX.NET.Components;
using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Hms;

[Category("Unit")]
public class CHmsLightMapLayoutTests
{
    [Test]
    [Arguments(0x000, 13)]
    [Arguments(0x001, 1)]
    [Arguments(0x002, 1)]
    [Arguments(0x003, 1)]
    [Arguments(0x004, 15)]
    [Arguments(0x005, 1)]
    [Arguments(0x006, 3)]
    [Arguments(0x007, 5)]
    [Arguments(0x00A, 19)]
    [Arguments(0x00B, 7)]
    [Arguments(0x00C, 3)]
    [Arguments(0x00D, 3)]
    [Arguments(0x00F, 3)]
    [Arguments(0x010, 21)]
    [Arguments(0x011, 8)]
    [Arguments(0x013, 1)]
    [Arguments(0x014, 1)]
    [Arguments(0x015, 9)]
    [Arguments(0x016, 10)]
    [Arguments(0x018, 6)]
    [Arguments(0x019, 2)]
    [Arguments(0x01A, 3)]
    [Arguments(0x01B, 4)]
    [Arguments(0x01E, 11)]
    [Arguments(0x01F, 1)]
    [Arguments(0x020, 4)]
    [Arguments(0x021, 7)]
    [Arguments(0x022, 1)]
    [Arguments(0x023, 1)]
    [Arguments(0x024, 1)]
    [Arguments(0x026, 1)]
    [Arguments(0x028, 6)]
    public async Task ReferenceChunksPreserveFixedCountsAndExternalFiles(int offset, int count)
    {
        var files = Files(count);
        var payload = Payload(w =>
        {
            for (var i = 1; i <= count; i++) w.Write(i);
            if (offset is 0x00D or 0x00F) w.Write(uint.MaxValue);
            if (offset == 0x00F) w.Write(0x81234567u);
        });
        var node = new CHmsLightMap();
        var chunk = Chunk(offset);
        await RoundTrip(payload, files, rw => chunk.ReadWrite(node, rw));

        if (offset is 0x00C or 0x00D or 0x00F)
        {
            await Assert.That(node.BitmapSM_ColorPeeledFile).IsSameReferenceAs(files[1]);
            await Assert.That(node.BitmapLM_ILightInputFile).IsSameReferenceAs(files[2]);
            await Assert.That(node.BitmapLM_ILightDirFile).IsSameReferenceAs(files[3]);
        }
        if (offset == 0x028)
        {
            await Assert.That(node.BitmapLM_LListUVFile).IsSameReferenceAs(files[1]);
            await Assert.That(node.BitmapLM_LListWFile).IsSameReferenceAs(files[2]);
            await Assert.That(node.BitmapLM_LocalDirectFile).IsSameReferenceAs(files[3]);
        }
        if (offset == 0x01E)
        {
            await Assert.That(((CHmsLightMap.Chunk0602101E)chunk).U01![10].File).IsSameReferenceAs(files[11]);
        }
        if (offset == 0x006)
        {
            await Assert.That(node.BitmapLightSumBumpLegacy![2].File).IsSameReferenceAs(files[3]);
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
    [Arguments(7)]
    public async Task ProbeGridFilesFollowEachVersionBranch(int version)
    {
        var files = Files(version + 1);
        var payload = Payload(w =>
        {
            w.Write(version);
            for (var i = 1; i <= version + 1; i++) w.Write(i);
        });
        var node = new CHmsLightMap();
        var chunk = new CHmsLightMap.Chunk06021029();
        await RoundTrip(payload, files, rw => chunk.ReadWrite(node, rw));

        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP3 | GameVersion.TMT | GameVersion.TM2020);
        await Assert.That(node.BitmapProbeGridAmbSVFile).IsSameReferenceAs(version >= 6 ? files[7] : null);
        await Assert.That(node.BitmapProbeGridAmbSV_Spread1File).IsSameReferenceAs(version >= 7 ? files[8] : null);
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(1, 2)]
    [Arguments(2, 5)]
    [Arguments(3, 5)]
    [Arguments(4, 6)]
    [Arguments(5, 7)]
    public async Task ObsoleteShadersKeepReferencesAndUnsignedPayload(int version, int count)
    {
        var files = Files(count);
        var payload = Payload(w =>
        {
            w.Write(version);
            var shaderCount = version >= 2 ? 5 : version + 1;
            for (var i = 1; i <= shaderCount; i++) w.Write(i);
            if (version >= 3) w.Write(uint.MaxValue);
            if (version >= 4) w.Write(6);
            if (version >= 5) w.Write(7);
        });
        var node = new CHmsLightMap();
        var chunk = new CHmsLightMap.Chunk0602102A();
        await RoundTrip(payload, files, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.MP3 | GameVersion.TMT);
    }

    [Test]
    [Arguments(0, 7)]
    [Arguments(1, 8)]
    public async Task SpriteFilesKeepVersionOneInsertionBeforeTheFixedList(int version, int count)
    {
        var files = Files(count);
        var payload = Payload(w =>
        {
            w.Write(version);
            for (var i = 1; i <= count; i++) w.Write(i);
        });
        var node = new CHmsLightMap();
        var chunk = new CHmsLightMap.Chunk0602102B();
        await RoundTrip(payload, files, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.BitmapSprite3x3_LightFile).IsSameReferenceAs(files[1]);
        await Assert.That(node.BitmapSprite_PosAndRadiusFile).IsSameReferenceAs(files[2]);
        await Assert.That(node.BitmapSprite_ILightDirFile).IsSameReferenceAs(files[3]);
        await Assert.That(chunk.U02![0].File).IsSameReferenceAs(files[version == 0 ? 4 : 5]);
    }

    [Test]
    public async Task LightingSettingsKeepNativeDefaultsAndRawEnumBits()
    {
        var node = new CHmsLightMap();
        await Assert.That(node.SRGB).IsTrue();
        await Assert.That(node.UseHDR).IsFalse();
        await Assert.That(node.ClampAmb_PosY).IsFalse();
        await Assert.That(node.ClampDir_PosY).IsFalse();
        await Assert.That(node.IsSpriteLDirInAlpha).IsFalse();
        await Assert.That(node.BackgroundUseClouds).IsFalse();
        await Assert.That(node.GameTimerSeconds).IsEqualTo(250u);
        await Assert.That(node.CompressMode).IsEqualTo(CHmsLightMap.ECompressMode.Ldr_DXT1);
        await Assert.That(node.StoreLDir0).IsEqualTo(CHmsLightMap.EStoreLDir0.Sinc);
        await Assert.That(new CHmsLightMap.Chunk06021029(GameVersion.TM2020).Version).IsEqualTo(7);
        await Assert.That(new CHmsLightMap.Chunk0602102A().Version).IsEqualTo(5);
        await Assert.That(new CHmsLightMap.Chunk0602102B(GameVersion.TM2020).Version).IsEqualTo(1);

        await RoundTrip(Payload(w => { w.Write(1); w.Write(0); }),
            Files(0), rw => new CHmsLightMap.Chunk0602100E().ReadWrite(node, rw));
        await Assert.That(node.ClampAmb_PosY).IsTrue();
        await Assert.That(node.ClampDir_PosY).IsFalse();

        await RoundTrip(Payload(w => { w.Write(1); w.Write(0); w.Write(uint.MaxValue); }),
            Files(0), rw => new CHmsLightMap.Chunk06021017().ReadWrite(node, rw));
        await Assert.That(node.UseHDR).IsTrue();
        await Assert.That(node.SRGB).IsFalse();
        await Assert.That((int)node.CompressMode).IsEqualTo(-1);

        await RoundTrip(Payload(w => w.Write(0x81234567u)),
            Files(0), rw => new CHmsLightMap.Chunk06021025().ReadWrite(node, rw));
        await Assert.That((int)node.StoreLDir0).IsEqualTo(unchecked((int)0x81234567));

        await RoundTrip(Payload(w => { w.Write(1); w.Write(uint.MaxValue); }),
            Files(0), rw => new CHmsLightMap.Chunk06021027().ReadWrite(node, rw));
        await Assert.That(node.BackgroundUseClouds).IsTrue();
        await Assert.That(node.GameTimerSeconds).IsEqualTo(uint.MaxValue);
    }

    [Test]
    [Arguments(0x008)]
    [Arguments(0x009)]
    [Arguments(0x012)]
    [Arguments(0x01C)]
    [Arguments(0x01D)]
    public async Task LegacyBooleansKeepFourByteWidths(int offset)
    {
        var files = Files(offset == 0x012 ? 1 : 0);
        var payload = Payload(w =>
        {
            w.Write(1);
            if (offset == 0x012) w.Write(1);
        });
        var node = new CHmsLightMap();
        var chunk = Chunk(offset);
        await RoundTrip(payload, files, rw => chunk.ReadWrite(node, rw));
        if (offset == 0x008) await Assert.That(node.ClampDir_PosY).IsTrue();
        if (offset == 0x009)
        {
            await Assert.That(node.UseHDR).IsTrue();
            await Assert.That(node.CompressMode).IsEqualTo(CHmsLightMap.ECompressMode.Scale_sRGB_DXT1);
        }
        if (offset == 0x012) await Assert.That(node.SRGB).IsTrue();
        if (offset == 0x01C)
        {
            await Assert.That(node.StoreLDir0Legacy).IsTrue();
            await Assert.That(node.StoreLDir0).IsEqualTo(CHmsLightMap.EStoreLDir0.HalfPlusHalfCos);
        }
        if (offset == 0x01D) await Assert.That(node.IsSpriteLDirInAlpha).IsTrue();
    }

    private static Chunk<CHmsLightMap> Chunk(int offset)
        => (Chunk<CHmsLightMap>)Activator.CreateInstance(typeof(CHmsLightMap).GetNestedType($"Chunk{0x06021000 + offset:X8}")!)!;

    private static Dictionary<int, GbxRefTableNode> Files(int count)
    {
        var table = new GbxRefTable();
        return Enumerable.Range(1, count).ToDictionary(i => i,
            i => (GbxRefTableNode)new GbxRefTableFile(table, 0, true, $"LightMap{i}.Gbx"));
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Dictionary<int, GbxRefTableNode> files, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        reader.LoadRefTable(files);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
