using GBX.NET.Engines.System;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.System;

[Category("Unit")]
public class CSystemConfigDisplayLayoutTests
{
    private const int sentinel = 0x11223344;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task DisplaySettingsPreserveVersionBranchesAndUnknownFields(int version)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(version);
            writer.Write(new Int2(1920, 1080));
            writer.Write(2); // Window-size preset.
            writer.Write(0); // Deprecated color depth.
            writer.Write(4); // Antialiasing: 8 samples.
            writer.Write(144);
            writer.Write(1); // Display sync: 1 interval.
            writer.Write(2); // Display mode: WindowedFull.
            writer.Write(1); // Triple buffering: On.
            if (version >= 1) writer.Write(1); // D3D11.
            if (version >= 2)
            {
                foreach (var value in new[] { 11, 12, 13, 14 }) writer.Write(value);
            }
            if (version >= 3) writer.Write("Adapter");
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013036();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ScreenSizeWin).IsEqualTo(2);
        await Assert.That(node.Antialiasing).IsEqualTo(CSystemConfigDisplay.EAntialiasing._8Samples);
        await Assert.That(node.RefreshRate).IsEqualTo(144);
        await Assert.That(node.TripleBuffer).IsEqualTo(CSystemConfigDisplay.ETripleBuffer.On);
        if (version >= 1) await Assert.That(node.RenderingApi).IsEqualTo(CSystemConfigDisplay.ERenderingApi.D3D11);
        if (version >= 2)
            await Assert.That(new[] { chunk.U01, chunk.U02, chunk.U03, chunk.U04 })
                .IsEquivalentTo(new[] { 11, 12, 13, 14 }, CollectionOrdering.Matching);
        if (version >= 3) await Assert.That(node.Adapter).IsEqualTo("Adapter");
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyQualityEnumsKeepTheirOriginalValues()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(2); // AutoScale: Nicer.
            writer.Write(true);
            foreach (var value in new[] { 5, 3, 4, 5, 6 }) writer.Write(value);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013022();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AutoScale).IsEqualTo(CSystemConfigDisplay.EAutoScale.Nicer);
        await Assert.That(node.PresetOld).IsEqualTo(CSystemConfigDisplay.EPresetOld.VeryHighQuality);
        await Assert.That(node.TexturesQuality).IsEqualTo(CSystemConfigDisplay.ETexturesQuality.High);
        await Assert.That(node.ShaderQualityOld).IsEqualTo(CSystemConfigDisplay.EShaderQualityOld.PC3High);
        await Assert.That(node.Shadows).IsEqualTo(CSystemConfigDisplay.EShadows.Complex);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ModernQualityEnumsUseModernPresetValues()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(1);
            writer.Write(false);
            foreach (var value in new[] { 4, 2, 3, 4, 5 }) writer.Write(value);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B01302A();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Preset).IsEqualTo(CSystemConfigDisplay.EPreset.VeryNice);
        await Assert.That(node.ShaderQuality).IsEqualTo(CSystemConfigDisplay.EShaderQ.Very_Nice);
        await Assert.That(node.Shadows).IsEqualTo(CSystemConfigDisplay.EShadows.VeryHigh);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task DeprecatedBooleanSlotsPreserveNoncanonicalIntegers()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(unchecked((int)0x87654321));
            writer.Write(4);
            writer.Write(true);
            writer.Write(2);
            writer.Write(false);
            writer.Write(12345);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013026();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DisableShadowBufferRaw).IsEqualTo(unchecked((int)0x87654321));
        await Assert.That(node.VertexProcess).IsEqualTo(CSystemConfigDisplay.EVertexProcess.Software);
        await Assert.That(node.IgnoreDriverCrashesRaw).IsEqualTo(12345);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ColorWriteMaskPreservesNativeBooleanStorage()
    {
        using var payload = CreatePayload(writer => writer.Write(0x12345678));
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013004();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DisableColorWMask).IsEqualTo(0x12345678);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyGpuSynchronizationKeepsHalfFrameValues()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write(2); // Old enum: 2.5 frames. Modern value 2 means 2 frames.
            writer.Write(false);
            writer.Write(1);
            writer.Write(true);
            writer.Write(false);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013003();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.GpuSync).IsEqualTo(CSystemConfigDisplay.EGpuSyncOld._2AndHalfFrames);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task TrackmaniaReflectionSettingUsesVersionOne(int version)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(version);
            writer.Write(1); // FxBlur: On.
            if (version >= 1) writer.Write(1); // ReflectEverywhere: Enabled.
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B01303E();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.FxBlur).IsEqualTo(CSystemConfigDisplay.EFxBlur.On);
        await Assert.That(node.ReflectEverywhere).IsEqualTo(version >= 1
            ? CSystemConfigDisplay.EEverywhereReflect.Enabled
            : CSystemConfigDisplay.EEverywhereReflect.None);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task UnsupportedBlurVersionStopsBeforeReadingSettings()
    {
        using var payload = CreatePayload(writer => writer.Write(2));
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<NotSupportedException>(() => new CSystemConfigDisplay.Chunk0B01303E().ReadWrite(new CSystemConfigDisplay(), rw));
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
    }

    [Test]
    public async Task AutomaticSettingsIncludeMinimumFrameRate()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(true);
            writer.Write(60);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B01303F();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AutomaticEnabled).IsTrue();
        await Assert.That(node.AutomaticMinFps).IsEqualTo(60);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
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
