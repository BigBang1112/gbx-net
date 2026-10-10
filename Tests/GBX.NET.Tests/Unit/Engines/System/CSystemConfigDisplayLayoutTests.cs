using GBX.NET.Engines.System;
using GBX.NET.Serialization;
using System.Text;

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
        if (version >= 3) await Assert.That(node.AdapterDesc).IsEqualTo("Adapter");
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
    [Arguments(false, true)]
    [Arguments(true, false)]
    public async Task DeprecatedDriverBooleanSlotsRoundTrip(bool disableShadowBuffer, bool ignoreDriverCrashes)
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(disableShadowBuffer);
            writer.Write(4);
            writer.Write(true);
            writer.Write(2);
            writer.Write(false);
            writer.Write(ignoreDriverCrashes);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013026();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DisableShadowBufferRaw).IsEqualTo(disableShadowBuffer);
        await Assert.That(node.VertexProcess).IsEqualTo(CSystemConfigDisplay.EVertexProcess.Software);
        await Assert.That(node.IgnoreDriverCrashesRaw).IsEqualTo(ignoreDriverCrashes);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ColorWriteMaskRoundTripsBoolean(bool value)
    {
        using var payload = CreatePayload(writer => writer.Write(value));
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013004();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.DisableColorWMask).IsEqualTo(value);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0x004, -1)]
    [Arguments(0x004, 1700652)]
    [Arguments(0x017, -1)]
    [Arguments(0x017, 1700652)]
    public async Task BooleanSettingsNormalizeNoncanonicalTrueValues(int id, int value)
    {
        using var payload = CreatePayload(writer => writer.Write(value));
        var node = new CSystemConfigDisplay();
        Action<GbxReaderWriter> readWrite = context =>
        {
            if (id == 0x004) new CSystemConfigDisplay.Chunk0B013004().ReadWrite(node, context);
            else new CSystemConfigDisplay.Chunk0B013017().ReadWrite(node, context);
        };
        ReadChunk(payload, readWrite);
        await Assert.That(id == 0x004 ? node.DisableColorWMask : node.EnableRenderReadBack).IsTrue();
        using var normalized = CreatePayload(writer => writer.Write(true));
        await AssertRoundTrip(normalized, readWrite);
    }

    [Test]
    [Arguments(0x004, false)]
    [Arguments(0x004, true)]
    [Arguments(0x017, false)]
    [Arguments(0x017, true)]
    public async Task BooleanSettingsReadTextTokensAndExportBinary(int id, bool value)
    {
        using var payload = new MemoryStream([(byte)GbxFormat.Text, .. Encoding.UTF8.GetBytes($"{value}\r\n{sentinel}\r\n")]);
        using var reader = new GbxReader(payload);
        reader.ReadFormatByte();
        using var rw = new GbxReaderWriter(reader);
        var node = new CSystemConfigDisplay();
        var colorMask = new CSystemConfigDisplay.Chunk0B013004();
        var readBack = new CSystemConfigDisplay.Chunk0B013017();
        Action<GbxReaderWriter> readWrite = context =>
        {
            if (id == 0x004) colorMask.ReadWrite(node, context);
            else readBack.ReadWrite(node, context);
        };
        readWrite(rw);
        await Assert.That(id == 0x004 ? node.DisableColorWMask : node.EnableRenderReadBack).IsEqualTo(value);
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);

        using var saved = new MemoryStream();
        using var writer = new GbxWriter(saved);
        using var writeContext = new GbxReaderWriter(writer);
        readWrite(writeContext);
        await Assert.That(saved.ToArray()).IsEquivalentTo(BitConverter.GetBytes(value ? 1 : 0), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, true)]
    [Arguments(true, false)]
    public async Task DeprecatedDriverBooleanSlotsReadTextTokens(bool disableShadowBuffer, bool ignoreDriverCrashes)
    {
        using var payload = new MemoryStream([(byte)GbxFormat.Text, .. Encoding.UTF8.GetBytes($"{disableShadowBuffer}\r\n4\r\nTrue\r\n2\r\nFalse\r\n{ignoreDriverCrashes}\r\n{sentinel}\r\n")]);
        using var reader = new GbxReader(payload);
        reader.ReadFormatByte();
        using var rw = new GbxReaderWriter(reader);
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013026();
        chunk.ReadWrite(node, rw);
        await Assert.That(node.DisableShadowBufferRaw).IsEqualTo(disableShadowBuffer);
        await Assert.That(node.GpuSync0).IsEqualTo(CSystemConfigDisplay.EGpuSync.Immediate);
        await Assert.That(node.EmulateCursorGDI).IsTrue();
        await Assert.That(node.VertexProcess).IsEqualTo(CSystemConfigDisplay.EVertexProcess.Software);
        await Assert.That(node.OptimizePartialDynaGeom).IsFalse();
        await Assert.That(node.IgnoreDriverCrashesRaw).IsEqualTo(ignoreDriverCrashes);
        await Assert.That(reader.ReadInt32()).IsEqualTo(sentinel);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var writeContext = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, writeContext);
            writer.Write(sentinel);
        }
        var restored = new CSystemConfigDisplay();
        ReadChunk(saved, context => chunk.ReadWrite(restored, context));
        await GbxAssert.AreDeeplyEqual(node, restored);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RenderReadBackRoundTripsBoolean(bool value)
    {
        using var payload = CreatePayload(writer => writer.Write(value));
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013017();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.EnableRenderReadBack).IsEqualTo(value);
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
        await Assert.That(node.EverywhereReflect).IsEqualTo(version >= 1
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

    [Test]
    public async Task LegacyCombinedSettingsKeepLightingAndFilteringInTheirNativeSlots()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(new Int2(800, 600));
            foreach (var value in new[] { 2, 3, 1, 4, 2, 75, 1, 0, 2, 5, 3, 1, 7, 0, 2, 1 })
                writer.Write(value);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013000();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.ScreenSizeFS).IsEqualTo(new Int2(800, 600));
        await Assert.That(node.MaxFiltering).IsEqualTo(2);
        await Assert.That(node.HL_Quality).IsEqualTo(3);
        await Assert.That(node.GpuSync).IsEqualTo(CSystemConfigDisplay.EGpuSyncOld.Immediate);
        await Assert.That(node.VertexProcess).IsEqualTo(CSystemConfigDisplay.EVertexProcess.Software);
        await Assert.That(node.OptimizePartialDynaGeom).IsTrue();
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyLightMapSettingsKeepUltraAndLightIndexSeparate()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(2);
            writer.Write(true);
            writer.Write(false);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013024();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.LightMapQualityOld).IsEqualTo(CSystemConfigDisplay.ELightMapQualityOld._4k);
        await Assert.That(node.LightMapQualityUltra).IsTrue();
        await Assert.That(node.LightMapLightIndex).IsFalse();
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task IntermediateShaderQualityPreservesItsOriginalEnumValue()
    {
        using var payload = CreatePayload(writer =>
        {
            foreach (var value in new[] { 1, 1, 4, 3, 5, 4, 6 }) writer.Write(value);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013028();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Preset).IsEqualTo(CSystemConfigDisplay.EPreset.VeryNice);
        await Assert.That(node.ShaderQualityIntermediate).IsEqualTo(5);
        await Assert.That(node.FilterAnisoQ).IsEqualTo(CSystemConfigDisplay.EFilterAnisoQ.Aniso_16x_everywhere);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task AdditionalGpuSynchronizationKeepsEachGpuSetting()
    {
        using var payload = CreatePayload(writer =>
        {
            writer.Write(2);
            writer.Write(1);
            writer.Write(0);
        });
        var node = new CSystemConfigDisplay();
        var chunk = new CSystemConfigDisplay.Chunk0B013037();
        ReadChunk(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.GpuSync1).IsEqualTo(CSystemConfigDisplay.EGpuSync._2Frames);
        await Assert.That(node.GpuSync2).IsEqualTo(CSystemConfigDisplay.EGpuSync._3Frames);
        await Assert.That(node.GpuSync3).IsEqualTo(CSystemConfigDisplay.EGpuSync.None);
        await AssertRoundTrip(payload, rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task GameContextSelectsNativeDefaultsAndChunkVersions()
    {
        var forever = new CSystemConfigDisplay(GameVersion.TMF);
        var maniaplanet = new CSystemConfigDisplay(GameVersion.MP4);
        var trackmania = new CSystemConfigDisplay(GameVersion.TM2020);
        await Assert.That(forever.IgnoreDriverCrashes).IsTrue();
        await Assert.That(forever.LightMapCompute).IsEqualTo(CSystemConfigDisplay.ELightMapCompute.Normal);
        await Assert.That(forever.FilterAnisoQ).IsEqualTo(CSystemConfigDisplay.EFilterAnisoQ.Anisotropic__4x);
        await Assert.That(maniaplanet.LightMapQuality).IsEqualTo(CSystemConfigDisplay.ELightMapQuality.VeryFast);
        await Assert.That((int)maniaplanet.GeometryQuality).IsEqualTo(4);
        await Assert.That(trackmania.GeometryQuality).IsEqualTo(CSystemConfigDisplay.EGeometryQuality.Nice);
        await Assert.That(trackmania.LightMapQuality).IsEqualTo(CSystemConfigDisplay.ELightMapQuality.VeryFast);
        await Assert.That(trackmania.FxMotionBlur).IsEqualTo(CSystemConfigDisplay.EFxMotionBlur.Off);
        await Assert.That(trackmania.FxBlur).IsEqualTo(CSystemConfigDisplay.EFxBlur.Off);
        await Assert.That(trackmania.AutomaticEnabled).IsTrue();
        await Assert.That(trackmania.AutomaticMinFps).IsEqualTo(30);
        await Assert.That(trackmania.LightMapLightIndex).IsTrue();
        await Assert.That(new CSystemConfigDisplay().AutomaticEnabled).IsFalse();
        await Assert.That(new CSystemConfigDisplay.Chunk0B013036(GameVersion.TMT).Version).IsEqualTo(1);
        await Assert.That(new CSystemConfigDisplay.Chunk0B013036(GameVersion.MP4).Version).IsEqualTo(3);
        await Assert.That(new CSystemConfigDisplay.Chunk0B013036(GameVersion.TM2020).Version).IsEqualTo(3);
        await Assert.That(new CSystemConfigDisplay.Chunk0B01303E(GameVersion.TM2020).Version).IsEqualTo(1);
    }

    [Test]
    public async Task UnsupportedDisplayVersionStopsBeforeReadingSettings()
    {
        using var payload = CreatePayload(writer => writer.Write(4));
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<NotSupportedException>(() => new CSystemConfigDisplay.Chunk0B013036().ReadWrite(new CSystemConfigDisplay(), rw));
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
