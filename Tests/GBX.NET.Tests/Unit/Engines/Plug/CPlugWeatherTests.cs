using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugWeatherTests
{
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
    [Arguments(0x00B)]
    [Arguments(0x00C)]
    [Arguments(0x00D)]
    [Arguments(0x00E)]
    [Arguments(0x00F)]
    [Arguments(0x010)]
    [Arguments(0x011)]
    [Arguments(0x012)]
    [Arguments(0x013)]
    [Arguments(0x014)]
    [Arguments(0x015)]
    [Arguments(0x016)]
    [Arguments(0x017)]
    [Arguments(0x018)]
    public async Task ReadWrite_Chunk_PreservesNativePayloadAndExternalReferences(int offset)
    {
        using var payload = new ObjectModelPayload();
        WriteChunk(payload, offset);
        payload.Finish();
        var node = new CPlugWeather();
        var chunk = (Chunk<CPlugWeather>)node.Chunks.Create(CPlugWeather.Id + (uint)offset);
        payload.Read(rw => chunk.ReadWrite(node, rw));

        if (offset is 0 or 1 or 2 or 3 or 4 or 5 or 8 or 9 or 11)
        {
            await Assert.That(node.References.MaterialSky_Night.File!.FilePath).IsEqualTo("Night.Material.Gbx");
            await Assert.That(node.References.MaterialSky_SunFall.File!.FilePath).IsEqualTo("SunFall.Material.Gbx");
            await Assert.That(node.References.MaterialSea0.File!.FilePath).IsEqualTo("Sea0.Material.Gbx");
            await Assert.That(node.References.MaterialSea1.File!.FilePath).IsEqualTo(offset == 11 ? "Sea1.Material.Gbx" : "Sea0.Material.Gbx");
        }
        if (offset is 0 or 1 or 2 or 3 or 4 or 5 or 8 or 9 or 10 or 12 or 14)
        {
            await Assert.That(node.FogRGB).IsEqualTo(new Vec3(0.1f, 0.2f, 0.3f));
            await Assert.That(node.FogLinearStart).IsEqualTo(4.5f);
            await Assert.That(node.FogLinearEnd).IsEqualTo(5.5f);
            await Assert.That(node.FogExpDensity).IsEqualTo(6.5f);
            await Assert.That(node.FogFlags).IsEqualTo(13u);
            await Assert.That(node.DayFogColor).IsEqualTo(offset <= 3 ? node.FogRGB : new Vec3(1.1f, 1.2f, 1.3f));
            await Assert.That(node.DayFogFlags).IsEqualTo(offset <= 3 ? 13u : 6u);
            await Assert.That(node.References.ImageLightAmb.File!.FilePath).IsEqualTo("Ambient.Image.Gbx");
        }
        if (offset is 6 or 13)
        {
            await Assert.That(node.SeaTwkReflecIntensNight).IsEqualTo(1.5f);
            await Assert.That(node.SeaTwkReflecIntensDay).IsEqualTo(2.5f);
            await Assert.That(node.LDirSpecIntens).IsEqualTo(new Vec2(3.5f, 4.5f));
            await Assert.That(node.LDirSpecPower).IsEqualTo(new Vec2(5.5f, 6.5f));
            await Assert.That(node.SeaTwkWaterColor_Day).IsEqualTo(new Vec3(10.5f, 11.5f, 12.5f));
            if (offset == 13)
            {
                await Assert.That(node.SeaTwkReflecIntensMidNight).IsEqualTo(13.5f);
                await Assert.That(node.SeaTwkReflecIntensTMDay).IsEqualTo(16.5f);
                await Assert.That(node.SeaTwkReflecIntensTMmidDay).IsEqualTo(18.5f);
            }
        }
        if (offset is 16 or 18 or 21)
        {
            await Assert.That(node.Clouds!.SolidFids!.Count).IsEqualTo(2);
            await Assert.That(node.Clouds.SolidFids!.Get(1).File!.FilePath).IsEqualTo("Cloud2.Solid.Gbx");
            await Assert.That(node.Clouds.BottomNearZ).IsEqualTo(1200.5f);
            await Assert.That(node.Clouds.BottomFarZ).IsEqualTo(offset == 16 ? 1200.5f : 2400.5f);
            if (offset != 16)
            {
                await Assert.That(node.Clouds.References.ImageColorMax.File!.FilePath).IsEqualTo("CloudMax.Image.Gbx");
            }
            if (offset == 21) await Assert.That(node.Clouds.Lighting).IsEqualTo(3);
        }
        if (offset == 14)
        {
            await Assert.That(node.References.BitmapFlareMoon.File!.FilePath).IsEqualTo("Moon.Bitmap.Gbx");
            await Assert.That(node.FlareAngularSizeSun).IsEqualTo(15.5f);
            await Assert.That(node.CameraFarZ).IsEqualTo(45000.5f);
            await Assert.That(node.References.BitmapRainFid.File!.FilePath).IsEqualTo("Rain.Bitmap.Gbx");
            await Assert.That(node.References.SceneFxFid.File!.FilePath).IsEqualTo("SceneFx.Gbx");
        }
        if (offset == 24)
        {
            await Assert.That(((IVersionable)chunk).Version).IsEqualTo(1);
            await Assert.That(node.References.EmitterInFog.File!.FilePath).IsEqualTo("Fog.Emitter.Gbx");
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task Constructor_NewNode_UsesNativeDefaults()
    {
        var node = new CPlugWeather();
        await Assert.That(node.FogRGB).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.DayFogColor).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.FogFlags).IsEqualTo(6u);
        await Assert.That(node.DayFogFlags).IsEqualTo(6u);
        await Assert.That(node.FogLinearEnd).IsEqualTo(1f);
        await Assert.That(node.DayFogDensity).IsEqualTo(1f);
        await Assert.That(node.CameraFarZ).IsEqualTo(31000f);
        await Assert.That(node.FlareAngularSizeSun).IsEqualTo(20f);
        await Assert.That(node.FlareAngularSizeMoon).IsEqualTo(5f);
        await Assert.That(node.SeaTwkReflecIntensMidNight).IsEqualTo(0.3f);
        await Assert.That(node.SeaTwkReflecIntensTMDay).IsEqualTo(1f);
        await Assert.That(node.SeaTwkReflecIntensTMmidDay).IsEqualTo(0.3f);
        await Assert.That(new CPlugWeather.Chunk0917E018(GameVersion.TM2020).Version).IsEqualTo(1);
    }

    [Test]
    public async Task FogSettings_WhenChanged_PreserveOtherPackedBits()
    {
        var node = new CPlugWeather { FogFlags = 0xFFFF0000 };
        node.FogByVertex = true;
        node.FogFormula = CPlugWeather.EGxFogFormula.Exp2;
        node.FogSpace = CPlugWeather.EGxFogSpace.World;
        await Assert.That(node.FogFlags).IsEqualTo(0xFFFF000Du);
        node.FogByVertex = false;
        await Assert.That(node.FogFormula).IsEqualTo(CPlugWeather.EGxFogFormula.Exp2);
        await Assert.That(node.FogSpace).IsEqualTo(CPlugWeather.EGxFogSpace.World);
        await Assert.That(node.FogFlags).IsEqualTo(0xFFFF000Cu);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_FogArchive_ClearsNonpersistentFlags(bool writing)
    {
        var node = new CPlugWeather { FogFlags = 0xFFFF000D, DayFogFlags = 0xABCD0006 };
        var chunk = new CPlugWeather.Chunk0917E00E();
        using var stream = new MemoryStream();
        if (writing)
        {
            using var writer = new GbxWriter(stream);
            using var rw = new GbxReaderWriter(writer);
            chunk.ReadWrite(node, rw);
        }
        else
        {
            using var payload = new ObjectModelPayload();
            WriteChunk(payload, 14, flags: 0xFFFF000D, dayFlags: 0xABCD0006);
            payload.Finish();
            payload.Read(rw => chunk.ReadWrite(node, rw));
        }
        await Assert.That(node.FogFlags).IsEqualTo(13u);
        await Assert.That(node.DayFogFlags).IsEqualTo(6u);
    }

    [Test]
    public async Task ReadWrite_FogEmitter_VersionZeroPreservesReference()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(0);
        payload.NodeRef("Fog.Emitter.Gbx");
        payload.Finish();
        var node = new CPlugWeather();
        var chunk = new CPlugWeather.Chunk0917E018();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(node.References.EmitterInFog.File!.FilePath).IsEqualTo("Fog.Emitter.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ReadWrite_BitmapAndEmitterReferences_AcceptNativeNodeClasses()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.WriteNodeRef(new CPlugBitmap());
        payload.Finish();
        var node = new CPlugWeather();
        var sky = new CPlugWeather.Chunk0917E011();
        payload.Read(rw => sky.ReadWrite(node, rw));
        await Assert.That(node.BitmapSkyGradV).IsTypeOf<CPlugBitmap>();
        await payload.AssertRoundTrip(rw => sky.ReadWrite(node, rw));

        using var emitterPayload = new ObjectModelPayload();
        emitterPayload.Writer.Write(1);
        emitterPayload.Writer.WriteNodeRef(new CPlugParticleEmitterModel());
        emitterPayload.Finish();
        var emitter = new CPlugWeather.Chunk0917E018();
        emitterPayload.Read(rw => emitter.ReadWrite(node, rw));
        await Assert.That(node.EmitterInFog).IsTypeOf<CPlugParticleEmitterModel>();
        await emitterPayload.AssertRoundTrip(rw => emitter.ReadWrite(node, rw));
    }

    [Test]
    public async Task ReadWrite_LegacyClassAndChunkIds_MapToWeather()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(50);
        payload.Writer.Write(0x05034000u);
        payload.Writer.Write(0x0503400Bu);
        WriteMaterials(payload, paired: false, twoSeas: true);
        payload.Writer.Write(0xFACADE01u);
        payload.Finish();
        CPlugWeather? node = null;
        payload.Read(rw => node = rw.NodeRef<CPlugWeather>());
        await Assert.That(node!.References.MaterialSea1.File!.FilePath).IsEqualTo("Sea1.Material.Gbx");
        await Assert.That(node.Chunks.Single().Id).IsEqualTo(0x0917E00Bu);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        {
            writer.WriteNodeRef(node);
        }
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        reader.ReadInt32();
        await Assert.That(reader.ReadUInt32()).IsEqualTo(CPlugWeather.Id);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0x0917E00Bu);
    }

    private static void WriteChunk(ObjectModelPayload payload, int offset, uint flags = 13, uint dayFlags = 6)
    {
        var writer = payload.Writer;
        switch (offset)
        {
            case 0:
            case 1:
                writer.WriteIdAsString("LegacyWeather");
                WriteMaterials(payload, paired: false, twoSeas: false);
                WriteLights(payload);
                WriteFog(writer, day: false, flags);
                WriteUnknowns(writer);
                if (offset == 1) writer.Write(1);
                break;
            case 2:
            case 3:
                writer.WriteIdAsString("LegacyWeather");
                WriteMaterials(payload, paired: true, twoSeas: false);
                WriteLights(payload);
                WriteFog(writer, day: false, flags);
                WriteUnknowns(writer);
                writer.Write(1);
                if (offset == 3) writer.Write(45000.5f);
                break;
            case 4:
            case 5:
            case 8:
            case 9:
                writer.WriteIdAsString("LegacyWeather");
                WriteMaterials(payload, paired: true, twoSeas: false);
                WriteFog(writer, day: false, flags);
                WriteFog(writer, day: true, dayFlags);
                WriteLights(payload);
                WriteUnknowns(writer);
                writer.Write(1);
                writer.Write(45000.5f);
                if (offset >= 8) payload.NodeRef("Rain.Bitmap.Gbx");
                if (offset == 9) payload.NodeRef("SceneFx.Gbx");
                break;
            case 6:
            case 13:
                for (var i = 1; i <= (offset == 6 ? 12 : 18); i++) writer.Write(i + 0.5f);
                break;
            case 7:
                break;
            case 10:
            case 12:
            case 14:
                WriteFog(writer, day: false, flags);
                WriteFog(writer, day: true, dayFlags);
                writer.WriteIdAsString("Weather");
                WriteLights(payload);
                if (offset == 14)
                {
                    payload.NodeRef("Moon.Image.Gbx");
                    payload.NodeRef("Sun.Bitmap.Gbx");
                    payload.NodeRef("Moon.Bitmap.Gbx");
                    writer.Write(15.5f);
                    writer.Write(16.5f);
                }
                WriteUnknowns(writer);
                if (offset != 14) writer.Write(1);
                writer.Write(45000.5f);
                payload.NodeRef("Rain.Bitmap.Gbx");
                payload.NodeRef("SceneFx.Gbx");
                if (offset == 12) payload.NodeRef("Moon.Image.Gbx");
                break;
            case 11:
                WriteMaterials(payload, paired: false, twoSeas: true);
                break;
            case 15:
            case 17:
            case 19:
            case 20:
            case 22:
            case 23:
                payload.NodeRef("Resource.Gbx");
                break;
            case 16:
            case 18:
            case 21:
                writer.Write(2);
                payload.NodeRef("Cloud1.Solid.Gbx");
                payload.NodeRef("Cloud2.Solid.Gbx");
                writer.Write(1200.5f);
                if (offset != 16)
                {
                    writer.Write(2400.5f);
                    payload.NodeRef("CloudMin.Image.Gbx");
                    payload.NodeRef("CloudMax.Image.Gbx");
                }
                if (offset == 21) writer.Write(3);
                break;
            case 24:
                writer.Write(1);
                payload.NodeRef("Fog.Emitter.Gbx");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(offset));
        }
    }

    private static void WriteMaterials(ObjectModelPayload payload, bool paired, bool twoSeas)
    {
        var skies = new[] { "Night", "SunRise", "Day", "SunFall" };
        for (var i = 0; i < skies.Length; i++)
        {
            payload.NodeRef(skies[i] + ".Material.Gbx");
            if (paired) payload.NodeRef(i == 3 ? "Sea0.Material.Gbx" : "LegacySea" + i + ".Material.Gbx");
        }
        if (!paired) payload.NodeRef("Sea0.Material.Gbx");
        if (twoSeas) payload.NodeRef("Sea1.Material.Gbx");
    }

    private static void WriteLights(ObjectModelPayload payload)
    {
        payload.NodeRef("Ambient.Image.Gbx");
        payload.NodeRef("Sun.Image.Gbx");
    }

    private static void WriteFog(GbxWriter writer, bool day, uint flags)
    {
        writer.Write(day ? new Vec3(1.1f, 1.2f, 1.3f) : new Vec3(0.1f, 0.2f, 0.3f));
        writer.Write(day ? 14.5f : 4.5f);
        writer.Write(day ? 15.5f : 5.5f);
        writer.Write(day ? 16.5f : 6.5f);
        writer.Write(flags);
    }

    private static void WriteUnknowns(GbxWriter writer)
    {
        writer.Write(7.5f);
        writer.Write(8.5f);
        writer.Write(9.5f);
    }
}
